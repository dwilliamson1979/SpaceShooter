using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SocialPlatforms.Impl;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("References")]
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text ammoText;
    [SerializeField] private Sprite[] livesSprites;
    [SerializeField] private Image livesImage;
    [SerializeField] private GameObject gameOverMenu;

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
    }

    public void GameOver()
    {
        gameOverMenu.SetActive(true);
    }
}