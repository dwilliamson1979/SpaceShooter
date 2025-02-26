using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

namespace com.dhcc.spaceshooter
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance;

        [Header("References")]
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private TMP_Text ammoText;
        [SerializeField] private Slider thrusterSlider;
        [SerializeField] private Sprite[] livesSprites;
        [SerializeField] private Image livesImage;
        [SerializeField] private GameObject gameOverMenu;

        private Coroutine flashAmmoRoutine;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        private void OnEnable()
        {
            GameEvents.PlayerHealthChanged.Subscribe(HandlePlayerHealthChanged);
        }

        private void OnDisable()
        {
            GameEvents.PlayerHealthChanged.Unsubscribe(HandlePlayerHealthChanged);
        }

        void Start()
        {
            UpdateScore(0);
        }

        public void UpdateScore(int score)
        {
            scoreText.text = $"Score: {score}";
        }

        public void UpdateAmmo(int ammo)
        {
            ammoText.text = $"Ammo: {ammo}";

            if (ammo <= 0)
            {
                if (flashAmmoRoutine != null)
                    StopCoroutine(flashAmmoRoutine);

                flashAmmoRoutine = StartCoroutine(FlashAmmoRoutine());
            }
            else
            {
                if (flashAmmoRoutine != null)
                    StopCoroutine(flashAmmoRoutine);

                ammoText.gameObject.SetActive(true);
            }
        }

        public void UpdateThruster(float val)
        {
            thrusterSlider.value = Mathf.Clamp(val, 0f, 1f);
        }

        private IEnumerator FlashAmmoRoutine()
        {
            WaitForSeconds wfs = new(0.5f);
            while (true)
            {
                yield return wfs;
                ammoText.gameObject.SetActive(!ammoText.gameObject.activeInHierarchy);
            }
        }

        public void GameOver()
        {
            gameOverMenu.SetActive(true);
        }

        private void HandlePlayerHealthChanged(int delta, HealthComp healthComp)
        {
            livesImage.sprite = livesSprites[healthComp.Health.CurrentValue];
        }
    }
}