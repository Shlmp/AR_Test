using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class ARObject : MonoBehaviour
{
    public List<ARObject> interactables = new List<ARObject>();

    public bool isColliding;

    public enum State
    {
        On,
        Off
    }
    protected State state;

    void Start()
    {
        SetState(State.Off);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<ARObject>(out ARObject aRObject))
        {
            SetState(State.On);
        }
    }

    void OnTriggerStay(Collider other)
    {
        isColliding = true;

        if (state != State.On)
        {
            SetState(State.Off);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<ARObject>(out ARObject aRObject))
        {
            SetState(State.Off);
        }
    }

    protected abstract void Activate();
    protected virtual void SetState(State state) { }
}