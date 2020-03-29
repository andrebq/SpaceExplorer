using Godot;
using System;

public class PlayerOneInput : ShipInput {

    public static readonly PlayerOneInput Instance = new PlayerOneInput();

    public bool SpeedBrake {
        get {
            return Input.IsActionPressed("SpeedBrake");
        }
    }

    public float VerticalAxis {
        get {
            if (Forward) {
                return Input.GetActionStrength("Forward");
            } else if (Backward) {
                return -Input.GetActionStrength("Backward");
            }
            return 0;
        }
    }

    public float HorizontalAxis {
        get {
            if (RotateLeft) {
                return -Input.GetActionStrength("RotateLeft");
            } else if (RotateRight) {
                return Input.GetActionStrength("RotateRight");
            }
            return 0;
        }
    }

    public Boolean Forward {
        get {
            return Input.IsActionPressed("Forward");
        }
    }

    public Boolean Backward {
        get {
            return Input.IsActionPressed("Backward");
        }
    }

    public Boolean RotateLeft {
        get {
            return Input.IsActionPressed("RotateLeft");
        }
    }

    public Boolean RotateRight {
        get {
            return Input.IsActionPressed("RotateRight");
        }
    }

    public Vector2 UpdateHeading(Vector2 down, float rotationRad) {
        return (down * VerticalAxis).Rotated(rotationRad);
    }

    public float RotationAxis(float delta) {
        return HorizontalAxis * Mathf.Deg2Rad(180) * delta;
    }
}