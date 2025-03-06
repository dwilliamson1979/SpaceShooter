using UnityEngine;

namespace com.dhcc.spaceshooter
{
    [System.Serializable]
    public struct Boundary
    {
        public float Left; 
        public float Right; 
        public float Top; 
        public float Bottom;

        public bool IsOutOfBounds(Transform transform)
        {
            return transform.position.x < Left
                || transform.position.x > Right
                || transform.position.y < Bottom
                || transform.position.y > Top;
        }

        public bool IsOutOfLeftBounds(float val) => val < Left;
        public bool IsOutOfRightBounds(float val) => val > Right;
        public bool IsOutOfBottomBounds(float val) => val < Bottom;
        public bool IsOutOfTopBounds(float val) => val > Top;
    }
}