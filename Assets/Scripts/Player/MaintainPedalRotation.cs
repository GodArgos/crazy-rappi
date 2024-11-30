using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaintainPedalRotation : MonoBehaviour
{
    [SerializeField] private Transform target;
    
    void FixedUpdate()
    {
        transform.position = target.position;
    }
}
