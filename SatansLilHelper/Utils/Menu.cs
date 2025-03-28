using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SatansLilHelper.Constants;
using SatansLilHelper.Types;

namespace SatansLilHelper.Utils;

#nullable enable

internal struct MenuItem
{
    public string Key;
    public object[]? Args;
    public bool Enabled = true;

    public MenuItem(string key, bool enabled, params object[]? args)
    {
        Key = key;
        Enabled = enabled;
        Args = args;
    }

    public MenuItem(string key, params object[]? args)
    {
        Key = key;
        Args = args;
    }
}

internal class Menu(Color selected, Color unselected, FontID font) : Interfaces.IDrawable
{
    private Color _selectedColor = selected,
        _unselectedColor = unselected,
        _disabledColor = Color.Gray;
    private List<MenuItem> _items = [];
    private TextVecs _textVecs = new();
    private Vector2 _size = Vector2.Zero;
    private FontID _font = font;
    private int _index = 0;

    public void AddItem(string key, params object[]? args)
    {
        _items.Add(new MenuItem(key, args));
    }

    public void AddItem(string key, bool enabled, params object[]? args)
    {
        _items.Add(new MenuItem(key, enabled, args));
    }

    public void Draw(
        SpriteBatch spriteBatch,
        Dictionary<TextureID, Texture2D> textureMap,
        Dictionary<EffectID, SoundEffect> effectMap,
        Dictionary<SongID, Song> songMap,
        Dictionary<FontID, SpriteFont> fontMap
    )
    {
        if (_items.Count < 1)
            return;
        if (_size == Vector2.Zero)
        {
            _textVecs.Location.X = spriteBatch.GraphicsDevice.Viewport.Width / 2;
        }
        _size = fontMap[_font].MeasureString("String"); // only care about the height here
        _textVecs.Location.Y =
            spriteBatch.GraphicsDevice.Viewport.Height / 2 - (_size.Y * _items.Count / 2);
        foreach (MenuItem item in _items)
        {
            string formatted = string.Format(item.Key, item.Args ?? []);
            _size = fontMap[_font].MeasureString(formatted);
            _textVecs.Origin.X = _size.X / 2;
            spriteBatch.DrawString(
                fontMap[_font],
                formatted,
                _textVecs.Location,
                GetColor(item),
                0f,
                _textVecs.Origin,
                1f,
                SpriteEffects.None,
                1f
            );
            _textVecs.Location.Y += _size.Y;
        }
    }

    public void HandleKey(Keys key)
    {
        if (_items.Count < 1)
            return;
        switch (key)
        {
            case Keys.Up:
                if (--_index < 0)
                    _index = _items.Count - 1;
                break;
            case Keys.Down:
                _index = ++_index % _items.Count;
                break;
        }
    }

    public void SetEnabled(int index, bool enabled)
    {
        if (!(_items.Count > index))
            return;
        MenuItem item = _items[index];
        item.Enabled = enabled;
        _items[index] = item;
    }

    public MenuItem Selection
    {
        get { return _items[_index]; }
    }

    public int Index
    {
        get { return _index; }
    }

    private Color GetColor(MenuItem item)
    {
        if (!item.Enabled)
            return Color.Gray;
        if (_items.IndexOf(item) == _index)
            return _selectedColor;
        return _unselectedColor;
    }
}
