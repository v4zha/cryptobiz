using Godot;
using System.Collections.Generic;
using Newtonsoft.Json;

public class Dialogue
{
    public string character { get; set; }
    public string dialogue { get; set; }
}
public class DialogueHelper : RichTextLabel
{

    [Signal]
    public delegate void ConvoOver();
    public Globals gb { get; set; }
    public List<Dialogue> dialogues { get; set; }
    public int dialogueCounter { get; set; }
    public int maxDiags { get; set; }
    void setDialogues()
    {
        File file = new File();
        file.Open("res://story/story.json", File.ModeFlags.Read);
        string jsn = file.GetAsText();
        Dictionary<string, List<Dialogue>> allDiags = JsonConvert.DeserializeObject<Dictionary<string, List<Dialogue>>>(jsn);
        dialogues = allDiags[gb.SceneList[gb.CurSceneNo]];
        maxDiags = dialogues.Count;
    }
    void getDialogue()
    {
        if (dialogueCounter >= maxDiags)
        {
            dialogueCounter = 0;
            gb.SceneComplete = true;
            gb.ConvoStatus = false;
            gb.CurSceneNo++;
            EmitSignal("ConvoOver");
            GetParent().RemoveChild(this);
        }
        else
        {
            Dialogue d = dialogues[dialogueCounter];
            Text = $"[{d.character}] : {d.dialogue}";
            dialogueCounter++;
        }
    }
    public override void _Ready()
    {
        dialogueCounter = 0;
        gb = GetNode<Globals>("/root/Globals");
        setDialogues();
    }
    public override void _Process(float delta)
    {
        if (Input.IsActionJustPressed("ui_accept"))
        {
            getDialogue();
        }
    }

}
