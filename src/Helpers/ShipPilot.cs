using Godot;

struct Thrusters {
	public float Foward;
	public float Backward;
}
class ShipPilot {
	private KinematicBody2D body;

	private ShipInput input;

	private float SpeedBrakeFactor {
		get {
			if (input == null || !input.SpeedBrake) { return 1; }
			return 3;
		}
	}

	public float MaxSpeed { get; set; }
	public Thrusters Thrusters;
	public float Speed { get; set; }
	public float Drag { get; set; }
	public float MaxThrust {get; set;}

	public bool IsThurstersON {
		get {
			return input != null && input.VerticalAxis != 0;
		}
	}

	public ShipPilot() {
		MaxSpeed = 50;
		Drag = 20;
		MaxThrust = 30;
	}

	public void Ready(KinematicBody2D body, ShipInput input) {
		this.body = body;
		this.input = input;
	}

	public void Process(float delta ) {
		UpdateThursters(delta);
		Speed += ((Thrusters.Foward - Thrusters.Backward) - ComputeDrag(Speed)) * delta;

		Speed = ZeroValue(Speed);

		body.Rotate(input.RotationAxis(delta));
		body.MoveAndCollide(Vector2.Down.Rotated(body.Rotation) * Speed * -1);
	}

	public void UpdateThursters(float delta) {
		float axisValue = input.VerticalAxis;
		if (axisValue == 0) {
			Thrusters.Foward = 0;
			Thrusters.Backward = 0;
		} else if (axisValue > 0) {
			Thrusters.Foward = MaxThrust * Mathf.Abs(axisValue);
			Thrusters.Backward = 0;
		} else {
			Thrusters.Foward = 0;
			Thrusters.Backward = MaxThrust * Mathf.Abs(axisValue);
		}
	}

	private float CapSpeed(float newSpeed) {
		return Mathf.Min(MaxSpeed, Mathf.Abs(newSpeed)) * Mathf.Sign(newSpeed);
	}

	private float ComputeDrag(float speed) {
		float factor = speed / MaxSpeed;
		return factor * Drag * SpeedBrakeFactor;
	}

	private float ApplyDrag(float speed, float drag) {
		speed -= drag;
		return speed;
	}

	private float ZeroValue(float value, float tolerance = 0.5f) {
		if (Mathf.Abs(value) <= tolerance) {
			return 0.0f;
		}
		return value;
	}
}
