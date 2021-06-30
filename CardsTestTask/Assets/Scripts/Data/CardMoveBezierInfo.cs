using UnityEngine;

namespace CardsTeskTask.Data
{
    public class CardMoveBezierInfo : CardMoveInfo
    {
        public Vector3 ControlPos { get; }
        
        public CardMoveBezierInfo(Transform tr, Vector3 startPos, Vector3 controlPos, Vector3 desiredPos, float totalTime) : base(tr, startPos, desiredPos, totalTime)
        {
            ControlPos = controlPos;
        }
    }
}