using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Delta;

public class DeckManager : MonoBehaviour {

    public List<Card> allCards = new List<Card>();
    public int startingHandSize = 5;
    public int maxHandSize = 8;
    public int currentHandSize = 0;
    private int currentIndex = 0;

    void Start() {
        Card[] cards = Resources.LoadAll<Card>("Cards");

        allCards.AddRange(cards);

        HandManager hand = FindAnyObjectByType<HandManager>();
        for (int i = 0; i < startingHandSize; i++) {
            DrawCard(hand);
        }
    }

    public void DrawCard(HandManager handManager) {
        if (allCards.Count == 0 || currentHandSize >= maxHandSize) return;

        Card nexCard = allCards[currentIndex];
        handManager.AddCardToHand(nexCard);
        currentIndex = (currentIndex + 1) % allCards.Count;
        currentHandSize++;
    }
}
