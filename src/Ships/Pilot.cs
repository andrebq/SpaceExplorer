using Godot;
using System;

[Tool]
public class Pilot : Node2D
{
    private Steering _steering;
    private Thruster[] _thrusters;
    private Drag[] _drag;
    private Vector2 _velocity;
    private bool _debugDraw;
    private Color _velocityColor;
    private float _maxSpeed;

    [Signal]
    public delegate void OnCollision(KinematicCollision2D collision);

    [Export]
    public Color VelocityColor { get { return _velocityColor; } set { _velocityColor = value; Update(); } }
    [Export]
    public Vector2 Velocity { get { return _velocity; } set { _velocity = value; Update(); } }
    [Export]
    public bool DebugDraw { get { return _debugDraw; } set { _debugDraw = value; Update(); } }
    [Export]
    public bool Break { get; set; }

    public Pilot()
    {
        VelocityColor = Colors.Green;
    }

    public override void _Ready()
    {
        int thrustersCount = 0;
        int dragCount = 0;
        foreach (var n in GetChildren())
        {
            switch (n)
            {
                case Thruster t:
                    thrustersCount++;
                    break;
                case Drag d:
                    dragCount++;
                    break;
                case Steering s:
                    _steering = s;
                    break;
                default:
                    continue;
            }
        }
        _thrusters = new Thruster[thrustersCount];
        _drag = new Drag[dragCount];
        int tIdx = 0;
        int dIdx = 0;
        foreach (var n in GetChildren())
        {
            switch (n)
            {
                case Thruster t:
                    _thrusters[tIdx] = t;
                    tIdx++;
                    break;
                case Drag d:
                    _drag[dIdx] = d;
                    dIdx++;
                    break;
            }
        }

    }

    public override void _PhysicsProcess(float delta)
    {
        if (Engine.EditorHint) { return; }
        var parent = GetParentOrNull<KinematicBody2D>();
        if (parent == null) { return; }
        _DoPhysics(parent, delta);
    }

    private void _DoPhysics(KinematicBody2D parent, float delta)
    {
        _UpdateRotation(parent, delta);
        _UpdateVelocity(delta);

        var collision = parent.MoveAndCollide(Velocity.Rotated(parent.Rotation) * delta);
        if (collision != null)
        {
            EmitSignal(nameof(OnCollision), collision);
            Velocity = Vector2.Zero;
        }
        if (Break && Velocity.LengthSquared() <= 100000) {
            Velocity = Velocity.LinearInterpolate(Vector2.Zero, 0.3f);
        }
    }

    private void _UpdateRotation(KinematicBody2D parent, float delta)
    {
        if (_steering != null && _steering.HorizontalAxis != 0f)
        {
            parent.Rotate(delta * _steering.AngularChange);
        }
    }

    private void _UpdateVelocity(float delta)
    {
        var prevVelocity = Velocity;
        var totalThrust = Vector2.Zero;
        foreach (var t in _thrusters)
        {
            totalThrust += t.ThrustVector * t.Thrust;
        }
        var totalDrag = Vector2.Zero;
        foreach (var d in _drag) {
            d.Velocity = prevVelocity;
            totalDrag += d.TotalDrag;
        }
        Velocity = Velocity + (totalThrust + totalDrag) * delta;
    }

    public override void _Draw()
    {
        if (!(DebugDraw || Engine.EditorHint))
        {
            return;
        }
        _DrawVelocity();
    }

    private void _DrawVelocity()
    {
        var targetVel = Velocity;
        if (targetVel == Vector2.Zero && Engine.EditorHint)
        {
            targetVel = Vector2.Down * 10;
        }
        DrawLine(Vector2.Zero, targetVel, VelocityColor, 2);
    }
}
