using Godot;
using System;

public class portal : StaticBody2D
{
    private RichTextLabel HelperTxt;
    private Globals gb;
    public bool interactArea;
    public override void _Ready()
    {
        gb = GetNode<Globals>("/root/Globals");
        PackedScene HelperScene = (PackedScene)GD.Load("res://scenes/text/helper.tscn");
        HelperTxt = (RichTextLabel)HelperScene.Instance();
        interactArea = false;
    }
    public void OnPortalEntered(Node Body)
    {
        if (Body.Name.Equals("Damu") && gb.CurSceneNo == 3)
        {
            GD.Print("Add HelperText");
            if (!IsAParentOf(HelperTxt))
            {
                HelperTxt.SetPosition(new Vector2(15, 15));
                AddChild(HelperTxt);
                // HelperTxt.Text = "Press ENTER to Enter Secret Number";
                HelperTxt.Text = "<>";
                interactArea = true;
            }
        }
    }
    public void OnPortalExited(Node Body)
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

}
