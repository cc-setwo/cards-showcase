using System;
using UnityEngine;

namespace CardsTeskTask.Utils
{
    public class MouseCallbackReceiver : MonoBehaviour
    {
        private Action onDown;
        private Action onUp;

        public void Initialize(Action onDownCallback, Action onUpCallback)
        {
            onDown = onDownCallback;
            onUp = onUpCallback;
        }

        private void OnMouseUp()
        {
            onUp?.Invoke();
        }

        private void OnMouseDown()
        {
            onDown?.Invoke();
        }
    }
}