using Godot;

class ShipPilot {
    private KinematicBody2D body;
    private ShipInput input;

    public float MaxSpeed { get; set; }
    public float Accel { get; set; }
    public float Speed { get; set; }
    public float Drag { get; set; }

    public bool IsThurstersON {
        get {
            return input != null && input.VerticalAxis != 0;
        }
    }

    public ShipPilot() {
        MaxSpeed = 50;
        Accel = 25;
        Drag = 100;
    }

    public void Ready(KinematicBody2D body, ShipInput input) {
        this.body = body;
        this.input = input;
    }

    public void Process(float delta ) {
        Speed = CapSpeed(delta * (Mathf.Abs(input.VerticalAxis) * Accel));
        Speed = ApplyDrag(Speed, ComputeDrag(Speed, delta));
        Speed = ZeroValue(Speed, 0.1f);
        GD.Print("s: ", Speed);
		body.Rotate(input.RotationAxis(delta));
		body.MoveAndCollide(Vector2.Down.Rotated(body.Rotation) * Mathf.Sign(Speed) * Speed);
    }

    private float CapSpeed(float newSpeed) {
        return Mathf.Min(MaxSpeed, Mathf.Abs(newSpeed)) * Mathf.Sign(newSpeed);
    }

    private float ComputeDrag(float speed, float delta) {
        float factor = speed / MaxSpeed;
        return factor * delta * Drag;
    }

    private float ApplyDrag(float speed, float drag) {
        speed -= drag;
        return speed;
    }

    private float ZeroValue(float value, float tolerance) {
        if (Mathf.Abs(value) <= tolerance) {
            return 0.0f;
        }
        return value;
    }
}