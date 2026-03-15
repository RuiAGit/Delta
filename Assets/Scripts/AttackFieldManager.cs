using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Delta;

public class AttackFieldManager : MonoBehaviour {
    public GameObject cardPrefab;
    public Transform attackFieldTransform;
    public List<GameObject> cardsInAttackField = new List<GameObject>();
    public float cardSpacing = 170f;

    private Dictionary<GameObject, (Vector3 position, Quaternion rotation)> cardOriginalTransforms = new Dictionary<GameObject, (Vector3, Quaternion)>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveCardToAttackField(GameObject card, Card cardData) {
        // instantiate a new card in the attack field and set its data
        // GameObject newCard = Instantiate(cardPrefab, attackFieldTransform.position, Quaternion.identity);
        card.transform.SetParent(attackFieldTransform, false);
        // newCard.transform.localScale = Vector3.zero*85;
        cardsInAttackField.Add(card);


        card.GetComponentInChildren<CardDisplay>().cardData = cardData;

        UpdateAttackFieldVisuals();
    }

    public void UpdateAttackFieldVisuals() {
        int cardCount = cardsInAttackField.Count;

        for (int i = 0; i < cardCount; i++) {
            float horizontalOffset = cardSpacing * (i - (cardCount - 1) / 2f);
            cardsInAttackField[i].transform.localPosition = new Vector3(horizontalOffset, 0f, 0f);
            StoreCardOriginalTransform(cardsInAttackField[i]);
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
