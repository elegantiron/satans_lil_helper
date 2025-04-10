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

internal class MainMenuInputHandler : IInputHandler
{
    private Menu Menu;
    private Vector2 titlePosition,
        titleOrigin;

    public MainMenuInputHandler()
    {
        Menu = new(Color.CornflowerBlue, Color.White, FontID.Menu);
        Menu.AddItem(GameStrings.NewGame);
        Menu.AddItem(GameStrings.ViewBestiary);
        //Menu.AddItem(GameStrings.ViewSettings);
        Menu.AddItem(GameStrings.ToDesktop);
        titlePosition = titleOrigin = Vector2.Zero;
    }

    public void Draw(
        SpriteBatch spriteBatch,
        Dictionary<TextureID, Texture2D> textureMap,
        Dictionary<EffectID, SoundEffect> effectMap,
        Dictionary<SongID, Song> songMap,
        Dictionary<FontID, SpriteFont> fontMap
    )
    {
        spriteBatch.Begin();
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
        Menu.Draw(spriteBatch, textureMap, effectMap, songMap, fontMap);
        spriteBatch.End();
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

    private IInputHandler OnExit()
    {
        IInputHandler handler = this;
        if (Menu.Selection.Key == GameStrings.ViewBestiary) { }
        else if (Menu.Selection.Key == GameStrings.ToDesktop)
            EventBus.Send<EventMessage>(Events.QuitGame, new EventMessage());
        else if (Menu.Selection.Key == GameStrings.NewGame)
            handler = new GameInputHandler();
        else if (Menu.Selection.Key == GameStrings.ViewSettings)
            handler = new SettingsInputHandler(this);
        return handler;
    }
}
