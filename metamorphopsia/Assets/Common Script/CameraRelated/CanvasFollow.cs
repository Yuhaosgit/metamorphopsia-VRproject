using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class CanvasFollow : MonoBehaviour
{
    public Camera camera;
    void Update()
    {
        Vector3 headset_position = camera.transform.position;
        Quaternion headset_rotation = camera.transform.rotation;

        Vector3 look = camera.transform.TransformDirection(Vector3.forward);

        Vector3 player_pos_offset = headset_position + look * 80f;

        transform.SetPositionAndRotation(player_pos_offset, headset_rotation);
    }
}
