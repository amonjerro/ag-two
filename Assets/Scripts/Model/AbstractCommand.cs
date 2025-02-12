/// <summary>
/// Abstract command class.
/// Used to create specific commands that can be dynamically assigned to buttons to execute.
/// </summary>
public abstract class AbstractCommand
{
    public abstract void Execute();
}