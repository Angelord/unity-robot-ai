using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MrRob.GameLogic;

namespace MrRob {
	public class GameManager : MonoBehaviour {

		private const float STEP_DURATION = 0.4f;

		[SerializeField] private float spacing = 1.0f;
		[SerializeField] private GameObject tilePrefab;
		[SerializeField] private GameObject goalPrefab;
		[SerializeField] private GameObject robotPrefab;
		[SerializeField] private GameObject cargoPrefab;

		private RobotGame game; 
		private GameObject[] tileBlocks;
		private GameObject robot;
		private GameObject cargo;

		private void Start() {
			Initialize(10, 10);
		}

		private void Update() {
			if(Input.GetKeyDown(KeyCode.Space)) {
				RunSimulation();
			}
		}

		public void Initialize(int width, int length) {

			game = new RobotGame(width, length);

			tileBlocks = new GameObject[width * length];
			for(int y = 0; y < length; y++) {
				for(int x = 0; x < width; x++) {
					Vector3 pos = GridToWorldPos(new Point(x, y));
					tileBlocks[x + y * width] = Instantiate(tilePrefab, pos, Quaternion.identity, this.transform);
				}
			}
			Instantiate(goalPrefab, GridToWorldPos(game.GoalPosition), Quaternion.identity, this.transform);
			robot = Instantiate(robotPrefab, GridToWorldPos(game.Robot.Position), Quaternion.identity, this.transform);
			cargo = Instantiate(cargoPrefab, GridToWorldPos(game.Cargo.Position), Quaternion.identity, this.transform);
		}

		public void OnTileClick(Point pos) {
			game.ToggleBlocking(pos);
			tileBlocks[pos.X + pos.Y * game.Length].SetActive(game.GetTile(pos).Blocked);
		}

		public void OnTileRightClick(Point pos) {
			game.SetCargoPos(pos);
			cargo.transform.position = GridToWorldPos(pos);
		}

		public void RunSimulation() {
			GameResult result = game.Run();
		
			StopAllCoroutines();
			StartCoroutine(ReplaySimulation(result));
		}

		private IEnumerator ReplaySimulation(GameResult result) {

			foreach(GameResult.Frame frame in result.Frames) {
				yield return new WaitForSeconds(STEP_DURATION);

				robot.transform.position = GridToWorldPos(frame.RobotPos);
				robot.transform.rotation = Quaternion.LookRotation(
					new Vector3(frame.RobotOrientation.X, 0.0f, frame.RobotOrientation.Y), 
					Vector3.up
					);
			}
		}

		public Vector3 GridToWorldPos(Point pos) {
			return new Vector3(pos.X * spacing, 0.0f, pos.Y * spacing);
		}
	}
}
