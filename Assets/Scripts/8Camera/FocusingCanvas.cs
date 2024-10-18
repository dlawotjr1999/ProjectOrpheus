using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusingCanvas : MonoBehaviour
{
    private void FixedUpdate()
    {
        transform.forward = Camera.main.transform.forward;
    }
}
