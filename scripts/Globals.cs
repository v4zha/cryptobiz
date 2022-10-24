using Godot;
public class Globals : Node
{

    [Export]
    public int CurSceneNo { get; set; } = 0;
    [Export]
    public bool SceneComplete { get; set; } = false;
    [Export]
    public string[] SceneList { get; set; } = { "ramu_meet", "radeesh_arrival" };
    [Export]
    public bool ConvoStatus { get; set; } = false;
}
