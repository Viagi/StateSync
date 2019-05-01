using System;


namespace UnityEngine
{

    public partial struct Bounds
    {

        private Vector3 m_Center;
        private Vector3 m_Extents;

        // Creates new Bounds with a given /center/ and total /size/. Bound ::ref::extents will be half the given size.
        public Bounds(Vector3 center, Vector3 size)
        {
            m_Center = center;
            m_Extents = size * 0.5F;
        }

        // used to allow Bounds to be used as keys in hash tables
        public override int GetHashCode()
        {
            return center.GetHashCode() ^ (extents.GetHashCode() << 2);
        }

        // also required for being able to use Vector4s as keys in hash tables
        public override bool Equals(object other)
        {
            if (!(other is Bounds)) return false;

            Bounds rhs = (Bounds)other;
            return center.Equals(rhs.center) && extents.Equals(rhs.extents);
        }

        // The center of the bounding box.
        public Vector3 center { get { return m_Center; } set { m_Center = value; } }

        // The total size of the box. This is always twice as large as the ::ref::extents.
        public Vector3 size { get { return m_Extents * 2.0F; } set { m_Extents = value * 0.5F; } }

        // The extents of the box. This is always half of the ::ref::size.
        public Vector3 extents { get { return m_Extents; } set { m_Extents = value; } }

        // The minimal point of the box. This is always equal to ''center-extents''.
        public Vector3 min { get { return center - extents; } set { SetMinMax(value, max); } }
        // The maximal point of the box. This is always equal to ''center+extents''.
        public Vector3 max { get { return center + extents; } set { SetMinMax(min, value); } }

        private Vector3 GetCenter() { return center; }
        private Vector3 GetExtent() { return extents; }

        //*undoc*
        public static bool operator ==(Bounds lhs, Bounds rhs)
        {
            return (lhs.center == rhs.center && lhs.extents == rhs.extents);
        }
        //*undoc*
        public static bool operator !=(Bounds lhs, Bounds rhs)
        {
            return !(lhs == rhs);
        }

        // Sets the bounds to the /min/ and /max/ value of the box.
        public void SetMinMax(Vector3 min, Vector3 max)
        {
            extents = (max - min) * 0.5F;
            center = min + extents;
        }

        // Grows the Bounds to include the /point/.
        public void Encapsulate(Vector3 point)
        {
            SetMinMax(Vector3.Min(min, point), Vector3.Max(max, point));
        }

        // Grow the bounds to encapsulate the bounds.
        public void Encapsulate(Bounds bounds)
        {
            Encapsulate(bounds.center - bounds.extents);
            Encapsulate(bounds.center + bounds.extents);
        }

        // Expand the bounds by increasing its /size/ by /amount/ along each side.
        public void Expand(float amount)
        {
            amount *= .5f;
            extents += new Vector3(amount, amount, amount);
        }

        // Expand the bounds by increasing its /size/ by /amount/ along each side.
        public void Expand(Vector3 amount)
        {
            extents += amount * .5f;
        }

        // Does another bounding box intersect with this bounding box?
        public bool Intersects(Bounds bounds)
        {
            return (min.x <= bounds.max.x) && (max.x >= bounds.min.x) &&
                   (min.y <= bounds.max.y) && (max.y >= bounds.min.y) &&
                   (min.z <= bounds.max.z) && (max.z >= bounds.min.z);
        }

        private static bool Internal_Contains(Bounds m, Vector3 point) { return m.IsInside(point); }
        // Is /point/ contained in the bounding box?
        public bool Contains(Vector3 point) { return Internal_Contains(this, point); }

        private static float Internal_SqrDistance(Bounds m, Vector3 point) { return CalculateSqrDistance(point, m); }

        // The smallest squared distance between the point and this bounding box.
        public float SqrDistance(Vector3 point) { return Internal_SqrDistance(this, point); }

        //private static bool Internal_IntersectRay (ref Ray ray, ref Bounds bounds, out float distance) { return IntersectRayAABB(ray, bounds, out distance); }

        // Does /ray/ intersect this bounding box?
        public bool IntersectRay(Ray ray)
        {
            float dist;
            return IntersectRayAABB(ray, this, out dist);
        }

        // Does /ray/ intersect this bounding box?
        public bool IntersectRay(Ray ray, out float distance)
        {
            return IntersectRayAABB(ray, this, out distance);
        }

        private static Vector3 Internal_GetClosestPoint(Bounds bounds, ref Vector3 point)
        {
            Vector3 outPoint;
            float outSqrDistance;
            CalculateClosestPoint(point, bounds, out outPoint, out outSqrDistance);
            return outPoint;
        }

        public Vector3 ClosestPoint(Vector3 point)
        {
            return Internal_GetClosestPoint(this, ref point);
        }

        /// *listonly*
        override public string ToString()
        {
            return String.Format("Center: {0}, Extents: {1}", m_Center, m_Extents);
        }
        // Returns a nicely formatted string for the bounds.
        public string ToString(string format)
        {
            return String.Format("Center: {0}, Extents: {1}", m_Center.ToString(format), m_Extents.ToString(format));
        }

        bool IsInside(Vector3 inPoint)
        {
            if (inPoint[0] < m_Center[0] - m_Extents[0])
                return false;
            if (inPoint[0] > m_Center[0] + m_Extents[0])
                return false;

            if (inPoint[1] < m_Center[1] - m_Extents[1])
                return false;
            if (inPoint[1] > m_Center[1] + m_Extents[1])
                return false;

            if (inPoint[2] < m_Center[2] - m_Extents[2])
                return false;
            if (inPoint[2] > m_Center[2] + m_Extents[2])
                return false;

            return true;
        }

        static float CalculateSqrDistance(Vector3 rkPoint, Bounds rkBox)
        {
            Vector3 closest = rkPoint - rkBox.GetCenter();
            float sqrDistance = 0.0f;

            for (int i = 0; i < 3; ++i)
            {
                float clos = closest[i];
                float ext = rkBox.GetExtent()[i];
                if (clos < -ext)
                {
                    float delta = clos + ext;
                    sqrDistance += delta * delta;
                    closest[i] = -ext;
                }
                else if (clos > ext)
                {
                    float delta = clos - ext;
                    sqrDistance += delta * delta;
                    closest[i] = ext;
                }
            }

            return sqrDistance;
        }

        static void CalculateClosestPoint(Vector3 rkPoint, Bounds rkBox, out Vector3 outPoint, out float outSqrDistance)
        {
            // compute coordinates of point in box coordinate system
            Vector3 kClosest = rkPoint - rkBox.GetCenter();

            // project test point onto box
            float fSqrDistance = 0.0f;
            float fDelta;

            for (int i = 0; i < 3; i++)
            {
                if (kClosest[i] < -rkBox.GetExtent()[i])
                {
                    fDelta = kClosest[i] + rkBox.GetExtent()[i];
                    fSqrDistance += fDelta * fDelta;
                    kClosest[i] = -rkBox.GetExtent()[i];
                }
                else if (kClosest[i] > rkBox.GetExtent()[i])
                {
                    fDelta = kClosest[i] - rkBox.GetExtent()[i];
                    fSqrDistance += fDelta * fDelta;
                    kClosest[i] = rkBox.GetExtent()[i];
                }
            }

            // Inside
            if (fSqrDistance == 0.0F)
            {
                outPoint = rkPoint;
                outSqrDistance = 0.0F;
            }
            // Outside
            else
            {
                outPoint = kClosest + rkBox.GetCenter();
                outSqrDistance = fSqrDistance;
            }
        }


        static bool IntersectRayAABB(Ray ray, Bounds inAABB)
        {
            float t0, t1;
            return IntersectRayAABB(ray, inAABB, out t0, out t1);
        }

        static bool IntersectRayAABB(Ray ray, Bounds inAABB, out float outT0)
        {
            float t1;
            return IntersectRayAABB(ray, inAABB, out outT0, out t1);
        }

        static bool IntersectRayAABB(Ray ray, Bounds inAABB, out float outT0, out float outT1)
        {
            outT0 = 0; outT1 = 0;
            float tmin = -Mathf.Infinity;
            float tmax = Mathf.Infinity;

            float t0, t1, f;

            Vector3 p = inAABB.GetCenter() - ray.GetOrigin();
            Vector3 extent = inAABB.GetExtent();
            int i;
            for (i = 0; i < 3; i++)
            {
                // ray and plane are paralell so no valid intersection can be found
                {
                    f = 1.0F / ray.GetDirection()[i];
                    t0 = (p[i] + extent[i]) * f;
                    t1 = (p[i] - extent[i]) * f;
                    // Ray leaves on Right, Top, Back Side
                    if (t0 < t1)
                    {
                        if (t0 > tmin)
                            tmin = t0;

                        if (t1 < tmax)
                            tmax = t1;

                        if (tmin > tmax)
                            return false;

                        if (tmax < 0.0F)
                            return false;
                    }
                    // Ray leaves on Left, Bottom, Front Side
                    else
                    {
                        if (t1 > tmin)
                            tmin = t1;

                        if (t0 < tmax)
                            tmax = t0;

                        if (tmin > tmax)
                            return false;

                        if (tmax < 0.0F)
                            return false;
                    }
                }
            }

            outT0 = tmin;
            outT1 = tmax;

            return true;
        }

    }

}