using UnityEngine;

public class BuildStateMachine
{
    BuildStart _buildStart;
    BuildMake _buildMake;
    BuildState currentState;


    public BuildStateMachine(BuildController controller)
    {
        _buildStart = new BuildStart(controller, this);
        _buildMake = new BuildMake(controller, this);

        MoveToState(BuildStates.Start);
    }

    public void Update()
    {
        currentState.Update();
    }

    public void HandleClick(Vector3 position)
    {
        currentState.HandleClick(position);
    }

    public void MoveToState(BuildStates state)
    {
        switch (state)
        {
            case BuildStates.Make:
                currentState = _buildMake;
                break;
            default:
                currentState = _buildStart;
                break;
        }

        currentState.Enter();
    }
}