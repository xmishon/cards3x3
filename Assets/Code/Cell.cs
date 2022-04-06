using System;
using DG.Tweening;
using UnityEngine;
using Object = UnityEngine.Object;

namespace cards
{
    public class Cell : MonoBehaviour
    {
        public event Action<Cell> mouseClick;

        public Card card;
        public Point2 GridPosition { get; private set; }


        public void SetupCell(Vector2 transformPosition, Point2 gridPosition, Vector2 size)
        {
            transform.position = transformPosition;
            GridPosition = gridPosition;
            GetComponent<BoxCollider2D>().size = size;
        }

        public void AddCard(CardType cardType, int health)
        {
            GameObject cardView = Object.Instantiate(Resources.Load<GameObject>("Card"));
            switch (cardType)
            {
                case CardType.Green:
                    cardView.GetComponentOnlyInChildren<SpriteRenderer>().sprite = Resources.Load<Sprite>("GreenCardOverlay");
                    card = new Card(health, cardView);
                    break;
                case CardType.Red:
                    cardView.GetComponentOnlyInChildren<SpriteRenderer>().sprite = Resources.Load<Sprite>("RedCardOverlay");
                    card = new Card(-health, cardView);
                    break;
                default:
                    card = new PlayerCard(health, cardView);
                    break;
            }
            
            cardView.transform.position = transform.position;
            
            cardView.transform.parent = transform;
        }

        public void RemoveCard()
        {
            card.Dispose(0.4f);
            card = null;
        }

        public void MouseClick()
        {
            mouseClick?.Invoke(this);
        }

        public void Dispose()
        {
            RemoveCard();
            Destroy(gameObject);
        }
    }

    public static class MyExtensions
    {
        public static T GetComponentOnlyInChildren<T>(this GameObject gameObject)
        {
            var parentComponent = gameObject.GetComponent<T>();
            var components = gameObject.GetComponentsInChildren<T>();
            foreach (var component in components)
            {
                if (!component.Equals(parentComponent))
                    return component;
            }

            return default(T);
        }
    }
}