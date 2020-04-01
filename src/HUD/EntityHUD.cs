using Godot;
using System;

[Tool]
public class EntityHUD : Node2D
{
	private Vector2 size;
	[Export]
	public Vector2 Size {
		get {
			return size;
		}
		set {
			size = value;
			Update();
		}
	}

	public EntityHUD() {
		Size = new Vector2(64, 64);
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	public override void _Draw()
	{
		if (size.x == 0 || size.y == 0) {
			return;
		}
		var center = Vector2.Zero - Size/2;
		Rect2 rect = new Rect2(center, Size);
		this.DrawRect(rect, Colors.White, false, 2, true);
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
