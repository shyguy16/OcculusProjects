﻿using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
    protected OVRCameraRig CameraRig = null;
    public float FlyForce = 15;
    public Rigidbody rb;
    public AudioClip Fire;
    public GameObject bullet;
    // Use this for initialization
    void Start () {
        var p = CameraRig.transform.localPosition;
        CameraRig.transform.localPosition = p; 
    }
	void Awake()
    {
 
        OVRCameraRig[] CameraRigs = gameObject.GetComponentsInChildren<OVRCameraRig>();

        if (CameraRigs.Length == 0)
            Debug.LogWarning("OVRPlayerController: No OVRCameraRig attached.");
        else if (CameraRigs.Length > 1)
            Debug.LogWarning("OVRPlayerController: More then 1 OVRCameraRig attached.");
        else
            CameraRig = CameraRigs[0];
        rb = GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void Update () {
        Vector3 FlyDirection = Vector3.zero;

        //Checks if player's trying to fly and finds direction of flight 
        float LeftTrigger = OVRInput.Get(OVRInput.RawAxis1D.LIndexTrigger);
        if (LeftTrigger>0.1f)
        {
            Vector3 temp = CameraRig.leftHandAnchor.forward;
            temp.Normalize();
            FlyDirection += (temp * FlyForce*LeftTrigger);
        }
        //Checks if player's trying to shoot and spawns bullet
        else if (OVRInput.GetUp(OVRInput.Button.Three))
        {
            GameObject.Instantiate(bullet, CameraRig.leftHandAnchor.position, CameraRig.leftHandAnchor.localRotation);
        }
        float RightTrigger = OVRInput.Get(OVRInput.RawAxis1D.RIndexTrigger);

        //Checks if player's trying to fly and finds direction of flight 
        if (RightTrigger>0.1f)
        {
            
            Vector3 temp = CameraRig.rightHandAnchor.forward;
            temp.Normalize();
            FlyDirection += (temp * FlyForce* RightTrigger);
            //OVRHaptics.Channels[1].Mix(new OVRHapticsClip(Fire));
        }
        //Checks if player's trying to shoot and spawns bullet
        else if (OVRInput.GetUp(OVRInput.Button.One))
        {
            GameObject.Instantiate(bullet, CameraRig.rightHandAnchor.position, CameraRig.rightHandAnchor.rotation);

        }

            OVRInput.SetControllerVibration(0.5f, RightTrigger,OVRInput.Controller.RTouch);
            OVRInput.SetControllerVibration(0.5f, LeftTrigger,OVRInput.Controller.LTouch);
       

        rb.AddForce(FlyDirection);
    }
}
