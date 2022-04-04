using TMPro;
using DG.Tweening;
using UnityEngine;


namespace cards
{
    public class Card
    {
        public int Health { get; protected set; }
        public GameObject View { get; private set; }
        public TextMeshPro HealthText { get; private set; }

        public Card(int health, GameObject view)
        {
            Health = health;
            View = view;
            HealthText = View.GetComponentInChildren<TextMeshPro>();
            HealthText.text = health.ToString();
        }

        public void Dispose(float duration)
        {
            View.transform?.DOScale(0.0f, duration).OnComplete(() => { Object.Destroy(View); });
        }
    }
}