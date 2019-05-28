using System.Collections.Generic;

namespace MrRob.GameLogic {
    public class GameResult {
    
        private RobotGame game;
        private bool[] lastFrameReveal;
        private List<Frame> frames = new List<Frame>();

        public List<Frame> Frames { get { return frames; } }

        public GameResult(RobotGame game) {
            this.game = game;
            this.lastFrameReveal = new bool[game.Width * game.Length];
        }
        
        public void LogFrame() {
            Frame newFrame = new Frame() {
                RobotPos = game.Robot.Position,
                RobotOrientation = game.Robot.Orientation
            };

            bool isRevealed = false;
            for(int y = 0; y < game.Length; y++) {
                for(int x = 0; x < game.Width; x++) {
                    isRevealed = game.Robot.TileIsRevealed(new Point(x, y));
                    if(lastFrameReveal[x + y * game.Width] != isRevealed) {
                        lastFrameReveal[x + y * game.Width] = isRevealed;
                        newFrame.AddReveal(new Point(x, y));
                    } 
                }
            }

            frames.Add(newFrame);
        }

        public class Frame {
            private List<Point> revealedPositions = new List<Point>();

            public Point RobotPos { get; set; }
            public Point RobotOrientation { get; set; }
            public List<Point> RevealedPositions { get { return revealedPositions; } }

            public void AddReveal(Point pos) {
                revealedPositions.Add(pos);
            }
        }
    }
}