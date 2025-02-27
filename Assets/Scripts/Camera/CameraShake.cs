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

        private Camera _camera;
        private Vector3 originalPosition;
        private float currentDuration;

        private void Awake()
        {
            _camera = GetComponent<Camera>();
            originalPosition = _camera.transform.position;

            enabled = false;

            GameEvents.PlayerHealthChanged.Subscribe(HandlePlayerHealthChanged);
        }

        private void Update()
        {
            if (currentDuration.IsGreaterThanZero())
            {
                _camera.transform.localPosition = originalPosition + Random.insideUnitSphere * amount;
                currentDuration -= Time.deltaTime * decreaseFactor;
            }
            else
            {
                currentDuration = 0f;
                _camera.transform.localPosition = originalPosition;
                enabled = false;
            }
        }

        private void HandlePlayerHealthChanged(int delta, int health)
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