
using UnityEngine;

public class GridCell : MonoBehaviour {
    public Vector2 gridIndex; // The grid coordinates of this cell
    public bool isOccupied = false; // Whether the cell is currently occupied by a card
    public GameObject currentCard; // Reference to the card currently occupying this cell

}
