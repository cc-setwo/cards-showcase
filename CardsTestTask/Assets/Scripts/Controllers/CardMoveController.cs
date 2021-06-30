using System;
using System.Collections;
using System.Collections.Generic;
using CardsTeskTask.Data;
using UnityEngine;

namespace CardsTeskTask.Controllers
{
    public class CardMoveController : MonoBehaviour
    {
        private Coroutine currentMoveCour;
        private Dictionary<Type, IMover> movers;
        private readonly List<CardMoveInfo> moveInfos = new List<CardMoveInfo>();

        public void Initialize()
        {
            movers = new Dictionary<Type, IMover>
            {
                {typeof(CardMoveInfo), new CardMover()},
                {typeof(CardMoveBezierInfo), new CardMoverBezier()}
            };
        }

        public CardMoveInfo Get(Transform tr)
        {
            return moveInfos.Find(a => a.TrToMove == tr);
        }

        public void Remove(CardMoveInfo moveInfo)
        {
            CardMoveInfo possibleExistingItem = moveInfos.Find(a => a.TrToMove == moveInfo.TrToMove);

            if (possibleExistingItem != null)
            {
                moveInfos.Remove(moveInfo);
            }
        }

        public void Add(CardMoveInfo newMoveInfo, bool isResetTime = true)
        {
            CardMoveInfo possibleExistingItem = moveInfos.Find(a => a.TrToMove == newMoveInfo.TrToMove);

            if (possibleExistingItem != null)
            {
                possibleExistingItem.UpdateDesiredPos(newMoveInfo.DesiredPos, isResetTime, newMoveInfo.TotalTime);
            }
            else
            {
                moveInfos.Add(newMoveInfo);
            }

            if (currentMoveCour == null)
            {
                currentMoveCour = StartCoroutine(MoveCoroutine());
            }
        }

        private IEnumerator MoveCoroutine()
        {
            while (moveInfos.Count > 0)
            {
                List<CardMoveInfo> finishedItems = new List<CardMoveInfo>();
                
                foreach (CardMoveInfo item in moveInfos)
                {
                    if (item.TrToMove == null)
                    {
                        finishedItems.Add(item);
                        continue;
                    }

                    if (item.ElapsedTime < item.TotalTime)
                    {
                        IMover currentMover = GetMover(item.GetType());
                        currentMover.Move(item);
                        item.IncreaseElapsedTime(Time.deltaTime);
                    }
                    else
                    {
                        finishedItems.Add(item);
                    }
                }

                for (int i = 0; i < finishedItems.Count; i++)
                {
                    if (finishedItems[i].TrToMove == null)
                    {
                        moveInfos.Remove(finishedItems[i]);
                        continue;
                    }

                    finishedItems[i].TrToMove.localPosition = finishedItems[i].DesiredPos;
                    moveInfos.Remove(finishedItems[i]);
                }

                yield return null;
            }

            if (moveInfos.Count == 0)
            {
                currentMoveCour = null;
            }
        }

        private IMover GetMover(Type forType)
        {
            IMover mover = movers[forType];

            if (mover == null)
            {
                throw new Exception($"CardMoveController -> CardMoveController Can not find mover for type: {forType}");
            }

            return mover;
        }
    }
}