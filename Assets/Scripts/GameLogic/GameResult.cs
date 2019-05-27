using System.Collections.Generic;

namespace MrRob.GameLogic {
    public class GameResult {
    
        private RobotGame game;
        private List<Frame> frames = new List<Frame>();
        //private bool success

        public GameResult(RobotGame game) {
            this.game = game;
        }
        
        public void LogFrame() {
            frames.Add(new Frame() {
                RobotPos = game.RobotPosition
            });
        }

        public class Frame {
            public Point RobotPos { get; set; }
        }
    }
}