using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MrRob  {	
	public class Map : MonoBehaviour {
		
		[SerializeField] private float spacing = 1.0f;
		[SerializeField] private GameObject blockPrefab;

		private int _width;
		private int _length;
		//Treasure 
		//Robot
		//Tile[,]
		//TileObject[,]

		private void Start() {
			Initialize(10, 10);
		}

		public void Initialize(int width, int length) {
			this._width = width;
			this._length = length;

			for(int x = 0; x < width; x++) {
				for(int y = 0; y < length; y++) {
					GameObject newBlock = Instantiate(blockPrefab, this.transform);
					newBlock.transform.position = MapToWorldPos(new Point(x, y));
				}
			}

			//place robot
			//place goal
		}

		private Vector3 MapToWorldPos(Point pos) {
			return new Vector3(pos.x * spacing, 0.0f, pos.y * spacing);
		}
	}
}

