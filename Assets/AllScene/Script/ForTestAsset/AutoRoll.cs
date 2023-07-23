using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRoll : MonoBehaviour
{
    public Vector3 RollSpeed = Vector3.one;

    // Update is called once per frame
    void FixedUpdate()
    {
        this.transform.Rotate(RollSpeed);
    }
}
