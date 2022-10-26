using Godot;

public enum PlayerPos
{
    Left,
    Right,
    Up,
    Down,
}
public enum AnimState
{
    Idle,
    Run,
}
public class Damu : KinematicBody2D
{
    [Export]
    public int MaxSpeed { get; set; } = 100;
    [Export]
    public float Acc { get; set; } = 30;
    [Export]
    public float Friction { get; set; } = 100;

    private PlayerPos pos { get; set; } = PlayerPos.Up;

    public Vector2 transformVec { get; private set; } = new Vector2();

    public override void _Process(float delta)
    {
        Vector2 input = new Vector2();
        input.x = Input.GetActionStrength("ui_right") - Input.GetActionStrength("ui_left");
        input.y = Input.GetActionStrength("ui_down") - Input.GetActionStrength("ui_up");
        AnimatedSprite sprite = (AnimatedSprite)GetNode("AnimatedSprite");
        input = input.Normalized();
        int sprint_fac = Input.IsActionPressed("sprint") ? 30 : 0;
        if (input != Vector2.Zero)
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
            transformVec = input * (Acc + sprint_fac) * delta;
            transformVec = transformVec.LimitLength(MaxSpeed);
        }
        else
        {
            animatePlayer(sprite, AnimState.Idle);
            transformVec = transformVec.MoveToward(Vector2.Zero, Friction * delta);
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
