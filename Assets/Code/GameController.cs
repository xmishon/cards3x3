using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;
using Object = UnityEngine.Object;

namespace cards
{
    public class GameController
    {
        public event Action<int> levelChanged;
        public event Action<int> moveCountChanged;

        public int Level
        {
            get
            {
                return m_Level;
            }
            set
            {
                m_Level = value;
                levelChanged?.Invoke(m_Level);
            }
        }

        public int MoveCount
        {
            get
            {
                return m_moveCount;
            }
            set
            {
                m_moveCount = value;
                moveCountChanged?.Invoke(m_moveCount);
            }
        }

        [SerializeField]
        private float _moveTime = 0.4f;

        private readonly List<Cell> _cells = new List<Cell>();
        private Settings _settings;
        private Cell _playerCardCell;
        private int m_moveCount = 0;
        private bool _isMoving = false;
        private int m_Level;
        private int _previousRecord;
        public GameController()
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
            _previousRecord = SaveData.Load().levelRecord;
        }

        private void HandleClick(Cell cell)
        {
            Debug.Log($"Mouse clicked on: {{{cell.GridPosition.X}}}, {{{cell.GridPosition.Y}}}");
            Debug.Log($"Card has a {cell.card.Health} health points");
            Debug.Log($"Is PlayerCard: {cell.card is PlayerCard}");
            Debug.Log($"Move is possible: {moveIsPossible(_playerCardCell, cell)}");
            Coroutines.StartRoutine(HandleMove(cell));
        }

        private IEnumerator HandleMove(Cell cell)
        {
            cell.card.View.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
            cell.card.View.transform.DOScale(1.0f, _moveTime); 
            if (moveIsPossible(_playerCardCell, cell))
            {
                _isMoving = true;
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
                MoveCount++;
                if (MoveCount % 10 == 0)
                {
                    Level++;
                }
            }
        }

        private CardType RandomCardType()
        {
            float probability = Level * 0.1f;
            probability = Mathf.Clamp(probability, 0.0f, 0.5f);
            return Random.Range(0.0f, 1.0f) < probability ? CardType.Red : CardType.Green;
        }

        private void OnDie()
        {
            int n = DOTween.CompleteAll();
            if (MoveCount > _previousRecord)
            {
                SaveData.Save(new SaveDataItem(MoveCount, MoveCount));
            }
            else
            {
                SaveData.Save(new SaveDataItem(_previousRecord, MoveCount));
            }
            SceneManager.LoadScene(2);
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
            levelChanged = null;
            moveCountChanged = null;
        }
    }
}