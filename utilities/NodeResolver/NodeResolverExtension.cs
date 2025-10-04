using System;
using Godot;

public static class NodeResolverExtension
{
    public static void Resolve(this Node target)
    {
        var fields = target.GetType().GetFields(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        foreach (var field in fields)
        {
            if (Attribute.GetCustomAttribute(field, typeof(NodeAttribute)) is NodeAttribute nodeAttr)
            {
                var nodePath = nodeAttr.NodePath;
                var nodeType = field.FieldType;

                var node = target.GetNode(nodePath);

                if (node == null || !nodeType.IsAssignableFrom(node.GetType()))
                {
                    GD.PrintErr($"NodeResolver: Could not find node at path '{nodePath}' or type mismatch for field '{field.Name}' in '{target.GetType().Name}'.");
                    continue;
                }

                field.SetValue(target, node);
            }
        }
    }
}