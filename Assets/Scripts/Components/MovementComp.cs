using UnityEngine;

namespace com.dhcc.components
{
    public class MovementComp : MonoBehaviour
    {
        [Header("Settings")]
        [field: SerializeField] public float DefaultSpeed { get; private set; }
        [SerializeField] private float maxSpeed;

        [Tooltip("Smooth Damp Setting")]
        [SerializeField] private float moveSmoothTime = 0.2f;

        private Vector2 moveSmoothVelocity;
        private Vector3 moveVector;
        private float currentSpeed;

        public void Move(Vector2 inputVector)
        {
            moveVector = Vector2.SmoothDamp(moveVector, inputVector, ref moveSmoothVelocity, moveSmoothTime);
            transform.Translate(Time.deltaTime * currentSpeed * moveVector);
        }

        public void SetSpeed(float newSpeed)
        {
            currentSpeed = Mathf.Clamp(newSpeed, 0, maxSpeed);
        }
    }
}