using UnityEngine;

namespace CardsTeskTask.Data
{
    [CreateAssetMenu(fileName = "CardsData", menuName = "CardsData", order = 0)]
    public class CardData : ScriptableObject
    {
        public float CardPickWaitTime => cardPickWaitTime;
        public float CardPickUpSpeed => cardPickUpSpeed;
        public float CardPickUpHeight => cardPickUpHeight;
        public float CardsMoveTime => cardsMoveTime;
        public float CardsFollowTime => cardsFollowTime;
        
        [SerializeField] private float cardPickWaitTime;
        [SerializeField] private float cardPickUpSpeed;
        [SerializeField] private float cardPickUpHeight;
        [SerializeField] private float cardsMoveTime;
        [SerializeField] [Range(0.001f, 2.0f)] private float cardsFollowTime;
    }
}