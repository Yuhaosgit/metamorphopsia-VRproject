using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controlUI : MonoBehaviour
{
    bool OnUI = false;

    public Camera UIcamera;
    public Camera LeftEyeCamera;
    public Camera RightEyeCamera;

    private void Start()
    {
        LeftEyeCamera.enabled = true;
        RightEyeCamera.enabled = true;
        UIcamera.enabled = false;
    }
    void Update()
    {
        if (ControllerOutput.pressMenuButton)
        {
            OnUI = !OnUI;
            if (!OnUI)
            {
                LeftEyeCamera.enabled = true;
                RightEyeCamera.enabled = true;
                UIcamera.enabled = false;
                DisplayScene.panelManager.Pop();
            }
            else
            {
                LeftEyeCamera.enabled = false;
                RightEyeCamera.enabled = false;
                UIcamera.enabled = true;
                DisplayScene.panelManager.Push(new MenuPanel());
            }
        }
    }
}
