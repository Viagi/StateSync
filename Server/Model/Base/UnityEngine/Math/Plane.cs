using System;

namespace UnityEngine
{


    public struct Plane
    {
        Vector3 m_Normal;
        float m_Distance;

        // Normal vector of the plane.
        public Vector3 normal { get { return m_Normal; } set { m_Normal = value; } }
        // Distance from the origin to the plane.
        public float distance { get { return m_Distance; } set { m_Distance = value; } }

        // Creates a plane.
        public Plane(Vector3 inNormal, Vector3 inPoint)
        {
            m_Normal = Vector3.Normalize(inNormal);
            m_Distance = -Vector3.Dot(inNormal, inPoint);
        }

        // Creates a plane.
        public Plane(Vector3 inNormal, float d)
        {
            m_Normal = Vector3.Normalize(inNormal);
            m_Distance = d;
        }

        // Creates a plane.
        public Plane(Vector3 a, Vector3 b, Vector3 c)
        {
            m_Normal = Vector3.Normalize(Vector3.Cross(b - a, c - a));
            m_Distance = -Vector3.Dot(m_Normal, a);
        }

        // Sets a plane using a point that lies within it plus a normal to orient it (note that the normal must be a normalised vector).
        public void SetNormalAndPosition(Vector3 inNormal, Vector3 inPoint)
        {
            normal = Vector3.Normalize(inNormal);
            distance = -Vector3.Dot(inNormal, inPoint);
        }

        // Sets a plane using three points that lie within it.  The points go around clockwise as you look down on the top surface of the plane.
        public void Set3Points(Vector3 a, Vector3 b, Vector3 c)
        {
            normal = Vector3.Normalize(Vector3.Cross(b - a, c - a));
            distance = -Vector3.Dot(normal, a);
        }

        // Returns a signed distance from plane to point.
        public float GetDistanceToPoint(Vector3 inPt) { return Vector3.Dot(normal, inPt) + distance; }

        // Is a point on the positive side of the plane?
        public bool GetSide(Vector3 inPt) { return Vector3.Dot(normal, inPt) + distance > 0.0F; }

        // Are two points on the same side of the plane?
        public bool SameSide(Vector3 inPt0, Vector3 inPt1)
        {
            float d0 = GetDistanceToPoint(inPt0);
            float d1 = GetDistanceToPoint(inPt1);
            if (d0 > 0.0f && d1 > 0.0f)
                return true;
            else if (d0 <= 0.0f && d1 <= 0.0f)
                return true;
            else
                return false;
        }

        // Intersects a ray with the plane.
        public bool Raycast(Ray ray, out float enter)
        {
            float vdot = Vector3.Dot(ray.direction, normal);
            float ndot = -Vector3.Dot(ray.origin, normal) - distance;

            // is line parallel to the plane? if so, even if the line is
            // at the plane it is not considered as intersection because
            // it would be impossible to determine the point of intersection
            if (Mathf.Approximately(vdot, 0.0f))
            {
                enter = 0.0F;
                return false;
            }

            // the resulting intersection is behind the origin of the ray
            // if the result is negative ( enter < 0 )
            enter = ndot / vdot;

            return enter > 0.0F;
        }
    }
}
