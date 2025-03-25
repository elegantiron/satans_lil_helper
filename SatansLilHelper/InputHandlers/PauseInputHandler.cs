using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SatansLilHelper.Content.Text;
using SatansLilHelper.Interfaces;
using SatansLilHelper.Types;
using SatansLilHelper.Utils;

namespace SatansLilHelper.InputHandlers;

internal class PauseInputHandler : IInputHandler
{
    private IInputHandler Parent;
    private Rectangle ShadeShape;
    private readonly Menu Menu;

    public PauseInputHandler(IInputHandler parent)
    {
        Parent = parent;
        ShadeShape = Rectangle.Empty;
        Menu = new(Color.Blue, Color.White, FontID.Menu);
        Menu.AddItem(GameStrings.Resume);
        Menu.AddItem(GameStrings.ViewBestiary);
        Menu.AddItem(GameStrings.ToMenu);
        Menu.AddItem(GameStrings.ToDesktop);
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
            case Keys.Escape:
            case Keys.I:
                return Parent;
            case Keys.Up:
            case Keys.Down:
                Menu.HandleKey(key);
                break;
            case Keys.Enter:
                return OnExit();
        }
        return this;
    }

    public IInputHandler OnExit()
    {
        IInputHandler value = Parent;
        if (Menu.Selection == GameStrings.ViewBestiary)
        {
            // Return a bestiary handler here
        }
        else if (Menu.Selection == GameStrings.ToMenu)
            value = new TitleInputHandler();
        else if (Menu.Selection == GameStrings.ToDesktop)
            EventBus.Send<EventMessage>(Events.QuitGame, new EventMessage());
        return value;
    }
}
