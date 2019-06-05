using System.Collections;
using UnityEngine;
using MrRob.GameLogic;
using MrRob.GUI;
using MrRob.Objects;

namespace MrRob {
	public class GameManager : MonoBehaviour {

		private const float MIN_STEP_DURATION = 0.0025f;
		private const float MAX_STEP_DURATION = 2.0f;

		[SerializeField] private float spacing = 1.0f;
		[SerializeField] private GameObject tilePrefab;
		[SerializeField] private GameObject goalPrefab;
		[SerializeField] private GameObject robotPrefab;
		[SerializeField] private GameObject cargoPrefab;
		[SerializeField] private Vector2 minMaxCameraSz = new Vector2(6.0f, 12.5f);
		[SerializeField] private ObjectStateMachine gui;
		
		private static GameManager instance;
		private RobotGame game = null; 
		private TileBlock[] tileBlocks;
		private MovingObject robot;
		private MovingObject cargo;
		private GameObject goal;
		private GameResult lastResult;
		private bool replaying = false;

		public static GameManager Instance { get { return instance; } }
		public bool Initialized { get { return game != null; } }
		public bool Replaying { get { return replaying; } }
		public bool GameOver { get { return game.Over; } }
		public bool ExploreFirst { get { return game.Robot.ExploreFirst; } set { game.Robot.ExploreFirst = value; } }

		private float StepDuration {
			get { return robot.MoveDuration; }
			set { robot.MoveDuration = cargo.MoveDuration = value; }
		}

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
			if (instance == this) {
				instance = null;
			}
		}

		public bool IncreaseSpeed() {
			if(StepDuration > MIN_STEP_DURATION) {
				StepDuration /= 2.0f;
				return true;
			}
			return false;
		}

		public bool DecreaseSpeed() {
			if(StepDuration < MAX_STEP_DURATION) {
				StepDuration *= 2.0f;
				return true;
			}
			return false;
		}

		public void Quit() {
			Application.Quit();
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
			goal = Instantiate(goalPrefab, GridToWorldPos(game.GoalPosition), Quaternion.identity, this.transform);
			robot = Instantiate(robotPrefab, GridToWorldPos(game.Robot.Position), Quaternion.identity, this.transform).GetComponent<MovingObject>();
			cargo = Instantiate(cargoPrefab, GridToWorldPos(game.Cargo.Position), Quaternion.identity, this.transform).GetComponent<MovingObject>();
			
			gui.EnterState("SearchSettings");
		}
		
		public void OnTileClick(Point pos) {
			if(!game.Over) {
				game.ToggleBlocking(pos);
				tileBlocks[pos.X + pos.Y * game.Width].SetPassable(!game.GetTile(pos).Blocked);
			}
		}

		public void OnTileRightClick(Point pos) {
			
			if(Input.GetKey(KeyCode.G)) {
				if (!game.Over && !game.GetTile(pos).Blocked) {
					game.GoalPosition = pos;
					goal.transform.position = GridToWorldPos(pos);
				}
			}
			else {
				if(!game.Over && game.TrySetCargoPos(pos)) {
					cargo.transform.position = GridToWorldPos(pos);
				}
			}
		}

		public void RunSimulation() {

			if (game.Over) {
				if (replaying) {
					StopAllCoroutines();
					replaying = false;
				}
				Reset();
			}

			gui.EnterState("Search");
			
			foreach(TileBlock block in tileBlocks) {
				block.SetRevealed(false);
			}

			lastResult = game.Run();
		
			#if UNITY_EDITOR
			Debug.Log(string.Format("Game Result : {0}. Message : {1}", lastResult.Success, lastResult.Message));
			#endif
			
			StopAllCoroutines();
			StartCoroutine(ReplaySimulation(lastResult));
		}

		public void Reset() {
			StopAllCoroutines();
			
			gui.EnterState("SearchSettings");
			
			game.Reset();
			foreach(TileBlock block in tileBlocks) {
				block.SetRevealed(true);
			}
			robot.transform.position = GridToWorldPos(game.Robot.Position);
			robot.transform.rotation = DirToLookRotation(game.Robot.Orientation);
			cargo.transform.position = GridToWorldPos(game.Cargo.Position);
		}

		public void SkipToEnd() {
			if (!replaying) { return; }

			StopAllCoroutines();

			GameResult.Frame lastFrame = lastResult.LastFrame;
			robot.MoveInstant(GridToWorldPos(lastFrame.RobotPos));
			robot.transform.rotation = DirToLookRotation(lastFrame.RobotOrientation);
			cargo.MoveInstant(GridToWorldPos(lastFrame.CargoPos));
			
			for (int y = 0; y < game.Length; y++) {
				for (int x = 0; x < game.Width; x++) {
					tileBlocks[x + y * game.Width].SetRevealed(lastResult.TileWasRevealed(new Point(x, y)));
				}
			}

			ShowResults(lastResult);
			replaying = false;
		}

		private IEnumerator ReplaySimulation(GameResult result) {

			replaying = true;
			foreach(GameResult.Frame frame in result.Frames) {

				robot.Move(GridToWorldPos(frame.RobotPos));
				robot.transform.rotation = DirToLookRotation(frame.RobotOrientation);
				cargo.Move(GridToWorldPos(frame.CargoPos));

				foreach(Point reveal in frame.RevealedPositions) {
					tileBlocks[reveal.X + reveal.Y * game.Width].SetRevealed(true);
				}
				
				yield return new WaitUntil(() => (!robot.Moving && !cargo.Moving));
			}
			
			ShowResults(result);
			replaying = false;
		}
		
		private void ShowResults(GameResult result) {
			gui.EnterState("Results");
			gui.CurState.GetComponent<ResultsGUI>().ShowResults(result);
			
		}
		
		private Quaternion DirToLookRotation(Point direction) {
			return Quaternion.LookRotation(
					new Vector3(direction.X, 0.0f, direction.Y), 
					Vector3.up
					);
		}

		private Vector3 GridToWorldPos(Point pos) {
			return new Vector3((pos.X - game.Width / 2) * spacing, 0.0f, (pos.Y - game.Length / 2) * spacing);
		}
		
	}
}
