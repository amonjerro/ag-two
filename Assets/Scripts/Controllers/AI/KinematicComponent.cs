using UnityEngine;

public class KinematicComponent
{
    public Vector2 position;
    public float orientation;
    public Vector2 velocity;
    public float rotation;

    public KinematicComponent(Vector3 position, float orientation)
    {
        this.position = position;
        this.orientation = orientation;

        velocity = Vector2.zero;
        rotation = 0.0f;
    }

    public void Update(float dt, KinematicSteeringOutput steering)
    {
        position += velocity * dt;
        orientation += rotation;

        velocity = steering.linear;
        rotation = steering.angular;
    }
}