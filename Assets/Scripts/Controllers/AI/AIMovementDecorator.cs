using UnityEngine;

public abstract class AIMovementDecorator : AIMovement
{
    protected AIMovement wrappee;

    public AIMovementDecorator(AIMovement wrappee, ANPC npc)
    {
        this.npc = npc;
        this.wrappee = wrappee;
    }

    public override KinematicSteeringOutput UpdatePosition()
    {
        return wrappee.UpdatePosition();
    }
}

public class BoundedMovement : AIMovementDecorator
{

    public BoundedMovement(AIMovement wrappee, ANPC npc) : base(wrappee, npc) { }

    protected override Vector2 PickDestination()
    {
        throw new System.NotImplementedException();
    }

    protected override float UpdateRotation(Vector2 velocity)
    {
        if (velocity.magnitude > 0)
        {
            return Mathf.Rad2Deg * Mathf.Atan2(-velocity.x, velocity.y);
        }
        return Quaternion.Angle(Quaternion.identity, npc.transform.rotation);
    }

    public override KinematicSteeringOutput UpdatePosition()
    {
        // Do the original thing
        KinematicSteeringOutput kinematicOutput = base.UpdatePosition();

        //Bind the movement to the limits
        Vector2 npcPosition = npc.transform.position;
        bool directionUpdated = false;
        Vector2 dtVelocity = kinematicOutput.linear * Time.deltaTime;
        if (npcPosition.x + dtVelocity.x < -npc.boundDimensions.x)
        {
            kinematicOutput.linear.x *= -1;
            directionUpdated = true;
        } else if (npcPosition.x + dtVelocity.x > npc.boundDimensions.x)
        {
            kinematicOutput.linear.x *= -1;
            directionUpdated = true;
        }

        if (npcPosition.y + dtVelocity.y < -npc.boundDimensions.y)
        {
            kinematicOutput.linear.y *= -1;
            directionUpdated = true;
        } else if(npcPosition.y + dtVelocity.y > npc.boundDimensions.y)
        {
            kinematicOutput.linear.y *= -1;
            directionUpdated = true;
        }

        if (directionUpdated)
        {
            npc.SetOrientation((npc.GetOrientation() + 180) % 360);
        }
        return kinematicOutput;
    }
}