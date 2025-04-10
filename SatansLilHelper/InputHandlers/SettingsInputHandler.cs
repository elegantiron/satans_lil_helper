using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SatansLilHelper.Constants;
using SatansLilHelper.Interfaces;
using SatansLilHelper.Properties;
using SatansLilHelper.Types;
using SatansLilHelper.Utils;

namespace SatansLilHelper.InputHandlers;

internal class SettingsInputHandler : IInputHandler
{
    private IInputHandler _parent;
    private Menu Menu;
    private VecPair TextVecs;

    public SettingsInputHandler(IInputHandler parent)
    {
        _parent = parent;
        Menu = new(Color.CornflowerBlue, Color.White, FontID.Menu);
        TextVecs = new();
    }

    public void Draw(
        SpriteBatch spriteBatch,
        Dictionary<TextureID, Texture2D> textureMap,
        Dictionary<EffectID, SoundEffect> effectMap,
        Dictionary<SongID, Song> songMap,
        Dictionary<FontID, SpriteFont> fontMap
    )
    {
        if (TextVecs.Location == Vector2.Zero)
        {
            TextVecs.Location.X = spriteBatch.GraphicsDevice.Viewport.Width / 2;
            TextVecs.Location.Y = 5;
            Vector2 width = fontMap[FontID.Title].MeasureString(GameStrings.SettingsTitle);
            TextVecs.Origin.X = width.X / 2;
        }
        spriteBatch.Begin();
        spriteBatch.DrawString(
            fontMap[FontID.Title],
            GameStrings.SettingsTitle,
            TextVecs.Location,
            Constants.Colors.AmericanRose,
            0f,
            TextVecs.Origin,
            1f,
            SpriteEffects.None,
            1f
        );
        Menu.Draw(spriteBatch, textureMap, effectMap, songMap, fontMap);
        spriteBatch.End();
    }

    public IInputHandler HandleKey(Keys key)
    {
        switch (key)
        {
            case Keys.Escape:
                return _parent;
            case Keys.Up:
            case Keys.Down:
                Menu.HandleKey(key);
                break;
            case Keys.Left:
            case Keys.Right:
                ChangeSetting();
                break;
        }
        return this;
    }

    private void ChangeSetting()
    {
        if (Menu.Selection.Args?.Length < 1)
            return;
        Type argType =
            Menu.Selection.Args?[0].GetType()
            ?? throw new Exception("I don't know how you managed to throw this");
        if (argType == typeof(bool))
            Menu.Selection.Args[0] = !(bool)Menu.Selection.Args[0];
    }
}
