using System;
using System.Collections;
using UnityEngine;

namespace cards
{
    public class PlayerCard : Card
    {
        public event Action die;

        private float _duration;

        public PlayerCard(int health, GameObject view) : base(health, view) { }

        public void ChangeHealth(int health)
        {
            Health += health;
            HealthText.text = Health.ToString();
            if (Health <= 0)
            {
                Coroutines.StartRoutine(Die(_duration));
            }
        }

        public IEnumerator Die(float duration)
        {
            Dispose(duration);
            yield return new WaitForSeconds(duration);
            die?.Invoke();
        }
    }
}
