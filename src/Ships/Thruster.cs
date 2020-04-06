using Godot;
using System;

[Tool]
public class Thruster : Node2D
{
    private float _maxThurst;
    private float _thrust;
    private float _handleSize;
    [Export]
    public float MaxThurst
    {
        get { return _maxThurst; }
        set { _maxThurst = Mathf.Abs(value); if (Engine.EditorHint) { Update(); } }
    }

    [Export]
    public float Thrust
    {
        get { return _thrust; }
        set { _thrust = Mathf.Abs(value); if (Engine.EditorHint) { Update(); } }
    }

    [Export]
    public float HandleSize
    {
        get { return _handleSize; }
        set { _handleSize = Mathf.Abs(value); Update(); }
    }

    [Export]
    public bool Active { get; set; }
    [Export]
    public bool DebugPaint { get; set; }
    [Export(PropertyHint.Range, "0,1.0")]
    public float ThurstLimiter {get; set;}

    public Thruster()
    {
        MaxThurst = 1000;
        Thrust = 0;
        HandleSize = 100;
        ThurstLimiter = 1f;
        DebugPaint = false;
        Active = false;
    }

    public override void _Ready()
    {
    }

    public override void _PhysicsProcess(float delta) {
        if (Active) {
            Thrust = Interpolate(Thrust, MaxThurst*ThurstLimiter, 0.8f *delta);
        } else {
            Thrust = 0f;
        }
        Update();
    }

    private float Interpolate(float actual, float target, float step) {
        return Mathf.Lerp(actual, target, step);
    }

    public override void _Draw()
    {
        if (Engine.EditorHint || DebugPaint)
        {
            var finalHeight = Mathf.Max(0.1f, Mathf.Min(1, Mathf.Abs(Thrust / MaxThurst))) * HandleSize;
            var color = Active ? Colors.LightPink : Colors.LightCyan;
            DrawLine(Vector2.Zero, new Vector2(0, finalHeight), color, 4);
        }
    }
}