using Godot;
using System;

public class PlayerInfo : CenterContainer
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
    }

    public override void _Process(float delta) {
        this.SetRotation(0);
    }
}
