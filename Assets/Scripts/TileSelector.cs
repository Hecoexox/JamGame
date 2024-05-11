using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class TileSelector : MonoBehaviour
{
    // Bu materyali Inspector �zerinden atayabilirsiniz
    public Material selectedMaterial;

    bool isProcessed = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isProcessed)
        {
            StartCoroutine(SelectAndColorTiles());
        }
    }

    IEnumerator SelectAndColorTiles()
    {
        isProcessed = true;

        // �smi "Tile" tag'ine uygun olan t�m oyun nesnelerini bul
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");

        if (tiles.Length > 0)
        {
            for (int i = 0; i < 5; i++)
            {
                // Rastgele bir tile se�
                int randomIndex = Random.Range(0, tiles.Length);
                GameObject selectedTile = tiles[randomIndex];

                // Se�ilen tile'�n Renderer bile�enini al
                Renderer renderer = selectedTile.GetComponent<Renderer>();

                if (renderer != null)
                {
                    // Se�ilen tile'� yeni materyalle boyama
                    renderer.material = selectedMaterial;

                    // Se�ilen tile'a "Mine" tag'ini ekle
                    selectedTile.tag = "Mine";

                    Debug.Log("Selected Tile: " + selectedTile.name);
                }
                else
                {
                    Debug.LogWarning("Selected tile does not have a Renderer component.");
                }

                // 1 saniye bekle
                yield return new WaitForSeconds(1f);
            }

        }
        else
        {
            Debug.LogWarning("No tiles found with the specified tag.");
        }

    }
}
