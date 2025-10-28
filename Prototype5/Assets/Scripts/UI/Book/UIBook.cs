#region Packages

using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableVariables.Enums;
using TMPro;
using UI.Book.Button;
using UI.Book.Slider;
using UI.Book.TextInputField;
using UnityEngine;
using UnityEngine.UI;

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
        private int currentPageIndex = 0;

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
            if (turn == BookTurn.Null) return;

            this.invisiblyUI.SetActive(false);
            this.StartCoroutine(this.AnimationTrigger(turn, 0.5f));

            this.CopyTextures();

            if (turn == BookTurn.Close)
            {
                this.openLeft.GetComponent<Renderer>().material.SetTexture(PreRenderTextureID, this.preRenderTexture);
                this.openRight.GetComponent<Renderer>().material.SetTexture(PreRenderTextureID, this.preRenderTexture);
            }

            switch (turn)
            {
                case BookTurn.Open:
                    this.bookCanvas.gameObject.SetActive(true);
                    this.currentBookAction =
                        this.StartCoroutine(this.bookOpenActionAction.Operation(() => this.currentBookAction = null));
                    this.UpdatePageVisibility();
                    break;

                case BookTurn.Close:
                    this.currentBookAction =
                        this.StartCoroutine(this.closeBookActionAction.Operation(() =>
                        {
                            this.bookCanvas.gameObject.SetActive(false);
                            this.currentBookAction = null;
                        }));
                    break;

                case BookTurn.Left:
                    if (currentPageIndex > 0)
                    {
                        this.currentPageIndex--;
                        this.bookTurnBookAction.SetDirection(false);
                        this.currentBookAction =
                            this.StartCoroutine(this.bookTurnBookAction.Operation(() =>
                            {
                                this.currentBookAction = null;
                                UpdatePageVisibility();
                            }));
                    }

                    break;

                case BookTurn.Right:
                    if (currentPageIndex < pages.Count - 1)
                    {
                        currentPageIndex++;
                        this.bookTurnBookAction.SetDirection(true);
                        this.currentBookAction =
                            this.StartCoroutine(this.bookTurnBookAction.Operation(() =>
                            {
                                this.currentBookAction = null;
                                UpdatePageVisibility();
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

        private IEnumerator AnimationTrigger(BookTurn trigger, float time)
        {
            if (trigger == BookTurn.Null) yield break;

            yield return new WaitForSeconds(time);

            int hash = trigger switch
            {
                BookTurn.Close => HashCloseBook,
                BookTurn.Open => HashOpenBook,
                BookTurn.Left => HashTurnLeft,
                BookTurn.Right => HashTurnRight,
                _ => throw new ArgumentOutOfRangeException(nameof(trigger), trigger, null)
            };

            //if (trigger is BookTurn.Close or BookTurn.Open) this.playerManager.GetController().TriggerAnimator(hash);

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

                    if (o.GetComponent<Outline>() is { } outline)
                        Destroy(outline);

                    if (o.GetComponent<TextMeshProUGUI>() is { } text)
                        Destroy(text);
                }

                break;
            }

            this.invisiblyUI.SetActive(true);
        }
        
        private void UpdatePageVisibility()
        {
            for (int i = 0; i < pages.Count; i++)
            {
                pages[i].SetActive(i == currentPageIndex);
            }
            
            Debug.Log($"Showing page {currentPageIndex + 1} of {pages.Count}");
        }
        
        public void CloseBook() => Effect(BookTurn.Close);

        public void FlipRight() => Effect(BookTurn.Right);
        
        public void FlipLeft() => Effect(BookTurn.Left);
        
        private void Update()
        {
            // Only handle input if the book is open
            if (!bookCanvas.gameObject.activeSelf)
                return;

            // Prevent new input while an animation is running
            if (currentBookAction != null)
                return;

            // Right Arrow → turn right (next page)
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                FlipRight();
            }

            // Left Arrow → turn left (previous page)
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                FlipLeft();
            }

            // Optional: Escape key closes the book
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                CloseBook();
            }
        }


        #endregion
    }
}