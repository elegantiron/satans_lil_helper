using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SatansLilHelper.Types;

namespace SatansLilHelper.Utils;

internal class Menu(Color selected, Color unselected, FontID font) : Interfaces.IDrawable
{
    private Color SelectedColor = selected,
        UnselectedColor = unselected;
    private List<string> Items = [];
    private TextVecs TextVecs = new();
    private Vector2 Size = Vector2.Zero;
    private FontID Font = font;
    private int Index = 0;

    public void AddItem(string key)
    {
        Items.Add(key);
    }

    public void Draw(
        SpriteBatch spriteBatch,
        Dictionary<TextureID, Texture2D> textureMap,
        Dictionary<EffectID, SoundEffect> effectMap,
        Dictionary<SongID, Song> songMap,
        Dictionary<FontID, SpriteFont> fontMap
    )
    {
        if (Items.Count < 1)
            return;
        if (Size == Vector2.Zero)
        {
            TextVecs.Location.X = spriteBatch.GraphicsDevice.Viewport.Width / 2;
        }
        Size = fontMap[Font].MeasureString("String"); // only care about the height here
        TextVecs.Location.Y =
            spriteBatch.GraphicsDevice.Viewport.Height / 2 - (Size.Y * Items.Count / 2);
        foreach (string item in Items)
        {
            Size = fontMap[Font].MeasureString(item);
            TextVecs.Origin.X = Size.X / 2;
            spriteBatch.DrawString(
                fontMap[Font],
                item,
                TextVecs.Location,
                Items.IndexOf(item) == Index ? SelectedColor : UnselectedColor,
                0f,
                TextVecs.Origin,
                1f,
                SpriteEffects.None,
                1f
            );
            TextVecs.Location.Y += Size.Y;
        }
    }

    public void HandleKey(Keys key)
    {
        if (Items.Count < 1)
            return;
        switch (key)
        {
            case Keys.Up:
                if (--Index < 0)
                    Index = Items.Count - 1;
                break;
            case Keys.Down:
                Index = ++Index % Items.Count;
                break;
        }
    }

    public string Selection => Items[Index];
}
