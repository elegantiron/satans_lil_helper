using System.Collections.Generic;
using System.Linq;
using Friflo.Engine.ECS;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SatansLilHelper.Components;
using SatansLilHelper.Constants;
using SatansLilHelper.Extensions;
using SatansLilHelper.Interfaces;
using SatansLilHelper.Properties;

namespace SatansLilHelper.InputHandlers;

internal class SpellBookInputHandler(GameInputHandler parent) : IInputHandler
{
    private GameInputHandler _parent = parent;
    private int _index = 0;

    public void Draw(
        SpriteBatch spriteBatch,
        Dictionary<TextureID, Texture2D> textureMap,
        Dictionary<EffectID, SoundEffect> effectMap,
        Dictionary<SongID, Song> songMap,
        Dictionary<FontID, SpriteFont> fontMap
    )
    {
        _parent.Draw(spriteBatch, textureMap, effectMap, songMap, fontMap);
        Viewport viewport = spriteBatch.GraphicsDevice.Viewport;
        Rectangle shadeShape = new(
            viewport.Width / 10,
            viewport.Height / 10,
            viewport.Width * 8 / 10,
            viewport.Height * 8 / 10
        );
        spriteBatch.Draw(textureMap[TextureID.WhitePixel], shadeShape, Colors.TranslucentBlack);
        Vector2 size = fontMap[FontID.Menu].MeasureString(GameStrings.SpellBookTitle);
        spriteBatch.DrawString(
            fontMap[FontID.Menu],
            GameStrings.SpellBookTitle,
            new Vector2(
                spriteBatch.GraphicsDevice.Viewport.Width / 2.0f,
                (float)shadeShape.Y + size.Y
            ),
            Colors.White,
            new Vector2(size.X / 2, size.Y / 2)
        );
        Vector2 textDest = new(shadeShape.X + 25, shadeShape.Y + 2.5f * size.Y);
        EntityLinks<Grimoire> entities = _parent.CurrentMap.Player.GetIncomingLinks<Grimoire>();
        if (_index < 0)
            _index = entities.Count - 1;
        else if (_index >= entities.Count)
            _index %= entities.Count;
        if (entities.Count > 0)
        {
            foreach (int idx in Enumerable.Range(0, entities.Count))
            {
                if (textDest.Y > shadeShape.Height)
                {
                    textDest.Y = shadeShape.Y + 2.5f * size.Y;
                    textDest.X += (shadeShape.Width - 50) / 3;
                    if (textDest.X > shadeShape.Width)
                        break;
                }

                EntityName entName = entities.Entities[idx].GetComponent<EntityName>();
                string displayName = "" + entName.value;

                spriteBatch.DrawString(fontMap[FontID.Status], displayName, textDest, Colors.White);
                if (idx == _index)
                {
                    spriteBatch.Draw(
                        textureMap[TextureID.ArrowBlue],
                        new Vector2(textDest.X - 30, textDest.Y + 3),
                        Colors.White
                    );
                }
                textDest.Y += fontMap[FontID.Status].LineSpacing * 1.15f;
            }
        }
    }

    public IInputHandler HandleKey(Keys key)
    {
        switch (key)
        {
            case Keys.S:
                return _parent;
            case Keys.Escape:
                return _parent;
            case Keys.Up:
                _index--;
                break;
            case Keys.Down:
                _index++;
                break;
        }
        return this;
    }
}
