using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCEntity : Entity {

    [Space(10)]
    [Header("PC Properties")]
    public GameObject cameraPrefab;
    public GameObject canvasPrefab;
    public GameObject eventSystemPrefab;

    public GameObject camera;
    public GameObject canvas;
    public GameObject eventSystem;

    public override void InitializeHost()
    {
        camera = Instantiate(cameraPrefab);
        canvas = Instantiate(canvasPrefab);
        eventSystem = Instantiate(eventSystemPrefab);
    }

    public override void InitializeSpectator()
    {
        camera = Instantiate(cameraPrefab);
        canvas = Instantiate(canvasPrefab);
        eventSystem = Instantiate(eventSystemPrefab);
    }

    public override void OnHostInitializationFinished()
    {
        print("HI");
        base.OnHostInitializationFinished();
    }
}
