using UnityEngine.UI;
using TMPro;
using UnityEngine;
using Delta;

public class CardDisplay : MonoBehaviour {

    public Card cardData;
    public Image cardTemplate;
    public TMP_Text nameText;
    public TMP_Text powerText;
    public TMP_Text costText;
    public TMP_Text descriptionText;
    public Image cardImage;

    void Start() {
        updateCardDisplay();
    }

    public void updateCardDisplay() {
        nameText.text = cardData.cardName;
        powerText.text = cardData.power.ToString();
        if (costText != null) {
            costText.text = cardData.cost.ToString();
        }
        if (descriptionText != null) {
            descriptionText.text = cardData.description;
        }
        cardImage.sprite = cardData.cardImage;
    }
}
