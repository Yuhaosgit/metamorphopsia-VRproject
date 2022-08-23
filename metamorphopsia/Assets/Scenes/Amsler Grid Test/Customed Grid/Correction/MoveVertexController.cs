using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using CustomGrid;

public class MoveVertexController : MonoBehaviour
{
    public static UnityEvent ChangeGrid = new UnityEvent();
    public static UnityEvent ShowDotsUI = new UnityEvent();
    public static UnityEvent ResetSelectedPointPosition = new UnityEvent();

    static private int selectedRowLine;
    static private int selectedColumnLine;
    static private int selectedPoint;

    private bool selectionState = false;

    private Vector3[] vertices;
    static private int width, height;

    public float speed;

    public Vector4 rowLineColor;
    public Vector4 columnLineColor;

    static public bool showGrid = true;
    MaterialPropertyBlock materialBlock;

    bool Boundary(int index, Vector2 direction)
    {
        Vector3 xVector = Vector3.zero, yVector = Vector3.zero;
        if (direction.x > 0 && direction.y > 0)
        {
            yVector = vertices[index + GridGeneration.Instance().GetWidthVerticesNumber()] - vertices[index];
            xVector = vertices[index + 1] - vertices[index];
        }
        else if (direction.x > 0 && direction.y < 0)
        {
            yVector = vertices[index - GridGeneration.Instance().GetWidthVerticesNumber()] - vertices[index];
            xVector = vertices[index + 1] - vertices[index];
        }
        else if (direction.x < 0 && direction.y > 0)
        {
            yVector = vertices[index + GridGeneration.Instance().GetWidthVerticesNumber()] - vertices[index];
            xVector = vertices[index - 1] - vertices[index];
        }
        else if (direction.x < 0 && direction.y < 0)
        {
            yVector = vertices[index - GridGeneration.Instance().GetWidthVerticesNumber()] - vertices[index];
            xVector = vertices[index - 1] - vertices[index];
        }

        if (Vector3.Dot(xVector.normalized, yVector.normalized) <= -0.98)
            return false;

        return true;
    }

    bool MovePoint(float speed)
    {
        Vector2 direction = ControllerOutput.leftaxisDirection;
        int index = selectedPoint;
        if (direction != Vector2.zero && Boundary(index, direction))
        {
            vertices[index].x += speed * direction.x;
            vertices[index].y += speed * direction.y;

            return true;
        }

        return false;
    }

    void ChoosePoint()
    {
        Vector2 direction = ControllerOutput.rightaxisDirection;
        float threshold = 0.6f;

        if (direction == Vector2.zero)
        {
            selectionState = false;
        }
        if (direction.x >= threshold && !selectionState)
        {
            selectedColumnLine = (selectedColumnLine >= width - 2 ? selectedColumnLine : selectedColumnLine + 1);
            selectionState = true;
        }
        else if (direction.x <= -threshold && !selectionState)
        {
            selectedColumnLine = (selectedColumnLine <= 1 ? selectedColumnLine : selectedColumnLine - 1);
            selectionState = true;
        }
        else if (direction.y >= threshold && !selectionState)
        {
            selectedRowLine = (selectedRowLine >= height - 2 ? selectedRowLine : selectedRowLine + 1);
            selectionState = true;
        }
        else if (direction.y <= -threshold && !selectionState)
        {
            selectedRowLine = (selectedRowLine <= 1 ? selectedRowLine : selectedRowLine - 1);
            selectionState = true;
        }

        selectedPoint = selectedColumnLine + width * selectedRowLine;
    }

    void ShowDots()
    {
        if (showGrid)
        {
            for (int i = 0; i < GameObject.Find("Points").transform.childCount; ++i)
            {
                GameObject.Find("Points").transform.GetChild(i).gameObject.SetActive(true);
                GameObject.Find("Grid").GetComponent<Renderer>().material.SetFloat("_ShowGrid", 1.0f);
            }
        }
        else
        {
            for (int i = 0; i < GameObject.Find("Points").transform.childCount; ++i)
            {
                GameObject.Find("Points").transform.GetChild(i).gameObject.SetActive(false);
                GameObject.Find("Grid").GetComponent<Renderer>().material.SetFloat("_ShowGrid", 0.0f);
            }
        }
    }

    private void SetLineColor()
    {
        Vector4 lineColor = new Vector4(0, 0, 0, 1);
        
        GetComponent<Renderer>().material.SetInt("XSelected", selectedColumnLine);
        GetComponent<Renderer>().material.SetInt("YSelected", selectedRowLine);
        GetComponent<Renderer>().material.SetVector("restColor", lineColor);

        if (!showGrid)
            GameObject.Find("Points").transform.GetChild(selectedPoint).gameObject.SetActive(true);
    }

    void RefreshDotAtOneDotMode()
    {
        if (!showGrid)
            GameObject.Find("Points").transform.GetChild(selectedPoint).gameObject.SetActive(false);
    }

    static public void Initilize()
    {
        width = GridGeneration.Instance().GetWidthVerticesNumber();
        height = GridGeneration.Instance().GetHeightVerticesNumber();

        selectedRowLine = 1;
        selectedColumnLine = 1;
        selectedPoint = selectedColumnLine + width * selectedRowLine;
    }

    private void Awake()
    {
        speed = 0.8f;
        rowLineColor = new Vector4(1, 0, 0, 1);
        columnLineColor = new Vector4(0, 0, 1, 1);

        ChangeGrid.AddListener(ChangeStructure);
        ShowDotsUI.AddListener(ShowDots);
        ResetSelectedPointPosition.AddListener(ResetState);

        materialBlock = new MaterialPropertyBlock();
    }

    private void OnDisable()
    {
        materialBlock.Clear();
    }

    private void Start()
    {
        Initilize();
    }

    private void ResetState()
    {
        selectedRowLine *= 2;
        selectedColumnLine *= 2;

        speed *= 0.8f;
    }

    private void ChangeStructure()
    {
        float moveSpeed = Time.deltaTime * speed;

        width = GridGeneration.Instance().GetWidthVerticesNumber();
        height = GridGeneration.Instance().GetHeightVerticesNumber();

        vertices = GetComponent<MeshFilter>().mesh.vertices;

        RefreshDotAtOneDotMode();
        ChoosePoint();
        if (MovePoint(moveSpeed))
        {
            GridDecoration.changed = true;
            GetComponent<MeshFilter>().mesh.SetVertices(vertices);
        }

        SetLineColor();
    }
}