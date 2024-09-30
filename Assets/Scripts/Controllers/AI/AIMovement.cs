using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIMovement
{
    protected ANPC npc;
    protected Vector2 destination;
    protected bool bIsDestinationSet;
    protected abstract Vector2 PickDestination();

    public abstract KinematicSteeringOutput UpdatePosition();
    protected abstract float UpdateRotation(Vector2 velocity);

}

public struct KinematicSteeringOutput
{
    public Vector2 linear;
    public float angular;

    public KinematicSteeringOutput(Vector2 l, float a)
    {
        linear = l; angular = a;
    }
}


public static class AIMovementFactory
{
    public static AIMovement MakeMovement(ANPC npc)
    {
        if (npc.bMovementBounded)
        {
            return MakeBoundedMovement(npc);
        }
        return MakeNormalMovement(npc);
        
    }

    private static AIMovement MakeNormalMovement(ANPC npc)
    {
        switch (npc.type)
        {
            case NPCTypes.Staff:
                return new SeekMovement(npc);
            default:

                return new WanderMovement(npc);
        }
    }

    private static AIMovement MakeBoundedMovement(ANPC npc)
    {
        switch (npc.type)
        {
            case NPCTypes.Staff:
                return new BoundedMovement(new SeekMovement(npc), npc);
            default:

                return new BoundedMovement(new WanderMovement(npc), npc);
        }
    }
}