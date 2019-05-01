
using System;

namespace UnityEngine
{


    public struct Ray2D
    {
        private Vector2 m_Origin;
        private Vector2 m_Direction;

        // Creates a ray starting at /origin/ along /direction/.
        public Ray2D(Vector2 origin, Vector2 direction) { m_Origin = origin; m_Direction = direction.normalized; }

        // The origin point of the ray.
        public Vector2 origin { get { return m_Origin; } set { m_Origin = value; } }

        // The direction of the ray.
        public Vector2 direction { get { return m_Direction; } set { m_Direction = value.normalized; } }

        // Returns a point at /distance/ units along the ray.
        public Vector2 GetPoint(float distance) { return m_Origin + m_Direction * distance; }

        /// *listonly*
        override public string ToString() { return String.Format("Origin: {0}, Dir: {1}", m_Origin, m_Direction); }

        // Returns a nicely formatted string for this ray.
        public string ToString(string format)
        {
            return String.Format("Origin: {0}, Dir: {1}", m_Origin.ToString(format), m_Direction.ToString(format));
        }
    }

}
