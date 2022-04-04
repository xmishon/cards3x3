using System.Collections;
using UnityEngine;

namespace cards
{
    public class GameController : MonoBehaviour
    {
        private GridController _grid;
        private InputController _inputController;
        private int _difficultyLevel;

        private IEnumerator Start()
        {
            _difficultyLevel = 1;
            _inputController = new InputController(Camera.main);
            yield return new WaitForSeconds(0.2f);
            _difficultyLevel = 1;
            _grid = new GridController();
        }

        private void Update()
        {
            _inputController.Execute();
        }
    }
}