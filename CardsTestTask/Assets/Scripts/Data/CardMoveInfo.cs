using UnityEngine;

namespace CardsTeskTask.Data
{
    public class CardMoveInfo
    {
        public Transform TrToMove { get; }
        public Vector3 StartPos { get; private set; }
        public Vector3 DesiredPos { get; private set; }
        public float TotalTime { get; private set; }
        public float ElapsedTime { get; private set; }

        public CardMoveInfo(Transform tr, Vector3 startPos, Vector3 desiredPos, float totalTime)
        {
            TotalTime = totalTime;
            TrToMove = tr;
            DesiredPos = desiredPos;
            StartPos = startPos;
        }

        public void IncreaseElapsedTime(float newTime)
        {
            ElapsedTime += newTime;
        }

        public void UpdateDesiredPos(Vector3 newDesiredPos, bool isResetTime, float totalTime)
        {
            if (TrToMove.localPosition == DesiredPos)
            {
                StartPos = DesiredPos;
            }
            else
            {
                StartPos = TrToMove.localPosition;
            }

            TotalTime = totalTime;
            DesiredPos = newDesiredPos;

            if (isResetTime)
            {
                ElapsedTime = 0;
            }
        }
    }
}