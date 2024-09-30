using UnityEngine;

public class SeekMovement : AIMovement
{
    int currentDestinationIndex;
    Vector2 currentDestination = Vector2.zero;
    public SeekMovement(ANPC npc)
    {
        this.npc = npc;
        currentDestinationIndex = 0;
    }

    protected override Vector2 PickDestination()
    {
        Vector2 result = Vector2.zero;
        if (currentDestinationIndex == 0)
        {
            result = new Vector2(-4, -4);
        }
        else if (currentDestinationIndex == 1)
        {
            result = new Vector2(4, -4);
        }
        else if (currentDestinationIndex == 2) {
            result = new Vector2(4, 4);
        } else
        {
            result = new Vector2(-4, 4);
        }

        currentDestinationIndex++;
        if (currentDestinationIndex > 3)
        {
            currentDestinationIndex = 0;
        }

        return result;
    }

    protected override float UpdateRotation(Vector2 velocity)
    {
        if (velocity.magnitude > 0)
        {
            return Mathf.Atan2(-velocity.x, velocity.y) * Mathf.Rad2Deg;
        }
        return Quaternion.Angle(Quaternion.identity, npc.transform.rotation);
    }

    public override KinematicSteeringOutput UpdatePosition()
    {
        if (currentDestination == Vector2.zero || Vector2.Distance(currentDestination, npc.transform.position) < 0.1f) {
            currentDestination = PickDestination();
        }

        KinematicSteeringOutput output = new KinematicSteeringOutput();

        Vector2 pos = new Vector2(npc.transform.position.x, npc.transform.position.y);
        Vector2 velocity = currentDestination - pos;
        velocity = velocity.normalized;
        
        npc.SetOrientation(UpdateRotation(velocity));
        output.linear = velocity * npc.maxSpeed;
        output.angular = 0.0f;
        return output;
    }
}