using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("References")]
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text ammoText;
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

    void Start()
    {
        UpdateScore(0);
        UpdateLives(0);
    }

    public void UpdateScore(int score)
    {
        scoreText.text = $"Score: {score}";
    }

    public void UpdateLives(int lives)
    {
        livesImage.sprite = livesSprites[lives];
    }

    public void UpdateAmmo(int ammo)
    {
        ammoText.text = $"Ammo: {ammo}";

        if (ammo <= 0)
        {
            if(flashAmmoRoutine != null)
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

    private IEnumerator FlashAmmoRoutine()
    {
        WaitForSeconds wfs = new(0.5f);
        while(true)
        {
            yield return wfs;
            ammoText.gameObject.SetActive(!ammoText.gameObject.activeInHierarchy);
        }
    }

    public void GameOver()
    {
        gameOverMenu.SetActive(true);
    }
}