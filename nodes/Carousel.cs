using Godot;
using System.Collections.Generic;

public class Carousel : Node2D
{
    protected readonly List<Node> nodes = new List<Node>();
    private int activeNodeIndex = -1;

    public void AddNode(Node node)
    {
        nodes.Add(node);
    }

    public bool RemoveNode(Node node)
    {
        return nodes.Remove(node);
    }

    public void SetActiveNode(int index)
    {
        if (activeNodeIndex >= 0) RemoveChild(GetChild(0));
        AddChild(nodes[index]);
        MoveChild(nodes[index], 0);
        activeNodeIndex = index;
    }

    private void HandlePrevButtonPressed()
    {
        if (activeNodeIndex > 0) SetActiveNode(activeNodeIndex - 1);
        else SetActiveNode(nodes.Count - 1);
    }

    private void HandleNextButtonPressed()
    {
        if (activeNodeIndex < nodes.Count - 1) SetActiveNode(activeNodeIndex + 1);
        else SetActiveNode(0);
    }

    private void HandleTreeExited()
    {
        nodes.ForEach(node => node.Free());
        nodes.Clear();
    }
}
