using System;

namespace UnityEngine
{

    public struct Color
    {
        public float r;

        // Green component of the color.
        public float g;

        // Blue component of the color.
        public float b;

        // Alpha component of the color.
        public float a;

        // Constructs a new Color with given r,g,b,a components.
        public Color(float r, float g, float b, float a)
        {
            this.r = r; this.g = g; this.b = b; this.a = a;
        }

        // Constructs a new Color with given r,g,b components and sets /a/ to 1.
        public Color(float r, float g, float b)
        {
            this.r = r; this.g = g; this.b = b; this.a = 1.0F;
        }

        /// *listonly*
        override public string ToString()
        {
            return String.Format("RGBA({0:F3}, {1:F3}, {2:F3}, {3:F3})", r, g, b, a);
        }
        // Returns a nicely formatted string of this color.
        public string ToString(string format)
        {
            return String.Format("RGBA({0}, {1}, {2}, {3})", r.ToString(format), g.ToString(format), b.ToString(format), a.ToString(format));
        }

        // used to allow Colors to be used as keys in hash tables
        public override int GetHashCode()
        {
            return ((Vector4)this).GetHashCode();
        }

        // also required for being able to use Colors as keys in hash tables
        public override bool Equals(object other)
        {
            if (!(other is Color)) return false;
            Color rhs = (Color)other;
            return r.Equals(rhs.r) && g.Equals(rhs.g) && b.Equals(rhs.b) && a.Equals(rhs.a);
        }

        // Adds two colors together. Each component is added separately.
        public static Color operator +(Color a, Color b) { return new Color(a.r + b.r, a.g + b.g, a.b + b.b, a.a + b.a); }

        // Subtracts color /b/ from color /a/. Each component is subtracted separately.
        public static Color operator -(Color a, Color b) { return new Color(a.r - b.r, a.g - b.g, a.b - b.b, a.a - b.a); }

        // Multiplies two colors together. Each component is multiplied separately.
        public static Color operator *(Color a, Color b) { return new Color(a.r * b.r, a.g * b.g, a.b * b.b, a.a * b.a); }

        // Multiplies color /a/ by the float /b/. Each color component is scaled separately.
        public static Color operator *(Color a, float b) { return new Color(a.r * b, a.g * b, a.b * b, a.a * b); }

        // Multiplies color /a/ by the float /b/. Each color component is scaled separately.
        public static Color operator *(float b, Color a) { return new Color(a.r * b, a.g * b, a.b * b, a.a * b); }

        // Divides color /a/ by the float /b/. Each color component is scaled separately.
        public static Color operator /(Color a, float b) { return new Color(a.r / b, a.g / b, a.b / b, a.a / b); }

        //*undoc*
        public static bool operator ==(Color lhs, Color rhs)
        {
            return ((Vector4)lhs == (Vector4)rhs);
        }
        //*undoc*
        public static bool operator !=(Color lhs, Color rhs)
        {
            return ((Vector4)lhs != (Vector4)rhs);
        }

        // Interpolates between colors /a/ and /b/ by /t/.
        public static Color Lerp(Color a, Color b, float t)
        {
            t = Mathf.Clamp01(t);
            return new Color(
                a.r + (b.r - a.r) * t,
                a.g + (b.g - a.g) * t,
                a.b + (b.b - a.b) * t,
                a.a + (b.a - a.a) * t
            );
        }

        // Solid red. RGBA is (1, 0, 0, 1).
        public static Color red { get { return new Color(1F, 0F, 0F, 1F); } }
        // Solid green. RGBA is (0, 1, 0, 1).
        public static Color green { get { return new Color(0F, 1F, 0F, 1F); } }
        // Solid blue. RGBA is (0, 0, 1, 1).
        public static Color blue { get { return new Color(0F, 0F, 1F, 1F); } }
        // Solid white. RGBA is (1, 1, 1, 1).
        public static Color white { get { return new Color(1F, 1F, 1F, 1F); } }
        // Solid black. RGBA is (0, 0, 0, 1).
        public static Color black { get { return new Color(0F, 0F, 0F, 1F); } }
        // Yellow. RGBA is (1, 0.92, 0.016, 1), but the color is nice to look at!
        public static Color yellow { get { return new Color(1F, 235F / 255F, 4F / 255F, 1F); } }
        // Cyan. RGBA is (0, 1, 1, 1).
        public static Color cyan { get { return new Color(0F, 1F, 1F, 1F); } }
        // Magenta. RGBA is (1, 0, 1, 1).
        public static Color magenta { get { return new Color(1F, 0F, 1F, 1F); } }
        // Gray. RGBA is (0.5, 0.5, 0.5, 1).
        public static Color gray { get { return new Color(.5F, .5F, .5F, 1F); } }
        // English spelling for ::ref::gray. RGBA is the same (0.5, 0.5, 0.5, 1).
        public static Color grey { get { return new Color(.5F, .5F, .5F, 1F); } }
        // Completely transparent. RGBA is (0, 0, 0, 0).
        public static Color clear { get { return new Color(0F, 0F, 0F, 0F); } }

        // The grayscale value of the color (RO)
        public float grayscale { get { return 0.299F * r + 0.587F * g + 0.114F * b; } }

        /*
		// A version of the color that has had the inverse gamma curve applied
		public Color linear 
		{
			get {
				return new Color (Mathf.GammaToLinearSpace(r), Mathf.GammaToLinearSpace(g), Mathf.GammaToLinearSpace(b), a);
			}
		}
		
		// A version of the color that has had the gamma curve applied
		public Color gamma 
		{
			get {
				return new Color (Mathf.LinearToGammaSpace(r), Mathf.LinearToGammaSpace(g), Mathf.LinearToGammaSpace(b), a);
			}
		}
		*/
        // Colors can be implicitly converted to and from [[Vector4]].
        public static implicit operator Vector4(Color c)
        {
            return new Vector4(c.r, c.g, c.b, c.a);
        }
        // Colors can be implicitly converted to and from [[Vector4]].
        public static implicit operator Color(Vector4 v)
        {
            return new Color(v.x, v.y, v.z, v.w);
        }

        // Access the r, g, b,a components using [0], [1], [2], [3] respectively.
        public float this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: return r;
                    case 1: return g;
                    case 2: return b;
                    case 3: return a;
                    default:
                        throw new IndexOutOfRangeException("Invalid Vector3 index!");
                }
            }

            set
            {
                switch (index)
                {
                    case 0: r = value; break;
                    case 1: g = value; break;
                    case 2: b = value; break;
                    case 3: a = value; break;
                    default:
                        throw new IndexOutOfRangeException("Invalid Vector3 index!");
                }
            }
        }
    }

    // Representation of RGBA colors in 32 bit format
    public struct Color32
    {
        // Red component of the color.
        public byte r;

        // Green component of the color.
        public byte g;

        // Blue component of the color.
        public byte b;

        // Alpha component of the color.
        public byte a;

        // Constructs a new Color with given r, g, b, a components.
        public Color32(byte r, byte g, byte b, byte a)
        {
            this.r = r; this.g = g; this.b = b; this.a = a;
        }

        // Color32 can be implicitly converted to and from [[Color]].
        public static implicit operator Color32(Color c)
        {
            return new Color32((byte)(Mathf.Clamp01(c.r) * 255), (byte)(Mathf.Clamp01(c.g) * 255), (byte)(Mathf.Clamp01(c.b) * 255), (byte)(Mathf.Clamp01(c.a) * 255));
        }

        // Color32 can be implicitly converted to and from [[Color]].
        public static implicit operator Color(Color32 c)
        {
            return new Color(c.r / 255f, c.g / 255f, c.b / 255f, c.a / 255f);
        }

        /// *listonly*
        override public string ToString()
        {
            return String.Format("RGBA({0}, {1}, {2}, {3})", r, g, b, a);
        }
        // Returns a nicely formatted string of this color.
        public string ToString(string format)
        {
            return String.Format("RGBA({0}, {1}, {2}, {3})", r.ToString(format), g.ToString(format), b.ToString(format), a.ToString(format));
        }

        // Interpolates between colors /a/ and /b/ by /t/.
        public static Color32 Lerp(Color32 a, Color32 b, float t)
        {
            t = Mathf.Clamp01(t);
            return new Color32(
                (byte)(a.r + (b.r - a.r) * t),
                (byte)(a.g + (b.g - a.g) * t),
                (byte)(a.b + (b.b - a.b) * t),
                (byte)(a.a + (b.a - a.a) * t)
            );
        }
    }

}

