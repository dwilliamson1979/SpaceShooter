using UnityEngine;

public class Explosion : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private AudioClip explosionAudio;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animator;

    void Start()
    {
        AudioManager.Instance.PlaySoundFx(explosionAudio);
    }

    public void AnimationComplete()
    {
        Destroy(gameObject);
    }

    public void Explode()
    {
        animator.SetTrigger("OnExplode");
    }

    public void SetColor(Color color)
    {
        spriteRenderer.color = color;
    }

    public void SetScale(float scale)
    {
        transform.localScale = new Vector3(scale, scale, transform.localScale.z);
    }
}