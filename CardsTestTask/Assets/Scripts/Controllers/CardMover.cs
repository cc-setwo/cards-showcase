using CardsTeskTask.Data;
using UnityEngine;

namespace CardsTeskTask.Controllers
{
    public class CardMover : IMover
    {
        public void Move(CardMoveInfo info)
        {
            info.TrToMove.localPosition = Vector3.Lerp(info.StartPos, info.DesiredPos, info.ElapsedTime / info.TotalTime);
        }
    }
}