using System.Collections.Generic;
using Managers;
using UnityEngine;

namespace UI
{
    public class IntroScreen : MonoBehaviour
    {
        [SerializeField] private float fadeOutTime = 1f;
        [SerializeField] private UIManager uiManager;

        private int _currentIndex = 1;
        private List<Transform> _textList;
        
        private void Awake()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0;
            
            uiManager.SetCanvas(UIManager.CanvasType.Player, false);
            uiManager.SetCanvas(UIManager.CanvasType.Minimap, false);
            
            GetTextList();
        }

        private void OnEnable()
        {
            InputManager.Instance.ClickEvent.AddListener(DisplayNextText);
        }

        private void OnDisable()
        {
            InputManager.Instance.ClickEvent.RemoveListener(DisplayNextText);
        }

        private void GetTextList()
        {
            _textList = new List<Transform>();
            foreach (Transform child in transform)
                _textList.Add(child);   
        }

        private void DisplayNextText()
        {
            if (_currentIndex == _textList.Count)
            {
                StartGame();
                return;
            }
            
            if (_currentIndex != 0)
                _textList[_currentIndex - 1].gameObject.SetActive(false);
            
            _textList[_currentIndex].gameObject.SetActive(true);
            _currentIndex++;
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
        
    }
}
