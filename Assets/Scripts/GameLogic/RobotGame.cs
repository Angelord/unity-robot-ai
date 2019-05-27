using System.Collections;
using System.Collections.Generic;
using MrRob.GameLogic;

namespace MrRob.GameLogic  {	
	public class RobotGame {
		
		private int width;
		private int length;
		private Robot robot;
		private Point goalPos;
		//Treasure 
		//Tile[,]

		public int Width { get { return width; } }
		public int Length { get { return length; } }
		//public Treasure treasure { get; }
		public Point RobotPosition { get { return robot.Position; } }
		public Point GoalPosition { get { return goalPos; } }

		public RobotGame(int width, int length) {
			this.width = width;
			this.length = length;
			goalPos = new Point(width - 1, length - 1);


			robot = new Robot(this);
		}

		public GameResult Run() {
			return new GameResult();
		}
	}
}

