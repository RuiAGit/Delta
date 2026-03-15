using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Delta;

public class DeckManager : MonoBehaviour {

    public List<Card> allCards = new List<Card>();
    public int startingHandSize = 5;
    public int maxHandSize = 8;
    public int currentHandSize = 0;
    public int deckSize = 30;
    private int currentDeckSize = 30;
    private int currentIndex = 0;

    void Start() {
        List<Card> loadedCards = new List<Card>(Resources.LoadAll<Card>("Cards"));

        // Randomly select deckSize cards from loadedCards (with possible duplicates)
        for (int i = 0; i < deckSize; i++) {
            int randomIndex = Random.Range(0, loadedCards.Count);
            allCards.Add(loadedCards[randomIndex]);
        }

        HandManager hand = FindAnyObjectByType<HandManager>();
        for (int i = 0; i < startingHandSize; i++) {
            DrawCard(hand);
        }
    }

    public void DrawCard(HandManager handManager) {
        if (currentDeckSize <= 0 || currentHandSize >= maxHandSize) return;

        Card nexCard = allCards[currentIndex];
        handManager.AddCardToHand(nexCard);
        currentIndex = (currentIndex + 1) % allCards.Count;
        currentHandSize++;
        currentDeckSize--;
    }

    public void RemoveCardFromHand() {
        currentHandSize--;
    }
}
