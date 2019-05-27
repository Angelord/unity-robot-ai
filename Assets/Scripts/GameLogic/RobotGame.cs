﻿using System.Collections;
using System.Collections.Generic;
using MrRob.GameLogic;

namespace MrRob.GameLogic  {	
	public class RobotGame {
		
		private int width;
		private int length;
		private Robot robot;
		private Point goalPos;
		private Tile[] tiles;
		//Treasure 

		public int Width { get { return width; } }
		public int Length { get { return length; } }
		public Point RobotPosition { get { return robot.Position; } }
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
		}

		public GameResult Run() {
			GameResult result = new GameResult(this);

			//do
			//	robot.step();
			//	goal.step();

			return result;
		}

		public Tile GetTile(Point pos) {
			return tiles[pos.X + pos.Y * width];
		}
	}
}
