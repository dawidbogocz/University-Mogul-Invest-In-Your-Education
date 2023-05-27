using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField]
    GameObject focusObject;//the target object
    Vector3 cameraOffset;
    private bool isRotating;

    // Start is called before the first frame update
    void Start()
    {
        cameraOffset = new Vector3(0, 55, -125);
        SetCamera(focusObject, cameraOffset);
        isRotating = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isRotating)
        {
            RotateCamera();
        }
        else
        {
            MoveCamera();
        }
    }

    public void SetCamera(GameObject focusPoint, Vector3 offset)
    {
        isRotating = false;
        focusObject = focusPoint;
        cameraOffset = offset;
    }

    void MoveCamera()
    {
        this.transform.position = focusObject.transform.position + cameraOffset;
        this.transform.LookAt(focusObject.transform.position);
    }

    void RotateCamera()
    {
        this.transform.RotateAround(focusObject.transform.position, new Vector3(0.0f, 1.0f, 0.0f), 10 * Time.deltaTime);
    }
}
