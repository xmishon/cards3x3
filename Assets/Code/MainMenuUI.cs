using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace cards
{
    public class MainMenuUI : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _text;

        private void Awake()
        {
            SaveDataItem savedData = SaveData.Load();
            _text.text = $"Best record: {savedData.levelRecord}\r\nLast result: {savedData.lastRecord}";
        }

        public void Play()
        {
            SceneManager.LoadScene(1);
        }

        public void Close()
        {
            Application.Quit();
        }
    }
}
