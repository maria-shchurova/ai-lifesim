using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacePlayerCamera : MonoBehaviour
{
    public Transform PlayerCamera;

    private void Start()
    {
        PlayerCamera = Camera.main.transform;
    }
    void Update()
    {
        transform.LookAt(PlayerCamera);
    }
}
