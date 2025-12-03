using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class GameOverScreen : MonoBehaviour
    {
        public void ActivateScreen()
        {
            Time.timeScale = 0;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            this.gameObject.SetActive(true);
        }

        public void RestartButton()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
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