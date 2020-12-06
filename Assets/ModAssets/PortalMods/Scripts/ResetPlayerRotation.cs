using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPlayerRotation : MonoBehaviour
{
    /*// Maximum turn rate in degrees per second.
    public float turningRate = 30f;
    // Rotation we should blend towards.
    private Quaternion _targetRotation = Quaternion.identity;
    // Call this when you want to turn the object smoothly.
    public void SetBlendedEulerAngles(Vector3 angles)
    {
        _targetRotation = Quaternion.Euler(angles);
    }

    private void Update()
    {
        if (transform.rotation.z == -180){
            // Turn towards our target rotation.
            transform.rotation = Quaternion.RotateTowards(transform.rotation, _targetRotation, turningRate * Time.deltaTime);
        }
        
        
    }*/
    private void Update()
    {
        Vector3 rotationVector = transform.rotation.eulerAngles;
        rotationVector.x = 0;
        rotationVector.z = 0;
        transform.rotation = Quaternion.Euler(rotationVector);
    }
}
