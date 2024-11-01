using UnityEngine;


public class WanderMovement : AIMovement
{
    public WanderMovement(ANPC npc)
    {
        this.npc = npc;
        destination = Vector2.zero;
        bIsDestinationSet = false;
    }

    protected override Vector2 PickDestination()
    {
        throw new System.NotImplementedException();
    }

    protected override float UpdateRotation(Vector2 velocity)
    {
        return Random.Range(0.0f, 1.0f) - Random.Range(0.0f, 1.0f);
    }

    public override KinematicSteeringOutput UpdatePosition()
    {
        // Update position
        KinematicSteeringOutput output = new KinematicSteeringOutput();
        float currentOrientation = npc.GetOrientation();

        Vector3 velocity = npc.transform.up;
        Debug.DrawLine(npc.transform.position, npc.transform.position + npc.transform.up);
        Debug.DrawLine(npc.transform.position, npc.transform.position + velocity, Color.red);

        velocity *= npc.maxSpeed;

        output.linear = new Vector2(velocity.x, velocity.y);
        output.angular = UpdateRotation(Vector2.zero) * npc.maxRotation;
        return output;
    }
}