using System.Collections.Generic;
using System.Linq;
using CardsTeskTask.Data;
using CardsTeskTask.Utils;
using UnityEngine;

namespace CardsTeskTask.Controllers
{
    public class CardGroupController : MonoBehaviour
    {
        [SerializeField] private Transform parent;
        [SerializeField] private CardHolder cardPrefab;
        [SerializeField] private CardMoveController moveController;
        [SerializeField] private CardDragController dragController;

        private Bounds camBounds;
        private Pool<CardHolder> cardsPool;
        private readonly List<CardHolder> spawnedCards = new List<CardHolder>();

        private void Start()
        {
            cardsPool = new Pool<CardHolder>(cardPrefab, 25);
            camBounds = GetOrthographicBounds(Camera.main);
            dragController.Initialize(OnCardDrag);
            moveController.Initialize();
        }

        private void OnCardDrag(CardHolder holder, Vector3 cardDragStartPos, bool isDragFinished)
        {
            List<CardHolder> sortedCards = spawnedCards.OrderBy(a => a.Tr.localPosition.x).ToList();
            int newIndex = sortedCards.IndexOf(holder);
            int oldIndex = spawnedCards.IndexOf(holder);

            if (newIndex == oldIndex)
            {
                holder.Tr.localPosition = cardDragStartPos;
                
                for (int i = 0; i < spawnedCards.Count; i++)
                {
                    spawnedCards[i].Tr.SetSiblingIndex(i);
                    spawnedCards[i].SetSortOrder(i, -i / (float) spawnedCards.Count);
                }

                if (!isDragFinished)
                {
                    holder.Tr.SetAsLastSibling();
                    int newOrderIndex = holder.Tr.GetSiblingIndex() + 1;
                    holder.SetSortOrder(newOrderIndex, -newOrderIndex / (float) spawnedCards.Count);
                }
                else
                {
                    for (int i = 0; i < spawnedCards.Count; i++)
                    {
                        spawnedCards[i].Tr.localPosition = new Vector3(spawnedCards[i].Tr.localPosition.x, 0.0f, spawnedCards[i].Tr.localPosition.z);
                    }
                }
                
                return;
            }

            Vector3 followCardStartPos = spawnedCards[newIndex].Tr.localPosition;
            Vector3 followCardFinalPos = new Vector3(cardDragStartPos.x, 0.0f, cardDragStartPos.z);
            CardMoveInfo newFollowCardInfo = new CardMoveBezierInfo(spawnedCards[newIndex].Tr, followCardStartPos, holder.Tr.localPosition,followCardFinalPos, CardDataHolder.Instance.Data.CardsFollowTime);
            CardMoveInfo oldFollowCardInfo = moveController.Get(spawnedCards[newIndex].Tr);
            
            if (oldFollowCardInfo == null)
            {
                moveController.Add(newFollowCardInfo);
            }
            else
            {
                moveController.Remove(oldFollowCardInfo);
                oldFollowCardInfo.TrToMove.localPosition = new Vector3(oldFollowCardInfo.DesiredPos.x, 0.0f, oldFollowCardInfo.DesiredPos.z);
            }

            Vector3 newSelectedCardPos = spawnedCards[newIndex].Tr.localPosition;
            
            if (oldFollowCardInfo != null)
            {
                spawnedCards[newIndex].Tr.localPosition = new Vector3(cardDragStartPos.x, 0.0f, cardDragStartPos.z);
                holder.Tr.localPosition = new Vector3(newSelectedCardPos.x, 0.0f, newSelectedCardPos.z);
            }

            CardHolder tempCardIndex = spawnedCards[newIndex];
            spawnedCards[newIndex] = holder;
            spawnedCards[oldIndex] = tempCardIndex;

            dragController.UpdateStartPos(newSelectedCardPos);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                AddCard();
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                RemoveCard();
            }
        }

        private void AddCard()
        {
            CardHolder newCard = cardsPool.Get();
            newCard.Tr.SetParent(parent);
            newCard.Tr.localScale = Vector3.one;
            newCard.Tr.localPosition = Vector3.zero;
            spawnedCards.Add(newCard);
            
            int currentOrder = newCard.Tr.GetSiblingIndex();
            newCard.SetSortOrder(currentOrder, -currentOrder / (float) spawnedCards.Count);
            
            UpdateCardsPosition();
        }

        private void RemoveCard()
        {
            if (spawnedCards.Count == 0)
            {
                return;
            }

            CardHolder cardToReturn = spawnedCards[spawnedCards.Count - 1];
            cardToReturn.Tr.SetParent(null);
            cardsPool.Return(cardToReturn);
            dragController.RemoveCard(cardToReturn);
            spawnedCards.Remove(cardToReturn);
            UpdateCardsPosition();
        }

        private void UpdateCardsPosition()
        {
            if (spawnedCards.Count == 0)
            {
                return;
            }

            float middle = spawnedCards.Count * -0.5f;
            float moveMultiplier = 1.0f;
            float allowedCardsSize = spawnedCards[0].Bounds.size.x / 2;
            float allowedCardsAmount = camBounds.min.x / allowedCardsSize + 1;

            if (middle < allowedCardsAmount)
            {
                moveMultiplier = allowedCardsAmount / middle;
            }

            float halfCardSize = spawnedCards[0].Bounds.size.x * (0.5f * moveMultiplier);
            float quarterCardSize = spawnedCards[0].Bounds.size.x * (0.25f * moveMultiplier);

            for (int i = 0; i < spawnedCards.Count; i++)
            {
                int previousCardIndex = i > 0 ? i - 1 : i;
                Vector3 finalLocalPos = new Vector3(middle * halfCardSize + quarterCardSize, spawnedCards[i].Tr.localPosition.y, spawnedCards[i].Tr.localPosition.z);

                CardMoveInfo newInfo = new CardMoveInfo(spawnedCards[i].Tr, spawnedCards[previousCardIndex].Tr.localPosition, finalLocalPos, CardDataHolder.Instance.Data.CardsMoveTime);
                moveController.Add(newInfo);
                dragController.RegisterCard(spawnedCards[i]);
                middle++;
            }
        }
        
        private Bounds GetOrthographicBounds(Camera cam)
        {
            float aspect = Screen.width / (float) Screen.height;
            float cameraHeight = cam.orthographicSize * 2;
            return new Bounds(cam.transform.position, new Vector3(cameraHeight * aspect, cameraHeight, 0));
        }
    }
}