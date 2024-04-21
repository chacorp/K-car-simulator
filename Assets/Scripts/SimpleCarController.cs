using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class AxleInfo {
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool motor;
    public bool steering;
}
     
public class SimpleCarController : MonoBehaviour
{
    public List<AxleInfo> axleInfos; 
    public float maxMotorTorque = 400f;
    public float maxSteeringAngle = 30f;
    public float maxFriction = 2f;
    public float maxBrakesTorque = 350f;
    // finds the corresponding visual wheel
    // correctly applies the transform
    public void ApplyLocalPositionToVisuals(WheelCollider collider)
    {
        if (collider.transform.childCount == 0) {
            return;
        }
     
        Transform visualWheel = collider.transform.GetChild(0);
     
        Vector3 position;
        Quaternion rotation;

        collider.GetWorldPose(out position, out rotation);
     
        visualWheel.transform.position = position;
        visualWheel.transform.rotation = rotation;
    }
     
    public void FixedUpdate()
    {
        float motor = maxMotorTorque * Input.GetAxis("Vertical") * Time.deltaTime;
        float steering = maxSteeringAngle * Input.GetAxis("Horizontal") * Time.deltaTime;
        float friction = maxFriction * (1 - Mathf.Abs(Input.GetAxis("Vertical"))) * Time.deltaTime;
        float brakes = maxBrakesTorque;// * 1 - Mathf.Abs(Input.GetAxis("Vertical"));
        // 
     
        foreach (AxleInfo axleInfo in axleInfos) {
            if (axleInfo.steering) {
                axleInfo.leftWheel.steerAngle = steering;
                axleInfo.rightWheel.steerAngle = steering;
            }
            if (axleInfo.motor) {
                axleInfo.leftWheel.motorTorque = motor;
                axleInfo.rightWheel.motorTorque = motor;
            }
            if (!Input.anyKey){
                axleInfo.leftWheel.brakeTorque = friction;
                axleInfo.rightWheel.brakeTorque = friction;
            }else{
                axleInfo.leftWheel.brakeTorque = 0;
                axleInfo.rightWheel.brakeTorque = 0;
            }
            ApplyLocalPositionToVisuals(axleInfo.leftWheel);
            ApplyLocalPositionToVisuals(axleInfo.rightWheel);
        }
        // Debug.Log("m: "+motor+"\ts: "+steering+"\tf:"+friction);
        // Debug.Log() ;
    }
}