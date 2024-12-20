using GameCursor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TileBuilder
{
    public enum BuildStates
    {
        Start,
        Make,
        Designate
    }

    public abstract class BuildState
    {
        protected BuildController controller;
        protected BuildStateMachine fsm;
        protected CursorManager cursorReference;
        public abstract void HandleClick(Vector3 mouseLocation);

        public abstract void Cancel();

        public BuildState(BuildController controller, BuildStateMachine fsm, CursorManager cursorManager)
        {
            this.controller = controller;
            this.fsm = fsm;
            cursorReference = cursorManager;
        }

        public abstract void OnExit();

        public void Exit()
        {
            OnExit();
        }

        public abstract void OnEnter();

        public void Enter()
        {
            OnEnter();
        }
        public abstract void OnUpdate();

        public void Update()
        {
            OnUpdate();
        }

    }

    public class BuildStart : BuildState
    {

        public BuildStart(BuildController controller, BuildStateMachine fsm, CursorManager csm) : base(controller, fsm, csm) { }

        public override void HandleClick(Vector3 mouseLocation)
        {
            controller.SetRectOrigin(mouseLocation);
            //cursorReference.SetBoxOrigin(mouseLocation.x, mouseLocation.y);
            Exit();
        }

        public override void Cancel()
        {
            //cursorReference.HideSelectionBox();
            cursorReference.SetCursorState(CursorStates.FreeHand);
            fsm.MoveToState(BuildStates.Start);
        }

        public override void OnEnter()
        {
        }

        public override void OnUpdate()
        {

        }

        public override void OnExit()
        {
            //cursorReference.ShowSelectionBox();
            fsm.MoveToState(BuildStates.Make);
        }
    }

    public class BuildMake : BuildState
    {
        public BuildMake(BuildController controller, BuildStateMachine fsm, CursorManager csm) : base(controller, fsm, csm) { }
        public override void HandleClick(Vector3 mouseLocation)
        {
            int x = (int)mouseLocation.x;
            int y = (int)mouseLocation.y;
            controller.BuildRoom();
            Exit();
        }

        public override void Cancel()
        {
            Exit();
        }

        public override void OnUpdate()
        {
            controller.SetRectEnd(Mouse.current.position.ReadValue());
        }

        public override void OnEnter()
        {

        }

        public override void OnExit()
        {
            controller.UpdateTileColors(Color.white);
            //cursorReference.HideSelectionBox();
            cursorReference.SetCursorState(CursorStates.FreeHand);
            fsm.MoveToState(BuildStates.Start);
        }
    }
}
