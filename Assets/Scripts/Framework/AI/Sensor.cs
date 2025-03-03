using System.Collections.Generic;
using UnityEngine;

namespace com.dhcc.framework.ai
{
    [RequireComponent (typeof(CircleCollider2D))]
    public class Sensor : MonoBehaviour
    {
        public float detectionRadius = 10f;
        public List<string> targetTags = new();

        private readonly List<Transform> detectedObjects = new(10); //Why 10? Limit the AI to a certain amount of objects to focus on?
        private CircleCollider2D circleCollider;

        private void Awake()
        {
            circleCollider = GetComponent<CircleCollider2D>();
        }

        private void Start()
        {
            circleCollider.isTrigger = true;
            circleCollider.radius = detectionRadius;
        }
    }
}