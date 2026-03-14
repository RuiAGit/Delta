using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Delta;

public class HandManager : MonoBehaviour {
    public GameObject cardPrefab; // Assign card prefab in inspector
    public Transform handTransform; // Root of the hand position
    public List<GameObject> cardsInHand = new List<GameObject>(); // List of cards in hand
    public float fanSpread = 5f; // How match the hand is spread out
    public float cardSpacing = -125f;
    public float verticalSpacing = 22f;
    
    // Dictionary to store original positions, rotations, and scales for each card
    private Dictionary<GameObject, (Vector3 position, Quaternion rotation, Vector3 scale)> cardOriginalTransforms = new Dictionary<GameObject, (Vector3, Quaternion, Vector3)>();
 

    void Start() {

    }

    void Update() {
        // UpdateHandVisuals();
    }

    public void AddCardToHand(Card cardData) {
        GameObject newCard = Instantiate(cardPrefab, handTransform.position, Quaternion.identity, handTransform);
        cardsInHand.Add(newCard);

        newCard.GetComponent<CardDisplay>().cardData = cardData;

        UpdateHandVisuals();
    }

    // TODO remove card from hand when played and update visuals
    public void RemoveCardFromHand(GameObject card) {
        cardsInHand.Remove(card);
        cardOriginalTransforms.Remove(card);
        UpdateHandVisuals();
    }

    public void UpdateHandVisuals() {
        int cardCount = cardsInHand.Count;

        if (cardCount == 1) {
            cardsInHand[0].transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            cardsInHand[0].transform.localPosition = new Vector3(0f, 0f, 0f);
            StoreCardOriginalTransform(cardsInHand[0]);
        } else {
            for (int i = 0; i < cardCount; i++) {
                float rotationAngle = fanSpread * (i - (cardCount - 1) / 2f);
                cardsInHand[i].transform.localRotation = Quaternion.Euler(0f, 0f, rotationAngle);

                float horizontalOffset = cardSpacing * (i - (cardCount - 1) / 2f);

                float normalizedPosition = 2f * i / (cardCount - 1) - 1f;
                float verticalOffset = verticalSpacing * (1 - normalizedPosition * normalizedPosition);

                // Set card position
                cardsInHand[i].transform.localPosition = new Vector3(horizontalOffset, verticalOffset, 0f);
                StoreCardOriginalTransform(cardsInHand[i]);
            }

        }
    }

    // Store the original transform of a card
    public void StoreCardOriginalTransform(GameObject card) {
        if (card != null) {
            RectTransform rectTransform = card.GetComponent<RectTransform>();
            if (rectTransform != null) {
                cardOriginalTransforms[card] = (rectTransform.localPosition, rectTransform.localRotation, rectTransform.localScale);
            }
        }
    }

    // Get the original position of a card
    public Vector3 GetCardOriginalPosition(GameObject card) {
        if (cardOriginalTransforms.ContainsKey(card)) {
            return cardOriginalTransforms[card].position;
        }
        return Vector3.zero;
    }

    // Get the original rotation of a card
    public Quaternion GetCardOriginalRotation(GameObject card) {
        if (cardOriginalTransforms.ContainsKey(card)) {
            return cardOriginalTransforms[card].rotation;
        }
        return Quaternion.identity;
    }

    // Get the original scale of a card
    public Vector3 GetCardOriginalScale(GameObject card) {
        if (cardOriginalTransforms.ContainsKey(card)) {
            return cardOriginalTransforms[card].scale;
        }
        return Vector3.one;
    }
}
