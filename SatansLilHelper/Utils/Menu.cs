using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace SatansLilHelper.Utils;

public class Menu : Utils.IDrawable
{
    private Color SelectedColor,
        UnselectedColor;
    private List<string> Items;
    private TextVecs TextVecs;
    private Vector2 Size;
    private FontID Font;

    public Menu(Color selected, Color unselected, FontID font)
    {
        SelectedColor = selected;
        UnselectedColor = unselected;
        Items = [];
        TextVecs = new();
        Size = Vector2.Zero;
        Font = font;
    }

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
                UnselectedColor,
                0f,
                TextVecs.Origin,
                1f,
                SpriteEffects.None,
                1f
            );
            TextVecs.Location.Y += Size.Y;
        }
    }
}
