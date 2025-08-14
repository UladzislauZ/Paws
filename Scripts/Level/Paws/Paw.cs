using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace FruityPaw.Scripts.Level.Paws
{
    public class Paw : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Collider2D colliderPaw;
        
        public bool IsBot {get; private set; }
        public TypePaw TypePaw { get; private set; }
        public int RowIndexField { get; private set; }
        public int ColumnIndexField { get; private set; }
        public bool OnFreeze { get; private set; }

        public event Action<Paw> OnPawSelected;

        public void Initialize(TypePaw typePaw, Sprite sprite, bool isBot)
        {
            spriteRenderer.sprite = sprite;
            TypePaw = typePaw;
            IsBot = isBot;
        }

        public void UpdatePosition(Vector3 position, int rowIndex, int columnIndex)
        {
            transform.position = position;
            RowIndexField = rowIndex;
            ColumnIndexField = columnIndex;
        }

        public void ActivateColliderPaw(bool value)
        {
            colliderPaw.enabled = value;
        }

        public void UpdateLayout(int value)
        {
            spriteRenderer.sortingOrder = value;
        }

        public void Freeze()
        {
            OnFreeze = true;
            var color = spriteRenderer.color;
            color.a = 0.4f;
            spriteRenderer.color = color;
        }

        public void UnFreeze()
        {
            OnFreeze = false;
            var color = spriteRenderer.color;
            color.a = 1f;
            spriteRenderer.color = color;
        }

        public async UniTask MovePaw(Vector3 position, int rowIndex, int columnIndex)
        {
            transform.DOMove(position, 0.5f);
            await UniTask.Delay(500);
            RowIndexField = rowIndex;
            ColumnIndexField = columnIndex;
        }

        private void OnMouseDown()
        {
            OnPawSelected?.Invoke(this);
        }
    }
}