using System;

namespace UnityEngine
{


    public struct Rect
    {
        // Creates a new rectangle.
        public Rect(float left, float top, float width, float height)
        {
            this.x = left;
            this.y = top;
            this.width = width;
            this.height = height;
        }

        //*undocumented*
        public Rect(Rect source)
        {
            x = source.x;
            y = source.y;
            width = source.width;
            height = source.height;
        }

        // Creates a rectangle from min/max coordinate values.
        static public Rect MinMaxRect(float left, float top, float right, float bottom)
        {
            return new Rect(left, top, right - left, bottom - top);
        }

        // Set components of an existing Rect.
        public void Set(float left, float top, float width, float height)
        {
            this.x = left;
            this.y = top;
            this.width = width;
            this.height = height;
        }

        // Left coordinate of the rectangle.
        public float x;

        // Top coordinate of the rectangle.
        public float y;

        // Width of the rectangle.
        public float width;

        // Height of the rectangle.
        public float height;

        // Top Left coordinates of the rectangle.
        public Vector2 position { get { return new Vector2(x, y); } set { x = value.x; y = value.y; } }

        // Center coordinate of the rectangle.
        public Vector2 center { get { return new Vector2(x + width / 2f, y + height / 2f); } set { x = value.x - width / 2f; y = value.y - height / 2f; } }

        // Top left corner of the rectangle.
        public Vector2 min { get { return new Vector2(xMin, yMin); } set { xMin = value.x; yMin = value.y; } }

        // Bottom right corner of the rectangle.
        public Vector2 max { get { return new Vector2(xMax, yMax); } set { xMax = value.x; yMax = value.y; } }

        // Size of the rectangle.
        public Vector2 size { get { return new Vector2(width, height); } set { width = value.x; height = value.y; } }

        public float left { get { return x; } }
        public float right { get { return x + width; } }
        public float top { get { return y; } }
        public float bottom { get { return y + height; } }

        // Left coordinate of the rectangle.
        public float xMin { get { return x; } set { float oldxmax = xMax; x = value; width = oldxmax - x; } }
        // Top coordinate of the rectangle.
        public float yMin { get { return y; } set { float oldymax = yMax; y = value; height = oldymax - y; } }
        // Right coordinate of the rectangle.
        public float xMax { get { return width + x; } set { width = value - x; } }
        // Bottom coordinate of the rectangle.
        public float yMax { get { return height + y; } set { height = value - y; } }

        /// *listonly*
        override public string ToString() { return String.Format("(x:{0:F2}, y:{1:F2}, width:{2:F2}, height:{3:F2})", x, y, width, height); }
        // Returns a nicely formatted string for this Rect.
        public string ToString(string format)
        {
            return String.Format("(x:{0}, y:{1}, width:{2}, height:{3})", x.ToString(format), y.ToString(format), width.ToString(format), height.ToString(format));
        }

        /// *listonly*
        public bool Contains(Vector2 point)
        {
            return (point.x >= xMin) && (point.x < xMax) && (point.y >= yMin) && (point.y < yMax);
        }
        // Returns true if the /x/ and /y/ components of /point/ is a point inside this rectangle.
        public bool Contains(Vector3 point)
        {
            return (point.x >= xMin) && (point.x < xMax) && (point.y >= yMin) && (point.y < yMax);
        }
        public bool Contains(Vector3 point, bool allowInverse)
        {
            if (!allowInverse)
            {
                return Contains(point);
            }
            bool xAxis = false;
            if (width < 0f && (point.x <= xMin) && (point.x > xMax) || width >= 0f && (point.x >= xMin) && (point.x < xMax))
                xAxis = true;
            if (xAxis && (height < 0f && (point.y <= yMin) && (point.y > yMax) || height >= 0f && (point.y >= yMin) && (point.y < yMax)))
                return true;
            return false;
        }
        // removed for 2.0
        // Clamp a point to be within a rectangle.
        //	public Vector2 Clamp (Vector2 point) {
        //		return new Vector2 (Mathf.Clamp (point.x, left, xMax-1), Mathf.Clamp (point.y, yMin, yMax-1));
        //	}

        // Swaps min and max if min was greater than max.
        private static Rect OrderMinMax(Rect rect)
        {
            if (rect.xMin > rect.xMax)
            {
                float temp = rect.xMin;
                rect.xMin = rect.xMax;
                rect.xMax = temp;
            }
            if (rect.yMin > rect.yMax)
            {
                float temp = rect.yMin;
                rect.yMin = rect.yMax;
                rect.yMax = temp;
            }
            return rect;
        }

        public bool Overlaps(Rect other)
        {
            return (other.xMax > xMin &&
                    other.xMin < xMax &&
                    other.yMax > yMin &&
                    other.yMin < yMax);
        }

        public bool Overlaps(Rect other, bool allowInverse)
        {
            Rect self = this;
            if (allowInverse)
            {
                self = OrderMinMax(self);
                other = OrderMinMax(other);
            }
            return self.Overlaps(other);
        }

        public static Vector2 NormalizedToPoint(Rect rectangle, Vector2 normalizedRectCoordinates)
        {
            return new Vector2(
                Mathf.Lerp(rectangle.x, rectangle.xMax, normalizedRectCoordinates.x),
                Mathf.Lerp(rectangle.y, rectangle.yMax, normalizedRectCoordinates.y)
            );
        }

        public static Vector2 PointToNormalized(Rect rectangle, Vector2 point)
        {
            return new Vector2(
                Mathf.InverseLerp(rectangle.x, rectangle.xMax, point.x),
                Mathf.InverseLerp(rectangle.y, rectangle.yMax, point.y)
            );
        }

        // Returns true if the rectangles are different.
        public static bool operator !=(Rect lhs, Rect rhs)
        {
            return lhs.x != rhs.x || lhs.y != rhs.y || lhs.width != rhs.width || lhs.height != rhs.height;
        }

        // Returns true if the rectangles are the same.
        public static bool operator ==(Rect lhs, Rect rhs)
        {
            return lhs.x == rhs.x && lhs.y == rhs.y && lhs.width == rhs.width && lhs.height == rhs.height;
        }

        public override int GetHashCode()
        {
            return x.GetHashCode() ^ (width.GetHashCode() << 2) ^ (y.GetHashCode() >> 2) ^ (height.GetHashCode() >> 1);
        }

        public override bool Equals(object other)
        {
            if (!(other is Rect)) return false;

            Rect rhs = (Rect)other;
            return x.Equals(rhs.x) && y.Equals(rhs.y) && width.Equals(rhs.width) && height.Equals(rhs.height);
        }
    }

}
