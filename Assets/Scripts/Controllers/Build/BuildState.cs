using GameCursor;
using UnityEngine;

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

    public BuildState(BuildController controller, BuildStateMachine fsm)
    {
        this.controller = controller;
        this.fsm = fsm;
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

    public BuildStart(BuildController controller, BuildStateMachine fsm) : base(controller, fsm) { }

    public override void HandleClick(Vector3 mouseLocation)
    {
        int x = (int) mouseLocation.x;
        int y = (int) mouseLocation.y;
        controller.SetRectOrigin(x, y);
    }

    public override void Cancel()
    {
        cursorReference.SetCursorState(CursorStates.FreeHand);
    }

    public override void OnEnter()
    {
        if (cursorReference == null)
        {
            cursorReference = ServiceLocator.Instance.GetService<CursorManager>();
        }
    }

    public override void OnUpdate()
    {
        
    }

    public override void OnExit() {

        fsm.MoveToState(BuildStates.Make);
    }
}

public class BuildMake : BuildState
{
    public BuildMake(BuildController controller, BuildStateMachine fsm) : base(controller, fsm) { }
    public override void HandleClick(Vector3 mouseLocation)
    {
        int x = (int)mouseLocation.x;
        int y = (int)mouseLocation.y;
        controller.BuildRoom();
    }

    public override void Cancel()
    {
        Exit();
    }

    public override void OnUpdate()
    {
        
    }

    public override void OnEnter()
    {
        if (cursorReference == null)
        {
            cursorReference = ServiceLocator.Instance.GetService<CursorManager>();
        }
    }

    public override void OnExit()
    {
        

        cursorReference.SetCursorState(CursorStates.FreeHand);
        fsm.MoveToState(BuildStates.Start);
    }
}