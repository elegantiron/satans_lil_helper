using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MLEM.Input;

namespace SatansLilHelper.GameComponents;

internal class SatanFace : DrawableGameComponent
{
    private List<Keys> _keyList;
    private SpriteBatch? _spriteBatch;
    private EventHandler<InputKeyEventArgs> _keyDown,
        _keyUp;
    private Texture2D? _satanMain,
        _satanEyesOpen,
        _satanEyesClosed,
        _satanMouthOpen,
        _satanMouthClosed;

    public SatanFace(Game game)
        : base(game)
    {
        _keyList = [];
        _keyDown = new(HandleKeyDown);
        _keyUp = new(HandleKeyUp);
    }

    protected override void OnEnabledChanged(object sender, EventArgs args) { }

    public override void Update(GameTime gameTime)
    {
        if (Game is Engine engine)
        {
            if (
                engine.Handler.TryConsumePressed(Keys.Enter)
                || engine.Handler.TryConsumePressed(MouseButton.Left)
            )
            {
                Enabled = false;
                Visible = false;
                engine.MainMenuScreen.Visible = true;
                engine.MainMenuScreen.Enabled = true;
            }
        }
    }

    public void HandleKeyDown(object? sender, InputKeyEventArgs eventArgs)
    {
        if (_keyList.Contains(eventArgs.Key))
            return;
        _keyList.Add(eventArgs.Key);
        switch (eventArgs.Key)
        {
            case Keys.Enter:
                Enabled = false;
                Visible = false;
                if (Game is Engine engine)
                {
                    engine.MainMenuScreen.Enabled = true;
                    engine.MainMenuScreen.Visible = true;
                }
                break;
            case Keys.Escape:
                Game.Exit();
                break;
        }
    }

    public void HandleKeyUp(object? sender, InputKeyEventArgs eventArgs)
    {
        _keyList.Remove(eventArgs.Key);
    }

    protected override void LoadContent()
    {
        base.LoadContent();
        _spriteBatch = new(Game.GraphicsDevice);
        _satanMain = Game.Content.Load<Texture2D>(FilePaths.SatanMain);
        _satanEyesOpen = Game.Content.Load<Texture2D>(FilePaths.SatanEyesOpen);
        _satanEyesClosed = Game.Content.Load<Texture2D>(FilePaths.SatanEyesClosed);
        _satanMouthClosed = Game.Content.Load<Texture2D>(FilePaths.SatanMouthClosed);
        _satanMouthOpen = Game.Content.Load<Texture2D>(FilePaths.SatanMouthOpen);
    }

    public override void Draw(GameTime gameTime)
    {
        base.Draw(gameTime);
        _spriteBatch?.Begin(samplerState: SamplerState.PointClamp);
        List<Texture2D> satan = [_satanMain, _satanMouthClosed, _satanEyesOpen];
        foreach (Texture2D texture in satan)
        {
            _spriteBatch?.Draw(
                texture,
                new Vector2(
                    Game.GraphicsDevice.Viewport.Width / 2,
                    Game.GraphicsDevice.Viewport.Height / 2
                ),
                null,
                Color.White,
                0f,
                new Vector2(64),
                1f,
                SpriteEffects.None,
                1f
            );
        }
        _spriteBatch?.End();
    }
}
