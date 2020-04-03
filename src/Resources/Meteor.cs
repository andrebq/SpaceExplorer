using Godot;
using System;

public class Meteor : KinematicBody2D
{
    private float angularSpeedRad;
    private float angularSpeedDeg;
    [Export]
    public float AngularSpeed {
        get {
            return angularSpeedDeg;
        }
        set {
            angularSpeedDeg = value;
            angularSpeedRad = Mathf.Deg2Rad(angularSpeedDeg);
        }
    }
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    public Meteor() {
        this.angularSpeedDeg = 180;
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
    }

    public void Explode() {
        GD.Print("Exploding and leaving some parts behind");
        QueueFree();
    }

    public override void _Process(float delta) {
        this.Rotate(delta * angularSpeedRad);
    }
}
