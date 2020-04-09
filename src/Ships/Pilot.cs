using Godot;
using System;

[Tool]
public class Pilot : Node2D
{
    private Steering _steering;
    private Thruster[] _thrusters;
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
    public float MaxSpeed { get { return _maxSpeed; } set { _maxSpeed = Math.Abs(value); Update(); } }

    public Pilot()
    {
        VelocityColor = Colors.Green;
        MaxSpeed = 1000;
    }

    public override void _Ready()
    {
        int thrustersCount = 0;
        foreach (var n in GetChildren())
        {
            switch (n)
            {
                case Thruster t:
                    thrustersCount++;
                    break;
                case Steering s:
                    _steering = s;
                    break;
                default:
                    continue;
            }
        }
        _thrusters = new Thruster[thrustersCount];
        int idx = 0;
        foreach (var n in GetChildren())
        {
            if (n is Thruster t)
            {
                _thrusters[idx] = t;
                idx++;
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
    }

    private void _UpdateRotation(KinematicBody2D parent, float delta)
    {
        if (_steering != null && _steering.HorizontalAxis != 0f) {
            parent.Rotate(delta * _steering.AngularChange);
        }
    }

    private void _UpdateVelocity(float delta)
    {
        foreach (var t in _thrusters)
        {
            Velocity = Velocity + t.ThrustVector * t.Thrust * delta;
        }
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
