using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using CustomGrid;

[System.Serializable]
class MeshInformation
{
    [SerializeField]
    public Vector3[] vertices;
    [SerializeField]
    public uint subdivisionLevel;
}

[System.Serializable]
class UVInformation
{
    [SerializeField]
    public Vector2[] uvData;
}

[System.Serializable]
class UIInformation
{
    [SerializeField]
    public StereoTargetEyeMask currentTarget;
}

[System.Serializable]
class DisplayMode
{
    [SerializeField]
    public bool isIndividual;
}

static class SaveAndLoad
{
    static MeshInformation meshInformation = new MeshInformation();
    static UIInformation uiInformation = new UIInformation();
    static DisplayMode displayMode = new DisplayMode();
    static UVInformation uvInformation = new UVInformation();

    static public Texture2D RenderTextureToTexture2D(ref RenderTexture renderTexture)
    {
        Texture2D savedTexture = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGBAFloat, false);

        var oldActive = RenderTexture.active;
        RenderTexture.active = renderTexture;

        savedTexture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0, false);
        savedTexture.Apply();

        RenderTexture.active = oldActive;

        return savedTexture;
    }

    static void SaveUV(ref RenderTexture UVmap, string fileName)
    {
        Texture2D savedTexture = RenderTextureToTexture2D(ref UVmap);
        Texture2DExtension.SaveUncompressed(savedTexture, fileName + "UV", Texture2DExtension.DataFormat.ARGBFloat);
    }

    static public Texture2D ReadUV(string fileName)
    {
        string saveFileName = Application.dataPath + "/Storage/" + fileName + "UV";
        if (File.Exists(saveFileName))
        {
            Texture2D readTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGBAFloat, false);
            Texture2DExtension.ReadUncompressed(readTexture, saveFileName);

            return readTexture;
        }
        return null;
    }


    static public void Save(Mesh storedMesh, RenderTexture UVmap, string fileName)
    {
        if (SceneManager.GetActiveScene().name == "Eyes Test")
            GameObject.Find("Canvas").GetComponent<Processing>().enabled = true;

        meshInformation.vertices = storedMesh.vertices;
        meshInformation.subdivisionLevel = GridGeneration.Instance().subdivisionLevel;

        string saveFileName = Application.dataPath + "/Storage/" + fileName;
        string jsonString = JsonUtility.ToJson(meshInformation);
        StreamWriter writer = new StreamWriter(saveFileName + ".json");
        writer.Write(jsonString);
        writer.Close();

        SaveUV(ref UVmap, saveFileName);
    }

    static public Mesh Load(string fileName)
    {
        if (File.Exists(Application.dataPath + "/Storage/" + fileName + ".json"))
        {
            if (SceneManager.GetActiveScene().name == "Eyes Test")
                GameObject.Find("Canvas").GetComponent<Processing>().enabled = true;

            StreamReader reader = new StreamReader(Application.dataPath + "/Storage/" + fileName + ".json");
            string jsonString = reader.ReadToEnd();
            reader.Close();
            meshInformation = JsonUtility.FromJson<MeshInformation>(jsonString);

            Mesh mesh = GridGeneration.Instance().Initilize(meshInformation.subdivisionLevel);
            mesh.SetVertices(meshInformation.vertices);

            return mesh;
        }

        return GridGeneration.Instance().Initilize(1);
    }

    static public void SaveUI(StereoTargetEyeMask currentTarget)
    {
        uiInformation.currentTarget = currentTarget;
        string jsonString = JsonUtility.ToJson(uiInformation);
        StreamWriter writer = new StreamWriter(Application.dataPath + "/Storage/UI_Information.json");
        writer.Write(jsonString);
        writer.Close();
    }
    static public StereoTargetEyeMask LoadCurrentTargetUI()
    {
        if (File.Exists(Application.dataPath + "/Storage/UI_Information.json"))
        {
            StreamReader reader = new StreamReader(Application.dataPath + "/Storage/UI_Information.json");
            string jsonString = reader.ReadToEnd();
            reader.Close();
            uiInformation = JsonUtility.FromJson<UIInformation>(jsonString);

            return uiInformation.currentTarget;
        }
        return StereoTargetEyeMask.None;
    }

    static public void SaveDisplayMode(bool mode)
    {
        displayMode.isIndividual = mode;
        string jsonString = JsonUtility.ToJson(displayMode);
        StreamWriter writer = new StreamWriter(Application.dataPath + "/Storage/DisplayMode.json");
        writer.Write(jsonString);
        writer.Close();
    }

    static public bool LoadDisplayMode()
    {
        if (File.Exists(Application.dataPath + "/Storage/DisplayMode.json"))
        {
            StreamReader reader = new StreamReader(Application.dataPath + "/Storage/DisplayMode.json");
            string jsonString = reader.ReadToEnd();
            reader.Close();
            displayMode = JsonUtility.FromJson<DisplayMode>(jsonString);

            return displayMode.isIndividual;
        }
        return false;
    }
}
