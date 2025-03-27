using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SatansLilHelper.Interfaces;
using SatansLilHelper.Types;
using SatansLilHelper.Utils;

namespace SatansLilHelper.InputHandlers;

internal class MainMenuInputHandler : IInputHandler
{
    private Menu Menu;

    public MainMenuInputHandler(Color selectedColor, Color unselectedColor, FontID font)
    {
        Menu = new(selectedColor, unselectedColor, font);
        Menu.AddItem(Properties.GameStrings.NewGame);
        Menu.AddItem(Properties.GameStrings.ViewBestiary);
        Menu.AddItem(Properties.GameStrings.ViewSettings);
        Menu.AddItem(Properties.GameStrings.ToDesktop);
    }

    public void Draw(
        SpriteBatch spriteBatch,
        Dictionary<TextureID, Texture2D> textureMap,
        Dictionary<EffectID, SoundEffect> effectMap,
        Dictionary<SongID, Song> songMap,
        Dictionary<FontID, SpriteFont> fontMap
    )
    {
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

    private IInputHandler OnExit()
    {
        IInputHandler handler = this;
        if (Menu.Selection == Properties.GameStrings.ViewBestiary) { }
        else if (Menu.Selection == Properties.GameStrings.ToDesktop)
            EventBus.Send<EventMessage>(Events.QuitGame, new EventMessage());
        else if (Menu.Selection == Properties.GameStrings.NewGame)
            handler = new GameInputHandler();
        else if (Menu.Selection == Properties.GameStrings.ViewSettings)
            handler = new SettingsInputHandler(this);
        return handler;
    }
}
