using Godot;
using System;

public class FPSCheck : Label
{
    float fps;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

    public override void _Process(float delta)
    {
        fps = Engine.GetFramesPerSecond();
        Text = "FPS: " + fps;
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}
