using UnityEngine;

namespace com.dhcc.spaceshooter
{
    public class Explosion : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private AudioClip explosionAudio;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Animator animator;

        public void AnimationComplete()
        {
            Destroy(gameObject);
        }

        public void Explode()
        {
            animator.SetTrigger("OnExplode");
            AudioManager.Instance.PlaySoundFx(explosionAudio);
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
}