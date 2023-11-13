using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calibration : MonoBehaviour
{
    public void CalibrateHeight(Vector3 position)
    {
        transform.position = new Vector3(transform.position.x, position.y, transform.position.z);
    }
}
