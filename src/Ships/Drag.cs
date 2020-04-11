using Godot;
using System;

[Tool]
public class Drag : Node2D
{
    private float _mediumDensity;
    private float _coeficient;
    private float _area;
    private Vector2 _velocity;

    [Export]
    public float MediumDensity
    {
        get { return _mediumDensity; }
        set { _mediumDensity = Mathf.Abs(value); Update(); }
    }

    [Export]
    public float Coeficient
    {
        get { return _coeficient; }
        set { _coeficient = Mathf.Abs(value); Update(); }
    }

    [Export]
    public float Area
    {
        get { return _area; }
        set { _area = Mathf.Abs(value); Update(); }
    }

    [Export]
    public bool DebugDraw { get; set; }

    [Export]
    public Vector2 Velocity
    {
        get { return _velocity; }
        set { _velocity = value; Update(); }
    }

    [Export]
    public bool Active { get; set; }

    public Vector2 TotalDrag
    {
        get
        {
            return ComputeForVelocity(Velocity);
        }
    }

    public override void _Ready()
    {
    }

    public override void _Draw()
    {
        if (Engine.EditorHint || DebugDraw)
        {
            DrawLine(Vector2.Zero, TotalDrag, Colors.IndianRed, 2);
        }
    }

    public Vector2 ComputeForVelocity(Vector2 velocity)
    {
        if (!Active) {
            return Vector2.Zero;
        }
        float scale = MediumDensity / 2 * velocity.LengthSquared() * Coeficient * Area;
        return velocity.Normalized() * -1 * scale;
    }
}
