using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SatansLilHelper.Constants;
using SatansLilHelper.Types;
using SatansLilHelper.Utils;

namespace SatansLilHelper.GameComponents;

internal class TitleScreen : DrawableGameComponent
{
    private List<Keys> _keyList;
    public EventHandler<InputKeyEventArgs> _keyDown,
        _keyUp;

    public TitleScreen(Game game)
        : base(game)
    {
        _keyList = [];
        _keyDown = new(HandleKeyDown);
        _keyUp = new(HandleKeyUp);
    }

    protected override void LoadContent()
    {
        base.LoadContent();
    }

    public override void Initialize()
    {
        base.Initialize();
        Game.Window.KeyDown += _keyDown;
        Game.Window.KeyUp += _keyUp;
    }

    public override void Draw(GameTime gameTime)
    {
        base.Draw(gameTime);
    }

    public override void Update(GameTime gameTime)
    {
        Debug.WriteLine("title update");
        base.Update(gameTime);
    }

    protected override void OnEnabledChanged(object sender, EventArgs args)
    {
        base.OnEnabledChanged(sender, args);
        if (Enabled)
        {
            Game.Window.KeyDown += _keyDown;
            Game.Window.KeyUp += _keyUp;
        }
        else
        {
            Game.Window.KeyDown -= _keyDown;
            Game.Window.KeyUp -= _keyUp;
        }
    }

    public void HandleKeyDown(object? sender, InputKeyEventArgs eventArgs)
    {
        if (_keyList.Contains(eventArgs.Key))
            return;
        _keyList.Add(eventArgs.Key);
        switch (eventArgs.Key)
        {
            case Keys.Escape:
                EventBus.Send(Events.QuitGame, new EventMessage());
                break;
        }
    }

    public void HandleKeyUp(object? sender, InputKeyEventArgs eventArgs)
    {
        _keyList.Remove(eventArgs.Key);
    }
}
