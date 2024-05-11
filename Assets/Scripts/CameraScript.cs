using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public float defaultDistance = 5f;
    public Transform centerPoint;
    public float radius = 5f;
    public float scrollSpeed = 5f;
    public float rotationSpeed = 5f;
    public float movementSpeed = 5f;
    public float zoomDistance=20f;

    public float minimumHeight = 2f;
    public float maximumHeight = 10f;

    public TileHover tilehover; 
    
    private void Start()
    {
        centerPoint = new GameObject("CenterPoint").transform;
        centerPoint.position = transform.position + transform.forward * 5f;
    }

    private void FixedUpdate()
    {
        if (centerPoint == null)
            return;

        // W, A, S, D tuþlarýyla centerPoint'in konumunu deðiþtirme
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Calculate movement direction based on world space axes
        Vector3 forwardDirection = Vector3.ProjectOnPlane(transform.forward, Vector3.up);
        Vector3 rightDirection = Vector3.ProjectOnPlane(transform.right, Vector3.up);
        Vector3 movementDirection = (forwardDirection * verticalInput) + (rightDirection * horizontalInput);

        centerPoint.Translate(movementDirection * movementSpeed * Time.deltaTime, Space.World);

        

        // Kameranýn centerPoint'i takip etmesi
        transform.position = centerPoint.position;

        // Q veya E tuþlarýna basýldýðýnda kamerayý döndürme
        if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.E))
        {
            float direction = Input.GetKey(KeyCode.Q) ? 1f : -1f;
            float rotationAmount = direction * rotationSpeed * Time.deltaTime;
            transform.RotateAround(centerPoint.position, Vector3.up, rotationAmount);
        }

        // Fare tekerleðiyle zoom yapma
        float scrollmove = Input.GetAxis("Mouse ScrollWheel");
        float movementAmount = scrollmove * scrollSpeed * Time.deltaTime;
        //centerPoint.Translate(Vector3.up * movementAmount);
        if (centerPoint.position.y + movementAmount < minimumHeight)
        {
            movementAmount = minimumHeight - centerPoint.position.y;
        }
        else if (centerPoint.position.y + movementAmount > maximumHeight)
        {
            movementAmount = maximumHeight - centerPoint.position.y;
        }

        centerPoint.Translate(Vector3.up * movementAmount);

    }

    private void OnDrawGizmosSelected()
    {
        if (centerPoint != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(centerPoint.position, radius);
        }
    }
    public void SetCenterPoint(Vector3 position)
    {
        // Yeni konumu oluþtur ve yüksekliði sýnýrla
        Vector3 newPosition = position + Vector3.up * zoomDistance;

        newPosition -= transform.forward * 5f;

        newPosition.y = Mathf.Clamp(newPosition.y, minimumHeight, maximumHeight);

        centerPoint.position = newPosition;
    }

}
