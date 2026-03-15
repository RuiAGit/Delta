using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Delta;

public class PlayFieldManager : MonoBehaviour {
    public GameObject cardPrefab;
    public Transform playFieldTransform;
    public List<GameObject> cardsInPlayField = new List<GameObject>();
    public float cardSpacing = 170f;
    public float cardDownScale = 0.6f;

    private Dictionary<GameObject, (Vector3 position, Quaternion rotation)> cardOriginalTransforms = new Dictionary<GameObject, (Vector3, Quaternion)>();

     void Start() {

    }

     void Update() {
        // UpdatePlayFieldVisuals();
    }

    // public void AddCardToPlayField(Card cardData) {
    //     GameObject newCard = Instantiate(cardPrefab, playFieldTransform.position, Quaternion.identity, playFieldTransform);
    //     cardsInPlayField.Add(newCard);

    //     newCard.GetComponent<CardDisplay>().cardData = cardData;

    //     UpdatePlayFieldVisuals();
    // }

    public void MoveCardToPlayField(GameObject card, Card cardData) {
        // Reparent the card from hand to playfield
        card.transform.SetParent(playFieldTransform, false);
        card.transform.localPosition = Vector3.zero;
        card.transform.localRotation = Quaternion.identity;
        card.transform.localScale *= cardDownScale;

        cardsInPlayField.Add(card);

        card.GetComponentInChildren<CardDisplay>().cardData = cardData;

        UpdatePlayFieldVisuals();
    }

    public void RemoveCardFromPlayField(GameObject card) {
        cardsInPlayField.Remove(card);
        cardOriginalTransforms.Remove(card);
        UpdatePlayFieldVisuals();
    }

    public void UpdatePlayFieldVisuals() {
        int cardCount = cardsInPlayField.Count;

        for (int i = 0; i < cardCount; i++) {
            float horizontalOffset = cardSpacing * (i - (cardCount - 1) / 2f);
            cardsInPlayField[i].transform.localPosition = new Vector3(horizontalOffset, 0f, 0f);
            StoreCardOriginalTransform(cardsInPlayField[i]);
        }
    }

    public void StoreCardOriginalTransform(GameObject card) {
        if (card != null) {
            RectTransform rectTransform = card.GetComponent<RectTransform>();
            if (rectTransform != null) {
                cardOriginalTransforms[card] = (rectTransform.localPosition, rectTransform.localRotation);
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
}
