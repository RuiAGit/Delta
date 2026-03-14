
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager Instance { get; private set; }

    private int playerHealth;
    private int playerMana;
    private int difficultyLevel;

    public OptionsManager optionsManager { get; private set; }
    public DeckManager deckManager { get; private set; }

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeManagers();
        } else {
            Destroy(gameObject);
        }
    }

    private void InitializeManagers() {
        optionsManager = GetComponentInChildren<OptionsManager>();
        if (optionsManager == null) {
            GameObject prefabOptions = Resources.Load<GameObject>("Prefabs/OptionsManager");
            if (prefabOptions != null) {
                GameObject optionsObj = Instantiate(prefabOptions, transform.position, Quaternion.identity, transform);
                optionsManager = optionsObj.GetComponent<OptionsManager>();
            }
        }

        deckManager = GetComponentInChildren<DeckManager>();
        if (deckManager == null) {
            GameObject prefabDeck = Resources.Load<GameObject>("Prefabs/DeckManager");
            if (prefabDeck != null) {
                GameObject deckObj = Instantiate(prefabDeck, transform.position, Quaternion.identity, transform);
                deckManager = deckObj.GetComponent<DeckManager>();
            }
        }
    }

    public int PlayerHealth {
        get { return playerHealth; }
        set { playerHealth = value; }
    }

    public int PlayerMana {
        get { return playerMana; }
        set { playerMana = value; }
    }

    public int DifficultyLevel {
        get { return difficultyLevel; }
        set { difficultyLevel = value; }
    }
}
