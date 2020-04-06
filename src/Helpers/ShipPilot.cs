using Godot;

struct Thrusters
{
    public float Foward;
    public float Backward;

    public bool HasThrust
    {
        get
        {
            return Foward > 0 || Backward > 0;
        }
    }

    public Vector2 ComputeVector() {
        return Vector2.Up * Foward + Vector2.Down * Backward;
    }
}
class ShipPilot
{
    private KinematicBody2D body;

    private ShipInput input;

    private bool BreakersON
    {
        get
        {
            return input.SpeedBrake;
        }
    }

    public float SpeedBreakerFactor { get; set; }
    public float MaxSpeed { get; set; }
    public Thrusters Thrusters;
    public Vector2 Velocity { get; set; }
    public float Drag { get; set; }
    public float MaxThrust { get; set; }

    public bool IsThurstersON
    {
        get
        {
            return input != null && input.VerticalAxis != 0;
        }
    }

    public ShipPilot()
    {
        MaxSpeed = 50;
        Drag = 20;
        MaxThrust = 500;
        SpeedBreakerFactor = 10;
    }

    public void Ready(KinematicBody2D body, ShipInput input)
    {
        this.body = body;
        this.input = input;
    }

    public void Process(float delta)
    {
        UpdateThursters(delta);
        if (!Thrusters.HasThrust)
        {
            Velocity = ZeroValueVector(Velocity);
        }
        // Instant speed
        Velocity = Thrusters.ComputeVector();
        var movement = Velocity.Rotated(body.Rotation);
        var collision = body.MoveAndCollide(movement * delta);
        if (collision != null)
        {
            Velocity = Vector2.Zero;
        }
        body.Rotate(input.RotationAxis(delta));
    }

    public void UpdateThursters(float delta)
    {
        float axisValue = input.VerticalAxis;
        if (axisValue == 0)
        {
            Thrusters.Foward = 0;
            Thrusters.Backward = 0;
        }
        else if (axisValue > 0)
        {
            //Thrusters.Foward = Mathf.Lerp(Thrusters.Foward, MaxThrust, Mathf.Abs(axisValue) * delta);
            // Instant Thrust
            Thrusters.Foward = MaxThrust * Mathf.Abs(axisValue);
            Thrusters.Backward = 0;
        }
        else
        {
            Thrusters.Foward = 0;
            //Thrusters.Backward = Mathf.Lerp(Thrusters.Backward, MaxThrust, Mathf.Abs(axisValue) * delta);
            // Instant Thrust
            Thrusters.Backward = MaxThrust * Mathf.Abs(axisValue);
        }
    }

    private float CapSpeed(float newSpeed)
    {
        return Mathf.Min(MaxSpeed, Mathf.Abs(newSpeed)) * Mathf.Sign(newSpeed);
    }

    private float ComputeDrag(float speed)
    {
        return Mathf.Lerp(0, MaxSpeed, Mathf.Abs(speed)/MaxSpeed);
    }
    private Vector2 ZeroValueVector(Vector2 vector, float tolerance = 0.5f) {
        if (vector.LengthSquared() <= tolerance*tolerance) {
            return Vector2.Zero;
        }
        return vector;
    }
    private float ZeroValue(float value, float tolerance = 0.5f)
    {
        if (Mathf.Abs(value) <= tolerance)
        {
            return 0.0f;
        }
        return value;
    }
}
