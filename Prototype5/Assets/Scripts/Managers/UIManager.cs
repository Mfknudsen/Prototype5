using UnityEngine;

namespace Managers
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private GameObject canvasPauseScreen;
        [SerializeField] private GameObject canvasPlayerScreen;
        [SerializeField] private GameObject canvasMinimapScreen;

        public enum CanvasType
        {
            Pause,
            Player,
            Minimap
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
                default:
                    break;
            }
        }
    }
}
