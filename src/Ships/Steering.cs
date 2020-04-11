using Godot;
using System;

[Tool]
public class Steering : Node2D
{
    // In GODOT 0 is rendered to left and we want to render it down
    static readonly float angleOffset = Mathf.Deg2Rad(90);
    private float _speed;
    private float _horizontalAxis;
    [Export]
    public float AngularSpeed { get { return _speed; } set { _speed = Mathf.Abs(value); Update(); } }
    public float HorizontalAxis
    {
        get
        {
            return _horizontalAxis;
        }
        set
        {
            _horizontalAxis = value;
            Update();
        }
    }
    public float AngularChange
    {
        get
        {
            return Mathf.Deg2Rad(AngularSpeed * HorizontalAxis);
        }
    }

    [Export]
    public bool DebugDraw { get; set; }
    public override void _Ready()
    {
    }

    public override void _Draw()
    {
        if (Engine.EditorHint || DebugDraw)
        {
            var angleDraw = angleOffset + AngularChange;
            DrawArc(Position,
                Mathf.Lerp(10, 20, Mathf.Min(_speed / 100, 1f)),
                angleDraw - Mathf.Deg2Rad(20),
                angleDraw + Mathf.Deg2Rad(20),
                32,
                Colors.Green,
                Mathf.Lerp(2, 8, Mathf.Min(_speed / 1000, 1f)));
        }
    }
}
