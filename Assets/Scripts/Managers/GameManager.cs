using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace com.dhcc.spaceshooter
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        public static bool ApplicationIsQuitting { get; private set; }

        private bool isGameOver;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        private void Update()
        {
            if (isGameOver)
            {
                if (Input.GetKeyDown(KeyCode.R))
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                else if (Input.GetKeyDown(KeyCode.Escape))
                {
#if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
                }
            }
        }

        public void GameOver()
        {
            isGameOver = true;
        }

        private void OnApplicationQuit()
        {
            ApplicationIsQuitting = true;
        }
    }
}