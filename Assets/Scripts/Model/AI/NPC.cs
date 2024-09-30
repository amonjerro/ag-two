using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class ANPC : MonoBehaviour
{
    public float maxSpeed;
    public float maxRotation;
    public bool bMovementBounded;
    public Vector2 boundDimensions;
    public NPCTypes type;
    protected AIMovement movement;
    protected KinematicComponent kinematics;
    protected abstract void Move();
    public float GetOrientation()
    {
        return kinematics.orientation;
    }

    public void SetOrientation(float val)
    {
        kinematics.orientation = val;
    }

    public abstract NPCInformation GetInformation();
}
