using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GridManager : MonoBehaviour
{
    public int gridSizeX = 5;
    public int gridSizeY = 5;
    public GameObject gridCellPrefab;
    public GameObject textMeshProPrefab; // Assign your TextMeshPro prefab in the Unity Editor
    public float cellSize = 1.0f;
    public float xOffset = 0.75f; // Horizontal offset between cells
    public float yOffset = 0.75f; // Vertical offset for odd rows

    public int displayValue = 0; // Public integer to be displayed on TextMeshPro objects

    public AudioClip tilePlacementSound; // Assign your AudioClip in the Unity Editor

    private List<GameObject> textObjects = new List<GameObject>();

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(GenerateGridCoroutine());
    }

    IEnumerator GenerateGridCoroutine()
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

                if (tilePlacementSound != null)
                {
                    audioSource.PlayOneShot(tilePlacementSound);
                }
                // Instantiate TMP object for displaying text on top of the tile
                GameObject textObject = Instantiate(textMeshProPrefab, gridCell.transform.position, Quaternion.identity);
                textObject.transform.SetParent(gridCell.transform, false); // Use SetParent method with worldPositionStays argument set to false
                textObject.transform.localPosition = new Vector3(0, 5f, 0);
                TextMeshPro textMeshPro = textObject.GetComponent<TextMeshPro>();
                textMeshPro.text = displayValue.ToString(); // Display integer value
                textObjects.Add(textObject);

                yield return new WaitForSeconds(0.05f); // Wait for 0.1 seconds before creating the next grid cell
            }
        }

        // Disable the prefab object after generating the grid
        gridCellPrefab.SetActive(false);
    }

    void Update()
    {
        foreach (GameObject textObject in textObjects)
        {
            if (Camera.main != null)
            {
                textObject.transform.LookAt(Camera.main.transform);
                textObject.transform.Rotate(0, 180f, 0); // Correct for initial rotation
            }
        }
    }
}