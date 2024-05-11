using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Face
{
    public List<Vector3> vertices { get; private set; }
    public List<int> triangles { get; private set; }
    public List<Vector2> uvs { get; private set; }

    public Face(List<Vector3> vertices, List<int> triangles, List<Vector2> uvs)
    {
        this.vertices = vertices;
        this.triangles = triangles;
        this.uvs = uvs;
    }
}

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class HexRenderer : MonoBehaviour
{
    private Mesh m_Mesh;
    private MeshFilter m_MeshFilter;
    private MeshRenderer m_MeshRenderer;
    public Material material;
    public float innerSize;
    public float outerSize;
    public float height;
    public bool isFlatTopped;

    private List<Face> m_faces;

    private void Awake()
    {
        m_MeshFilter = GetComponent<MeshFilter>();
        m_MeshRenderer = GetComponent<MeshRenderer>();
        m_Mesh = new Mesh();
        m_Mesh.name = "Hex";
        m_MeshFilter.mesh = m_Mesh;
        m_MeshRenderer.material = material;
    }

    private void OnEnable()
    {
        DrawMesh();
    }

    private void OnValidate()
    {
        if (Application.isPlaying)
        {
            DrawMesh();
        }
    }

    public void DrawMesh()
    {
        DrawFaces();
        CombineFaces();
    }

    public void DrawFaces()
    {
        m_faces = new List<Face>();

        // Top
        for (int point = 0; point < 6; point++)
        {
            m_faces.Add(CreateFace(innerSize, outerSize, height / 2f, height / 2f, point));
        }

        // Bottom
        for (int point = 0; point < 6; point++)
        {
            m_faces.Add(CreateFace(innerSize, outerSize, -height / 2f, -height / 2f, point, true));
        }

        // Outer
        for (int point = 0; point < 6; point++)
        {
            m_faces.Add(CreateFace(outerSize, outerSize, height / 2f, -height / 2f, point, true));
        }

        // Inner
        for (int point = 0; point < 6; point++)
        {
            m_faces.Add(CreateFace(innerSize, innerSize, height / 2f, -height / 2f, point, true));
        }
    }

    public void CombineFaces()
    {

        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        List<Vector2> uvs = new List<Vector2>();

        for (int i = 0; i < m_faces.Count; i++)
        {
            vertices.AddRange(m_faces[i].vertices);
            uvs.AddRange(m_faces[i].uvs);

            int offset = 4 * i;
            foreach (int triangle in m_faces[i].triangles)
            {
                triangles.Add(triangle + offset);
            }
        }

        m_Mesh.vertices = vertices.ToArray();
        m_Mesh.triangles = triangles.ToArray();
        m_Mesh.uv = uvs.ToArray();
        m_Mesh.RecalculateNormals();
    }

    private Face CreateFace(float innerRad, float outerRad, float heightA, float heightB, int point, bool reverse = false)
    {
        Vector3 pointA = GetPoint(innerRad, heightB, point);
        Vector3 pointB = GetPoint(innerRad, heightB, point < 5 ? point + 1 : 0);
        Vector3 pointC = GetPoint(outerRad, heightA, point < 5 ? point + 1 : 0);
        Vector3 pointD = GetPoint(outerRad, heightA, point);

        List<Vector3> vertices = new List<Vector3>() { pointA, pointB, pointC, pointD };
        List<int> triangles = new List<int>() { 0, 1, 2, 2, 3, 0 };
        List<Vector2> uvs = new List<Vector2>() { new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1) };

        if (reverse)
        {
            triangles.Reverse(); // Reverse the triangles when necessary
        }

        return new Face(vertices, triangles, uvs);
    }

    protected Vector3 GetPoint(float size, float height, int index)
    {
        float angle_deg = isFlatTopped ? 60 * index : 60 * index - 30;
        float angle_rad = Mathf.PI / 180f * angle_deg;
        return new Vector3(size * Mathf.Cos(angle_rad), height, size * Mathf.Sin(angle_rad));
    }
    public void SetMaterial(Material newMaterial)
    {
        if (m_MeshRenderer != null)
        {
            m_MeshRenderer.material = newMaterial;
        }
    }

}
