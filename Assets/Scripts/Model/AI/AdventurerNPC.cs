using UnityEngine;
public class AdventurerNPC : ANPC
{
    public int index;
    private void Awake()
    {
        movement = AIMovementFactory.MakeMovement(this);
        kinematics = new KinematicComponent(transform.position, Quaternion.Angle(Quaternion.identity, transform.rotation));
    }

    private void Update()
    {
        Move();
    }

    protected override void Move()
    {
        KinematicSteeringOutput steering = movement.UpdatePosition();
        kinematics.Update(Time.deltaTime, steering);
        transform.position = new Vector3(kinematics.position.x, kinematics.position.y, 0);
        transform.rotation = Quaternion.AngleAxis(kinematics.orientation, Vector3.forward);
    }

    public override NPCInformation GetInformation()
    {
        NPCInformation info = new NPCInformation();
        info.index = index;
        info.type = NPCTypes.Adventurer;
        return info;
    }
}