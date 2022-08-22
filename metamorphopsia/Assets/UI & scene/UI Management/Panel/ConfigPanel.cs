using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using CustomGrid;


public class ConfigPanel : BasePanel
{
    static readonly string path = "Panels/ConfigPanel";
    public ConfigPanel() : base(new UIType(path)) { }
    //static public UnityEvent<Vector4> ShowDots = new UnityEvent<Vector4>();

    public override void OnAwake()
    {
        Toggle showTexture = ui_tool.GetOrAddComponentInChildren<Toggle>("Show Texture");
        Toggle showGrid = ui_tool.GetOrAddComponentInChildren<Toggle>("Show Grid");

        showTexture.isOn = GridManager.showTexture;
        showGrid.isOn = MoveVertexController.showGrid;

        showTexture.onValueChanged.AddListener((bool isOn) =>
        {
            GridManager.showTexture = isOn;
        });

        showGrid.onValueChanged.AddListener((bool isOn) =>
        {
            MoveVertexController.showGrid = isOn;

            //if (isOn)
            //    ShowDots.Invoke(new Vector4(0, 0, 0, 1));
            //else
            //    ShowDots.Invoke(new Vector4(0, 0, 0, 0));
        });

        ui_tool.GetOrAddComponentInChildren<Button>("Play").onClick.AddListener(() =>
        {
            GameRoot.Instance.scene_system.SetScene(new DisplayScene());
        });

        ui_tool.GetOrAddComponentInChildren<Button>("Quit").onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }
}
