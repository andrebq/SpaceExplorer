using Godot;
using System;

[Tool]
public class DockingPilot : Node2D
{
	private bool _active;
	[Signal]
	public delegate void DockingCompleted(Node2D dockPort);
	[Signal]
	public delegate void DockingStarted(Node2D dockPort);
    [Export]
    public bool DebugDraw { get; set; }
	[Export]
	public bool Active
	{
		get
		{
			return _active;
		}
		set
		{
			if (DockPort == null)
			{
				_active = false;
				return;
			}
			if (value != _active)
			{
				_active = value;
				if (_active)
				{
					EmitSignal(nameof(DockingStarted), DockPort);
				}
				else
				{
					EmitSignal(nameof(DockingCompleted), DockPort);
				}
			}
            Update();
		}
	}
	[Export]
	public Position2D DockPort { get; set; }
	public override void _Ready()
	{
	}

    public override void _Draw() {
        GD.Print(DebugDraw, "/", Active);
        if (DebugDraw && Active) {
            var from = ToLocal(DockPort.GlobalPosition);
            var to = ToLocal((GetParent() as Node2D).GlobalPosition);
            DrawLine(from, to, Colors.Honeydew, 2);
            var angle = (GetParent() as Node2D).Rotation;
            var down = (Vector2.Down * 20).Rotated(angle);
            DrawLine(to, to + down, Colors.BlueViolet, 3);
        }
    }

	public override void _PhysicsProcess(float delta)
	{
		if (!Active)
		{
			return;
		}
        Update();
		var parent = GetParent<KinematicBody2D>();
		if (parent == null) { return; }
		_DoDock(parent, delta);
	}

	private void _DoDock(KinematicBody2D parent, float delta)
	{
		_DoAdjustAngle(parent, delta);
		_DoMovement(parent, delta);
	}

	private void _DoAdjustAngle(KinematicBody2D parent, float delta)
	{
		var sourcePoint = parent.GlobalPosition;
		var targetPoint = DockPort.GlobalPosition;
		var angle = Mathf.Deg2Rad(180) - (targetPoint - sourcePoint).Angle();
		parent.Rotation = Mathf.Lerp(parent.Rotation, angle, delta);

		// too lazy to do it properly
		if (angle <= 0.05f)
		{
			//Active = false;
		}
	}

	private void _DoMovement(KinematicBody2D parent, float delta)
	{
	}
}
