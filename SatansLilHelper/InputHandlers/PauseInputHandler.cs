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
    private IInputHandler Parent;
    private Rectangle ShadeShape;
    private readonly Menu Menu;

    public PauseInputHandler(IInputHandler parent)
    {
        Parent = parent;
        ShadeShape = Rectangle.Empty;
        Menu = new(Color.CornflowerBlue, Color.White, FontID.Menu);
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
            case Keys.NumPad7:
            case Keys.NumPad2:
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
        IInputHandler value = Parent;
        if (Menu.Selection.Key == GameStrings.ViewBestiary)
        {
            // Return a bestiary handler here
        }
        else if (Menu.Selection.Key == GameStrings.ToMenu)
            value = new TitleInputHandler();
        else if (Menu.Selection.Key == GameStrings.ToDesktop)
            EventBus.Send(Events.QuitGame, new EventMessage());
        return value;
    }

    public void DumpData(string path)
    {
        if (Parent is ISaveable saveable)
            saveable.DumpData(path);
    }
}
