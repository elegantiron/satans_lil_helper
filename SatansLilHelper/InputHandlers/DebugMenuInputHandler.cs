#if DEBUG
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SatansLilHelper.Constants;
using SatansLilHelper.Interfaces;
using SatansLilHelper.Properties;
using SatansLilHelper.Utils;
using System.Collections.Generic;

namespace SatansLilHelper.InputHandlers;

internal class DebugMenuInputHandler : IInputHandler
{
    private GameInputHandler Parent;
    private Rectangle ShadeShape;
    private Menu Menu;

    public DebugMenuInputHandler(GameInputHandler parent)
    {
        Parent = parent;
        ShadeShape = Rectangle.Empty;
        Menu = new(Color.CornflowerBlue, Color.White, FontID.Menu);
        Menu.AddItem(GameStrings.Resume);
        Menu.AddItem(GameStrings.DebugHeal);
        Menu.AddItem(GameStrings.DebugSpawnNear);
        Menu.AddItem(GameStrings.DebugNewSeed);
    }

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
        Menu.Draw(spriteBatch, textureMap, effectMap, songMap, fontMap);
    }

    public IInputHandler HandleKey(Keys key)
    {
        switch (key)
        {
            case Keys.Up:
            case Keys.Down:
                Menu.HandleKey(key);
                break;
            case Keys.Enter:
                return OnExit();
        }
        return this;
    }

    public GameInputHandler OnExit()
    {
        if (Menu.Selection.Key == GameStrings.DebugHeal)
            Parent.HealPlayer();
        else if (Menu.Selection.Key == GameStrings.DebugSpawnNear)
            Parent.SpawnNear();
        else if (Menu.Selection.Key == GameStrings.DebugNewSeed)
            Parent.ResetSeed();

        return Parent;
    }
}
#endif
