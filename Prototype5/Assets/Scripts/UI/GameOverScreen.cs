using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class GameOverScreen : MonoBehaviour
    {
        public void ActivateScreen()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            gameObject.SetActive(true);
        }

        public void RestartButton()
        {
            SceneManager.LoadScene( SceneManager.GetActiveScene().name );
        }

        public void ExitButton()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.ExitPlaymode();
#else
            Application.Quit();
#endif
        }
    }
}
