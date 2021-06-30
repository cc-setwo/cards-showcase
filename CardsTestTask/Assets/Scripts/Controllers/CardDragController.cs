using System;
using System.Collections;
using CardsTeskTask.Data;
using UnityEngine;

namespace CardsTeskTask.Controllers
{
    public class CardDragController : MonoBehaviour
    {
        private CardHolder currentCard;
        private Vector3 cardOffset;
        private Vector3 cardDragStartPos;
        private Coroutine currentCoroutine;
        private Coroutine currentWaitCoroutine;
        private Action<CardHolder, Vector3, bool> onCardDrag;

        public void Initialize(Action<CardHolder, Vector3, bool> cardDrag)
        {
            onCardDrag = cardDrag;
        }

        public void RegisterCard(CardHolder holder)
        {
            holder.Initialize(OnCardPressed, OnCardReleased);
        }

        public void RemoveCard(CardHolder holder)
        {
            holder.ResetCallbacks();
        }

        public void UpdateStartPos(Vector3 newStartDragPos)
        {
            cardDragStartPos = newStartDragPos;
        }

        private void OnCardPressed(CardHolder holder)
        {
            if (currentCard != null)
            {
                return;
            }

            if (currentWaitCoroutine != null)
            {
                StopCoroutine(currentWaitCoroutine);
            }

            currentWaitCoroutine = holder.StartCoroutine(WaitAndAssignCard(holder));
        }

        private void OnCardReleased(CardHolder holder)
        {
            currentWaitCoroutine = null;
            currentCard = null;
            holder.StopAllCoroutines();
            onCardDrag.Invoke(holder, cardDragStartPos, true);
        }

        private IEnumerator WaitAndAssignCard(CardHolder possibleNewCard)
        {
            yield return new WaitForSeconds(CardDataHolder.Instance.Data.CardPickWaitTime);
            currentCard = possibleNewCard;
            cardDragStartPos = currentCard.Tr.localPosition;
            possibleNewCard.StartCoroutine(MoveCardY(CardDataHolder.Instance.Data.CardPickUpHeight));
        }

        private IEnumerator DragCard()
        {
            while (currentCard != null)
            {
                onCardDrag.Invoke(currentCard, cardDragStartPos, false);
                currentCard.Tr.localPosition = GetMousePosInCanvas() + cardOffset;
                yield return null;
            }
        }

        private IEnumerator MoveCardY(float yPos)
        {
            float elapsedTime = 0.0f;
            Vector3 startPos = currentCard.Tr.localPosition;
            Vector3 finalPos = new Vector3(startPos.x, startPos.y + yPos, startPos.z);
            
            while (elapsedTime < CardDataHolder.Instance.Data.CardPickUpSpeed)
            {
                currentCard.Tr.localPosition = Vector3.Lerp(startPos, finalPos, elapsedTime / CardDataHolder.Instance.Data.CardPickUpSpeed);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            currentCard.Tr.localPosition = finalPos;
            cardOffset = currentCard.Tr.localPosition - GetMousePosInCanvas();
            currentCard.StartCoroutine(DragCard());
        }

        private Vector3 GetMousePosInCanvas()
        {
            return Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }
}