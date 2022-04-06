using System.Collections;
using UnityEngine;

namespace cards
{
    public class GameInitializer : MonoBehaviour
    {
        [SerializeField]
        private int _pixelsPerUnit = 467;
        
        private GameController _grid;
        private InputController _inputController;
        private UIController _uiController;
        private float _cameraSizeMultiplier = 3.0f;

        private void Start()
        {
            _inputController = new InputController(Camera.main);
            _grid = new GameController();
            _uiController = new UIController();
            _grid.moveCountChanged += _uiController.UpdateLevel;
            _grid.levelChanged += _uiController.UpdateDifficulty;
            ChangeCameraSize();
        }

        private void ChangeCameraSize()
        {
            Resolution currentResolution = Screen.currentResolution;
            int screenSizeMin = Mathf.Min(currentResolution.width, currentResolution.height);
            Camera.main.orthographicSize = (float)screenSizeMin / (float)_pixelsPerUnit * 0.5f * _cameraSizeMultiplier;
        }

        private void Update()
        {
            _inputController.Execute();
        }
    }
}