using Godot;
using System;

public class Ramu : KinematicBody2D
{
    private RichTextLabel HelperTxt;
    private RichTextLabel DialogueHelper;
    private bool InteractArea = false;
    private Globals gb;
    private Timer timer;
    public void OnDetectionBodyEntered(Node Body)
    {
        if (Body.Name.Equals("Damu") && gb.CurSceneNo == 0)
        {
            AddChild(HelperTxt);
            InteractArea = true;
        }
    }
    public void OnDetectionBodyExited(Node Body)
    {
        InteractArea = false;
        try
        {
            if (IsAParentOf(HelperTxt))
            {
                RemoveChild(HelperTxt);
            }
        }
        catch (Exception err)
        {
            //   For Debuggin Perpose : )
            GD.Print(err);
        }
    }

    public void OnTimerTimeout()
    {
        GD.Print("Stoping Timer, adding Radeesh");
        PackedScene RadeeshScene = (PackedScene)GD.Load("res://scenes/chars/Radeesh.tscn");
        KinematicBody2D radeesh = (KinematicBody2D)RadeeshScene.Instance();
        GetParent().AddChild(radeesh);
        this.RemoveChild(timer);
    }

    public void OnDialogueConvoOver()
    {
        GD.Print("Convo Over,Adding Radeesh");
        timer.Start();

    }
    public override void _Ready()
    {
        gb = GetNode<Globals>("/root/Globals");
        PackedScene HelperScene = (PackedScene)GD.Load("res://scenes/text/helper.tscn");
        PackedScene DialogueScene = (PackedScene)GD.Load("res://scenes/text/dialogue.tscn");
        HelperTxt = (RichTextLabel)HelperScene.Instance();
        DialogueHelper = (RichTextLabel)DialogueScene.Instance();
        HelperTxt.AddText("Press ENTER to interact");
        timer = new Timer();
        AddChild(timer);
        timer.WaitTime = 1;
        timer.Autostart = false;
        timer.Connect("timeout", this, "OnTimerTimeout");
    }

    public override void _Process(float delta)
    {
        if (Input.IsActionJustPressed("ui_accept") && InteractArea && gb.CurSceneNo == 0)
        {
            try
            {
                gb.ConvoStatus = true;
                if (IsAParentOf(HelperTxt))
                {
                    RemoveChild(HelperTxt);
                }
                if (!IsAParentOf(DialogueHelper))
                {
                    AddChild(DialogueHelper);
                }
            }
            catch (Exception err)
            {
                //   For Debuggin Perpose : )
                GD.Print(err);
            }
        }

    }

}
