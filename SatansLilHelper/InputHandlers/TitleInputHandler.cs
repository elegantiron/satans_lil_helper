using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SatansLilHelper.Interfaces;
using SatansLilHelper.Properties;
using SatansLilHelper.Types;
using SatansLilHelper.Utils;

namespace SatansLilHelper.InputHandlers;

internal class TitleInputHandler : IInputHandler
{
    private Vector2 titlePosition = Vector2.Zero;
    private Vector2 titleOrigin = Vector2.Zero;
    private TextVecs satanVecs = new();
    private List<TextureID> satanTextures =
    [
        TextureID.SatanMain,
        TextureID.SatanMouthClosed,
        TextureID.SatanEyesOpen,
    ];

    public void Draw(
        SpriteBatch spriteBatch,
        Dictionary<TextureID, Texture2D> textureMap,
        Dictionary<EffectID, SoundEffect> effectMap,
        Dictionary<SongID, Song> songMap,
        Dictionary<FontID, SpriteFont> fontMap
    )
    {
        if (titlePosition == Vector2.Zero)
        {
            titlePosition.X = spriteBatch.GraphicsDevice.Viewport.Width / 2;
            titlePosition.Y = 5;
            Vector2 width = fontMap[FontID.Title].MeasureString(GameStrings.GameTitle);
            titleOrigin.X = width.X / 2;
            satanVecs.Location.X = spriteBatch.GraphicsDevice.Viewport.Width / 2;
            satanVecs.Location.Y = spriteBatch.GraphicsDevice.Viewport.Height / 2;
            satanVecs.Origin.X = textureMap[TextureID.SatanMain].Width / 2;
            satanVecs.Origin.Y = textureMap[TextureID.SatanMain].Height / 2;
        }
        spriteBatch.DrawString(
            fontMap[FontID.Title],
            GameStrings.GameTitle,
            titlePosition,
            Constants.Colors.AmericanRose,
            0f,
            titleOrigin,
            1f,
            SpriteEffects.None,
            1f
        );
        foreach (TextureID index in satanTextures)
        {
            spriteBatch.Draw(
                textureMap[index],
                satanVecs.Location,
                null,
                Color.White,
                0f,
                satanVecs.Origin,
                1f,
                SpriteEffects.None,
                1f
            );
        }
    }

    public IInputHandler HandleKey(Keys key)
    {
        switch (key)
        {
            case Keys.Escape:
                EventBus.Send(Events.QuitGame, new EventMessage());
                break;
            case Keys.Enter:
                return new MainMenuInputHandler();
        }
        return this;
    }
}
