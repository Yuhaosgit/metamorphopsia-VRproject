using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class outside_camera : MonoBehaviour
{
    public string device_name;
    WebCamTexture input_tex;
    void Start()
    {
        //StartCoroutine(CallCamera());
    }

    IEnumerator CallCamera()
    {
        yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
        if (Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            WebCamDevice[] devices = WebCamTexture.devices;
            device_name = devices[0].name;
            input_tex = new WebCamTexture(device_name,400,300,60);
            GetComponent<Renderer>().material.mainTexture = input_tex;
            input_tex.Play();
        }
    }
}
