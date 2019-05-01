using System;

namespace UnityEngine
{

    public struct Vector2
    {
        // X component of the vector.
        public float x;
        // Y component of the vector.
        public float y;

        // Access the /x/ or /y/ component using [0] or [1] respectively.
        public float this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: return x;
                    case 1: return y;
                    default:
                        throw new IndexOutOfRangeException("Invalid Vector2 index!");
                }
            }

            set
            {
                switch (index)
                {
                    case 0: x = value; break;
                    case 1: y = value; break;
                    default:
                        throw new IndexOutOfRangeException("Invalid Vector2 index!");
                }
            }
        }

        // Constructs a new vector with given x, y components.
        public Vector2(float x, float y) { this.x = x; this.y = y; }

        // Set x and y components of an existing Vector2.
        public void Set(float new_x, float new_y) { x = new_x; y = new_y; }

        // Linearly interpolates between two vectors.
        public static Vector2 Lerp(Vector2 from, Vector2 to, float t)
        {
            t = Mathf.Clamp01(t);
            return new Vector2(
                from.x + (to.x - from.x) * t,
                from.y + (to.y - from.y) * t
            );
        }

        // Moves a point /current/ towards /target/.
        static public Vector2 MoveTowards(Vector2 current, Vector2 target, float maxDistanceDelta)
        {
            Vector2 toVector = target - current;
            float dist = toVector.magnitude;
            if (dist <= maxDistanceDelta || dist == 0)
                return target;
            return current + toVector / dist * maxDistanceDelta;
        }

        // Multiplies two vectors component-wise.
        public static Vector2 Scale(Vector2 a, Vector2 b) { return new Vector2(a.x * b.x, a.y * b.y); }

        // Multiplies every component of this vector by the same component of /scale/.
        public void Scale(Vector2 scale) { x *= scale.x; y *= scale.y; }

        // Makes this vector have a ::ref::magnitude of 1.
        public void Normalize()
        {
            float mag = this.magnitude;
            if (mag > kEpsilon)
                this = this / mag;
            else
                this = zero;
        }

        // Returns this vector with a ::ref::magnitude of 1 (RO).
        public Vector2 normalized
        {
            get
            {
                Vector2 v = new Vector2(x, y);
                v.Normalize();
                return v;
            }
        }

        /// *listonly*
        override public string ToString() { return String.Format("({0:F1}, {1:F1})", x, y); }
        // Returns a nicely formatted string for this vector.
        public string ToString(string format)
        {
            return String.Format("({0}, {1})", x.ToString(format), y.ToString(format));
        }

        // used to allow Vector2s to be used as keys in hash tables
        public override int GetHashCode()
        {
            return x.GetHashCode() ^ (y.GetHashCode() << 2);
        }

        // also required for being able to use Vector2s as keys in hash tables
        public override bool Equals(object other)
        {
            if (!(other is Vector2)) return false;

            Vector2 rhs = (Vector2)other;
            return x.Equals(rhs.x) && y.Equals(rhs.y);
        }

        // Dot Product of two vectors.
        public static float Dot(Vector2 lhs, Vector2 rhs) { return lhs.x * rhs.x + lhs.y * rhs.y; }

        // Returns the length of this vector (RO).
        public float magnitude { get { return Mathf.Sqrt(x * x + y * y); } }
        // Returns the squared length of this vector (RO).
        public float sqrMagnitude { get { return x * x + y * y; } }

        // Returns the angle in degrees between /from/ and /to/.
        public static float Angle(Vector2 from, Vector2 to) { return Mathf.Acos(Mathf.Clamp(Vector2.Dot(from.normalized, to.normalized), -1F, 1F)) * Mathf.Rad2Deg; }

        // Returns the distance between /a/ and /b/.
        public static float Distance(Vector2 a, Vector2 b) { return (a - b).magnitude; }

        // Returns a copy of /vector/ with its magnitude clamped to /maxLength/.
        public static Vector2 ClampMagnitude(Vector2 vector, float maxLength)
        {
            if (vector.sqrMagnitude > maxLength * maxLength)
                return vector.normalized * maxLength;
            return vector;
        }

        public static float SqrMagnitude(Vector2 a) { return a.x * a.x + a.y * a.y; }
        public float SqrMagnitude() { return x * x + y * y; }

        // Returns a vector that is made from the smallest components of two vectors.
        public static Vector2 Min(Vector2 lhs, Vector2 rhs) { return new Vector2(Mathf.Min(lhs.x, rhs.x), Mathf.Min(lhs.y, rhs.y)); }

        // Returns a vector that is made from the largest components of two vectors.
        public static Vector2 Max(Vector2 lhs, Vector2 rhs) { return new Vector2(Mathf.Max(lhs.x, rhs.x), Mathf.Max(lhs.y, rhs.y)); }

        // Adds two vectors.
        public static Vector2 operator +(Vector2 a, Vector2 b) { return new Vector2(a.x + b.x, a.y + b.y); }
        // Subtracts one vector from another.
        public static Vector2 operator -(Vector2 a, Vector2 b) { return new Vector2(a.x - b.x, a.y - b.y); }
        // Negates a vector.
        public static Vector2 operator -(Vector2 a) { return new Vector2(-a.x, -a.y); }
        // Multiplies a vector by a number.
        public static Vector2 operator *(Vector2 a, float d) { return new Vector2(a.x * d, a.y * d); }
        // Multiplies a vector by a number.
        public static Vector2 operator *(float d, Vector2 a) { return new Vector2(a.x * d, a.y * d); }
        // Divides a vector by a number.
        public static Vector2 operator /(Vector2 a, float d) { return new Vector2(a.x / d, a.y / d); }
        // Returns true if the vectors are equal.
        public static bool operator ==(Vector2 lhs, Vector2 rhs)
        {
            return SqrMagnitude(lhs - rhs) < kEpsilon * kEpsilon;
        }
        // Returns true if vectors different.
        public static bool operator !=(Vector2 lhs, Vector2 rhs)
        {
            return SqrMagnitude(lhs - rhs) >= kEpsilon * kEpsilon;
        }

        // Converts a [[Vector3]] to a Vector2.
        public static implicit operator Vector2(Vector3 v)
        {
            return new Vector2(v.x, v.y);
        }
        // Converts a Vector2 to a [[Vector3]].
        public static implicit operator Vector3(Vector2 v)
        {
            return new Vector3(v.x, v.y, 0);
        }

        // Shorthand for writing @@Vector2(0, 0)@@
        public static Vector2 zero { get { return new Vector2(0.0F, 0.0F); } }
        // Shorthand for writing @@Vector2(1, 1)@@
        public static Vector2 one { get { return new Vector2(1.0F, 1.0F); } }
        // Shorthand for writing @@Vector2(0, 1)@@
        public static Vector2 up { get { return new Vector2(0.0F, 1.0F); } }
        // Shorthand for writing @@Vector2(1, 0)@@
        public static Vector2 right { get { return new Vector2(1.0F, 0.0F); } }

        // *Undocumented*
        public const float kEpsilon = 0.00001F;
    }
}

