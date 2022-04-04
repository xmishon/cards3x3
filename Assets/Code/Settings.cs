using UnityEngine;

namespace cards
{
    [CreateAssetMenu(fileName = "New Settings", menuName = "Settings")]
    public class Settings : ScriptableObject
    {
        public float verticalGap = 1.5f;
        public float horizontalGap = 1.1f;
        public int verticalCellsNumber = 3;
        public int horizontalCellsNumber = 3;
    }
}
