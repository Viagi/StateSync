using System;

namespace UnityEngine
{

    public struct Vector3
    {

        // *undocumented*
        public const float kEpsilon = 0.00001F;

        // X component of the vector.
        public float x;
        // Y component of the vector.
        public float y;
        // Z component of the vector.
        public float z;

        public static Vector3 infinity = new Vector3(Mathf.Infinity, Mathf.Infinity, Mathf.Infinity);

        // Linearly interpolates between two vectors.
        public static Vector3 Lerp(Vector3 from, Vector3 to, float t)
        {
            t = Mathf.Clamp01(t);
            return new Vector3(
                from.x + (to.x - from.x) * t,
                from.y + (to.y - from.y) * t,
                from.z + (to.z - from.z) * t
            );
        }

        // Spherically interpolates between two vectors.
        static Vector3 Slerp(Vector3 from, Vector3 to, float t) { return Slerp(from, to, Mathf.Clamp01(t)); }


        private static void Internal_OrthoNormalize2(ref Vector3 a, ref Vector3 b) { OrthoNormalize(ref a, ref b); }
        private static void Internal_OrthoNormalize3(ref Vector3 a, ref Vector3 b, ref Vector3 c) { OrthoNormalize(ref a, ref b, ref c); }

        // Makes vectors normalized and orthogonal to each other.
        static public void OrthoNormalize(ref Vector3 normal, ref Vector3 tangent) { Internal_OrthoNormalize2(ref normal, ref tangent); }
        // Makes vectors normalized and orthogonal to each other.
        static public void OrthoNormalize(ref Vector3 normal, ref Vector3 tangent, ref Vector3 binormal) { Internal_OrthoNormalize3(ref normal, ref tangent, ref binormal); }


        // Moves a point /current/ in a straight line towards a /target/ point.
        static public Vector3 MoveTowards(Vector3 current, Vector3 target, float maxDistanceDelta)
        {
            Vector3 toVector = target - current;
            float dist = toVector.magnitude;
            if (dist <= maxDistanceDelta || dist == 0)
                return target;
            return current + toVector / dist * maxDistanceDelta;
        }

        // Rotates a vector /current/ towards /target/.
        static Vector3 RotateTowards(Vector3 current, Vector3 target, float maxRadiansDelta, float maxMagnitudeDelta) { return RotateTowards(current, target, maxRadiansDelta, maxMagnitudeDelta); }

        // Gradually changes a vector towards a desired goal over time.
        public static Vector3 SmoothDamp(Vector3 current, Vector3 target, ref Vector3 currentVelocity, float smoothTime, float maxSpeed = Mathf.Infinity, float deltaTime = 0.02F)
        {
            // Based on Game Programming Gems 4 Chapter 1.10
            smoothTime = Mathf.Max(0.0001F, smoothTime);
            float omega = 2F / smoothTime;

            float x = omega * deltaTime;
            float exp = 1F / (1F + x + 0.48F * x * x + 0.235F * x * x * x);
            Vector3 change = current - target;
            Vector3 originalTo = target;

            // Clamp maximum speed
            float maxChange = maxSpeed * smoothTime;
            change = Vector3.ClampMagnitude(change, maxChange);
            target = current - change;

            Vector3 temp = (currentVelocity + omega * change) * deltaTime;
            currentVelocity = (currentVelocity - omega * temp) * exp;
            Vector3 output = target + (change + temp) * exp;

            // Prevent overshooting
            if (Vector3.Dot(originalTo - current, output - originalTo) > 0)
            {
                output = originalTo;
                currentVelocity = (output - originalTo) / deltaTime;
            }

            return output;
        }

        // Access the x, y, z components using [0], [1], [2] respectively.
        public float this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: return x;
                    case 1: return y;
                    case 2: return z;
                    default:
                        throw new IndexOutOfRangeException("Invalid Vector3 index!");
                }
            }

            set
            {
                switch (index)
                {
                    case 0: x = value; break;
                    case 1: y = value; break;
                    case 2: z = value; break;
                    default:
                        throw new IndexOutOfRangeException("Invalid Vector3 index!");
                }
            }
        }

        // Creates a new vector with given x, y, z components.
        public Vector3(float x, float y, float z) { this.x = x; this.y = y; this.z = z; }
        // Creates a new vector with given x, y components and sets /z/ to zero.
        public Vector3(float x, float y) { this.x = x; this.y = y; z = 0F; }

        // Set x, y and z components of an existing Vector3.
        public void Set(float new_x, float new_y, float new_z) { x = new_x; y = new_y; z = new_z; }

        // Multiplies two vectors component-wise.
        public static Vector3 Scale(Vector3 a, Vector3 b) { return new Vector3(a.x * b.x, a.y * b.y, a.z * b.z); }

        // Multiplies every component of this vector by the same component of /scale/.
        public void Scale(Vector3 scale) { x *= scale.x; y *= scale.y; z *= scale.z; }

        // Cross Product of two vectors.
        public static Vector3 Cross(Vector3 lhs, Vector3 rhs)
        {
            return new Vector3(
            lhs.y * rhs.z - lhs.z * rhs.y,
            lhs.z * rhs.x - lhs.x * rhs.z,
            lhs.x * rhs.y - lhs.y * rhs.x);
        }

        // used to allow Vector3s to be used as keys in hash tables
        public override int GetHashCode()
        {
            return x.GetHashCode() ^ (y.GetHashCode() << 2) ^ (z.GetHashCode() >> 2);
        }

        // also required for being able to use Vector3s as keys in hash tables
        public override bool Equals(object other)
        {
            if (!(other is Vector3)) return false;

            Vector3 rhs = (Vector3)other;
            return x.Equals(rhs.x) && y.Equals(rhs.y) && z.Equals(rhs.z);
        }

        // Reflects a vector off the plane defined by a normal.
        public static Vector3 Reflect(Vector3 inDirection, Vector3 inNormal)
        {
            return -2F * Dot(inNormal, inDirection) * inNormal + inDirection;
        }

        // *undoc* --- we have normalized property now
        public static Vector3 Normalize(Vector3 value)
        {
            float mag = Magnitude(value);
            if (mag > kEpsilon)
                return value / mag;
            else
                return zero;
        }

        // Makes this vector have a ::ref::magnitude of 1.
        public void Normalize()
        {
            float mag = Magnitude(this);
            if (mag > kEpsilon)
                this = this / mag;
            else
                this = zero;
        }

        // Returns this vector with a ::ref::magnitude of 1 (RO).
        public Vector3 normalized { get { return Vector3.Normalize(this); } }

        /// *listonly*
        override public string ToString() { return String.Format("({0:F1}, {1:F1}, {2:F1})", x, y, z); }
        // Returns a nicely formatted string for this vector.
        public string ToString(string format)
        {
            return String.Format("({0}, {1}, {2})", x.ToString(format), y.ToString(format), z.ToString(format));
        }

        // Dot Product of two vectors.
        public static float Dot(Vector3 lhs, Vector3 rhs) { return lhs.x * rhs.x + lhs.y * rhs.y + lhs.z * rhs.z; }

        // Projects a vector onto another vector.
        public static Vector3 Project(Vector3 vector, Vector3 onNormal)
        {
            float sqrMag = Dot(onNormal, onNormal);
            if (sqrMag < Mathf.Epsilon)
                return zero;
            else
                return onNormal * Dot(vector, onNormal) / sqrMag;
        }

        //*undocumented* --------------------------- TODO is this generally useful? What is the intention? I know i understood it once upon a time but it evaded my mind.
        public static Vector3 Exclude(Vector3 excludeThis, Vector3 fromThat)
        {
            return fromThat - Project(fromThat, excludeThis);
        }

        // Returns the angle in degrees between /from/ and /to/. This is always the smallest
        public static float Angle(Vector3 from, Vector3 to) { return Mathf.Acos(Mathf.Clamp(Vector3.Dot(from.normalized, to.normalized), -1F, 1F)) * Mathf.Rad2Deg; }

        // Returns the distance between /a/ and /b/.
        public static float Distance(Vector3 a, Vector3 b) { Vector3 vec = new Vector3(a.x - b.x, a.y - b.y, a.z - b.z); return Mathf.Sqrt(vec.x * vec.x + vec.y * vec.y + vec.z * vec.z); }

        // Returns a copy of /vector/ with its magnitude clamped to /maxLength/.
        public static Vector3 ClampMagnitude(Vector3 vector, float maxLength)
        {
            if (vector.sqrMagnitude > maxLength * maxLength)
                return vector.normalized * maxLength;
            return vector;
        }

        // *undoc* --- there's a property now
        public static float Magnitude(Vector3 a) { return Mathf.Sqrt(a.x * a.x + a.y * a.y + a.z * a.z); }

        // Returns the length of this vector (RO).
        public float magnitude { get { return Mathf.Sqrt(x * x + y * y + z * z); } }

        // *undoc* --- there's a property now
        public static float SqrMagnitude(Vector3 a) { return a.x * a.x + a.y * a.y + a.z * a.z; }

        // Returns the squared length of this vector (RO).
        public float sqrMagnitude { get { return x * x + y * y + z * z; } }


        // Returns a vector that is made from the smallest components of two vectors.
        public static Vector3 Min(Vector3 lhs, Vector3 rhs) { return new Vector3(Mathf.Min(lhs.x, rhs.x), Mathf.Min(lhs.y, rhs.y), Mathf.Min(lhs.z, rhs.z)); }

        // Returns a vector that is made from the largest components of two vectors.
        public static Vector3 Max(Vector3 lhs, Vector3 rhs) { return new Vector3(Mathf.Max(lhs.x, rhs.x), Mathf.Max(lhs.y, rhs.y), Mathf.Max(lhs.z, rhs.z)); }

        // Shorthand for writing @@Vector3(0, 0, 0)@@
        public static Vector3 zero { get { return new Vector3(0F, 0F, 0F); } }
        // Shorthand for writing @@Vector3(1, 1, 1)@@
        public static Vector3 one { get { return new Vector3(1F, 1F, 1F); } }
        // Shorthand for writing @@Vector3(0, 0, 1)@@
        public static Vector3 forward { get { return new Vector3(0F, 0F, 1F); } }
        public static Vector3 back { get { return new Vector3(0F, 0F, -1F); } }
        // Shorthand for writing @@Vector3(0, 1, 0)@@
        public static Vector3 up { get { return new Vector3(0F, 1F, 0F); } }
        public static Vector3 down { get { return new Vector3(0F, -1F, 0F); } }
        public static Vector3 left { get { return new Vector3(-1F, 0F, 0F); } }
        // Shorthand for writing @@Vector3(1, 0, 0)@@
        public static Vector3 right { get { return new Vector3(1F, 0F, 0F); } }


        // Adds two vectors.
        public static Vector3 operator +(Vector3 a, Vector3 b) { return new Vector3(a.x + b.x, a.y + b.y, a.z + b.z); }
        // Subtracts one vector from another.
        public static Vector3 operator -(Vector3 a, Vector3 b) { return new Vector3(a.x - b.x, a.y - b.y, a.z - b.z); }
        // Negates a vector.
        public static Vector3 operator -(Vector3 a) { return new Vector3(-a.x, -a.y, -a.z); }
        // Multiplies a vector by a number.
        public static Vector3 operator *(Vector3 a, float d) { return new Vector3(a.x * d, a.y * d, a.z * d); }
        // Multiplies a vector by a number.
        public static Vector3 operator *(float d, Vector3 a) { return new Vector3(a.x * d, a.y * d, a.z * d); }
        // Divides a vector by a number.
        public static Vector3 operator /(Vector3 a, float d) { return new Vector3(a.x / d, a.y / d, a.z / d); }

        // Returns true if the vectors are equal.
        public static bool operator ==(Vector3 lhs, Vector3 rhs)
        {
            return SqrMagnitude(lhs - rhs) < kEpsilon * kEpsilon;
        }

        // Returns true if vectors different.
        public static bool operator !=(Vector3 lhs, Vector3 rhs)
        {
            return SqrMagnitude(lhs - rhs) >= kEpsilon * kEpsilon;
        }

        public static Vector3 fwd { get { return new Vector3(0F, 0F, 1F); } }

        public static float AngleBetween(Vector3 from, Vector3 to) { return Mathf.Acos(Mathf.Clamp(Vector3.Dot(from.normalized, to.normalized), -1F, 1F)); }

        internal bool IsFinate()
        {
            return Mathf.IsValid(x) && Mathf.IsValid(y) && Mathf.IsValid(z);
        }
    }
}

