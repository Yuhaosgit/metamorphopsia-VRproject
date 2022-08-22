using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CustomGrid;


public class ConfigPanel : BasePanel
{
    static readonly string path = "Panels/ConfigPanel";
    public ConfigPanel() : base(new UIType(path)) { }

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
