using System;
using CardsTeskTask.Utils;
using UnityEngine;

namespace CardsTeskTask.Data
{
    public class CardHolder : MonoBehaviour
    {
        public Transform Tr => tr;
        public Bounds Bounds => sprite.bounds;
        
        [SerializeField] private Transform tr;
        [SerializeField] private SpriteRenderer sprite;
        [SerializeField] private MouseCallbackReceiver mouseCallback;
        
        private Action<CardHolder> onDownCallback;
        private Action<CardHolder> onUpCallback;

        public void Initialize(Action<CardHolder> downCallback, Action<CardHolder> upCallback)
        {
            onDownCallback = downCallback;
            onUpCallback = upCallback;
            mouseCallback.Initialize(MouseDown, MouseUp);
        }

        public void SetSortOrder(int newOrder, float zOrder)
        {
            sprite.transform.localPosition = new Vector3(sprite.transform.localPosition.x, sprite.transform.localPosition.y, zOrder);
            sprite.sortingOrder = newOrder;
        }

        public void ResetCallbacks()
        {
            onDownCallback = null;
            onUpCallback = null;
        }

        private void MouseDown()
        {
            onDownCallback?.Invoke(this);
        }

        private void MouseUp()
        {
            onUpCallback?.Invoke(this);
        }
    }
}