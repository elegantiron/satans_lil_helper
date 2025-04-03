using System.Collections.Generic;
using System.IO;
using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SatansLilHelper.Constants;
using SatansLilHelper.Interfaces;

namespace SatansLilHelper.InputHandlers;

internal class ConfirmInputHandler : IInputHandler
{
    private string _message;
    private IInputHandler _parent;
    private Rectangle _shadeShape = Rectangle.Empty;
    private Vector2 _messageLocation = Vector2.Zero;
    private FontSystem _fontSystem;

    private ConfirmInputHandler() { }

    public ConfirmInputHandler(IInputHandler parent, string message)
    {
        _parent = parent;
        _message = message;
        _fontSystem = new();
        _fontSystem.AddFont(File.ReadAllBytes(@"Content/Fonts/CrayonLibre.ttf"));
    }

    public void Draw(
        SpriteBatch spriteBatch,
        Dictionary<TextureID, Texture2D> textureMap,
        Dictionary<EffectID, SoundEffect> effectMap,
        Dictionary<SongID, Song> songMap,
        Dictionary<FontID, SpriteFont> fontMap
    )
    {
        if (_messageLocation == Vector2.Zero)
            SetVecs(spriteBatch);

        _parent.Draw(spriteBatch, textureMap, effectMap, songMap, fontMap);
    }

    public IInputHandler HandleKey(Keys key)
    {
        return this;
    }

    private void SetVecs(SpriteBatch spriteBatch) { }
}
