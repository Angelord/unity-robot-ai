using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MrRob.GameLogic;

namespace MrRob {
	public class GameManager : MonoBehaviour {

		private const float STEP_DURATION = 0.05f;

		[SerializeField] private float spacing = 1.0f;
		[SerializeField] private GameObject tilePrefab;
		[SerializeField] private GameObject goalPrefab;
		[SerializeField] private GameObject robotPrefab;
		[SerializeField] private GameObject cargoPrefab;

		private static GameManager instance;
		private RobotGame game; 
		private TileBlock[] tileBlocks;
		private GameObject robot;
		private GameObject cargo;

		public static GameManager Instance { get { return instance; } }

		private void Start() {
			if(instance == null) {
				instance = this;
			}
			else {
				Destroy(this);
				return;
			}

			Initialize(10, 10);
		}

		private void OnDestroy() {
			Destroy(this);
		}

		private void Update() {
			if(Input.GetKeyDown(KeyCode.Space)) {
				RunSimulation();
			}
		}

		public void Initialize(int width, int length) {

			game = new RobotGame(width, length);

			tileBlocks = new TileBlock[width * length];
			for(int y = 0; y < length; y++) {
				for(int x = 0; x < width; x++) {
					Point gridPos = new Point(x, y);

					Vector3 pos = GridToWorldPos(gridPos);
					GameObject newBlock = Instantiate(tilePrefab, pos, Quaternion.identity, this.transform);
					TileBlock blockComp = newBlock.GetComponent<TileBlock>();
					blockComp.Position = gridPos;
					blockComp.SetRevealed(true);
					tileBlocks[x + y * width] = blockComp;
				}
			}
			Instantiate(goalPrefab, GridToWorldPos(game.GoalPosition), Quaternion.identity, this.transform);
			robot = Instantiate(robotPrefab, GridToWorldPos(game.Robot.Position), Quaternion.identity, this.transform);
			cargo = Instantiate(cargoPrefab, GridToWorldPos(game.Cargo.Position), Quaternion.identity, this.transform);
		}

		public void OnTileClick(Point pos) {
			game.ToggleBlocking(pos);
			MeshRenderer blockRenderer = tileBlocks[pos.X + pos.Y * game.Width].GetComponent<MeshRenderer>();
			blockRenderer.enabled = !game.GetTile(pos).Blocked;
		}

		public void OnTileRightClick(Point pos) {
			if(game.TrySetCargoPos(pos)) {
				cargo.transform.position = GridToWorldPos(pos);
			}
		}

		public void RunSimulation() {
			foreach(TileBlock block in tileBlocks) {
				block.SetRevealed(false);
			}

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
				cargo.transform.position = GridToWorldPos(frame.CargoPos);

				foreach(Point reveal in frame.RevealedPositions) {
					tileBlocks[reveal.X + reveal.Y * game.Width].SetRevealed(true);
				}
			}
		}

		public Vector3 GridToWorldPos(Point pos) {
			return new Vector3(pos.X * spacing, 0.0f, pos.Y * spacing);
		}
	}
}
