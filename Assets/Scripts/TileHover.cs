using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileHover : MonoBehaviour
{
    public float hoverHeight = 0.1f; // Height to raise the tile when hovered
    public float hoverSpeed = 5.0f; // Speed of the hover animation
    public Material hoverMaterial; // Material to apply when hovered over
    public Material clickedMaterial; // Material to apply when clicked

    private Vector3 originalPosition;
    private bool isHovering = false;
    public bool isClicked = false;
    private Coroutine hoverCoroutine;
    private Material originalMaterial;

    void Start()
    {
        originalPosition = transform.position;
        originalMaterial = GetComponent<Renderer>().material;
    }

    void OnMouseEnter()
    {
        if (!isHovering && !isClicked && !IsAnyTileClicked())
        {
            hoverCoroutine = StartCoroutine(HoverUp());
            GetComponent<Renderer>().material = hoverMaterial;
        }
    }

    void OnMouseExit()
    {
        if (isHovering && !isClicked)
        {
            StopCoroutine(hoverCoroutine);
            hoverCoroutine = StartCoroutine(HoverDown());
            GetComponent<Renderer>().material = originalMaterial;
        }
    }

    void OnMouseDown()
    {
        if (!isClicked && !IsAnyTileClicked())
        {
            isClicked = true;
            GetComponent<Renderer>().material = clickedMaterial;
        }
    }

    void OnMouseOver()
    {
        if (isClicked && Input.GetMouseButtonDown(1))
        {
            ResetClickedTile();
        }
    }

    IEnumerator HoverUp()
    {
        isHovering = true;
        Vector3 targetPosition = originalPosition + Vector3.up * hoverHeight;
        while (transform.position.y < targetPosition.y)
        {
            transform.position += Vector3.up * Time.deltaTime * hoverSpeed;
            yield return null;
        }
    }

    IEnumerator HoverDown()
    {
        Vector3 targetPosition = originalPosition;
        while (transform.position.y > targetPosition.y)
        {
            transform.position -= Vector3.up * Time.deltaTime * hoverSpeed;
            yield return null;
        }
        isHovering = false;
    }

    bool IsAnyTileClicked()
    {
        TileHover[] tiles = FindObjectsOfType<TileHover>();
        foreach (TileHover tile in tiles)
        {
            if (tile != this && tile.isClicked)
            {
                return true;
            }
        }
        return false;
    }

    void ResetClickedTile()
    {
        isClicked = false;
        GetComponent<Renderer>().material = originalMaterial;
    }
}
