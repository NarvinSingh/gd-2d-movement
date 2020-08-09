using Godot;
using System.Collections.Generic;

public class Carousel : Node2D
{
    protected readonly List<Node> carouselNodes = new List<Node>();
    private int activeNodeIndex = -1;

    protected void SetActiveNode(int index)
    {
        if (activeNodeIndex >= 0) RemoveChild(GetChild(0));
        AddChild(carouselNodes[index]);
        MoveChild(carouselNodes[index], 0);
        activeNodeIndex = index;
    }

    private void HandlePrevButtonPressed()
    {
        if (activeNodeIndex > 0) SetActiveNode(activeNodeIndex - 1);
        else SetActiveNode(carouselNodes.Count - 1);
    }

    private void HandleNextButtonPressed()
    {
        if (activeNodeIndex < carouselNodes.Count - 1) SetActiveNode(activeNodeIndex + 1);
        else SetActiveNode(0);
    }
}
