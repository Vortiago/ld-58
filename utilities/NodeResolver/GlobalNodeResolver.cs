using Godot;

public partial class GlobalNodeResolver : Node
{
    public override void _EnterTree()
    {
        GetTree().NodeAdded += OnNodeAdded;
    }

    public override void _ExitTree()
    {
        GetTree().NodeAdded -= OnNodeAdded;
    }

    private void OnNodeAdded(Node node)
    {
        node.Resolve();
    }
}