#if DEBUG
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using SatansLilHelper.Constants;
using SatansLilHelper.Interfaces;
using SatansLilHelper.Properties;
using SatansLilHelper.Utils;

namespace SatansLilHelper.InputHandlers;

internal class DebugMenuInputHandler : IInputHandler
{
    private GameInputHandler _parent;
    private Rectangle _shadeShape;
    private Menu _menu;

    public DebugMenuInputHandler(GameInputHandler parent)
    {
        _parent = parent;
        _shadeShape = Rectangle.Empty;
        _menu = new(Color.CornflowerBlue, Color.White, FontID.Menu);
        _menu.AddItem(GameStrings.Resume);
        _menu.AddItem(GameStrings.DebugHeal);
        _menu.AddItem(GameStrings.DebugSpawnNear);
        _menu.AddItem(GameStrings.DebugNewSeed);
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
        spriteBatch.Draw(textureMap[TextureID.WhitePixel], _shadeShape, Colors.TranslucentBlack);
        _menu.Draw(spriteBatch, textureMap, effectMap, songMap, fontMap);
        spriteBatch.End();
    }

    public IInputHandler HandleKey(Keys key)
    {
        switch (key)
        {
            case Keys.Up:
            case Keys.Down:
                _menu.HandleKey(key);
                break;
            case Keys.Enter:
                return OnExit();
        }
        return this;
    }

    public GameInputHandler OnExit()
    {
        if (_menu.Selection.Key == GameStrings.DebugHeal)
            _parent.HealPlayer();
        else if (_menu.Selection.Key == GameStrings.DebugSpawnNear)
            _parent.SpawnNear();
        else if (_menu.Selection.Key == GameStrings.DebugNewSeed)
            _parent.ResetSeed();

        return _parent;
    }
}
#endif
