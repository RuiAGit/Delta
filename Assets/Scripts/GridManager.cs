using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour {
    public int width = 8;
    public int height = 4;
    public GameObject cellPrefab;
    public List<GameObject> gridCards = new List<GameObject>();
    private GameObject[,] gridCells;

    void Start() {
        CreateGrid();
    }

    void CreateGrid() {
        gridCells = new GameObject[width, height];
        Vector2 centerOffset = new Vector2((width / 2.0f) - .5f, (height / 2.0f) - .5f);
        Debug.Log(centerOffset);

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                Vector2 position = new Vector2(x, y) - centerOffset;

                GameObject cell = Instantiate(cellPrefab, position, Quaternion.identity);

                cell.transform.SetParent(transform);
                cell.name = $"Cell_{x+1}_{y+1}";

                GridCell gridCellComponent = cell.GetComponent<GridCell>();
                if (gridCellComponent != null) {
                    gridCellComponent.gridIndex = new Vector2(x, y);
                }

                gridCells[x, y] = cell;
            }
        }

        transform.localScale *= 12f; // adjust scale
    }

    private bool IsValidGridIndex(Vector2 gridIndex) {
        return gridIndex.x >= 0 && gridIndex.x < width && gridIndex.y >= 0 && gridIndex.y < height;
    }

    public bool AddCardToCell(GameObject card, Vector2 gridIndex) {
        if (IsValidGridIndex(gridIndex)) {
            GameObject cell = gridCells[(int)gridIndex.x, (int)gridIndex.y];
            GridCell gridCellComponent = cell.GetComponent<GridCell>();

            if (gridCellComponent != null && !gridCellComponent.isOccupied) {
                gridCellComponent.currentCard = card;
                gridCellComponent.isOccupied = true;
                card.transform.position = cell.transform.position;
                gridCards.Add(card);
                return true;
            }
        }
        return false;
    }
   
}
