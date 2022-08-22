using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRoot : MonoBehaviour
{
    public static GameRoot Instance { get; private set; }
    public SceneSystem scene_system { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
            Instance = this;
        scene_system = new SceneSystem();

        DontDestroyOnLoad(this.gameObject);
    }
    private void Start()
    {
        //scene_system.SetScene(new DisplayScene());
        scene_system.SetScene(new EyeTestScene());
    }
}