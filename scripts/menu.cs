using Godot;
using System;

public class menu : Control
{
    public override void _Ready()
    {

    }
    public override void _Input(InputEvent inputEvent)
    {
        if (inputEvent.IsActionPressed("ui_accept"))
        {
            GetTree().ChangeScene("res://scenes/intro.tscn");
        }
    }
}
