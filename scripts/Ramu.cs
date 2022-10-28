using Godot;
using System;

public class Ramu : KinematicBody2D
{
    private RichTextLabel HelperTxt;
    private DialogueHelper dialogueHelper;
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
        gb.SceneComplete = true;
        gb.ConvoStatus = false;
        if (gb.CurSceneNo == 0)
        {
            gb.CurSceneNo++;
        }
        GD.Print("Convo Over,Adding Radeesh");
        dialogueHelper.Disconnect("ConvoOver", this, "OnDialogueConvoOver");
        RemoveChild(dialogueHelper);
        timer.Start();

    }
    public override void _Ready()
    {
        gb = GetNode<Globals>("/root/Globals");
        PackedScene HelperScene = (PackedScene)GD.Load("res://scenes/text/helper.tscn");
        PackedScene DialogueScene = (PackedScene)GD.Load("res://scenes/text/dialogue.tscn");
        HelperTxt = (RichTextLabel)HelperScene.Instance();
        dialogueHelper = (DialogueHelper)DialogueScene.Instance();
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
                if (!IsAParentOf(dialogueHelper))
                {
                    AddChild(dialogueHelper);
                    dialogueHelper.Connect("ConvoOver", this, "OnDialogueConvoOver");
                    InteractArea = false;
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
