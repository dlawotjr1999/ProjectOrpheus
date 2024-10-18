using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFocusScript : MonoBehaviour            // 오브젝트에 달려있는 UI
{
    private void TrackCamRotation()
    {
        transform.forward = Camera.main.transform.forward;
    }

    private void Update()
    {
        TrackCamRotation();
    }
}