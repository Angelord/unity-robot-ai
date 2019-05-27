using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MrRob.GameLogic;

namespace MrRob {
	public class GameManager : MonoBehaviour {

		[SerializeField] private float spacing = 1.0f;
		[SerializeField] private GameObject tilePrefab;
		[SerializeField] private GameObject goalPrefab;
		[SerializeField] private GameObject robotPrefab;
		[SerializeField] private GameObject cargoPrefab;

		private RobotGame game; 
		private GameObject robot;
		private GameObject cargo;

		private void Start() {
			Initialize(10, 10);
		}

		public void Initialize(int width, int length) {

			game = new RobotGame(width, length);

			for(int y = 0; y < length; y++) {
				for(int x = 0; x < width; x++) {
					Vector3 pos = MapToWorldPos(new Point(x, y));
					GameObject newBlock = Instantiate(tilePrefab, pos, Quaternion.identity, this.transform);
				}
			}
			Instantiate(goalPrefab, MapToWorldPos(game.GoalPosition), Quaternion.identity, this.transform);
			robot = Instantiate(robotPrefab, MapToWorldPos(game.RobotPosition), Quaternion.identity, this.transform);
		}

		//TODO : Use block clicks to update game state

		public void RunSimulation() {
			GameResult result = game.Run();
		
			//TODO : Visualize result
		}

		public Vector3 MapToWorldPos(Point pos) {
			return new Vector3(pos.X * spacing, 0.0f, pos.Y * spacing);
		}
	}
}
