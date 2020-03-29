using Godot;
using System;

public class Root : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    private PlayerShip playerShip;
    private Camera2D camera;
    private Label speedValue;
    private Node2D hud;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        camera = FindNode("Camera2D", false, true) as Camera2D;
        playerShip = FindNode("PlayerShip", false, true) as PlayerShip;
    }

    public override void _Process(float delta) {
        
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
