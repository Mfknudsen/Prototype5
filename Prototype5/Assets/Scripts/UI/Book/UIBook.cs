#region Packages

using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using ScriptableVariables.Enums;
using ScriptableVariables.Objects;
using TMPro;
using UI.Book.Button;
using UI.Book.Slider;
using UI.Book.TextInputField;
using UnityEngine;
using UnityEngine.UI;

// ReSharper disable CanSimplifyDictionaryLookupWithTryGetValue

#endregion

namespace UI.Book
{
    #region Enums

    public enum BookTurn
    {
        Null,
        Open,
        Close,
        Left,
        Right
    }

    #endregion

    public class UIBook : MonoBehaviour
    {
        #region Values

        [SerializeField] private TransformVariable playerTransformVariable;

        [SerializeField] private Canvas bookCanvas;

        [SerializeField] private PlayerStateVariable playerStateVariable;

        [SerializeField] private RenderTexture preRenderTexture, curRenderTexture;

        [SerializeField] private GameObject openLeft, openRight, turnLeft, turnRight;

        [SerializeField] private Animator bookAnimator;

        [SerializeField] private GameObject invisiblyUI, visuals;

        [SerializeField] private BookOpenAction bookOpenActionAction;
        [SerializeField] private BookTurnAction bookTurnBookAction;
        [SerializeField] private CloseBookAction closeBookActionAction;

        [SerializeField] private List<GameObject> pages = new();
        private int currentPageIndex;

        private readonly Dictionary<string, BookButton> buttonReferences = new Dictionary<string, BookButton>();
        private readonly Dictionary<string, BookSlider> sliderReferences = new Dictionary<string, BookSlider>();

        private readonly Dictionary<string, BookTextInputField> textInputFieldReferences =
            new Dictionary<string, BookTextInputField>();

        private static readonly int PreRenderTextureID = Shader.PropertyToID("RenderTexture");

        private Coroutine currentBookAction;

        #region Hash

        private static readonly int HashCloseBook = Animator.StringToHash("CloseBook"),
            HashOpenBook = Animator.StringToHash("OpenBook"),
            HashTurnLeft = Animator.StringToHash("TurnLeftToRight"),
            HashTurnRight = Animator.StringToHash("TurnRightToLeft");

        #endregion

        #endregion

        #region Build In States

        private void OnEnable()
        {
            if (playerTransformVariable.Value != null)
                this.OnCameraTransformUpdate(this.playerTransformVariable.Value);
            this.playerTransformVariable.AddListener(this.OnCameraTransformUpdate);
        }

        private void OnDisable()
        {
            this.playerTransformVariable.RemoveListener(this.OnCameraTransformUpdate);
        }

        private void Start()
        {
            this.bookCanvas.gameObject.SetActive(false);
            this.invisiblyUI.transform.localScale /= 10000;

            this.turnRight.SetActive(false);
            this.turnLeft.SetActive(false);
        }

        #endregion

        #region In

        public void ConstructUI()
        {
            this.CopyTextures();
            this.ConstructAsync();
        }

        public void Effect(BookTurn turn)
        {
            if (turn == BookTurn.Null || this.currentBookAction != null) return;

            if (turn == BookTurn.Close || turn == BookTurn.Open)
                this.AnimationTrigger(turn);

            this.CopyTextures();

            if (turn == BookTurn.Close)
            {
                this.openLeft.GetComponent<Renderer>().material.SetTexture(PreRenderTextureID, this.preRenderTexture);
                this.openRight.GetComponent<Renderer>().material.SetTexture(PreRenderTextureID, this.preRenderTexture);
            }

            switch (turn)
            {
                case BookTurn.Open:
                    InputManager.Instance.InteractInputEvent.AddListener(this.OnInteractInput);
                    InputManager.Instance.ArrowAxisInputEvent.AddListener(this.OnArrowInput);
                    this.invisiblyUI.SetActive(false);
                    this.bookCanvas.gameObject.SetActive(true);
                    this.UpdatePageVisibility();
                    this.currentBookAction =
                        this.StartCoroutine(this.bookOpenActionAction.Operation(() => this.currentBookAction = null));
                    break;

                case BookTurn.Close:
                    InputManager.Instance.InteractInputEvent.RemoveListener(this.OnInteractInput);
                    InputManager.Instance.ArrowAxisInputEvent.RemoveListener(this.OnArrowInput);
                    this.invisiblyUI.SetActive(false);
                    this.currentBookAction =
                        this.StartCoroutine(this.closeBookActionAction.Operation(() =>
                        {
                            this.bookCanvas.gameObject.SetActive(false);
                            this.currentBookAction = null;
                            Debug.Log($"End: {this.currentBookAction == null}");
                        }));
                    break;

                case BookTurn.Left:
                    if (this.currentPageIndex > 0)
                    {
                        this.invisiblyUI.SetActive(false);
                        this.currentPageIndex--;
                        this.UpdatePageVisibility();
                        this.AnimationTrigger(turn);
                        this.bookTurnBookAction.SetDirection(false);
                        this.currentBookAction =
                            this.StartCoroutine(this.bookTurnBookAction.Operation(() =>
                            {
                                this.currentBookAction = null;
                            }));
                    }

                    break;

                case BookTurn.Right:
                    if (this.currentPageIndex < this.pages.Count - 1)
                    {
                        this.invisiblyUI.SetActive(false);
                        this.currentPageIndex++;
                        this.UpdatePageVisibility();
                        this.AnimationTrigger(turn);
                        this.bookTurnBookAction.SetDirection(true);
                        this.currentBookAction =
                            this.StartCoroutine(this.bookTurnBookAction.Operation(() =>
                            {
                                this.currentBookAction = null;
                            }));
                    }

                    break;

                case BookTurn.Null:
                default:
                    throw new ArgumentOutOfRangeException(nameof(turn), turn, null);
            }
        }

        private void CopyTextures()
        {
            try
            {
                Graphics.CopyTexture(this.curRenderTexture, this.preRenderTexture);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        #endregion

        #region Internal

        private static List<GameObject> GetAllByRoot(GameObject obj)
        {
            List<GameObject> result = new List<GameObject>();

            foreach (Transform t in obj.transform)
            {
                result.Add(t.gameObject);
                result.AddRange(GetAllByRoot(t.gameObject));
            }

            return result;
        }

        private void AnimationTrigger(BookTurn trigger)
        {
            if (trigger == BookTurn.Null) return;

            int hash = trigger switch
            {
                BookTurn.Close => HashCloseBook,
                BookTurn.Open => HashOpenBook,
                BookTurn.Left => HashTurnLeft,
                BookTurn.Right => HashTurnRight,
                _ => throw new ArgumentOutOfRangeException(nameof(trigger), trigger, null)
            };

            this.bookAnimator.SetTrigger(hash);
        }

        private static void AddToReference<TElement>(GameObject obj, IDictionary<string, TElement> dictionary)
            where TElement : MonoBehaviour, ICustomGUIElement
        {
            if (obj.GetComponent<TElement>() is { } element)
                dictionary.Add(obj.name, element);
        }

        private void Replace<TReference, TElement>(GameObject obj, IReadOnlyDictionary<string, TReference> list)
            where TReference : MonoBehaviour, ICustomGUIElement
            where TElement : MonoBehaviour, ICustomGUIElementReference
        {
            string n = obj.name;

            if (!list.ContainsKey(n)) return;

            TReference element = list[n];

            if (element is null) return;

            obj.GetComponent<RectTransform>().localPosition += new Vector3(0, 0, -100f);

            Destroy(obj.GetComponent<TReference>());
            this.StartCoroutine(AddGUIReferenceComponent<TElement>(obj, this, element));
        }

        private static IEnumerator AddGUIReferenceComponent<T>(GameObject obj, UIBook uiBook, ICustomGUIElement element)
            where T : MonoBehaviour, ICustomGUIElementReference
        {
            yield return null;

            if (obj is null || element is null) yield break;

            obj.AddComponent<T>().Setup(uiBook, element);
        }

        private void ConstructAsync()
        {
            this.invisiblyUI.SetActive(false);

            foreach (Transform t in this.invisiblyUI.transform)
                Destroy(t.gameObject);

            foreach (Transform t in this.bookCanvas.transform)
            {
                GameObject transObj = t.gameObject;
                string objName = transObj.name;

                if (!transObj.activeSelf ||
                    objName.Equals("Transition UI") ||
                    objName.Equals("Template Page"))
                    continue;

                GameObject obj = Instantiate(t.gameObject, this.invisiblyUI.transform);

                this.buttonReferences.Clear();
                this.sliderReferences.Clear();
                this.textInputFieldReferences.Clear();
                foreach (GameObject o in GetAllByRoot(t.gameObject))
                {
                    AddToReference(o, this.buttonReferences);
                    AddToReference(o, this.sliderReferences);
                    AddToReference(o, this.textInputFieldReferences);
                }

                //Clean the copied ui
                foreach (GameObject o in GetAllByRoot(obj))
                {
                    if (!o.activeSelf)
                    {
                        Destroy(o);
                        continue;
                    }

                    this.Replace<BookButton, BookButtonReference>(o, this.buttonReferences);
                    this.Replace<BookSlider, BookSliderReference>(o, this.sliderReferences);
                    this.Replace<BookTextInputField, BookTextInputFieldReference>(o, this.textInputFieldReferences);

                    if (o.GetComponent<Image>() is { } image)
                        image.color = Color.clear;

                    if (o.GetComponent<Outline>() is { } outline)
                        Destroy(outline);

                    if (o.GetComponent<TextMeshProUGUI>() is { } text)
                        Destroy(text);

                    if (o.GetComponent<Collider>() is { } collider)
                        Destroy(collider);

                    if (o.GetComponent<Rigidbody>() is { } rigidbody)
                        Destroy(rigidbody);
                }

                break;
            }

            this.invisiblyUI.SetActive(true);
        }

        private void UpdatePageVisibility()
        {
            for (int i = 0; i < this.pages.Count; i++)
            {
                this.pages[i].SetActive(i == this.currentPageIndex);
            }

            //Debug.Log($"Showing page {this.currentPageIndex + 1} of {this.pages.Count}");
        }

        public void FlipRight()
        {
            this.Effect(BookTurn.Right);
        }

        public void FlipLeft()
        {
            this.Effect(BookTurn.Left);
        }

        public void SetPages(List<GameObject> newPages)
        {
            this.pages = newPages;
            this.currentPageIndex = 0;
            this.UpdatePageVisibility();
        }

        private void OnCameraTransformUpdate(Transform input)
        {
            this.invisiblyUI.GetComponent<Canvas>().worldCamera = input?.GetComponent<Camera>();
        }

        private void OnInteractInput()
        {
            this.Effect(BookTurn.Close);
        }

        private void OnArrowInput(Vector2 input)
        {
            if (input.x == 0)
                return;

            this.Effect(input.x < 0 ? BookTurn.Left : BookTurn.Right);
        }

        #endregion
    }
}