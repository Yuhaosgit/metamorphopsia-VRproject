using UnityEngine;
using UnityEngine.Events;
using CustomGrid;
using RasterizerCS;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class GridManager : MonoBehaviour
{
    public static GridDecoration gridDecoration;

    public Camera gridCamera;
    public Camera UICamera;

    private bool OnUI = false;
    SampleSavePanel samplePanel = new SampleSavePanel();
    ConfigPanel configurationPanel = new ConfigPanel();

    static public bool showTexture = false;

    public ComputeShader rasterizerShader;

    void InitializeGrid()
    {
        GetComponent<MeshFilter>().mesh = GridGeneration.Instance().Initilize(1);
        if (GetComponent<MeshFilter>().mesh == null)
        {
            Debug.Log("Mesh initialization fault.");
        }
    }

    void Subdivide()
    {
        Mesh subdivisionMesh = GridGeneration.Instance().Subdivision(GetComponent<MeshFilter>().mesh);
        if (subdivisionMesh != null)
            GetComponent<MeshFilter>().mesh = subdivisionMesh;
        if (!gridDecoration.Reconstruct(GetComponent<MeshFilter>().mesh, transform))
        {
            Debug.Log("Can't subdivide the grid, the vertices number is same to previous.");
        }
    }

    void CallUI()
    {
        if (UICamera != null)
        {
            gridCamera.enabled = !gridCamera.enabled;

            if (OnUI)
            {
                EyeTestScene.panelManager.Push(samplePanel);

                UICamera.cullingMask = LayerMask.GetMask("UI") | LayerMask.GetMask("Default");
                UICamera.stereoTargetEye = StereoTargetEyeMask.Both;
            }
            else
            {
                EyeTestScene.panelManager.Clear();

                UICamera.cullingMask = LayerMask.GetMask("UI");

                if (SaveAndLoad.LoadCurrentTargetUI() == StereoTargetEyeMask.None)
                {
                    UICamera.stereoTargetEye = StereoTargetEyeMask.Both;
                    gridCamera.stereoTargetEye = StereoTargetEyeMask.None;
                }
                else if (SaveAndLoad.LoadCurrentTargetUI() == StereoTargetEyeMask.Both)
                {
                    UICamera.stereoTargetEye = StereoTargetEyeMask.None;
                    gridCamera.stereoTargetEye = StereoTargetEyeMask.Both;
                }
                else
                {
                    UICamera.stereoTargetEye =
                    SaveAndLoad.LoadCurrentTargetUI() == StereoTargetEyeMask.Left ? StereoTargetEyeMask.Right : StereoTargetEyeMask.Left;
                    gridCamera.stereoTargetEye =
                    SaveAndLoad.LoadCurrentTargetUI() == StereoTargetEyeMask.Left ? StereoTargetEyeMask.Left : StereoTargetEyeMask.Right;
                }
            }
        }
    }

    private void Start()
    {
        InitializeGrid();
        Rasterizer.Instance().Initilize(rasterizerShader);
        EyeTestScene.OnCallUI.AddListener(CallUI);
        gridDecoration = new GridDecoration(GetComponent<MeshFilter>().mesh, transform);
    }

    private void ShaderDecorateUpdate()
    {
        GetComponent<Renderer>().material.SetInt("width", GridGeneration.Instance().GetWidthVerticesNumber());
        GetComponent<Renderer>().material.SetInt("height", GridGeneration.Instance().GetHeightVerticesNumber());
        GetComponent<Renderer>().material.SetFloat("scale", 0.5f - 0.01f * (float)GridGeneration.Instance().subdivisionLevel);
        if (showTexture)
            GetComponent<Renderer>().material.SetFloat("_ShowTexture", 1f);
        else
            GetComponent<Renderer>().material.SetFloat("_ShowTexture", 0f);
    }

    void OnGridUpdate()
    {
        if (ControllerOutput.pressPrimaryButton)
        {
            Subdivide();
        }
        MoveVertexController.ChangeGrid.Invoke();

        GenerateBoard.UVTex = Rasterizer.Instance().Refresh(GetComponent<MeshFilter>().mesh, transform, gridCamera);

        gridDecoration.Update(GetComponent<MeshFilter>().mesh, transform);
        ShaderDecorateUpdate();
    }

    void OnUIUpdate()
    {
        Vector2 direction = ControllerOutput.leftaxisDirection;

        if (direction.x >= 0.5f && direction.x <= 1.0f && samplePanel.ui_tool.LocalPosition().x >= -1000f)
        {
            if (EyeTestScene.panelManager.CurrentActivePanel() == "GridPanel")
            {
                EyeTestScene.panelManager.Push(configurationPanel);
                configurationPanel.ui_tool.Move(new Vector3(110, 0f, 0f));
            }

            configurationPanel.ui_tool.Move(new Vector3(-direction.x * 5, 0f, 0f));
            samplePanel.ui_tool.Move(new Vector3(-direction.x * 5, 0f, 0f));
        }


        else if (direction.x <= -0.5f && direction.x >= -1.0f && EyeTestScene.panelManager.CurrentActivePanel() != "GridPanel")
        {
            configurationPanel.ui_tool.Move(new Vector3(-direction.x * 5, 0f, 0f));
            samplePanel.ui_tool.Move(new Vector3(-direction.x * 5, 0f, 0f));

            if (samplePanel.ui_tool.LocalPosition().x >= 0)
                EyeTestScene.panelManager.Pop();
        }
    }

    private void Update()
    {
        if (!OnUI)
            OnGridUpdate();

        else if (OnUI)
            OnUIUpdate();

        if (ControllerOutput.pressMenuButton)
        {
            OnUI = !OnUI;
            EyeTestScene.OnCallUI.Invoke();
        }
    }
}