using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int gridSizeX = 5;
    public int gridSizeY = 5;
    public GameObject gridCellPrefab;
    public float cellSize = 1.0f;
    public float xOffset = 0.75f; // Horizontal offset between cells
    public float yOffset = 0.75f; // Vertical offset for odd rows

    void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                float xPos = x * (cellSize + xOffset); // Horizontal spacing adjusted by xOffset
                float yPos = y * (cellSize * Mathf.Sqrt(3) / 2 + yOffset); // Vertical spacing adjusted by yOffset

                // Offset odd rows
                if (x % 2 == 1)
                {
                    yPos += (cellSize * Mathf.Sqrt(3) / 2 + yOffset) / 2;
                }

                Vector3 spawnPosition = new Vector3(xPos, 0, yPos);
                GameObject gridCell = Instantiate(gridCellPrefab, spawnPosition, Quaternion.identity);
                gridCell.transform.localScale = new Vector3(cellSize, cellSize, cellSize);
                gridCell.transform.parent = transform;

                // Assign coordinates to the cell
                gridCell.name = string.Format("({0},{1})", x, y);

                // Rotate the grid cell by 30 degrees around the y-axis
                gridCell.transform.Rotate(0, 30f, 0);
            }
        }

        // Disable the prefab object after generating the grid
        gridCellPrefab.SetActive(false);
    }
}
