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

internal class PauseInputHandler : IInputHandler, ISaveable
{
    private IInputHandler _parent;
    private Rectangle _shadeShape;
    private readonly Menu _menu;

    public PauseInputHandler(IInputHandler parent)
    {
        _parent = parent;
        _shadeShape = Rectangle.Empty;
        _menu = new(Color.CornflowerBlue, Color.White, FontID.Menu);
        _menu.AddItem(GameStrings.Resume);
        _menu.AddItem(GameStrings.ViewBestiary);
        _menu.AddItem(GameStrings.ToMenu);
        _menu.AddItem(GameStrings.ToDesktop);
    }

    public void Draw(
        SpriteBatch spriteBatch,
        Dictionary<TextureID, Texture2D> textureMap,
        Dictionary<EffectID, SoundEffect> effectMap,
        Dictionary<SongID, Song> songMap,
        Dictionary<FontID, SpriteFont> fontMap
    )
    {
        if (_shadeShape == Rectangle.Empty)
        {
            _shadeShape.X = spriteBatch.GraphicsDevice.Viewport.Width / 8;
            _shadeShape.Y = spriteBatch.GraphicsDevice.Viewport.Height / 8;
            _shadeShape.Width = spriteBatch.GraphicsDevice.Viewport.Width * 6 / 8;
            _shadeShape.Height = spriteBatch.GraphicsDevice.Viewport.Height * 6 / 8;
        }
        _parent.Draw(spriteBatch, textureMap, effectMap, songMap, fontMap);

        spriteBatch.Begin();
        spriteBatch.Draw(
            textureMap[TextureID.WhitePixel],
            _shadeShape,
            Constants.Colors.TranslucentBlack
        );
        _menu.Draw(spriteBatch, textureMap, effectMap, songMap, fontMap);
        spriteBatch.End();
    }

    public IInputHandler HandleKey(Keys key)
    {
        switch (key)
        {
            case Keys.Escape:
            case Keys.I:
                return _parent;
            case Keys.NumPad7:
            case Keys.NumPad2:
            case Keys.Up:
            case Keys.Down:
                _menu.HandleKey(key);
                break;
            case Keys.Enter:
                return OnExit();
        }
        return this;
    }

    private IInputHandler OnExit()
    {
        IInputHandler value = _parent;
        if (_menu.Selection.Key == GameStrings.ViewBestiary)
        {
            // Return a bestiary handler here
        }
        else if (_menu.Selection.Key == GameStrings.ToMenu)
            value = new TitleInputHandler();
        else if (_menu.Selection.Key == GameStrings.ToDesktop)
            EventBus.Send(Events.QuitGame, new EventMessage());
        return value;
    }

    public void DumpData(string path)
    {
        if (_parent is ISaveable saveable)
            saveable.DumpData(path);
    }
}
