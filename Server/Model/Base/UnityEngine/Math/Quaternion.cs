using System;


namespace UnityEngine
{

    public struct Quaternion
    {
        // X component of the Quaternion. Don't modify this directly unless you know quaternions inside out.
        public float x;
        // Y component of the Quaternion. Don't modify this directly unless you know quaternions inside out.
        public float y;
        // Z component of the Quaternion. Don't modify this directly unless you know quaternions inside out.
        public float z;
        // W component of the Quaternion. Don't modify this directly unless you know quaternions inside out.
        public float w;

        // Access the x, y, z, w components using [0], [1], [2], [3] respectively.
        public float this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: return x;
                    case 1: return y;
                    case 2: return z;
                    case 3: return w;
                    default:
                        throw new IndexOutOfRangeException("Invalid Quaternion index!");
                }
            }

            set
            {
                switch (index)
                {
                    case 0: x = value; break;
                    case 1: y = value; break;
                    case 2: z = value; break;
                    case 3: w = value; break;
                    default:
                        throw new IndexOutOfRangeException("Invalid Quaternion index!");
                }
            }
        }


        // Constructs new Quaternion with given x,y,z,w components.
        public Quaternion(float x, float y, float z, float w) { this.x = x; this.y = y; this.z = z; this.w = w; }

        // Set x, y, z and w components of an existing Quaternion.
        public void Set(float new_x, float new_y, float new_z, float new_w) { x = new_x; y = new_y; z = new_z; w = new_w; }

        // The identity rotation (RO). This quaternion corresponds to "no rotation": the object
        public static Quaternion identity { get { return new Quaternion(0F, 0F, 0F, 1F); } }

        // Combines rotations /lhs/ and /rhs/.
        public static Quaternion operator *(Quaternion lhs, Quaternion rhs)
        {
            return new Quaternion(
                    lhs.w * rhs.x + lhs.x * rhs.w + lhs.y * rhs.z - lhs.z * rhs.y,
                    lhs.w * rhs.y + lhs.y * rhs.w + lhs.z * rhs.x - lhs.x * rhs.z,
                    lhs.w * rhs.z + lhs.z * rhs.w + lhs.x * rhs.y - lhs.y * rhs.x,
                    lhs.w * rhs.w - lhs.x * rhs.x - lhs.y * rhs.y - lhs.z * rhs.z);
        }

        // Rotates the point /point/ with /rotation/.
        public static Vector3 operator *(Quaternion rotation, Vector3 point)
        {
            float x = rotation.x * 2F;
            float y = rotation.y * 2F;
            float z = rotation.z * 2F;
            float xx = rotation.x * x;
            float yy = rotation.y * y;
            float zz = rotation.z * z;
            float xy = rotation.x * y;
            float xz = rotation.x * z;
            float yz = rotation.y * z;
            float wx = rotation.w * x;
            float wy = rotation.w * y;
            float wz = rotation.w * z;

            Vector3 res;
            res.x = (1F - (yy + zz)) * point.x + (xy - wz) * point.y + (xz + wy) * point.z;
            res.y = (xy + wz) * point.x + (1F - (xx + zz)) * point.y + (yz - wx) * point.z;
            res.z = (xz - wy) * point.x + (yz + wx) * point.y + (1F - (xx + yy)) * point.z;
            return res;
        }

        // *undocumented*
        public const float kEpsilon = 0.000001F;

        // Are two quaternions equal to each other?
        public static bool operator ==(Quaternion lhs, Quaternion rhs)
        {
            return Dot(lhs, rhs) > 1.0f - kEpsilon;
        }

        // Are two quaternions different from each other?
        public static bool operator !=(Quaternion lhs, Quaternion rhs)
        {
            return Dot(lhs, rhs) <= 1.0f - kEpsilon;
        }

        // The dot product between two rotations.
        public static float Dot(Quaternion a, Quaternion b) { return a.x * b.x + a.y * b.y + a.z * b.z + a.w * b.w; }

        // Creates a rotation which rotates /angle/ degrees around /axis/.
        public static Quaternion AngleAxis(float angle, Vector3 axis) { return AxisAngleToQuaternionSafe(axis, Mathf.Deg2Rad * angle); }

        // Converts a rotation to angle-axis representation.
        public void ToAngleAxis(out float angle, out Vector3 axis) { Internal_ToAxisAngleRad(this, out axis, out angle); angle *= Mathf.Rad2Deg; }

        // Creates a rotation which rotates from /fromDirection/ to /toDirection/.
        public static Quaternion FromToRotation(Vector3 fromDirection, Vector3 toDirection) { return FromToQuaternionSafe(fromDirection, toDirection); }

        // Creates a rotation which rotates from /fromDirection/ to /toDirection/.
        public void SetFromToRotation(Vector3 fromDirection, Vector3 toDirection) { this = FromToRotation(fromDirection, toDirection); }

        // Creates a rotation with the specified /forward/ and /upwards/ directions.
        public static Quaternion LookRotation(Vector3 forward, Vector3 upwards)
        {
            Quaternion q = Quaternion.identity;
            if (!LookRotationToQuaternion(forward, upwards, ref q))
            {
                float mag = forward.magnitude;
                if (mag > Vector3.kEpsilon)
                {
                    Matrix4x4 m = new Matrix4x4();
                    m.SetFromToRotation(Vector3.forward, forward / mag);
                    MatrixToQuaternion(m, ref q);
                }
            }
            return q;
        }
        public static Quaternion LookRotation(Vector3 forward)
        {
            return LookRotation(forward, Vector3.up);
        }

        // Creates a rotation with the specified /forward/ and /upwards/ directions.
        public void SetLookRotation(Vector3 view, Vector3 up) { this = LookRotation(view, up); }
        public void SetLookRotation(Vector3 view) { SetLookRotation(view, Vector3.up); }

        // Spherically interpolates between /from/ and /to/ by t.
        public static Quaternion Slerp(Quaternion from, Quaternion to, float t) { return _Slerp(from, to, Mathf.Clamp01(t)); }

        // Interpolates between /from/ and /to/ by /t/ and normalizes the result afterwards.
        public static Quaternion Lerp(Quaternion from, Quaternion to, float t) { return _Lerp(from, to, Mathf.Clamp01(t)); }

        // Rotates a rotation /from/ towards /to/.
        public static Quaternion RotateTowards(Quaternion from, Quaternion to, float maxDegreesDelta)
        {
            float angle = Quaternion.Angle(from, to);
            if (angle == 0.0f)
                return to;
            float slerpValue = Mathf.Min(1.0f, maxDegreesDelta / angle);
            return UnclampedSlerp(from, to, slerpValue);
        }

        private static Quaternion UnclampedSlerp(Quaternion from, Quaternion to, float t) { return _Slerp(from, to, t); }

        // Returns the Inverse of /rotation/.
        public static Quaternion Inverse(Quaternion q)
        {
            return new Quaternion(-q.x, -q.y, -q.z, q.w);
        }


        /// *listonly*
        override public string ToString()
        {
            return String.Format("({0:F1}, {1:F1}, {2:F1}, {3:F1})", x, y, z, w);
        }
        // Returns a nicely formatted string of the Quaternion
        public string ToString(string format)
        {
            return String.Format("({0}, {1}, {2}, {3})", x.ToString(format), y.ToString(format), z.ToString(format), w.ToString(format));
        }

        // Returns the angle in degrees between two rotations /a/ and /b/.
        static public float Angle(Quaternion a, Quaternion b)
        {
            float dot = Dot(a, b);
            return Mathf.Acos(Mathf.Min(Mathf.Abs(dot), 1.0F)) * 2.0F * Mathf.Rad2Deg;
        }

        // Returns the euler angle representation of the rotation.
        public Vector3 eulerAngles { get { return Internal_ToEulerRad(this) * Mathf.Rad2Deg; } set { this = Internal_FromEulerRad(value * Mathf.Deg2Rad); } }

        // Returns a rotation that rotates z degrees around the z axis, x degrees around the x axis, and y degrees around the y axis (in that order).
        static public Quaternion Euler(float x, float y, float z) { return Internal_FromEulerRad(new Vector3(x, y, z) * Mathf.Deg2Rad); }

        // Returns a rotation that rotates z degrees around the z axis, x degrees around the x axis, and y degrees around the y axis (in that order).
        static public Quaternion Euler(Vector3 euler) { return Internal_FromEulerRad(euler * Mathf.Deg2Rad); }

        // Internal implementation. Note Rad suffix that indicates that this method works in radians.
        static internal Vector3 Internal_ToEulerRad(Quaternion rotation)
        {
            Quaternion outRotation = NormalizeSafe(rotation);
            return QuaternionToEuler(outRotation);
        }

        // Internal implementation. Note Rad suffix that indicates that this method works in radians.
        static internal Quaternion Internal_FromEulerRad(Vector3 euler)
        {
            return EulerToQuaternion(euler);
        }

        // Internal implementation. Note Rad suffix that indicates that this method works in radians.
        private static void Internal_ToAxisAngleRad(Quaternion q, out Vector3 axis, out float angle)
        {
            QuaternionToAxisAngle(NormalizeSafe(q), out axis, out angle);
        }

        static public Quaternion EulerRotation(float x, float y, float z) { return Internal_FromEulerRad(new Vector3(x, y, z)); }
        public static Quaternion EulerRotation(Vector3 euler) { return Internal_FromEulerRad(euler); }
        public void SetEulerRotation(float x, float y, float z) { this = Internal_FromEulerRad(new Vector3(x, y, z)); }
        public void SetEulerRotation(Vector3 euler) { this = Internal_FromEulerRad(euler); }
        public Vector3 ToEuler() { return Internal_ToEulerRad(this); }

        static public Quaternion EulerAngles(float x, float y, float z) { return Internal_FromEulerRad(new Vector3(x, y, z)); }
        public static Quaternion EulerAngles(Vector3 euler) { return Internal_FromEulerRad(euler); }

        public void ToAxisAngle(out Vector3 axis, out float angle) { Internal_ToAxisAngleRad(this, out axis, out angle); }

        public void SetEulerAngles(float x, float y, float z) { SetEulerRotation(new Vector3(x, y, z)); }
        public void SetEulerAngles(Vector3 euler) { this = EulerRotation(euler); }
        public static Vector3 ToEulerAngles(Quaternion rotation) { return Quaternion.Internal_ToEulerRad(rotation); }
        public Vector3 ToEulerAngles() { return Quaternion.Internal_ToEulerRad(this); }

        static Quaternion AxisAngle(Vector3 axis, float angle) { return AxisAngleToQuaternionSafe(axis, angle); }

        public void SetAxisAngle(Vector3 axis, float angle) { this = AxisAngle(axis, angle); }

        // used to allow Quaternions to be used as keys in hash tables
        public override int GetHashCode()
        {
            return x.GetHashCode() ^ (y.GetHashCode() << 2) ^ (z.GetHashCode() >> 2) ^ (w.GetHashCode() >> 1);
        }

        // also required for being able to use Quaternions as keys in hash tables
        public override bool Equals(object other)
        {
            if (!(other is Quaternion)) return false;

            Quaternion rhs = (Quaternion)other;
            return x.Equals(rhs.x) && y.Equals(rhs.y) && z.Equals(rhs.z) && w.Equals(rhs.w);
        }

        static Quaternion _Slerp(Quaternion q1, Quaternion q2, float t)
        {
            float dot = Dot(q1, q2);

            // dot = cos(theta)
            // if (dot < 0), q1 and q2 are more than 90 degrees apart,
            // so we can invert one to reduce spinning
            Quaternion tmpQuat = new Quaternion();
            if (dot < 0.0f)
            {
                dot = -dot;
                tmpQuat.Set(-q2.x,
                    -q2.y,
                    -q2.z,
                    -q2.w);
            }
            else
                tmpQuat = q2;


            if (dot < 0.95f)
            {
                float angle = Mathf.Acos(dot);
                float sinadiv, sinat, sinaomt;
                sinadiv = 1.0f / Mathf.Sin(angle);
                sinat = Mathf.Sin(angle * t);
                sinaomt = Mathf.Sin(angle * (1.0f - t));
                tmpQuat.Set((q1.x * sinaomt + tmpQuat.x * sinat) * sinadiv,
                    (q1.y * sinaomt + tmpQuat.y * sinat) * sinadiv,
                    (q1.z * sinaomt + tmpQuat.z * sinat) * sinadiv,
                    (q1.w * sinaomt + tmpQuat.w * sinat) * sinadiv);
                return tmpQuat;

            }
            // if the angle is small, use linear interpolation
            else
            {
                return _Lerp(q1, tmpQuat, t);
            }

        }

        static Quaternion _Lerp(Quaternion q1, Quaternion q2, float t)
        {
            Quaternion tmpQuat = new Quaternion();
            // if (dot < 0), q1 and q2 are more than 360 deg apart.
            // The problem is that quaternions are 720deg of freedom.
            // so we - all components when lerping
            if (Dot(q1, q2) < 0.0F)
            {
                tmpQuat.Set(q1.x + t * (-q2.x - q1.x),
                    q1.y + t * (-q2.y - q1.y),
                    q1.z + t * (-q2.z - q1.z),
                    q1.w + t * (-q2.w - q1.w));
            }
            else
            {
                tmpQuat.Set(q1.x + t * (q2.x - q1.x),
                    q1.y + t * (q2.y - q1.y),
                    q1.z + t * (q2.z - q1.z),
                    q1.w + t * (q2.w - q1.w));
            }
            return Normalize(tmpQuat);
        }

        static float AngularDistance(Quaternion lhs, Quaternion rhs)
        {
            float dot = Dot(lhs, rhs);
            if (dot < 0.0f)
                dot = -dot;
            return Mathf.Acos(Mathf.Min(1.0F, dot)) * 2.0F;
        }

        internal static Quaternion EulerToQuaternion(Vector3 someEulerAngles)
        {
            float cX = (Mathf.Cos(someEulerAngles.x / 2.0f));
            float sX = (Mathf.Sin(someEulerAngles.x / 2.0f));

            float cY = (Mathf.Cos(someEulerAngles.y / 2.0f));
            float sY = (Mathf.Sin(someEulerAngles.y / 2.0f));

            float cZ = (Mathf.Cos(someEulerAngles.z / 2.0f));
            float sZ = (Mathf.Sin(someEulerAngles.z / 2.0f));

            Quaternion qX = new Quaternion(sX, 0.0F, 0.0F, cX);
            Quaternion qY = new Quaternion(0.0F, sY, 0.0F, cY);
            Quaternion qZ = new Quaternion(0.0F, 0.0F, sZ, cZ);

            Quaternion q = (qY * qX) * qZ;
            //			Assert (CompareApproximately (SqrMagnitude (q), 1.0F));
            return q;
        }

        internal static Vector3 QuaternionToEuler(Quaternion quat)
        {
            Matrix4x4 m = new Matrix4x4();
            Vector3 rot = new Vector3();
            QuaternionToMatrix(quat, ref m);
            MatrixToEuler(m, ref rot);
            return rot;
        }

        public static void QuaternionToMatrix(Quaternion q, ref Matrix4x4 m)
        {
            // If q is guaranteed to be a unit quaternion, s will always
            // be 1.  In that case, this calculation can be optimized out.
#if DEBUGMODE
			if (!CompareApproximately (SqrMagnitude (q), 1.0F, Vector3f::epsilon))
			{
			AssertString(Format("Quaternion To Matrix conversion failed because input Quaternion is invalid {%f, %f, %f, %f} l=%f", q.x, q.y, q.z, q.w, SqrMagnitude(q)));
			}
#endif

            // Precalculate coordinate products
            float x = q.x * 2.0F;
            float y = q.y * 2.0F;
            float z = q.z * 2.0F;
            float xx = q.x * x;
            float yy = q.y * y;
            float zz = q.z * z;
            float xy = q.x * y;
            float xz = q.x * z;
            float yz = q.y * z;
            float wx = q.w * x;
            float wy = q.w * y;
            float wz = q.w * z;

            // Calculate 3x3 matrix from orthonormal basis
            m[0] = 1.0f - (yy + zz);
            m[1] = xy + wz;
            m[2] = xz - wy;
            m[3] = 0.0F;

            m[4] = xy - wz;
            m[5] = 1.0f - (xx + zz);
            m[6] = yz + wx;
            m[7] = 0.0F;

            m[8] = xz + wy;
            m[9] = yz - wx;
            m[10] = 1.0f - (xx + yy);
            m[11] = 0.0F;

            m[12] = 0.0F;
            m[13] = 0.0F;
            m[14] = 0.0F;
            m[15] = 1.0F;
        }

        public static void MatrixToQuaternion(Matrix4x4 m, ref Quaternion q)
        {
            // Algorithm in Ken Shoemake's article in 1987 SIGGRAPH course notes
            // article "Quaternionf Calculus and Fast Animation".
#if DEBUGMODE
			float det = kRot.GetDeterminant ();
			Assert (CompareApproximately (det, 1.0F, .005f));
#endif
            float fTrace = m[0, 0] + m[1, 1] + m[2, 2];
            float fRoot;

            if (fTrace > 0.0f)
            {
                // |w| > 1/2, may as well choose w > 1/2
                fRoot = Mathf.Sqrt(fTrace + 1.0f);  // 2w
                q.w = 0.5f * fRoot;
                fRoot = 0.5f / fRoot;  // 1/(4w)
                q.x = (m[2, 1] - m[1, 2]) * fRoot;
                q.y = (m[0, 2] - m[2, 0]) * fRoot;
                q.z = (m[1, 0] - m[0, 1]) * fRoot;
            }
            else
            {
                // |w| <= 1/2
                int[] s_iNext = new int[3] { 1, 2, 0 };
                int i = 0;
                if (m[1, 1] > m[0, 0])
                    i = 1;
                if (m[2, 2] > m[i, i])
                    i = 2;
                int j = s_iNext[i];
                int k = s_iNext[j];

                fRoot = Mathf.Sqrt(m[i, i] - m[j, j] - m[k, k] + 1.0f);
                //float* apkQuat[3] = { &q.x, &q.y, &q.z };
                //Assert (fRoot >= Vector3.kEpsilon);
                q[i] = 0.5f * fRoot;
                fRoot = 0.5f / fRoot;
                q.w = (m[k, j] - m[j, k]) * fRoot;
                q[j] = (m[j, i] + m[i, j]) * fRoot;
                q[k] = (m[k, i] + m[i, k]) * fRoot;
            }
            q = Normalize(q);
        }

        internal static bool LookRotationToQuaternion(Vector3 viewVec, Vector3 upVec, ref Quaternion res)
        {
            Matrix4x4 m = new Matrix4x4();
            if (!LookRotationToMatrix(viewVec, upVec, ref m))
                return false;
            MatrixToQuaternion(m, ref res);
            return true;
        }

        public static Quaternion operator /(Quaternion q, float v)
        {
            return new Quaternion(q.x / v, q.y / v, q.z / v, q.w / v);
        }

        internal static Quaternion NormalizeSafe(Quaternion q)
        {
            float mag = Magnitude(q);
            if (mag < Vector3.kEpsilon)
                return Quaternion.identity;
            else
                return q / mag;
        }

        static Quaternion Normalize(Quaternion q) { return q / Magnitude(q); }

        static float Magnitude(Quaternion q)
        {
            return Mathf.Sqrt(SqrMagnitude(q));
        }

        static float SqrMagnitude(Quaternion q)
        {
            return Dot(q, q);
        }

        internal static Quaternion AxisAngleToQuaternionSafe(Vector3 axis, float angle)
        {
            Quaternion q;
            float mag = axis.magnitude;
            if (mag > 0.000001F)
            {
                float halfAngle = angle * 0.5F;

                q.w = Mathf.Cos(halfAngle);

                float s = Mathf.Sin(halfAngle) / mag;
                q.x = s * axis.x;
                q.y = s * axis.y;
                q.z = s * axis.z;
                return q;
            }
            else
            {
                return Quaternion.identity;
            }
        }

        internal static Quaternion FromToQuaternionSafe(Vector3 lhs, Vector3 rhs)
        {
            float lhsMag = lhs.magnitude;
            float rhsMag = rhs.magnitude;
            if (lhsMag < Vector3.kEpsilon || rhsMag < Vector3.kEpsilon)
                return Quaternion.identity;
            else
                return FromToQuaternion(lhs / lhsMag, rhs / rhsMag);
        }

        internal static Quaternion FromToQuaternion(Vector3 from, Vector3 to)
        {
            Matrix4x4 m = new Matrix4x4();
            m.SetFromToRotation(from, to);
            Quaternion q = new Quaternion();
            MatrixToQuaternion(m, ref q);
            return q;
        }

        static void QuaternionToAxisAngle(Quaternion q, out Vector3 axis, out float targetAngle)
        {
            //Assert (CompareApproximately(SqrMagnitude (q), 1.0F));
            targetAngle = 2.0f * Mathf.Acos(q.w);
            if (Mathf.Approximately(targetAngle, 0.0F))
            {
                axis = Vector3.right;
                return;
            }

            float div = 1.0f / Mathf.Sqrt(1.0f - Mathf.Sqrt(q.w));
            axis = new Vector3(q.x * div, q.y * div, q.z * div);
        }

        internal static Vector3 RotateVectorByQuat(Quaternion lhs, Vector3 rhs)
        {
            float x = lhs.x * 2.0F;
            float y = lhs.y * 2.0F;
            float z = lhs.z * 2.0F;
            float xx = lhs.x * x;
            float yy = lhs.y * y;
            float zz = lhs.z * z;
            float xy = lhs.x * y;
            float xz = lhs.x * z;
            float yz = lhs.y * z;
            float wx = lhs.w * x;
            float wy = lhs.w * y;
            float wz = lhs.w * z;

            Vector3 res;
            res.x = (1.0f - (yy + zz)) * rhs.x + (xy - wz) * rhs.y + (xz + wy) * rhs.z;
            res.y = (xy + wz) * rhs.x + (1.0f - (xx + zz)) * rhs.y + (yz - wx) * rhs.z;
            res.z = (xz - wy) * rhs.x + (yz + wx) * rhs.y + (1.0f - (xx + yy)) * rhs.z;
            return res;
        }

        static void MakePositive(ref Vector3 euler)
        {
            const float kPI = Mathf.PI;
            const float negativeFlip = -0.0001F;
            const float positiveFlip = (kPI * 2.0F) - 0.0001F;

            if (euler.x < negativeFlip)
                euler.x += 2.0f * kPI;
            else if (euler.x > positiveFlip)
                euler.x -= 2.0f * kPI;

            if (euler.y < negativeFlip)
                euler.y += 2.0f * kPI;
            else if (euler.y > positiveFlip)
                euler.y -= 2.0f * kPI;

            if (euler.z < negativeFlip)
                euler.z += 2.0f * kPI;
            else if (euler.z > positiveFlip)
                euler.z -= 2.0f * kPI;
        }
        static bool MatrixToEuler(Matrix4x4 matrix, ref Vector3 v)
        {
            const float kPI = Mathf.PI;
            // from http://www.geometrictools.com/Documentation/EulerAngles.pdf
            // YXZ order
            if (matrix[1, 2] < 0.999F) // some fudge for imprecision
            {
                if (matrix[1, 2] > -0.999F) // some fudge for imprecision
                {
                    v.x = Mathf.Asin(-matrix[1, 2]);
                    v.y = Mathf.Atan2(matrix[0, 2], matrix[2, 2]);
                    v.z = Mathf.Atan2(matrix[1, 0], matrix[1, 1]);
                    MakePositive(ref v);
                    return true;
                }
                else
                {
                    // WARNING.  Not unique.  YA - ZA = atan2(r01,r00)
                    v.x = kPI * 0.5F;
                    v.y = Mathf.Atan2(matrix[0, 1], matrix[0, 0]);
                    v.z = 0.0F;
                    MakePositive(ref v);

                    return false;
                }
            }
            else
            {
                // WARNING.  Not unique.  YA + ZA = atan2(-r01,r00)
                v.x = -kPI * 0.5F;
                v.y = Mathf.Atan2(-matrix[0, 1], matrix[0, 0]);
                v.z = 0.0F;
                MakePositive(ref v);
                return false;
            }
        }

        static bool LookRotationToMatrix(Vector3 viewVec, Vector3 upVec, ref Matrix4x4 m)
        {
            Vector3 z = viewVec;
            // compute u0
            float mag = z.magnitude;
            if (mag < Vector3.kEpsilon)
            {
                m.SetIdentity();
                return false;
            }
            z /= mag;

            Vector3 x = Vector3.Cross(upVec, z);
            mag = x.magnitude;
            if (mag < Vector3.kEpsilon)
            {
                m.SetIdentity();
                return false;
            }
            x /= mag;

            Vector3 y = (Vector3.Cross(z, x));
            if (!Mathf.Approximately(y.sqrMagnitude, 1.0F))
                return false;

            m.SetBasis(x, y, z);
            return true;
        }
    }
}
