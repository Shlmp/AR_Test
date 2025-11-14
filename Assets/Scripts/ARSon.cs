using TMPro;
using UnityEngine;

public class ARSon : ARObject
{
    [SerializeField] TextMeshProUGUI display;
    [SerializeField] Transform mainCamera;

    void Awake()
    {
        mainCamera = Camera.main.transform;
    }
    protected override void Activate()
    {

    }

    protected override void SetState(State state)
    {
        base.SetState(state);

        switch (state)
        {
            case State.On:
                display.text = "Peleando";
                break;
            case State.Off:
                display.text = "No pelien";
                break;
        }
    }

    void Update()
    {
        Vector3 lookDirection = display.transform.position - mainCamera.position;
        display.transform.rotation = Quaternion.LookRotation(lookDirection);
        if (!isColliding)
        {
            if (state != State.Off)
            {
                SetState(State.Off);
            }
        }

        isColliding = false;
    }
}