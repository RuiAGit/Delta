using UnityEngine.UI;
using UnityEngine;

namespace Delta {

    [CreateAssetMenu(fileName = "new Card", menuName = "Card")]
    
    public class Card : ScriptableObject {
        public string cardName;
        public int power;
        public int cost;
        public string description;
        public Sprite cardImage;
    }
}
