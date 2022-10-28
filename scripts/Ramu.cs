using Godot;
using System;

/*
p=23
g=5
Ramu's Secret No : 7
Ramu's Public No : 17
Damu's Secret No : 8
Damu's Public No : 16

Shared secret : 18
Required number : 1668
*/
public class Ramu : KinematicBody2D
{
    private RichTextLabel HelperTxt;
    private PackedScene DialogueScene;
    private DialogueHelper dialogueHelper;
    private Globals gb;
    private Timer timer;
    private bool interactArea { get; set; }
    public void OnDetectionBodyEntered(Node Body)
    {
        if (Body.Name.Equals("Damu") && (gb.CurSceneNo == 0 || gb.CurSceneNo == 2))
        {
            if (!IsAParentOf(HelperTxt))
            {
                AddChild(HelperTxt);
                HelperTxt.Text = "Press ENTER to interact";
                interactArea = true;
            }
        }
    }
    public void OnDetectionBodyExited(Node Body)
    {
        try
        {
            if (IsAParentOf(HelperTxt))
            {
                RemoveChild(HelperTxt);
                interactArea = false;
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
        PackedScene RadeeshScene = (PackedScene)GD.Load("res://scenes/chars/Radeesh.tscn");
        KinematicBody2D radeesh = (KinematicBody2D)RadeeshScene.Instance();
        GetParent().AddChild(radeesh);
        this.RemoveChild(timer);
    }

    public void OnDialogueConvoOver()
    {
        gb.SceneComplete = true;
        gb.ConvoStatus = false;
        if (gb.CurSceneNo == 0 || gb.CurSceneNo == 2)
        {
            gb.CurSceneNo++;
        }
        if (dialogueHelper.IsConnected("ConvoOver", this, "OnDialogueConvoOver"))
        {
            dialogueHelper.Disconnect("ConvoOver", this, "OnDialogueConvoOver");
        }
        if (IsAParentOf(dialogueHelper))
        {
            RemoveChild(dialogueHelper);
        }
        if (gb.CurSceneNo == 1)
        {
            timer.Start();
        }

    }
    public override void _Ready()
    {
        gb = GetNode<Globals>("/root/Globals");
        PackedScene HelperScene = (PackedScene)GD.Load("res://scenes/text/helper.tscn");
        DialogueScene = (PackedScene)GD.Load("res://scenes/text/dialogue.tscn");
        HelperTxt = (RichTextLabel)HelperScene.Instance();
        dialogueHelper = (DialogueHelper)DialogueScene.Instance();
        timer = new Timer();
        AddChild(timer);
        timer.WaitTime = 1;
        timer.Autostart = false;
        timer.Connect("timeout", this, "OnTimerTimeout");
        interactArea = false;
    }

    public override void _Process(float delta)
    {
        if (Input.IsActionJustPressed("ui_accept") && (gb.CurSceneNo == 0 || gb.CurSceneNo == 2) && interactArea)
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

                    if (gb.CurSceneNo == 2)
                    {
                        //Create New Instance : )
                        dialogueHelper = (DialogueHelper)DialogueScene.Instance();
                        AddChild(dialogueHelper);
                    }
                    else
                    {
                        AddChild(dialogueHelper);
                    }
                    if (!dialogueHelper.IsConnected("ConvoOver", this, "OnDialogueConvoOver"))
                    {
                        dialogueHelper.Connect("ConvoOver", this, "OnDialogueConvoOver");
                    }
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
