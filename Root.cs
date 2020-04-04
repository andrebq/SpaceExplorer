using Godot;
using System;

public class Root : Node2D
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	private Node2D AsteroidField;
	private PlayerShip playerShip;
	private Camera2D camera;
	private HUD hud;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		camera = FindNode("Camera2D", true, true) as Camera2D;
		playerShip = FindNode("PlayerShip", true, true) as PlayerShip;
		AsteroidField = FindNode(nameof(AsteroidField)) as Node2D;
		hud = FindNode("HUD") as HUD;
		hud.Say("Go!");
	}

	public override void _Process(float delta) {
		camera.Offset = playerShip.Position;
		hud.SetSpeed(playerShip.Speed);
	}

	private void _on_PlayerShip_Warning(String msg) {
		hud.Say(msg);
	}

	private void _Meteor_Created(Meteor meteor) {
		AsteroidField.AddChild(meteor);
	}
}
