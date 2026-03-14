using UnityEngine;
using UnityEngine.EventSystems;
using Delta;

public class CardMovement : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler {
    
    private RectTransform rectTransform;
    private Canvas canvas;
    private HandManager handManager;
    private PlayFieldManager playFieldManager;
    private AttackFieldManager attackFieldManager;
    private GameObject clickBlocker;
    private GameObject playField;
    private Vector2 originalLocalPointerPosition;
    private Vector3 originalPanelLocalPosition;
    private int originalSiblingIndex;
    private int currentState = 0;
    private int prevState = 0;
    private bool isDragging = false;
    private bool cardBeingShown = false;
    private bool isPlayed = false;
    private bool isAttacking = false;
    private GameObject playFieldHighlightEffect;
    private GameObject attackFieldHighlightEffect;

    [SerializeField] private float selectScale = 1.1f;
    [SerializeField] private float showScale = 1.5f;
    [SerializeField] private float dragThreshold = 15f;
    [SerializeField] private float lerpSpeed = 28f;
    [SerializeField] private Vector2 cardPlay;
    [SerializeField] private Vector2 cardAttack;
    [SerializeField] private Vector3 showPosition;
    [SerializeField] private GameObject highlightEffect;
    [SerializeField] private GameObject clickBlockerPrefab;

    void Awake() {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        handManager = GameObject.Find("HandManager").GetComponent<HandManager>();
        playFieldManager = GameObject.Find("PlayFieldManager").GetComponent<PlayFieldManager>();
        playFieldHighlightEffect = playFieldManager.playFieldTransform.Find("PlayFieldHighlight").gameObject;
        attackFieldManager = GameObject.Find("AttackFieldManager").GetComponent<AttackFieldManager>();
        attackFieldHighlightEffect = attackFieldManager.attackFieldTransform.Find("AttackFieldHighlight").gameObject;
        originalSiblingIndex = transform.GetSiblingIndex();
    }

    void Update() {
        Debug.Log("Current State: " + currentState);
        switch (currentState) {
            case 0: // Normal
                HandleNormalState();
                break;
            case 1: // Hovered
                HandleHoverState();
                break;
            case 2: // Dragged
                HandleDragState();
                break;
            case 3: // Played
                HandlePlayState();
                break;
            case 4: // Show
                HandleCardClicked();
                break;
            case 5: // Attack
                HandleAttackState();
                break;
        }
    }

    private void TransitionToState(int newState) {
        currentState = newState;
        if (newState == 0) {
            highlightEffect.SetActive(false);
            if (clickBlocker != null) {
                Destroy(clickBlocker);
                clickBlocker = null;
            }
            cardBeingShown = false;
            rectTransform.SetParent(handManager.handTransform, false);
            rectTransform.SetSiblingIndex(originalSiblingIndex);
        }
        else if (newState == 3) {
            highlightEffect.SetActive(false);
        }
    }

    public void OnPointerEnter(PointerEventData eventData) {
        if (currentState == 0 || currentState == 3) {
            prevState = currentState;
            currentState = 1;
        }
    }

    public void OnPointerExit(PointerEventData eventData) {
        if (currentState == 1) {
            TransitionToState(prevState);
        }
    }

    public void OnPointerDown(PointerEventData eventData) {
        if (currentState == 1) {
            isDragging = false;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), eventData.position, eventData.pressEventCamera, out originalLocalPointerPosition);
            originalPanelLocalPosition = rectTransform.localPosition;
        }
    }

    public void OnDrag(PointerEventData eventData) {
        if (currentState == 1 || currentState == 2) {
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), eventData.position, eventData.pressEventCamera, out Vector2 localPointerPosition)) {
                localPointerPosition /= canvas.scaleFactor;

                // Check if drag distance exceeds threshold
                Vector2 dragDistance = localPointerPosition - originalLocalPointerPosition;
                if (!isDragging && dragDistance.magnitude > dragThreshold) {
                    isDragging = true;
                    currentState = 2; // Now transition to drag state after threshold exceeded
                }

                if (isDragging) {
                    Vector3 offsetToOriginal = localPointerPosition - originalLocalPointerPosition;
                    rectTransform.localPosition = originalPanelLocalPosition + offsetToOriginal;
                }
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData) {
        if (currentState == 1 && !isPlayed) {
            // Mouse was released while still hovered but never dragged - it was a click
            currentState = 4;
            // Stay in state 1 (will exit via OnPointerExit if mouse leaves)
        } else if (currentState == 2) {
            // It was a drag
            if (isPlayed) {
                if (rectTransform.localPosition.y > cardAttack.y) {
                    attackFieldHighlightEffect.SetActive(false);
                    currentState = 5; // Transition to attack state
                } else {
                    TransitionToState(3); // If already played, just snap back to play state
                }
            } else {
                if (rectTransform.localPosition.y > cardPlay.y) {
                    playFieldHighlightEffect.SetActive(false);
                    currentState = 3; // Transition to played state
                } else {
                    TransitionToState(0);
                }
            }
        } else if (currentState == 4) {
            // Clicked state - reset to normal on release
            TransitionToState(0);
        }
    }

    private void HandleNormalState() {
        if (rectTransform.localPosition != handManager.GetCardOriginalPosition(rectTransform.gameObject)) {
            Debug.Log("LERPING BACK TO ORIGINAL POSITION");
            rectTransform.localPosition = Vector3.Lerp(rectTransform.localPosition, handManager.GetCardOriginalPosition(rectTransform.gameObject), Time.deltaTime * lerpSpeed);
        }
        if (rectTransform.localRotation != handManager.GetCardOriginalRotation(rectTransform.gameObject)) {
            Debug.Log("LERPING BACK TO ORIGINAL ROTATION");
            rectTransform.localRotation = Quaternion.Lerp(rectTransform.localRotation, handManager.GetCardOriginalRotation(rectTransform.gameObject), Time.deltaTime * lerpSpeed);
        }
        if (rectTransform.localScale != handManager.GetCardOriginalScale(rectTransform.gameObject)) {
            Debug.Log("LERPING BACK TO ORIGINAL SCALE");
            rectTransform.localScale = Vector3.Lerp(rectTransform.localScale, handManager.GetCardOriginalScale(rectTransform.gameObject), Time.deltaTime * lerpSpeed);
        }
    }

    private void HandleHoverState() {
        highlightEffect.SetActive(true);
        // Debug.Log(handManager.GetCardOriginalScale(rectTransform.gameObject));
        if (!isPlayed)
            rectTransform.localScale = handManager.GetCardOriginalScale(rectTransform.gameObject) * selectScale;
    }

    private void HandleDragState() {
        rectTransform.localRotation = Quaternion.identity;

        if (isPlayed) {
            if (rectTransform.localPosition.y > cardAttack.y) {
                attackFieldHighlightEffect.SetActive(true);
            } else {
                attackFieldHighlightEffect.SetActive(false);
            }
        } else {
            if (rectTransform.localPosition.y > cardPlay.y) {
                playFieldHighlightEffect.SetActive(true);
            } else {
                playFieldHighlightEffect.SetActive(false);
            }
        }
        
        if (!isPlayed) {
            if (rectTransform.localPosition.y > cardPlay.y) {
                playFieldHighlightEffect.SetActive(true);
            } else {
                playFieldHighlightEffect.SetActive(false);
            }
        }
    }

    private void HandlePlayState() {
        highlightEffect.SetActive(false);
        
        if (!isPlayed) {
            Card cardData = rectTransform.gameObject.GetComponent<CardDisplay>().cardData;
            handManager.RemoveCardFromHand(rectTransform.gameObject);
            playFieldManager.MoveCardToPlayField(rectTransform.gameObject, cardData);
            isPlayed = true;
        }

        if (rectTransform.localPosition != playFieldManager.GetCardOriginalPosition(rectTransform.gameObject)) {
            Debug.Log("LERPING BACK TO ORIGINAL POSITION");
            rectTransform.localPosition = Vector3.Lerp(rectTransform.localPosition, playFieldManager.GetCardOriginalPosition(rectTransform.gameObject), Time.deltaTime * lerpSpeed);
        }
        if (rectTransform.localRotation != playFieldManager.GetCardOriginalRotation(rectTransform.gameObject)) {
            Debug.Log("LERPING BACK TO ORIGINAL ROTATION");
            rectTransform.localRotation = Quaternion.Lerp(rectTransform.localRotation, playFieldManager.GetCardOriginalRotation(rectTransform.gameObject), Time.deltaTime * lerpSpeed);
        }
    }

    private void HandleCardClicked() {
        if (!cardBeingShown) {
            Debug.Log("Card clicked!");
            clickBlocker = Instantiate(clickBlockerPrefab, canvas.transform);
            clickBlocker.transform.SetAsLastSibling();
            rectTransform.SetParent(canvas.transform);
            rectTransform.SetAsLastSibling();
            highlightEffect.SetActive(false);
            cardBeingShown = true;
        }
        // Lerp every frame while in this state
        rectTransform.localPosition = Vector3.Lerp(rectTransform.localPosition, showPosition, Time.deltaTime * lerpSpeed);
        rectTransform.localRotation = Quaternion.Lerp(rectTransform.localRotation, Quaternion.identity, Time.deltaTime * lerpSpeed);
        rectTransform.localScale = Vector3.Lerp(rectTransform.localScale, handManager.GetCardOriginalScale(rectTransform.gameObject) * showScale, Time.deltaTime * lerpSpeed);
    }

    private void HandleAttackState() {
        highlightEffect.SetActive(false);

        if (!isAttacking) {
            Card cardData = rectTransform.gameObject.GetComponent<CardDisplay>().cardData;
            playFieldManager.RemoveCardFromPlayField(rectTransform.gameObject);
            attackFieldManager.MoveCardToAttackField(cardData);
            isAttacking = true;
        }

        if (rectTransform.localPosition != attackFieldManager.GetCardOriginalPosition(rectTransform.gameObject)) {
            Debug.Log("LERPING BACK TO ORIGINAL POSITION");
            rectTransform.localPosition = Vector3.Lerp(rectTransform.localPosition, attackFieldManager.GetCardOriginalPosition(rectTransform.gameObject), Time.deltaTime * lerpSpeed);
        }
        if (rectTransform.localRotation != attackFieldManager.GetCardOriginalRotation(rectTransform.gameObject)) {
            Debug.Log("LERPING BACK TO ORIGINAL ROTATION");
            rectTransform.localRotation = Quaternion.Lerp(rectTransform.localRotation, attackFieldManager.GetCardOriginalRotation(rectTransform.gameObject), Time.deltaTime * lerpSpeed);
        }
    }
}
