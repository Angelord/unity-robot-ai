using System.Collections;
using System.Collections.Generic;
using MrRob.GameLogic;

namespace MrRob.GameLogic  {	
	public class RobotGame {

		private const int MAX_STEPS = 1000;

		private int width;
		private int length;
		private Robot robot;
		private Cargo cargo;
		private Point goalPos;
		private Tile[] tiles;

		public int Width { get { return width; } }
		public int Length { get { return length; } }
		public Robot Robot { get { return robot; } }
		public Cargo Cargo { get { return cargo; } }
		public Point GoalPosition { get { return goalPos; } }

		public RobotGame(int width, int length) {
			this.width = width;
			this.length = length;
			goalPos = new Point(width - 1, length - 1);

			tiles = new Tile[width * length];
			for(int y = 0; y < length; y++) {
				for(int x = 0; x < width; x++) {
					tiles[x + y * width] = new Tile(new Point(x, y));
				}
			}

			robot = new Robot(this);
			cargo = new Cargo(this) { Position = new Point(width / 2, length / 2) };
		}

		public GameResult Run() {
			GameResult result = new GameResult(this);

			int curStep = 0;
			do {
				robot.Step();
				result.LogFrame();
				curStep++;
			} while(!robot.Done && curStep < MAX_STEPS);

			return result;
		}

		public void ToggleBlocking(Point pos) {
			if(pos != cargo.Position) {
				Tile tile = GetTile(pos);
				tile.Blocked = !tile.Blocked;
			}
		}

		public bool TrySetCargoPos(Point pos) {
			if(GetTile(pos).Blocked) {
				return false;
			}
			cargo.Position = pos;
			return true;
		}

		public bool Contains(Point pos) {
            return (pos.X >= 0 && pos.Y >= 0 && pos.X < width && pos.Y < length);
		}

		public Tile GetTile(Point pos) {
			return tiles[pos.X + pos.Y * width];
		}
	}
}

