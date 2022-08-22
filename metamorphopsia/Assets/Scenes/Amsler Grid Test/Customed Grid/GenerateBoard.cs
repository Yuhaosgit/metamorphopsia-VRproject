using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateBoard : MonoBehaviour
{
    public static RenderTexture UVTex;
    // Start is called before the first frame update
    void Start()
    {
        Vector3[] vertices = new Vector3[4];
        Vector2[] uvCoordinate = new Vector2[4];
        int[] triangles = { 0, 2, 3, 0, 3, 1 };

        float width = (float)Screen.width / 100.0f;
        float height = (float)Screen.height / 100.0f;

        vertices[0] = new Vector3(-width / 2, -height / 2, 0f);
        vertices[1] = new Vector3(width / 2, -height / 2, 0f);
        vertices[2] = new Vector3(-width / 2, height / 2, 0f);
        vertices[3] = new Vector3(width / 2, height / 2, 0f);

        uvCoordinate[0] = new Vector2(0, 0);
        uvCoordinate[1] = new Vector2(1, 0);
        uvCoordinate[2] = new Vector2(0, 1);
        uvCoordinate[3] = new Vector2(1, 1);

        GetComponent<MeshFilter>().mesh.SetVertices(vertices);
        GetComponent<MeshFilter>().mesh.SetUVs(0, uvCoordinate);
        GetComponent<MeshFilter>().mesh.SetTriangles(triangles, 0);
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Renderer>().material.SetTexture("_UVTex", UVTex);

        if (GridManager.showTexture)
            GetComponent<Renderer>().material.SetFloat("_ShowTexture", 1.0f);
        else
            GetComponent<Renderer>().material.SetFloat("_ShowTexture", 0.0f);
    }
}
