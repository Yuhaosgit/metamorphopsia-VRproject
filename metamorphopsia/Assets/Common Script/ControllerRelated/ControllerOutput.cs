using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR;
using UnityEngine;
using Valve.VR;

public class ControllerOutput : MonoBehaviour
{
    static public Vector2 leftaxisDirection;
    static public Vector2 rightaxisDirection;

    static public bool pressPrimaryButton = false;
    static public bool pressMenuButton = false;

    void Start()
    {
        SteamVR.Initialize();
    }

    void Update()
    {
        leftaxisDirection = Vector2.zero;
        rightaxisDirection = Vector2.zero;

        if (SteamVR_Actions.mixedreality_LeftPadTrackerPressed.state)
            leftaxisDirection = SteamVR_Actions.mixedreality_LeftPadTracker.GetAxis(SteamVR_Input_Sources.LeftHand);

        if (SteamVR_Actions.mixedreality_RightPadTrackerPressed.stateUp)
            rightaxisDirection = SteamVR_Actions.mixedreality_RightPadTracker.GetAxis(SteamVR_Input_Sources.RightHand);

        pressMenuButton = SteamVR_Actions.mixedreality_PressMenu.stateUp;
        pressPrimaryButton = SteamVR_Actions.mixedreality_PressTrigger.stateUp;
    }
}
