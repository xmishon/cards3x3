using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace cards
{
    public class GameOverUI : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _text;

        private void Awake()
        {
            SaveDataItem savedData = SaveData.Load();
            _text.text = $"GameOver\r\nBest record: {savedData.levelRecord}\r\nCurrent result: {savedData.lastRecord}";
        }

        public void Play()
        {
            SceneManager.LoadScene(1);
        }

        public void Close()
        {
            SceneManager.LoadScene(0);
        }
    }
}
