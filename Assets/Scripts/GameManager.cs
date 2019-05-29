using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MrRob.GameLogic;
using MrRob.GUI;

namespace MrRob {
	public class GameManager : MonoBehaviour {

		private const float MIN_STEP_DURATION = 0.0025f;
		private const float MAX_STEP_DURATION = 5.0f;

		[SerializeField] private float spacing = 1.0f;
		[SerializeField] private GameObject tilePrefab;
		[SerializeField] private GameObject goalPrefab;
		[SerializeField] private GameObject robotPrefab;
		[SerializeField] private GameObject cargoPrefab;
		[SerializeField] private Vector2 minMaxCameraSz = new Vector2(6.0f, 12.5f);
		[SerializeField] private ResultsGUI resultsGui;
		
		private static GameManager instance;
		private RobotGame game; 
		private TileBlock[] tileBlocks;
		private GameObject robot;
		private GameObject cargo;
		private float stepDuration = 0.05f;

		public static GameManager Instance { get { return instance; } }

		private void Start() {
			if(instance == null) {
				instance = this;
			}
			else {
				Destroy(this);
				return;
			}
		}

		private void OnDestroy() {
			Destroy(this);
		}

		private void Update() {
			if(Input.GetKeyDown(KeyCode.Space)) {
				RunSimulation();
			}
			if(Input.GetButtonDown("Speed_Decr") && stepDuration > MIN_STEP_DURATION) {
				stepDuration *= 2.0f;
			}
			if(Input.GetButtonDown("Speed_Incr") && stepDuration < MAX_STEP_DURATION) {
				stepDuration /= 2.0f;
			}
		}

		public void Initialize(int width, int length) {

			Camera cam = Camera.main;
			if (cam != null) {
				//Set camera size to account for grid sie
				cam.orthographicSize = minMaxCameraSz.x +
				                       (float) (Mathf.Max(width, length) - RobotGame.MIN_DIMENSION) /
				                       (RobotGame.MAX_DIMENSION - RobotGame.MIN_DIMENSION) *
				                       (minMaxCameraSz.y - minMaxCameraSz.x);
			}

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
			if(!game.Over) {
				game.ToggleBlocking(pos);
				tileBlocks[pos.X + pos.Y * game.Width].SetPassable(!game.GetTile(pos).Blocked);
			}
		}

		public void OnTileRightClick(Point pos) {
			if(!game.Over && game.TrySetCargoPos(pos)) {
				cargo.transform.position = GridToWorldPos(pos);
			}
		}

		public void RunSimulation() {
			if(game.Over) {
				Reset();
				return;
			}

			foreach(TileBlock block in tileBlocks) {
				block.SetRevealed(false);
			}

			GameResult result = game.Run();
		
			Debug.Log(string.Format("Game Result : {0}. Message : {1}", result.Success, result.Message));

			StopAllCoroutines();
			StartCoroutine(ReplaySimulation(result));
		}

		public void Reset() {
			StopAllCoroutines();
			resultsGui.Hide();
			game.Reset();
			foreach(TileBlock block in tileBlocks) {
				block.SetRevealed(true);
			}
			robot.transform.position = GridToWorldPos(game.Robot.Position);
			robot.transform.rotation = DirToLookRotation(game.Robot.Orientation);
			cargo.transform.position = GridToWorldPos(game.Cargo.Position);
		}

		private IEnumerator ReplaySimulation(GameResult result) {

			foreach(GameResult.Frame frame in result.Frames) {
				yield return new WaitForSeconds(stepDuration);

				robot.transform.position = GridToWorldPos(frame.RobotPos);
				robot.transform.rotation = DirToLookRotation(frame.RobotOrientation);
				cargo.transform.position = GridToWorldPos(frame.CargoPos);

				foreach(Point reveal in frame.RevealedPositions) {
					tileBlocks[reveal.X + reveal.Y * game.Width].SetRevealed(true);
				}
			}
			
			resultsGui.ShowResults(result);
		}

		public Quaternion DirToLookRotation(Point direction) {
			return Quaternion.LookRotation(
					new Vector3(direction.X, 0.0f, direction.Y), 
					Vector3.up
					);
		}

		public Vector3 GridToWorldPos(Point pos) {
			return new Vector3((pos.X - game.Width / 2) * spacing, 0.0f, (pos.Y - game.Length / 2) * spacing);
		}
	}
}
