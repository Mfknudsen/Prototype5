using System.Collections;
using System.Collections.Generic;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class IntroScreen : MonoBehaviour
    {
        [SerializeField] private float fadeTimeIncrement = 0.01f;
        [SerializeField] private UIManager uiManager;

        private int _currentIndex = 0;
        private List<TextMeshProUGUI> _textList;
        private Image _background;
        private bool _isFadingIn;
        private bool _isFadingOut;
        
        private void Awake()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0;
            
            _isFadingIn = false;
            _isFadingOut = false;
            
            uiManager.SetCanvas(UIManager.CanvasType.Player, false);
            uiManager.SetCanvas(UIManager.CanvasType.Minimap, false);
            
            GetUIElements();
        }

        private void OnEnable()
        {
            InputManager.Instance.ClickEvent.AddListener(DisplayNextText);
        }

        private void OnDisable()
        {
            InputManager.Instance.ClickEvent.RemoveListener(DisplayNextText);
        }

        private void GetUIElements()
        {
            _background = GetComponent<Image>();
            _textList = new List<TextMeshProUGUI>();
            foreach (Transform child in transform)
            {
                child.GetComponent<TextMeshProUGUI>().color -= new Color(0, 0, 0, 1);
                _textList.Add(child.GetComponent<TextMeshProUGUI>());   
            }
        }

        private void DisplayNextText()
        {
            if (_currentIndex == _textList.Count)
                return;
            
            if (_textList[_currentIndex].color.a < 1 && !_isFadingOut)
                StartCoroutine(FadeIn());
            else
                StartCoroutine(FadeOut());
        }

        private void StartGame()
        {
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            
            uiManager.SetCanvas(UIManager.CanvasType.Player, true);
            uiManager.SetCanvas(UIManager.CanvasType.Minimap, true);
            
            gameObject.SetActive(false);
        }

        private IEnumerator FadeIn()
        {
            if (_isFadingIn)
            {
                _textList[_currentIndex].color += new Color(0, 0, 0, 1f);
                yield break;
            }
            _isFadingIn = true;
            _textList[_currentIndex].gameObject.SetActive(true);
            
            while (_textList[_currentIndex].color.a < 1f)
            {
                _textList[_currentIndex].color += new Color(0, 0, 0, fadeTimeIncrement);
                yield return null;
            }
            _isFadingIn = false;
        }

        private IEnumerator FadeOut()
        {
            if (_isFadingOut)
            {
                _textList[_currentIndex].color -= new Color(0, 0, 0, 1f);
                yield break;
            }
            
            _isFadingOut = true;
            while (_textList[_currentIndex].color.a > 0f)
            {
                _textList[_currentIndex].color -= new Color(0, 0, 0, fadeTimeIncrement);
                yield return null;
            }

            _isFadingOut = false;
            _textList[_currentIndex].gameObject.SetActive(false);
            _currentIndex++;
            
            if (_currentIndex == _textList.Count)
            {
                StartCoroutine(FadeOutBackground());
                yield break;
            }
            StartCoroutine(FadeIn());
        }

        private IEnumerator FadeOutBackground()
        {
            while (_background.color.a > 0f)
            {
                _background.color -= new Color(0, 0, 0, fadeTimeIncrement);
                yield return null;
            }
            
            StartGame();
        }
    }
}
