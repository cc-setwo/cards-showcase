using CardsTeskTask.Data;

namespace CardsTeskTask.Controllers
{
    public interface IMover
    {
        void Move(CardMoveInfo info);
    }
}