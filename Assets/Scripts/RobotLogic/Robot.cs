using System.Collections.Generic;
using UnityEngine;

namespace MrRob.RobotLogic {
    public class Robot {

        private Map map;
        private GameObject instance;
        private Dictionary<string, RobotState> states = new Dictionary<string, RobotState>();
        private bool treasureFound;
        private bool[] tilesRevealed;

        public Map Map { get { return map; } }
        public bool TreasureFound { get { return treasureFound; } }

        public Robot(Map map, GameObject prefab) {
            this.map = map;
            tilesRevealed = new bool[map.Width * map.Length];

            instance = GameObject.Instantiate(prefab, map.MapToWorldPos(Point.ZERO), Quaternion.identity);

            states.Add("Waiting", instance.AddComponent<State_Waiting>());

            EnterState("Waiting");
        }

        public void SetTileRevealed(Point pos) {
            tilesRevealed[pos.X + pos.Y * map.Width] = true;
        } 

        public bool TileIsRevealed(Point pos) {
            return tilesRevealed[pos.X + pos.Y * map.Width];
        }
    
        public void EnterState(string stateName) {
            foreach(var state in states) {
                state.Value.enabled = false;
            }

            states[stateName].enabled = true;
        }
    }
}