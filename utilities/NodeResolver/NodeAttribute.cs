using System;

[AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
public sealed class NodeAttribute : Attribute
{
    public string NodePath { get; }

    public NodeAttribute(string nodePath)
    {
        NodePath = nodePath;
    }
}