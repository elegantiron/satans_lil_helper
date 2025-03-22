using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SatansLilHelper.Utils;

namespace SatansLilHelper.InputHandlers;

internal class PauseInputHandler(IInputHandler parent) : IInputHandler
{
    private IInputHandler Parent = parent;
    private Rectangle ShadeShape = Rectangle.Empty;

    public void Draw(
        SpriteBatch spriteBatch,
        Dictionary<TextureID, Texture2D> textureMap,
        Dictionary<EffectID, SoundEffect> effectMap,
        Dictionary<SongID, Song> songMap,
        Dictionary<FontID, SpriteFont> fontMap
    )
    {
        if (ShadeShape == Rectangle.Empty)
        {
            ShadeShape.X = spriteBatch.GraphicsDevice.Viewport.Width / 8;
            ShadeShape.Y = spriteBatch.GraphicsDevice.Viewport.Height / 8;
            ShadeShape.Width = spriteBatch.GraphicsDevice.Viewport.Width * 6 / 8;
            ShadeShape.Height = spriteBatch.GraphicsDevice.Viewport.Height * 6 / 8;
        }
        Parent.Draw(spriteBatch, textureMap, effectMap, songMap, fontMap);
        spriteBatch.Draw(
            textureMap[TextureID.WhitePixel],
            ShadeShape,
            Constants.Colors.TranslucentBlack
        );
    }

    public IInputHandler HandleKey(Keys key)
    {
        switch (key)
        {
            case Keys.Escape:
                return Parent;
        }
        return this;
    }
}
