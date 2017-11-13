using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PCEntity : Entity {

    [Space(10)]
    [Header("Initialization PC Properties")]
    public GameObject cameraPrefab;
    public GameObject canvasPrefab;
    public GameObject eventSystemPrefab;

    public GameObject cam;
    public GameObject canvas;
    public GameObject eventSystem;

    [Header("Post-Initialization PC Properties")]
    public string postCameraPrefabName = "PCPostCamera";

    public override void InitializeHost()
    {
        cam = Instantiate(cameraPrefab);
        canvas = Instantiate(canvasPrefab);
        canvas.transform.Find("ConnectButton").GetComponent<Button>().onClick.AddListener(OnHostInitializationFinished);
        eventSystem = Instantiate(eventSystemPrefab);
    }

    // PC host and client behaviour are the same.
    public override void InitializeClient()
    {
        InitializeHost();
    }

    public override void OnHostInitializationFinished()
    {
        // Clean up setup objects.
        Destroy(cam);
        Destroy(canvas);
        Destroy(eventSystem);

        // Extract pos from canvas.
        float posX = GetFloat(canvas.transform.Find("Position").Find("XInput").GetComponent<InputField>().text);
        float posY = GetFloat(canvas.transform.Find("Position").Find("YInput").GetComponent<InputField>().text);
        float posZ = GetFloat(canvas.transform.Find("Position").Find("ZInput").GetComponent<InputField>().text);
        markerPosition = new Vector3(posX, posY, posZ);

        // Extract rot from canvas.
        float rotX = GetFloat(canvas.transform.Find("Rotation").Find("XInput").GetComponent<InputField>().text);
        float rotY = GetFloat(canvas.transform.Find("Rotation").Find("YInput").GetComponent<InputField>().text);
        float rotZ = GetFloat(canvas.transform.Find("Rotation").Find("ZInput").GetComponent<InputField>().text);
        markerRotation = Quaternion.LookRotation(new Vector3(rotX, rotY, rotZ));

        // Position the entity appropriately.
        PositionEntity();

        // Setup post-initialization objects.
        cam = PhotonNetwork.Instantiate(postCameraPrefabName, transform.position, transform.rotation, 0);
    }

    // PC host and client behaviour are the same.
    public override void OnClientInitializationFinished()
    {
        OnHostInitializationFinished();
    }

    // Parses a float from a string. Returns 0 if invalid.
    private float GetFloat(string s)
    {
        if (s.Length == 0)
            return 0f;
        return float.Parse(s);
    }
}
