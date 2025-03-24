using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SatansLilHelper.Content.Text;
using SatansLilHelper.Utils;

namespace SatansLilHelper.InputHandlers;

public class TitleInputHandler : IInputHandler
{
    private Vector2 titlePosition = Vector2.Zero;
    private Vector2 titleOrigin = Vector2.Zero;

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
    }

    public IInputHandler HandleKey(Keys key)
    {
        switch (key)
        {
            case Keys.Escape:
                EventBus.Send(Events.QuitGame);
                break;
            case Keys.Enter:
                return new GameInputHandler();
        }
        return this;
    }
}
