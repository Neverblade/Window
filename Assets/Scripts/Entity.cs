using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour {

    private void Update()
    {
        SpectatorUpdate();
    }

    public virtual void InitializeHost()
    {

    }

    public virtual void OnHostInitializationFinished()
    {

    }

    public virtual void InitializeSpectator()
    {

    }

    public virtual void OnInitializeSpectatorFinished()
    {

    }

    public virtual void SpectatorUpdate()
    {

    }
}