using CardsTeskTask.Data;
using UnityEngine;

namespace CardsTeskTask.Controllers
{
    public class CardMoverBezier : IMover
    {
        public void Move(CardMoveInfo info)
        {
            CardMoveBezierInfo moveBezierInfo = (CardMoveBezierInfo) info;
            moveBezierInfo.TrToMove.localPosition = GetBezierPoint(moveBezierInfo.StartPos, moveBezierInfo.ControlPos, moveBezierInfo.DesiredPos, moveBezierInfo.ElapsedTime / moveBezierInfo.TotalTime);
        }

        private Vector2 GetBezierPoint(Vector2 start, Vector2 control, Vector2 end, float t)
        {
            return (1 - t) * (1 - t) * start + 2 * t * (1 - t) * control + t * t * end;
        }
    }
}