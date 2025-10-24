using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class GameOverScreen : MonoBehaviour
    {
        public void ActivateScreen()
        {
            gameObject.SetActive(true);
        }

        public void RestartButton()
        {
            SceneManager.LoadScene( SceneManager.GetActiveScene().name );
        }

        public void ExitButton()
        {
            Application.Quit();
        }
    }
}
