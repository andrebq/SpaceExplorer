using Godot;
using System;

public class HomeStation : KinematicBody2D, IDockStation
{
    private Position2D DockPosition;

    public override void _Ready()
    {
        DockPosition = FindNode(nameof(DockPosition)) as Position2D;
    }

    private void _ShipEnteredDockingDistance(PhysicsBody2D body)
    {
        if (body is IDockable t)
        {
            t.OfferDock(this);
        }
    }

    private void _ShipExitedDockingDistance(PhysicsBody2D body)
    {
        if (body is IDockable t)
        {
            t.RevokeDock(this);
        }
    }

    public Position2D Center { get { return DockPosition; }}
}
