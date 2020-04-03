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
    public float Speed { get; set; }
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
        MaxThrust = 30;
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
        var thrust = (Thrusters.Foward - Thrusters.Backward) * delta;
        var drag = ComputeDrag(Speed) * delta;
        Speed += (thrust - drag);

        if (!Thrusters.HasThrust)
        {
            Speed = ZeroValue(Speed);
        }

        body.Rotate(input.RotationAxis(delta));
        var movement = Vector2.Down.Rotated(body.Rotation) * Speed * -1;
        var collision = body.MoveAndCollide(movement);
        if (collision != null)
        {
            Speed = 0;
        }
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
            Thrusters.Foward = Mathf.Lerp(Thrusters.Foward, MaxThrust, Mathf.Abs(axisValue) * delta);
            Thrusters.Backward = 0;
        }
        else
        {
            Thrusters.Foward = 0;
            Thrusters.Backward = Mathf.Lerp(Thrusters.Backward, MaxThrust, Mathf.Abs(axisValue) * delta);
        }
    }

    private float CapSpeed(float newSpeed)
    {
        return Mathf.Min(MaxSpeed, Mathf.Abs(newSpeed)) * Mathf.Sign(newSpeed);
    }

    private float ComputeDrag(float speed)
    {
        if (BreakersON)
        {
            speed *= SpeedBreakerFactor;
        }
        float factor = speed / MaxSpeed;
        return factor * Drag;
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
