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

        [SerializeField] private int _x;
        [SerializeField] private int _y;

        public int x { get { return _x; } set { _x = value; } }
        public int y { get { return _y; } set { _y = value; } }
        public int length { get { return Mathf.Abs(_x) + Mathf.Abs(_y); } }

        public Point(int x, int y) {
            this._x = x;
            this._y = y;
        }

     	public Point()
			: this(0, 0) {
		}

        public void Set(int x, int y) {
            this._x = x;
            this._y = y;
        }

        /// <summary>  
        /// Returns the distance to the specified point. This ignores any obstacles. 
        /// </summary>  
        public int GetDistance(Point endPoint) { 
            int distanceX = (int)Mathf.Abs(this._x - endPoint._x);
            int distanceY = (int)Mathf.Abs(this._y - endPoint._y);

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
            return new Point(_y, _x);
        }

        /// <summary>  
        /// Returns a point whose x and y have been swapped and negated. 
        /// </summary>  
        public Point GetOpposite() {
            return new Point(-_y, -_x);
        }
		public static Point operator +(Point lhs, Point rhs) {
			return new Point (lhs._x + rhs._x, lhs._y + rhs._y);
		}

        public static Point operator -(Point lhs, Point rhs) {
            return new Point(lhs._x - rhs._x, lhs._y - rhs._y);
        }

        public static bool operator ==(Point lhs, Point rhs) {
            if (ReferenceEquals(lhs, null) || ReferenceEquals(rhs, null)) {
                return ReferenceEquals(lhs, rhs);
            }
            return lhs.x == rhs.x && lhs.y == rhs.y;
        }

        public static bool operator !=(Point lhs, Point rhs) {
            return !(lhs == rhs);
        }

        public bool Equals(Point toCompare) {
            return this.x == toCompare.x && this.y == toCompare.y;
        }

        public override bool Equals(object obj) {
            if (obj is Point) {
                return (this == (Point)obj);
            }
            return false;
        }

        public override int GetHashCode() {
            return (53 + _x.GetHashCode()) * 53 + _y.GetHashCode();
        }

        public override string ToString() {
            return System.String.Format("( {0}, {1} )", _x, _y);
        }
    }
}