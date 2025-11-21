using UnityEngine;

namespace Managers
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private GameObject canvasPauseScreen;
        [SerializeField] private GameObject canvasPlayerScreen;
        [SerializeField] private GameObject canvasMinimapScreen;
        [SerializeField] private GameObject canvasIntroScreen;

        public enum CanvasType
        {
            Pause,
            Player,
            Minimap,
            Intro
        }
        
        public void SetCanvas(CanvasType canvasType, bool isActive)
        {
            switch (canvasType)
            {
                case CanvasType.Pause:
                    canvasPauseScreen?.SetActive(isActive);
                    break;
                case CanvasType.Player:
                    canvasPlayerScreen?.SetActive(isActive);
                    break;
                case CanvasType.Minimap:
                    canvasMinimapScreen?.SetActive(isActive);
                    break;
                case CanvasType.Intro:
                    canvasIntroScreen?.SetActive(isActive);
                    break;
                default:
                    Debug.Log("Canvas type not in list.");
                    break;
            }
        }
    }
}
