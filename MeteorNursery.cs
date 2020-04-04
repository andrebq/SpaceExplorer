using Godot;
using System;

public class MeteorNursery : Position2D
{
    [Signal]
    public delegate void MeteorCreated(Meteor m);

    [Export]
    public float CooldownPeriod { get; set; }
    [Export]
    public int MaxBurst { get; set; }
    private float cooldownCounter;
    private PackedScene meteorScene;
    private RandomNumberGenerator rng;
    public override void _Ready()
    {
        CooldownPeriod = 10;
        cooldownCounter = CooldownPeriod/2;
        meteorScene = ResourceLoader.Load("res://src/Resources/Meteor.tscn") as PackedScene;
        rng = new RandomNumberGenerator();
        rng.Randomize();
    }

    public override void _Process(float delta) {
        cooldownCounter -= delta;
        if (cooldownCounter <= 0) {
            CreateMeteors();
            cooldownCounter = CooldownPeriod;
        }
    }

    private void CreateMeteors() {
        var m = meteorScene.Instance() as Meteor;
        var pos = this.GlobalPosition;
        var parentPos = (GetParent() as Node2D).GlobalPosition;
        GD.Print("pos/parentPos", pos, "/", parentPos);

        m.LinearVelocity = (parentPos - pos).Normalized() * 1000;
        GD.Print("pos: ", m.LinearVelocity);
        pos += Vector2.Right * rng.RandfRange(-30, 30);
        m.Lifetime *= 0.75f;
        m.GlobalPosition = pos;

        EmitSignal(nameof(MeteorCreated), m);
    }
}
