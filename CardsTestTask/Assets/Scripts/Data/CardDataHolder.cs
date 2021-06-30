using CardsTeskTask.Utils;
using UnityEngine;

namespace CardsTeskTask.Data
{
    public class CardDataHolder : Singleton<CardDataHolder>
    {
        public CardData Data => data;
        
        [SerializeField] private CardData data;
    }
}