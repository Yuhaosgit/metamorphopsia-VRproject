using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraViewAdapt : MonoBehaviour
{
    public float sceneWidth = 14.4f;

    float unitsPerPixel;
    float desiredHalfHeight;

    public GameObject screenPlane;

    private void Start()
    {
        unitsPerPixel = sceneWidth / (float)Screen.width;
        desiredHalfHeight = 0.5f * unitsPerPixel * Screen.height;
    }
    private void Update()
    {
        Camera camera = gameObject.GetComponent<Camera>();

        Vector3 headsetPosition = camera.transform.position;
        Quaternion headsetRotation = camera.transform.rotation;
        Vector3 look = camera.transform.TransformDirection(Vector3.forward);

        float tangent = Mathf.Tan(Mathf.Deg2Rad * camera.fieldOfView / 2);
        float distance = desiredHalfHeight / tangent;
        Vector3 playerPosition = headsetPosition + look * distance;

        screenPlane.transform.SetPositionAndRotation(playerPosition, headsetRotation);
    }
}
