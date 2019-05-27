using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MrRob {
    [System.Serializable]
	public class Point {

        /// <summary>
        /// Shorthand for Point(1, 1)
        /// </summary>
		public static readonly Point ONE = new Point(1, 1);

        /// <summary>
        /// Shorthand for Point(0, 0)
        /// </summary>
        public static readonly Point ZERO = new Point(0, 0);

        /// <summary>
        /// Shorthand for Point(-1, -1)
        /// </summary>
        public static readonly Point MINUS = new Point(-1, -1);

        public static readonly Point[] DIRECTIONS = { new Point(0, 1), new Point(1, 0), new Point(0, -1), new Point(-1, 0)  };

        public static readonly Point UP = DIRECTIONS[0];
        public static readonly Point RIGHT = DIRECTIONS[1];
        public static readonly Point DOWN = DIRECTIONS[2];
        public static readonly Point LEFT = DIRECTIONS[3];

        [SerializeField] private int x;
        [SerializeField] private int y;

        public int X { get { return x; } set { x = value; } }
        public int Y { get { return y; } set { y = value; } }
        public int Length { get { return Mathf.Abs(x) + Mathf.Abs(y); } }

        public Point(int x, int y) {
            this.x = x;
            this.y = y;
        }

     	public Point()
			: this(0, 0) {
		}

        public void Set(int x, int y) {
            this.x = x;
            this.y = y;
        }

        /// <summary>  
        /// Returns the distance to the specified point. This ignores any obstacles. 
        /// </summary>  
        public int GetDistance(Point endPoint) { 
            int distanceX = (int)Mathf.Abs(this.x - endPoint.x);
            int distanceY = (int)Mathf.Abs(this.y - endPoint.y);

            return distanceX + distanceY;
        }

        /// <summary>  
        /// Returns the distance to the specified point. This ignores any obstacles. 
        /// </summary>  
        public int GetDistance(int x, int y) { 
            return GetDistance(new Point(x, y));
        }

        /// <summary>  
        /// Returns a point whose x and y have been swapped.
        /// </summary>  
        public Point GetInverted() {
            return new Point(y, x);
        }

        /// <summary>  
        /// Returns a point whose x and y have been swapped and negated. 
        /// </summary>  
        public Point GetOpposite() {
            return new Point(-y, -x);
        }
		public static Point operator +(Point lhs, Point rhs) {
			return new Point (lhs.x + rhs.x, lhs.y + rhs.y);
		}

        public static Point operator -(Point lhs, Point rhs) {
            return new Point(lhs.x - rhs.x, lhs.y - rhs.y);
        }

        public static bool operator ==(Point lhs, Point rhs) {
            if (ReferenceEquals(lhs, null) || ReferenceEquals(rhs, null)) {
                return ReferenceEquals(lhs, rhs);
            }
            return lhs.X == rhs.X && lhs.Y == rhs.Y;
        }

        public static bool operator !=(Point lhs, Point rhs) {
            return !(lhs == rhs);
        }

        public bool Equals(Point toCompare) {
            return this.X == toCompare.X && this.Y == toCompare.Y;
        }

        public override bool Equals(object obj) {
            if (obj is Point) {
                return (this == (Point)obj);
            }
            return false;
        }

        public override int GetHashCode() {
            return (53 + x.GetHashCode()) * 53 + y.GetHashCode();
        }

        public override string ToString() {
            return System.String.Format("( {0}, {1} )", x, y);
        }
    }
}