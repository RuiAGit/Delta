using UnityEngine;

public class DeckPile : MonoBehaviour {

    public void OnDeckPileClicked() {
        Debug.Log("DRAW CARD");
        DeckManager deckManager = FindAnyObjectByType<DeckManager>();
        HandManager handManager = FindAnyObjectByType<HandManager>();
        deckManager.DrawCard(handManager);
    }
}
