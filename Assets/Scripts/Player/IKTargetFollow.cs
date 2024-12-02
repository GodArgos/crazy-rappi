using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RigPartMap
{
    public Transform target;
    public Transform source;
    public Vector3 trackingPositionOffset;
    public Vector3 trackingRotationOffset;
    public bool allowPos = true;
    public bool allowRot = true;

    public void Map()
    {
        if (allowPos)
        {
            source.position = target.TransformPoint(trackingPositionOffset);
        }
        
        if (allowRot)
        {
            source.rotation = target.rotation * Quaternion.Euler(trackingRotationOffset);
        }
        
    }
}


public class IKTargetFollow : MonoBehaviour
{
    [Range(0, 1)]
    public float turnSmoothness = 0.1f;
    [SerializeField] private RigPartMap leftHand;
    [SerializeField] private RigPartMap rightHand;
    [SerializeField] private RigPartMap leftFoot;
    [SerializeField] private RigPartMap rightFoot;

    private void FixedUpdate()
    {     
        leftHand.Map();
        rightHand.Map();
        leftFoot.Map();
        rightFoot.Map();
    }
}
