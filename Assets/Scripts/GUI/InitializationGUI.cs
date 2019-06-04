using System;
using UnityEngine;
using UnityEngine.UI;
using MrRob.GameLogic;

namespace MrRob.GUI {

    public class InitializationGUI : MonoBehaviour {

        [SerializeField] private Slider widthSlider;
        [SerializeField] private Slider lengthSlider;
        [SerializeField] private Text widthLabel;
        [SerializeField] private Text lengthLabel;
        [SerializeField] private GameManager manager;
        
        private int WidthVal {
            get { return Mathf.RoundToInt(widthSlider.value); }
        }

        private int LengthVal {
            get { return Mathf.RoundToInt(lengthSlider.value); }
        }

        private void Start() {
            widthSlider.minValue = lengthSlider.minValue = RobotGame.MIN_DIMENSION;
            widthSlider.maxValue = lengthSlider.maxValue = RobotGame.MAX_DIMENSION;
            widthSlider.value = lengthSlider.value = RobotGame.MIN_DIMENSION + (RobotGame.MAX_DIMENSION - RobotGame.MIN_DIMENSION) / 2.0f;
            
            OnWidthChange();
            OnLengthChange();
        }

        public void OnClick() {
            manager.Initialize(WidthVal, LengthVal);
            this.gameObject.SetActive(false);
        }

        public void OnWidthChange() {
            widthLabel.text = String.Format("Width {0}", WidthVal);
        }

        public void OnLengthChange() {
            lengthLabel.text = String.Format("Length {0}", LengthVal);
        }
    }
}
