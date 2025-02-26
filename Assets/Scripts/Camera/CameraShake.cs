using UnityEngine;
using com.dhcc.framework;

namespace com.dhcc.spaceshooter
{
    public class CameraShake : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float duration;
        [SerializeField] private float amount;
        [SerializeField] private float decreaseFactor;

        private Camera m_camera;
        private Vector3 m_originalPosition;
        private float currentDuration;

        private void Awake()
        {
            m_camera = GetComponent<Camera>();
            m_originalPosition = m_camera.transform.position;

            enabled = false;

            GameEvents.PlayerHealthChanged.Subscribe(HandlePlayerHealthChanged);
        }

        private void Update()
        {
            if (currentDuration.IsGreaterThanZero())
            {
                m_camera.transform.localPosition = m_originalPosition + Random.insideUnitSphere * amount;
                currentDuration -= Time.deltaTime * decreaseFactor;
            }
            else
            {
                currentDuration = 0f;
                m_camera.transform.localPosition = m_originalPosition;
                enabled = false;
            }
        }

        private void HandlePlayerHealthChanged(int delta, HealthComp healthComp)
        {
            if (enabled || delta > 0) return;

            enabled = true;
            currentDuration = duration;
        }

        private void OnDestroy()
        {
            GameEvents.PlayerHealthChanged.Unsubscribe(HandlePlayerHealthChanged);
        }
    }
}