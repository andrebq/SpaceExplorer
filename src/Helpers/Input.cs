using Godot;

public interface ShipInput {
    float VerticalAxis { get; }

    Vector2 UpdateHeading(Vector2 down, float rotationRad);

    float RotationAxis(float delta);
}