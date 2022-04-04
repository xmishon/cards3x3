using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace cards
{
    public class GridController
    {
        public int Level { get; set; }

        [SerializeField]
        private float _moveTime = 0.4f;

        private readonly List<Cell> _cells = new List<Cell>();
        private Settings _settings;
        private Cell _playerCardCell;
        private int moveCounter = 0;
        private bool _isMoving = false;
        public GridController()
        {
            DOTween.Init();
            Level = 1;
            _settings = Resources.Load<Settings>("Settings");
            float verticalStartPosition = (_settings.verticalCellsNumber - 1) * _settings.verticalGap / 2;
            float horizontalStartPosition = -(_settings.horizontalCellsNumber - 1) * _settings.horizontalGap / 2;
            for (int i = 0; i < _settings.horizontalCellsNumber; i++)
            {
                for (int j = 0; j < _settings.verticalCellsNumber; j++)
                {
                    float x = horizontalStartPosition + i * _settings.horizontalGap;
                    float y = verticalStartPosition - j * _settings.verticalGap;
                    GameObject cellGO = Object.Instantiate(Resources.Load<GameObject>("Cell"));
                    Cell cell = cellGO.GetComponent<Cell>();
                    cell.SetupCell(new Vector2(x, y), 
                        new Point2(i, j), 
                        new Vector2(_settings.horizontalGap, _settings.verticalGap)
                        );
                    cell.mouseClick += HandleClick;
                    _cells.Add(cell);
                }
            }
            foreach(Cell cell in _cells)
            {
                if (cell.GridPosition.X == _settings.horizontalCellsNumber/2
                    && cell.GridPosition.Y == _settings.verticalCellsNumber/2)
                {
                    cell.AddCard(CardType.Player, 1);
                    PlayerCard playerCard = (PlayerCard) cell.card;
                    playerCard.die += OnDie;
                    _playerCardCell = cell;
                }
                else
                {
                    cell.AddCard(RandomCardType(), 1 + Random.Range(0, Level));
                }
            }
        }

        private void HandleClick(Cell cell)
        {
            Debug.Log($"Mouse clicked on: {{{cell.GridPosition.X}}}, {{{cell.GridPosition.Y}}}");
            Debug.Log($"Card has a {cell.card.Health} health points");
            Debug.Log($"Is PlayerCard: {cell.card is PlayerCard}");
            Debug.Log($"Move is possible: {moveIsPossible(_playerCardCell, cell)}");
            if (moveIsPossible(_playerCardCell, cell))
            {
                Coroutines.StartRoutine(HandleMove(cell));
            }
        }

        private IEnumerator HandleMove(Cell cell)
        {
            _isMoving = true;
            //cell.card.View.transform.DOScale(1.0f, _moveTime); поместить эту строчку в другое место
            // надо проверку на возможность движения сделать внутри корутины, а не снаружи масштаб менять всегда,
            // а двигать карту, если движение возможно
            PlayerCard card = (PlayerCard)_playerCardCell.card;
            Card oldCard = cell.card;
            cell.RemoveCard();
            cell.card = _playerCardCell.card;
            cell.card.View.transform.parent = cell.transform;
            cell.card.View.transform.DOMove(cell.transform.position, _moveTime);

            yield return new WaitForSeconds(_moveTime);

            card.ChangeHealth(oldCard.Health);
            _isMoving = false;
            _playerCardCell.AddCard(RandomCardType(), 1 + Random.Range(0, Level));
            _playerCardCell = cell;
            moveCounter++;
            if (moveCounter % 10 == 0)
            {
                Level++;
            }
        }

        private CardType RandomCardType()
        {
            float probability = Level * 0.1f;
            probability = 1.0f;
            probability = Mathf.Clamp(probability, 0.0f, 0.6f);
            return Random.Range(0.0f, 1.0f) < probability ? CardType.Red : CardType.Green;
        }

        private void OnDie()
        {
            int n = DOTween.CompleteAll();
            Debug.Log($"You have died! {n} tweens killed. Level restarted");
            SceneManager.LoadScene(0);
        }
        public bool moveIsPossible(Cell start, Cell finish)
        {
            if (!_isMoving && start.GridPosition.X == finish.GridPosition.X && Mathf.Abs(start.GridPosition.Y - finish.GridPosition.Y) == 1)
                return true;
            else if (!_isMoving && start.GridPosition.Y == finish.GridPosition.Y && Mathf.Abs(start.GridPosition.X - finish.GridPosition.X) == 1)
                return true;
            return false;
        }

        private void Dispose()
        {
            foreach (var cell in _cells)
            {
                cell.Dispose();
            }
        }
    }
}