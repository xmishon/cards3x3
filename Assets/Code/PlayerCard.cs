using System;
using UnityEngine;

namespace cards
{
    public class PlayerCard : Card
    {
        public event Action die;

        public PlayerCard(int health, GameObject view) : base(health, view) { }

        public void ChangeHealth(int health)
        {
            Health += health;
            HealthText.text = Health.ToString();
            if (Health <= 0)
            {
                Die();
            }
        }

        public void Die()
        {
            die?.Invoke();
            Dispose();
        }
    }
}
