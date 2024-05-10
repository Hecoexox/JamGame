using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileHover : MonoBehaviour
{
    public float hoverHeight = 0.1f; // Height to raise the tile when hovered
    public float hoverSpeed = 5.0f; // Speed of the hover animation

    private Vector3 originalPosition;
    private bool isHovering = false;
    private Coroutine hoverCoroutine;

    void Start()
    {
        originalPosition = transform.position;
    }

    void OnMouseEnter()
    {
        if (!isHovering)
        {
            hoverCoroutine = StartCoroutine(HoverUp());
        }
    }

    void OnMouseExit()
    {
        if (isHovering)
        {
            StopCoroutine(hoverCoroutine);
            hoverCoroutine = StartCoroutine(HoverDown());
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
}
