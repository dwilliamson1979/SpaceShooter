using com.dhcc.framework;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace com.dhcc.spaceshooter
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager instance;
        public static GameManager Instance => instance != null ? instance : SingletonEmulator.Get(instance);
        public static bool ApplicationIsQuitting { get; private set; }

        private bool isGameOver;

        private void Awake()
        {
            SingletonEmulator.Enforce(this, instance, out instance);
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