using System.Collections.Generic;

namespace MrRob.GameLogic {
    public class GameResult {
    
        private RobotGame game;
        private List<Frame> frames = new List<Frame>();

        public List<Frame> Frames { get { return frames; } }

        public GameResult(RobotGame game) {
            this.game = game;
        }
        
        public void LogFrame() {
            frames.Add(new Frame() {
                RobotPos = game.Robot.Position,
                RobotOrientation = game.Robot.Orientation
            });
        }

        public class Frame {
            public Point RobotPos { get; set; }
            public Point RobotOrientation { get; set; }
        }
    }
}