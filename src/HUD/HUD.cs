using Godot;
using System;

public class HUD : CanvasLayer
{
    private Label speedLabel;
    private Label messageLabel;
    private AnimationPlayer messageAnimation;

    public void SetSpeed(float value) {
        speedLabel.Text = Mathf.FloorToInt(Mathf.Abs(value)).ToString("F");
    }
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        speedLabel = FindNode("Speed", true, true) as Label;
        messageLabel = FindNode("Message") as Label;
        messageAnimation = FindNode("MessageAnimation") as AnimationPlayer;

        messageAnimation.Play("Initial");
    }

    public void Say(String msg) {
        messageLabel.Text = msg;
        messageAnimation.Play("ShowMessage");
    }
}
