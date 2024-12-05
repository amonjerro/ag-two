using UnityEngine;
using GameCursor;

namespace TileBuilder
{
    public class BuildStateMachine
    {
        BuildStart _buildStart;
        BuildMake _buildMake;
        CursorManager _cursorManager;
        BuildState currentState;


        public BuildStateMachine(BuildController controller, CursorManager cursorManager)
        {
            _buildStart = new BuildStart(controller, this, cursorManager);
            _buildMake = new BuildMake(controller, this, cursorManager);

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

        public void HandleCancel()
        {
            currentState.Cancel();
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
}
