using System;

namespace UnityEngine
{


    public struct Matrix4x4
    {
        ///*undocumented*
        public float m00;
        ///*undocumented*
        public float m10;
        ///*undocumented*
        public float m20;
        ///*undocumented*
        public float m30;

        ///*undocumented*
        public float m01;
        ///*undocumented*
        public float m11;
        ///*undocumented*
        public float m21;
        ///*undocumented*
        public float m31;

        ///*undocumented*
        public float m02;
        ///*undocumented*
        public float m12;
        ///*undocumented*
        public float m22;
        ///*undocumented*
        public float m32;

        ///*undocumented*
        public float m03;
        ///*undocumented*
        public float m13;
        ///*undocumented*
        public float m23;
        ///*undocumented*
        public float m33;



        // Access element at [row, column].
        public float this[int row, int column]
        {
            get
            {
                return this[row + column * 4];
            }

            set
            {
                this[row + column * 4] = value;
            }
        }

        // Access element at sequential index (0..15 inclusive).
        public float this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: return m00;
                    case 1: return m10;
                    case 2: return m20;
                    case 3: return m30;
                    case 4: return m01;
                    case 5: return m11;
                    case 6: return m21;
                    case 7: return m31;
                    case 8: return m02;
                    case 9: return m12;
                    case 10: return m22;
                    case 11: return m32;
                    case 12: return m03;
                    case 13: return m13;
                    case 14: return m23;
                    case 15: return m33;
                    default:
                        throw new IndexOutOfRangeException("Invalid matrix index!");
                }
            }

            set
            {
                switch (index)
                {
                    case 0: m00 = value; break;
                    case 1: m10 = value; break;
                    case 2: m20 = value; break;
                    case 3: m30 = value; break;
                    case 4: m01 = value; break;
                    case 5: m11 = value; break;
                    case 6: m21 = value; break;
                    case 7: m31 = value; break;
                    case 8: m02 = value; break;
                    case 9: m12 = value; break;
                    case 10: m22 = value; break;
                    case 11: m32 = value; break;
                    case 12: m03 = value; break;
                    case 13: m13 = value; break;
                    case 14: m23 = value; break;
                    case 15: m33 = value; break;

                    default:
                        throw new IndexOutOfRangeException("Invalid matrix index!");
                }
            }
        }

        // used to allow Matrix4x4s to be used as keys in hash tables
        public override int GetHashCode()
        {
            return GetColumn(0).GetHashCode() ^ (GetColumn(1).GetHashCode() << 2) ^ (GetColumn(2).GetHashCode() >> 2) ^ (GetColumn(3).GetHashCode() >> 1);
        }

        // also required for being able to use Matrix4x4s as keys in hash tables
        public override bool Equals(object other)
        {
            if (!(other is Matrix4x4)) return false;

            Matrix4x4 rhs = (Matrix4x4)other;
            return GetColumn(0).Equals(rhs.GetColumn(0))
                && GetColumn(1).Equals(rhs.GetColumn(1))
                && GetColumn(2).Equals(rhs.GetColumn(2))
                && GetColumn(3).Equals(rhs.GetColumn(3));
        }

        // Multiplies two matrices.
        static public Matrix4x4 operator *(Matrix4x4 lhs, Matrix4x4 rhs)
        {
            Matrix4x4 res = new Matrix4x4();
            res.m00 = lhs.m00 * rhs.m00 + lhs.m01 * rhs.m10 + lhs.m02 * rhs.m20 + lhs.m03 * rhs.m30;
            res.m01 = lhs.m00 * rhs.m01 + lhs.m01 * rhs.m11 + lhs.m02 * rhs.m21 + lhs.m03 * rhs.m31;
            res.m02 = lhs.m00 * rhs.m02 + lhs.m01 * rhs.m12 + lhs.m02 * rhs.m22 + lhs.m03 * rhs.m32;
            res.m03 = lhs.m00 * rhs.m03 + lhs.m01 * rhs.m13 + lhs.m02 * rhs.m23 + lhs.m03 * rhs.m33;

            res.m10 = lhs.m10 * rhs.m00 + lhs.m11 * rhs.m10 + lhs.m12 * rhs.m20 + lhs.m13 * rhs.m30;
            res.m11 = lhs.m10 * rhs.m01 + lhs.m11 * rhs.m11 + lhs.m12 * rhs.m21 + lhs.m13 * rhs.m31;
            res.m12 = lhs.m10 * rhs.m02 + lhs.m11 * rhs.m12 + lhs.m12 * rhs.m22 + lhs.m13 * rhs.m32;
            res.m13 = lhs.m10 * rhs.m03 + lhs.m11 * rhs.m13 + lhs.m12 * rhs.m23 + lhs.m13 * rhs.m33;

            res.m20 = lhs.m20 * rhs.m00 + lhs.m21 * rhs.m10 + lhs.m22 * rhs.m20 + lhs.m23 * rhs.m30;
            res.m21 = lhs.m20 * rhs.m01 + lhs.m21 * rhs.m11 + lhs.m22 * rhs.m21 + lhs.m23 * rhs.m31;
            res.m22 = lhs.m20 * rhs.m02 + lhs.m21 * rhs.m12 + lhs.m22 * rhs.m22 + lhs.m23 * rhs.m32;
            res.m23 = lhs.m20 * rhs.m03 + lhs.m21 * rhs.m13 + lhs.m22 * rhs.m23 + lhs.m23 * rhs.m33;

            res.m30 = lhs.m30 * rhs.m00 + lhs.m31 * rhs.m10 + lhs.m32 * rhs.m20 + lhs.m33 * rhs.m30;
            res.m31 = lhs.m30 * rhs.m01 + lhs.m31 * rhs.m11 + lhs.m32 * rhs.m21 + lhs.m33 * rhs.m31;
            res.m32 = lhs.m30 * rhs.m02 + lhs.m31 * rhs.m12 + lhs.m32 * rhs.m22 + lhs.m33 * rhs.m32;
            res.m33 = lhs.m30 * rhs.m03 + lhs.m31 * rhs.m13 + lhs.m32 * rhs.m23 + lhs.m33 * rhs.m33;

            return res;
        }

        // Transforms a [[Vector4]] by a matrix.
        static public Vector4 operator *(Matrix4x4 lhs, Vector4 v)
        {
            Vector4 res;
            res.x = lhs.m00 * v.x + lhs.m01 * v.y + lhs.m02 * v.z + lhs.m03 * v.w;
            res.y = lhs.m10 * v.x + lhs.m11 * v.y + lhs.m12 * v.z + lhs.m13 * v.w;
            res.z = lhs.m20 * v.x + lhs.m21 * v.y + lhs.m22 * v.z + lhs.m23 * v.w;
            res.w = lhs.m30 * v.x + lhs.m31 * v.y + lhs.m32 * v.z + lhs.m33 * v.w;
            return res;
        }

        //*undoc*
        public static bool operator ==(Matrix4x4 lhs, Matrix4x4 rhs)
        {
            return lhs.GetColumn(0) == rhs.GetColumn(0)
                && lhs.GetColumn(1) == rhs.GetColumn(1)
                && lhs.GetColumn(2) == rhs.GetColumn(2)
                && lhs.GetColumn(3) == rhs.GetColumn(3);
        }
        //*undoc*
        public static bool operator !=(Matrix4x4 lhs, Matrix4x4 rhs)
        {
            return !(lhs == rhs);
        }

        //*undocumented* --- have a property now
        static Matrix4x4 Inverse(Matrix4x4 m) { Matrix4x4 output = m; output.Invert_Full(); return output; }
        //*undocumented* --- have a property now
        static Matrix4x4 Transpose(Matrix4x4 m) { Matrix4x4 output = m; output.Transpose(); return output; }

        // Invert a matrix and return the success code.
        internal static bool Invert(Matrix4x4 inMatrix, out Matrix4x4 dest) { return Matrix4x4.Invert_Full(inMatrix, out dest); }

        // The inverse of this matrix (RO).
        public Matrix4x4 inverse { get { return Matrix4x4.Inverse(this); } }

        // Returns the transpose of this matrix (RO).
        public Matrix4x4 transpose { get { return Matrix4x4.Transpose(this); } }

        // Is this the identity matrix?
        public bool isIdentity { get { return _IsIdentity(); } }

        // Get a column of the matrix.
        public Vector4 GetColumn(int i) { return new Vector4(this[0, i], this[1, i], this[2, i], this[3, i]); }
        // Returns a row of the matrix.
        public Vector4 GetRow(int i) { return new Vector4(this[i, 0], this[i, 1], this[i, 2], this[i, 3]); }
        // Sets a column of the matrix.
        public void SetColumn(int i, Vector4 v) { this[0, i] = v.x; this[1, i] = v.y; this[2, i] = v.z; this[3, i] = v.w; }
        // Sets a row of the matrix.
        public void SetRow(int i, Vector4 v) { this[i, 0] = v.x; this[i, 1] = v.y; this[i, 2] = v.z; this[i, 3] = v.w; }


        private void SetByMatrix(Matrix4x4 other)
        {
            this.m00 = other.m00;
            this.m01 = other.m01;
            this.m02 = other.m02;
            this.m03 = other.m03;
            this.m10 = other.m10;
            this.m11 = other.m11;
            this.m12 = other.m12;
            this.m13 = other.m13;
            this.m20 = other.m20;
            this.m21 = other.m21;
            this.m22 = other.m22;
            this.m23 = other.m23;
            this.m30 = other.m30;
            this.m31 = other.m31;
            this.m32 = other.m32;
            this.m33 = other.m33;
        }

        // Transforms a position by this matrix (generic).
        public Vector3 MultiplyPoint(Vector3 v)
        {
            Vector3 res;
            float w;
            res.x = this.m00 * v.x + this.m01 * v.y + this.m02 * v.z + this.m03;
            res.y = this.m10 * v.x + this.m11 * v.y + this.m12 * v.z + this.m13;
            res.z = this.m20 * v.x + this.m21 * v.y + this.m22 * v.z + this.m23;
            w = this.m30 * v.x + this.m31 * v.y + this.m32 * v.z + this.m33;

            w = 1F / w;
            res.x *= w;
            res.y *= w;
            res.z *= w;
            return res;
        }

        // Transforms a position by this matrix (fast).
        public Vector3 MultiplyPoint3x4(Vector3 v)
        {
            Vector3 res;
            res.x = this.m00 * v.x + this.m01 * v.y + this.m02 * v.z + this.m03;
            res.y = this.m10 * v.x + this.m11 * v.y + this.m12 * v.z + this.m13;
            res.z = this.m20 * v.x + this.m21 * v.y + this.m22 * v.z + this.m23;
            return res;
        }

        // Transforms a direction by this matrix.
        public Vector3 MultiplyVector(Vector3 v)
        {
            Vector3 res;
            res.x = this.m00 * v.x + this.m01 * v.y + this.m02 * v.z;
            res.y = this.m10 * v.x + this.m11 * v.y + this.m12 * v.z;
            res.z = this.m20 * v.x + this.m21 * v.y + this.m22 * v.z;
            return res;
        }

        // Creates a scaling matrix.
        static public Matrix4x4 Scale(Vector3 v)
        {
            Matrix4x4 m = new Matrix4x4();
            m.m00 = v.x; m.m01 = 0F; m.m02 = 0F; m.m03 = 0F;
            m.m10 = 0F; m.m11 = v.y; m.m12 = 0F; m.m13 = 0F;
            m.m20 = 0F; m.m21 = 0F; m.m22 = v.z; m.m23 = 0F;
            m.m30 = 0F; m.m31 = 0F; m.m32 = 0F; m.m33 = 1F;
            return m;
        }

        // Returns a matrix with all elements set to zero (RO).
        public static Matrix4x4 zero
        {
            get
            {
                Matrix4x4 m = new Matrix4x4();
                m.m00 = 0F; m.m01 = 0F; m.m02 = 0F; m.m03 = 0F;
                m.m10 = 0F; m.m11 = 0F; m.m12 = 0F; m.m13 = 0F;
                m.m20 = 0F; m.m21 = 0F; m.m22 = 0F; m.m23 = 0F;
                m.m30 = 0F; m.m31 = 0F; m.m32 = 0F; m.m33 = 0F;
                return m;
            }
        }

        // Returns the identity matrix (RO).
        public static Matrix4x4 identity
        {
            get
            {
                Matrix4x4 m = new Matrix4x4();
                m.m00 = 1F; m.m01 = 0F; m.m02 = 0F; m.m03 = 0F;
                m.m10 = 0F; m.m11 = 1F; m.m12 = 0F; m.m13 = 0F;
                m.m20 = 0F; m.m21 = 0F; m.m22 = 1F; m.m23 = 0F;
                m.m30 = 0F; m.m31 = 0F; m.m32 = 0F; m.m33 = 1F;
                return m;
            }
        }

        /// *listonly*
        override public string ToString()
        {
            return String.Format("{0:F5}\t{1:F5}\t{2:F5}\t{3:F5}\n{4:F5}\t{5:F5}\t{6:F5}\t{7:F5}\n{8:F5}\t{9:F5}\t{10:F5}\t{11:F5}\n{12:F5}\t{13:F5}\t{14:F5}\t{15:F5}\n", m00, m01, m02, m03, m10, m11, m12, m13, m20, m21, m22, m23, m30, m31, m32, m33);
        }
        // Returns a nicely formatted string for this matrix.
        public string ToString(string format)
        {
            return String.Format("{0}\t{1}\t{2}\t{3}\n{4}\t{5}\t{6}\t{7}\n{8}\t{9}\t{10}\t{11}\n{12}\t{13}\t{14}\t{15}\n",
                m00.ToString(format), m01.ToString(format), m02.ToString(format), m03.ToString(format),
                m10.ToString(format), m11.ToString(format), m12.ToString(format), m13.ToString(format),
                m20.ToString(format), m21.ToString(format), m22.ToString(format), m23.ToString(format),
                m30.ToString(format), m31.ToString(format), m32.ToString(format), m33.ToString(format));
        }

        // Creates an orthogonal projection matrix.
        static public Matrix4x4 Ortho(float left, float right, float bottom, float top, float zNear, float zFar)
        {
            Matrix4x4 m = new Matrix4x4();
            m.SetOrtho(left, right, bottom, top, zNear, zFar);
            return m;
        }

        // Creates a perspective projection matrix.
        static public Matrix4x4 Perspective(float fov, float aspect, float zNear, float zFar)
        {
            Matrix4x4 m = new Matrix4x4();
            m.SetPerspective(fov, aspect, zNear, zFar);
            return m;
        }

        bool _IsIdentity()
        {
            if (Mathf.Approximately(this[0, 0], 1.0f) && Mathf.Approximately(this[0, 1], 0.0f) && Mathf.Approximately(this[0, 2], 0.0f) && Mathf.Approximately(this[0, 3], 0.0f) &&
                Mathf.Approximately(this[1, 0], 0.0f) && Mathf.Approximately(this[1, 1], 1.0f) && Mathf.Approximately(this[1, 2], 0.0f) && Mathf.Approximately(this[1, 3], 0.0f) &&
                Mathf.Approximately(this[2, 0], 0.0f) && Mathf.Approximately(this[2, 1], 0.0f) && Mathf.Approximately(this[2, 2], 1.0f) && Mathf.Approximately(this[2, 3], 0.0f) &&
                Mathf.Approximately(this[3, 0], 0.0f) && Mathf.Approximately(this[3, 1], 0.0f) && Mathf.Approximately(this[3, 2], 0.0f) && Mathf.Approximately(this[3, 3], 1.0f))
                return true;
            return false;
        }

        public static Matrix4x4 TRS(Vector3 pos, Quaternion q, Vector3 s)
        {
            var m = new Matrix4x4();
            m.SetTRS(pos, q, s);
            return m;
        }

        // Sets this matrix to a translation, rotation and scaling matrix.
        public void SetTRS(Vector3 pos, Quaternion q, Vector3 s)
        {
            // Quaternion.QuaternionToMatrix (q, ref this);
            Matrix4x4 n = new Matrix4x4();
            Quaternion.QuaternionToMatrix(q, ref n);
            SetByMatrix(n);

            this[0] *= s[0];
            this[1] *= s[0];
            this[2] *= s[0];

            this[4] *= s[1];
            this[5] *= s[1];
            this[6] *= s[1];

            this[8] *= s[2];
            this[9] *= s[2];
            this[10] *= s[2];

            this[12] = pos[0];
            this[13] = pos[1];
            this[14] = pos[2];
        }

        internal void SetOrtho(float left, float right, float bottom, float top, float zNear, float zFar)
        {
            SetIdentity();

            float deltax = right - left;
            float deltay = top - bottom;
            float deltaz = zFar - zNear;

            this[0, 0] = 2.0F / deltax;
            this[0, 3] = -(right + left) / deltax;
            this[1, 1] = 2.0F / deltay;
            this[1, 3] = -(top + bottom) / deltay;
            this[2, 2] = -2.0F / deltaz;
            this[2, 3] = -(zFar + zNear) / deltaz;
        }

        internal void SetPerspective(float fovy, float aspect, float zNear, float zFar)
        {
            float cotangent, deltaZ;
            float radians = Mathf.Deg2Rad * fovy / 2.0f;
            cotangent = Mathf.Cos(radians) / Mathf.Sin(radians);
            deltaZ = zNear - zFar;

            this[0, 0] = cotangent / aspect; this[0, 1] = 0.0F; this[0, 2] = 0.0F; this[0, 3] = 0.0F;
            this[1, 0] = 0.0F; this[1, 1] = cotangent; this[1, 2] = 0.0F; this[1, 3] = 0.0F;
            this[2, 0] = 0.0F; this[2, 1] = 0.0F; this[2, 2] = (zFar + zNear) / deltaZ; this[2, 3] = 2.0F * zNear * zFar / deltaZ;
            this[3, 0] = 0.0F; this[3, 1] = 0.0F; this[3, 2] = -1.0F; this[3, 3] = 0.0F;
        }

        public void SetFromToRotation(Vector3 from, Vector3 to)
        {
            Vector3 v = new Vector3();

            float e, h;
            v = Vector3.Cross(from, to);
            e = Vector3.Dot(from, to);
            float EPSILON = Vector3.kEpsilon;
            if (e > 1.0 - EPSILON)     /* "from" almost or equal to "to"-vector? */
            {
                /* return identity */
                this[0, 0] = 1.0f; this[0, 1] = 0.0f; this[0, 2] = 0.0f;
                this[1, 0] = 0.0f; this[1, 1] = 1.0f; this[1, 2] = 0.0f;
                this[2, 0] = 0.0f; this[2, 1] = 0.0f; this[2, 2] = 1.0f;
            }
            else if (e < -1.0 + EPSILON) /* "from" almost or equal to negated "to"? */
            {
                Vector3 up = new Vector3();
                Vector3 left = new Vector3();
                float invlen;
                float fxx, fyy, fzz, fxy, fxz, fyz;
                float uxx, uyy, uzz, uxy, uxz, uyz;
                float lxx, lyy, lzz, lxy, lxz, lyz;
                /* left=CROSS(from, (1,0,0)) */
                left[0] = 0.0f; left[1] = from[2]; left[2] = -from[1];
                if (Vector3.Dot(left, left) < EPSILON) /* was left=CROSS(from,(1,0,0)) a good choice? */
                {
                    /* here we now that left = CROSS(from, (1,0,0)) will be a good choice */
                    left[0] = -from[2]; left[1] = 0.0f; left[2] = from[0];
                }
                /* normalize "left" */
                invlen = 1.0f / Mathf.Sqrt(Vector3.Dot(left, left));
                left[0] *= invlen;
                left[1] *= invlen;
                left[2] *= invlen;
                up = Vector3.Cross(left, from);
                /* now we have a coordinate system, i.e., a basis;    */
                /* M=(from, up, left), and we want to rotate to:      */
                /* N=(-from, up, -left). This is done with the matrix:*/
                /* N*M^T where M^T is the transpose of M              */
                fxx = -from[0] * from[0]; fyy = -from[1] * from[1]; fzz = -from[2] * from[2];
                fxy = -from[0] * from[1]; fxz = -from[0] * from[2]; fyz = -from[1] * from[2];

                uxx = up[0] * up[0]; uyy = up[1] * up[1]; uzz = up[2] * up[2];
                uxy = up[0] * up[1]; uxz = up[0] * up[2]; uyz = up[1] * up[2];

                lxx = -left[0] * left[0]; lyy = -left[1] * left[1]; lzz = -left[2] * left[2];
                lxy = -left[0] * left[1]; lxz = -left[0] * left[2]; lyz = -left[1] * left[2];
                /* symmetric matrix */
                this[0, 0] = fxx + uxx + lxx; this[0, 1] = fxy + uxy + lxy; this[0, 2] = fxz + uxz + lxz;
                this[1, 0] = this[0, 1]; this[1, 1] = fyy + uyy + lyy; this[1, 2] = fyz + uyz + lyz;
                this[2, 0] = this[0, 2]; this[2, 1] = this[1, 2]; this[2, 2] = fzz + uzz + lzz;
            }
            else  /* the most common case, unless "from"="to", or "from"=-"to" */
            {
                /* ...otherwise use this hand optimized version (9 mults less) */
                float hvx, hvz, hvxy, hvxz, hvyz;
                h = (1.0f - e) / Vector3.Dot(v, v);
                hvx = h * v[0];
                hvz = h * v[2];
                hvxy = hvx * v[1];
                hvxz = hvx * v[2];
                hvyz = hvz * v[1];
                this[0, 0] = e + hvx * v[0]; this[0, 1] = hvxy - v[2]; this[0, 2] = hvxz + v[1];
                this[1, 0] = hvxy + v[2]; this[1, 1] = e + h * v[1] * v[1]; this[1, 2] = hvyz - v[0];
                this[2, 0] = hvxz - v[1]; this[2, 1] = hvyz + v[0]; this[2, 2] = e + hvz * v[2];
            }
            this[0, 3] = 0.0f;
            this[1, 3] = 0.0f;
            this[2, 3] = 0.0f;
            this[3, 0] = 0.0f; this[3, 1] = 0.0f; this[3, 2] = 0.0f; this[3, 3] = 1.0f;
        }

        void SWAP_ROWS(ref float[] a, ref float[] b)
        {
            float[] t = a;
            a = b;
            b = t;
        }

        internal bool Invert_Full()
        {
            //		#define MAT(m,r,c) (m)[(c)*4+(r)]

            // 4x4 matrix inversion by Gaussian reduction with partial pivoting followed by back/substitution;
            // with loops manually unrolled.

            //float wtmp[4][8];
            float m0, m1, m2, m3, s;
            //float *r0, *r1, *r2, *r3;

            float[] r0 = new float[8];
            float[] r1 = new float[8];
            float[] r2 = new float[8];
            float[] r3 = new float[8];

            r0[0] = this[0, 0]; r0[1] = this[0, 1];
            r0[2] = this[0, 2]; r0[3] = this[0, 3];
            r0[4] = 1.0f; r0[5] = 0.0f; r0[6] = 0.0f; r0[7] = 0.0f;

            r1[0] = this[1, 0]; r1[1] = this[1, 1];
            r1[2] = this[1, 2]; r1[3] = this[1, 3];
            r1[5] = 1.0f; r1[4] = r1[6] = r1[7] = 0.0f;

            r2[0] = this[2, 0]; r2[1] = this[2, 1];
            r2[2] = this[2, 2]; r2[3] = this[2, 3];
            r2[6] = 1.0f; r2[4] = r2[5] = r2[7] = 0.0f;

            r3[0] = this[3, 0]; r3[1] = this[3, 1];
            r3[2] = this[3, 2]; r3[3] = this[3, 3];
            r3[7] = 1.0f; r3[4] = r3[5] = r3[6] = 0.0f;

            /* choose pivot - or die */
            if (Mathf.Abs(r3[0]) > Mathf.Abs(r2[0])) SWAP_ROWS(ref r3, ref r2);
            if (Mathf.Abs(r2[0]) > Mathf.Abs(r1[0])) SWAP_ROWS(ref r2, ref r1);
            if (Mathf.Abs(r1[0]) > Mathf.Abs(r0[0])) SWAP_ROWS(ref r1, ref r0);
            if (0.0F == r0[0]) return false;

            /* eliminate first variable     */
            m1 = r1[0] / r0[0]; m2 = r2[0] / r0[0]; m3 = r3[0] / r0[0];
            s = r0[1]; r1[1] -= m1 * s; r2[1] -= m2 * s; r3[1] -= m3 * s;
            s = r0[2]; r1[2] -= m1 * s; r2[2] -= m2 * s; r3[2] -= m3 * s;
            s = r0[3]; r1[3] -= m1 * s; r2[3] -= m2 * s; r3[3] -= m3 * s;
            s = r0[4];
            if (s != 0.0F) { r1[4] -= m1 * s; r2[4] -= m2 * s; r3[4] -= m3 * s; }
            s = r0[5];
            if (s != 0.0F) { r1[5] -= m1 * s; r2[5] -= m2 * s; r3[5] -= m3 * s; }
            s = r0[6];
            if (s != 0.0F) { r1[6] -= m1 * s; r2[6] -= m2 * s; r3[6] -= m3 * s; }
            s = r0[7];
            if (s != 0.0F) { r1[7] -= m1 * s; r2[7] -= m2 * s; r3[7] -= m3 * s; }

            /* choose pivot - or die */
            if (Mathf.Abs(r3[1]) > Mathf.Abs(r2[1])) SWAP_ROWS(ref r3, ref r2);
            if (Mathf.Abs(r2[1]) > Mathf.Abs(r1[1])) SWAP_ROWS(ref r2, ref r1);
            if (0.0F == r1[1]) return false;

            /* eliminate second variable */
            m2 = r2[1] / r1[1]; m3 = r3[1] / r1[1];
            r2[2] -= m2 * r1[2]; r3[2] -= m3 * r1[2];
            r2[3] -= m2 * r1[3]; r3[3] -= m3 * r1[3];
            s = r1[4]; if (0.0F != s) { r2[4] -= m2 * s; r3[4] -= m3 * s; }
            s = r1[5]; if (0.0F != s) { r2[5] -= m2 * s; r3[5] -= m3 * s; }
            s = r1[6]; if (0.0F != s) { r2[6] -= m2 * s; r3[6] -= m3 * s; }
            s = r1[7]; if (0.0F != s) { r2[7] -= m2 * s; r3[7] -= m3 * s; }

            /* choose pivot - or die */
            if (Mathf.Abs(r3[2]) > Mathf.Abs(r2[2])) SWAP_ROWS(ref r3, ref r2);
            if (0.0F == r2[2]) return false;

            /* eliminate third variable */
            m3 = r3[2] / r2[2];
            r3[3] -= m3 * r2[3]; r3[4] -= m3 * r2[4];
            r3[5] -= m3 * r2[5]; r3[6] -= m3 * r2[6];
            r3[7] -= m3 * r2[7];

            /* last check */
            if (0.0F == r3[3]) return false;

            s = 1.0F / r3[3];             /* now back substitute row 3 */
            r3[4] *= s; r3[5] *= s; r3[6] *= s; r3[7] *= s;

            m2 = r2[3];                 /* now back substitute row 2 */
            s = 1.0F / r2[2];
            r2[4] = s * (r2[4] - r3[4] * m2); r2[5] = s * (r2[5] - r3[5] * m2);
            r2[6] = s * (r2[6] - r3[6] * m2); r2[7] = s * (r2[7] - r3[7] * m2);
            m1 = r1[3];
            r1[4] -= r3[4] * m1; r1[5] -= r3[5] * m1;
            r1[6] -= r3[6] * m1; r1[7] -= r3[7] * m1;
            m0 = r0[3];
            r0[4] -= r3[4] * m0; r0[5] -= r3[5] * m0;
            r0[6] -= r3[6] * m0; r0[7] -= r3[7] * m0;

            m1 = r1[2];                 /* now back substitute row 1 */
            s = 1.0F / r1[1];
            r1[4] = s * (r1[4] - r2[4] * m1); r1[5] = s * (r1[5] - r2[5] * m1);
            r1[6] = s * (r1[6] - r2[6] * m1); r1[7] = s * (r1[7] - r2[7] * m1);
            m0 = r0[2];
            r0[4] -= r2[4] * m0; r0[5] -= r2[5] * m0;
            r0[6] -= r2[6] * m0; r0[7] -= r2[7] * m0;

            m0 = r0[1];                 /* now back substitute row 0 */
            s = 1.0F / r0[0];
            r0[4] = s * (r0[4] - r1[4] * m0); r0[5] = s * (r0[5] - r1[5] * m0);
            r0[6] = s * (r0[6] - r1[6] * m0); r0[7] = s * (r0[7] - r1[7] * m0);

            this[0, 0] = r0[4]; this[0, 1] = r0[5]; this[0, 2] = r0[6]; this[0, 3] = r0[7];
            this[1, 0] = r1[4]; this[1, 1] = r1[5]; this[1, 2] = r1[6]; this[1, 3] = r1[7];
            this[2, 0] = r2[4]; this[2, 1] = r2[5]; this[2, 2] = r2[6]; this[2, 3] = r2[7];
            this[3, 0] = r3[4]; this[3, 1] = r3[5]; this[3, 2] = r3[6]; this[3, 3] = r3[7];

            return true;
        }

        internal static bool Invert_Full(Matrix4x4 inMatrix, out Matrix4x4 dest)
        {
            dest = inMatrix;
            return dest.Invert_Full();
        }

        void swap(ref float a, ref float b)
        {
            float t = a;
            a = b;
            b = t;
        }

        void Transpose()
        {
            swap(ref m01, ref m10);
            swap(ref m02, ref m20);
            swap(ref m03, ref m30);
            swap(ref m12, ref m21);
            swap(ref m13, ref m31);
            swap(ref m23, ref m32);
        }

        public void SetIdentity()
        {
            this[0, 0] = 1.0f; this[0, 1] = 0.0f; this[0, 2] = 0.0f; this[0, 3] = 0.0f;
            this[1, 0] = 0.0f; this[1, 1] = 1.0f; this[1, 2] = 0.0f; this[1, 3] = 0.0f;
            this[2, 0] = 0.0f; this[2, 1] = 0.0f; this[2, 2] = 1.0f; this[2, 3] = 0.0f;
            this[3, 0] = 0.0f; this[3, 1] = 0.0f; this[3, 2] = 0.0f; this[3, 3] = 1.0f;
        }

        public void SetBasis(Vector3 inX, Vector3 inY, Vector3 inZ)
        {
            this[0, 0] = inX[0]; this[0, 1] = inY[0]; this[0, 2] = inZ[0]; this[0, 3] = 0.0f;
            this[1, 0] = inX[1]; this[1, 1] = inY[1]; this[1, 2] = inZ[1]; this[1, 3] = 0.0f;
            this[2, 0] = inX[2]; this[2, 1] = inY[2]; this[2, 2] = inZ[2]; this[2, 3] = 0.0f;
            this[3, 0] = 0.0f; this[3, 1] = 0.0f; this[3, 2] = 0.0f; this[3, 3] = 1.0f;
        }

        internal void SetTRInverse(Vector3 pos, Quaternion q)
        {
            //Quaternion.QuaternionToMatrix (Quaternion.Inverse (q), ref this);
            Matrix4x4 n = new Matrix4x4();
            Quaternion.QuaternionToMatrix(Quaternion.Inverse(q), ref n);
            n.Translate(new Vector3(-pos[0], -pos[1], -pos[2]));
            SetByMatrix(n);
        }

        Matrix4x4 Translate(Vector3 inTrans)
        {
            this[0, 3] = this[0, 0] * inTrans[0] + this[0, 1] * inTrans[1] + this[0, 2] * inTrans[2] + this[0, 3];
            this[1, 3] = this[1, 0] * inTrans[0] + this[1, 1] * inTrans[1] + this[1, 2] * inTrans[2] + this[1, 3];
            this[2, 3] = this[2, 0] * inTrans[0] + this[2, 1] * inTrans[1] + this[2, 2] * inTrans[2] + this[2, 3];
            this[3, 3] = this[3, 0] * inTrans[0] + this[3, 1] * inTrans[1] + this[3, 2] * inTrans[2] + this[3, 3];
            return this;
        }

        internal bool PerspectiveMultiplyPoint3(Vector3 v, out Vector3 output)
        {
            Vector3 res = new Vector3();
            output = new Vector3();
            float w;
            res.x = this[0, 0] * v.x + this[0, 1] * v.y + this[0, 2] * v.z + this[0, 3];
            res.y = this[1, 0] * v.x + this[1, 1] * v.y + this[1, 2] * v.z + this[1, 3];
            res.z = this[2, 0] * v.x + this[2, 1] * v.y + this[2, 2] * v.z + this[2, 3];
            w = this[3, 0] * v.x + this[3, 1] * v.y + this[3, 2] * v.z + this[3, 3];
            if (Mathf.Abs(w) > 1.0e-7f)
            {
                float invW = 1.0f / w;
                output.x = res.x * invW;
                output.y = res.y * invW;
                output.z = res.z * invW;
                return true;
            }
            else
            {
                output.x = 0.0f;
                output.y = 0.0f;
                output.z = 0.0f;
                return false;
            }
        }

        /*
        internal Vector3 Matrix4x4f::GetAxisX() const {
        return Vector3f( Get(0,0), Get(1,0), Get(2,0) );
        }
        internal Vector3f Matrix4x4f::GetAxisY() const {
            return Vector3f( Get(0,1), Get(1,1), Get(2,1) );
        }

        inline Vector3f Matrix4x4f::GetAxis(int axis) const {
            return Vector3f( Get(0,axis), Get(1,axis), Get(2,axis) );
        }
        */
        internal Vector3 GetAxisZ()
        {
            return new Vector3(this[0, 2], this[1, 2], this[2, 2]);
        }
        internal Vector3 GetPosition()
        {
            return new Vector3(this[0, 3], this[1, 3], this[2, 3]);
        }

        internal bool IsPerspective() { return (this[3] != 0.0f || this[7] != 0.0f || this[11] != 0.0f || this[15] != 1.0f); }
    }

}