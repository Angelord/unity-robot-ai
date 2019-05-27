using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MrRob  {	
	public class Map : MonoBehaviour {
		
		[SerializeField] private float spacing = 1.0f;
		[SerializeField] private GameObject blockPrefab;
		[SerializeField] private GameObject goalPrefab;

		private int _width;
		private int _length;
		private Point _goalPosition;
		//Treasure 
		//Robot
		//Tile[,]
		//TileObject[,]

		//public Treasure treasure { get; }
		//public Robot robot { get; }
		public Point goalPosition { get { return _goalPosition; } }

		private void Start() {
			Initialize(10, 10);
		}

		public void Initialize(int width, int length) {
			this._width = width;
			this._length = length;
			_goalPosition = new Point(width - 1, length - 1);

			for(int x = 0; x < width; x++) {
				for(int y = 0; y < length; y++) {
					Vector3 pos = MapToWorldPos(new Point(x, y));
					GameObject newBlock = Instantiate(blockPrefab, pos, Quaternion.identity, this.transform);
				}
			}

			Instantiate(goalPrefab, MapToWorldPos(_goalPosition), Quaternion.identity, this.transform);
			//place robot
			//place goal
		}

		private Vector3 MapToWorldPos(Point pos) {
			return new Vector3(pos.x * spacing, 0.0f, pos.y * spacing);
		}
	}
}

