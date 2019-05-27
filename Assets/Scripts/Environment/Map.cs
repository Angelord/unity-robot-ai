using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MrRob.RobotLogic;

namespace MrRob  {	
	public class Map : MonoBehaviour {
		
		[SerializeField] private float spacing = 1.0f;
		[SerializeField] private GameObject blockPrefab;
		[SerializeField] private GameObject goalPrefab;
		[SerializeField] private GameObject robotPrefab;

		private int width;
		private int length;
		private Robot robot;
		private Point goalPos;
		//Treasure 
		//Robot
		//Tile[,]
		//TileObject[,]

		//public Treasure treasure { get; }
		//public Robot robot { get; }
		public int Width { get { return width; } }
		public int Length { get { return length; } }
		public Point GoalPosition { get { return goalPos; } }

		private void Start() {
			Initialize(10, 10);
		}

		public void Initialize(int width, int length) {
			this.width = width;
			this.length = length;
			goalPos = new Point(width - 1, length - 1);

			for(int y = 0; y < length; y++) {
				for(int x = 0; x < width; x++) {
					Vector3 pos = MapToWorldPos(new Point(x, y));
					GameObject newBlock = Instantiate(blockPrefab, pos, Quaternion.identity, this.transform);
				}
			}

			Instantiate(goalPrefab, MapToWorldPos(goalPos), Quaternion.identity, this.transform);

			robot = new Robot(this, robotPrefab);
		}

		public Vector3 MapToWorldPos(Point pos) {
			return new Vector3(pos.X * spacing, 0.0f, pos.Y * spacing);
		}
	}
}

