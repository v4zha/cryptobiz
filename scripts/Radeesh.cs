using Godot;
using System.Collections.Generic;

public class Radeesh : KinematicBody2D
{
    [Export]
    public int MaxSpeed { get; set; } = 100;
    [Export]
    public float Acc { get; set; } = 30;
    [Export]
    public float Friction { get; set; } = 100;

    private PlayerPos pos { get; set; } = PlayerPos.Up;

    private DialogueHelper dialogueHelper;
    public Vector2 transformVec { get; private set; } = new Vector2();
    // (Start,End)
    public Queue<(PlayerPos, Vector2)> CurTransform = new Queue<(PlayerPos, Vector2)>();

    private Globals gb;
    public override void _Ready()
    {
        CurTransform.Enqueue((PlayerPos.Down, new Vector2(0, 98)));
        CurTransform.Enqueue((PlayerPos.Left, new Vector2(-35, 92)));
        PackedScene DialogueScene = (PackedScene)GD.Load("res://scenes/text/dialogue.tscn");
        dialogueHelper = (DialogueHelper)DialogueScene.Instance();
    }
    public void OnDialogueConvoOver()
    {
        gb.SceneComplete = true;
        gb.ConvoStatus = false;
        if (gb.CurSceneNo == 1)
        {
            gb.CurSceneNo++;
        }
        GD.Print("Radeesh Convo Over");
    }
    bool CheckTransform()
    {
        PlayerPos pos = CurTransform.Peek().Item1;
        Vector2 qVec = CurTransform.Peek().Item2;
        bool res = false;
        switch (pos)
        {
            case PlayerPos.Down:
                res = Position.y <= qVec.y;
                break;
            case PlayerPos.Up:
                res = Position.y >= qVec.y;
                break;
            case PlayerPos.Left:
                res = Position.x >= qVec.x;
                break;
            case PlayerPos.Right:
                res = Position.x <= qVec.x;
                break;
        }
        return res;
    }
    Vector2 getInputVec(PlayerPos pos)
    {
        Vector2 res = Vector2.Zero;
        switch (pos)
        {
            case PlayerPos.Down:
                res = Vector2.Down;
                break;
            case PlayerPos.Up:
                res = Vector2.Up;
                break;
            case PlayerPos.Left:
                res = Vector2.Left;
                break;
            case PlayerPos.Right:
                res = Vector2.Right;
                break;
        }

        return res;
    }
    public override void _Process(float delta)
    {
        AnimatedSprite sprite = (AnimatedSprite)GetNode("AnimatedSprite");
        Vector2 input = Vector2.Zero;

        if (CurTransform.Count > 0)
        {
            input = getInputVec(CurTransform.Peek().Item1);
        }
        else
        {
            if (transformVec == Vector2.Zero)
            {
                if (!IsAParentOf(dialogueHelper))
                {
                    AddChild(dialogueHelper);
                    dialogueHelper.Connect("ConvoOver", this, "OnDialogueConvoOver");
                }
            }
        }
        if (CurTransform.Count > 0 && CheckTransform())
        {
            if (input.y < 0)
            {
                pos = PlayerPos.Up;
            }
            else if (input.y > 0)
            {
                pos = PlayerPos.Down;
            }
            else if (input.x > 0)
            {
                pos = PlayerPos.Right;
            }
            else
            {
                pos = PlayerPos.Left;

            }
            animatePlayer(sprite, AnimState.Run);
            transformVec = input * Acc * delta;
            transformVec = transformVec.LimitLength(MaxSpeed);
        }
        else
        {
            animatePlayer(sprite, AnimState.Idle);
            transformVec = transformVec.MoveToward(Vector2.Zero, Friction * delta);
            if (CurTransform.Count > 0)
            {
                CurTransform.Dequeue();
            }
        }
        MoveAndCollide(transformVec);
    }

    private void animatePlayer(AnimatedSprite sprite, AnimState anim)
    {
        string anim_key = anim == AnimState.Idle ? "idle" : "run";
        switch (this.pos)
        {
            case PlayerPos.Up:
                sprite.Play($"{anim_key}_up");
                break;
            case PlayerPos.Down:
                sprite.Play($"{anim_key}_down");
                break;
            case PlayerPos.Left:
                sprite.Play($"{anim_key}_left");
                break;
            case PlayerPos.Right:
                sprite.Play($"{anim_key}_right");
                break;
        }
    }
}