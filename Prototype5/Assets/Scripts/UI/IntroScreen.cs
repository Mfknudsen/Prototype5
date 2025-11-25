using System.Collections;
using System.Collections.Generic;
using Managers;
using ScriptableVariables.Enums;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class IntroScreen : MonoBehaviour
    {
        [SerializeField] private float fadeTimeIncrement = 0.01f;
        [SerializeField] private UIManager uiManager;

        [SerializeField] private PlayerStateVariable playerStateVariable;

        private int _currentIndex;
        private List<TextMeshProUGUI> _textList;
        private Image _background;
        private bool _isFadingIn;
        private bool _isFadingOut;

        private void Awake()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0;

            this._isFadingIn = false;
            this._isFadingOut = false;

            this.uiManager.SetCanvas(UIManager.CanvasType.Player, false);
            this.uiManager.SetCanvas(UIManager.CanvasType.Minimap, false);

            this.GetUIElements();

            this.playerStateVariable.Value = PlayerStateEnum.InMenu;
        }

        private void OnEnable()
        {
            InputManager.Instance.ClickEvent.AddListener(this.DisplayNextText);
        }

        private void OnDisable()
        {
            InputManager.Instance.ClickEvent.RemoveListener(this.DisplayNextText);
        }

        private void GetUIElements()
        {
            this._background = this.GetComponent<Image>();
            this._textList = new List<TextMeshProUGUI>();
            foreach (Transform child in this.transform)
            {
                child.GetComponent<TextMeshProUGUI>().color -= new Color(0, 0, 0, 1);
                this._textList.Add(child.GetComponent<TextMeshProUGUI>());
            }
        }

        private void DisplayNextText()
        {
            if (this._currentIndex == this._textList.Count)
                return;

            if (this._textList[this._currentIndex].color.a < 1 && !this._isFadingOut)
                this.StartCoroutine(this.FadeIn());
            else
                this.StartCoroutine(this.FadeOut());
        }

        private void StartGame()
        {
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            this.uiManager.SetCanvas(UIManager.CanvasType.Player, true);
            this.uiManager.SetCanvas(UIManager.CanvasType.Minimap, true);

            this.gameObject.SetActive(false);

            this.playerStateVariable.Value = PlayerStateEnum.Free;
        }

        private IEnumerator FadeIn()
        {
            if (this._isFadingIn)
            {
                this._textList[this._currentIndex].color += new Color(0, 0, 0, 1f);
                yield break;
            }

            this._isFadingIn = true;
            this._textList[this._currentIndex].gameObject.SetActive(true);

            while (this._textList[this._currentIndex].color.a < 1f)
            {
                this._textList[this._currentIndex].color +=
                    new Color(0, 0, 0, this.fadeTimeIncrement );
                yield return null;
            }

            this._isFadingIn = false;
        }

        private IEnumerator FadeOut()
        {
            if (this._isFadingOut)
            {
                this._textList[this._currentIndex].color -= new Color(0, 0, 0, 1f);
                yield break;
            }

            this._isFadingOut = true;
            while (this._textList[this._currentIndex].color.a > 0f)
            {
                this._textList[this._currentIndex].color -= new Color(0, 0, 0, this.fadeTimeIncrement );
                yield return null;
            }

            this._isFadingOut = false;
            this._textList[this._currentIndex].gameObject.SetActive(false);
            this._currentIndex++;

            if (this._currentIndex == this._textList.Count)
            {
                this.StartCoroutine(this.FadeOutBackground());
                yield break;
            }

            this.StartCoroutine(this.FadeIn());
        }

        private IEnumerator FadeOutBackground()
        {
            while (this._background.color.a > 0f)
            {
                this._background.color -= new Color(0, 0, 0, this.fadeTimeIncrement );
                yield return null;
            }

            this.StartGame();
        }
    }
}