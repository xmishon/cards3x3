using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace cards
{
    public class UIController
    {
        private const string LEVEL = "Level: ";
        private const string DIFFICULTY = "Difficulty: ";

        private TextMeshProUGUI _levelTMP;
        private TextMeshProUGUI _difficultyTMP;
        private GameObject _uiRoot;

        public UIController()
        {
            _uiRoot = Object.Instantiate(Resources.Load<GameObject>("UI/UI"));
            Transform panel = _uiRoot.transform.GetChild(0);
            _levelTMP = panel.GetChild(0).GetComponent<TextMeshProUGUI>();
            if (_levelTMP == null)
            {
                Debug.Log("Failed to load level text");
            }
            _difficultyTMP = panel.GetChild(1).GetComponent<TextMeshProUGUI>();
            if (_difficultyTMP == null)
            {
                Debug.Log("Failed to load difficulty text");
            }
            Button closeButton = _uiRoot.GetComponentInChildren<Button>();
            closeButton.onClick.AddListener(CloseLevel);
        }

        public void UpdateLevel(int number)
        {
            _levelTMP.text = LEVEL + number.ToString();
        }

        public void UpdateDifficulty(int number)
        {
            _difficultyTMP.text = DIFFICULTY + number.ToString();
        }

        public void Dispose()
        {
            Object.Destroy(_uiRoot);
        }

        public void CloseLevel()
        {
            SceneManager.LoadScene(0);
        }
    }
}
