using System.Collections.Generic;
using System.Diagnostics;
using Friflo.Engine.ECS;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SatansLilHelper.Components;
using SatansLilHelper.Constants;
using SatansLilHelper.Interfaces;
using SatansLilHelper.Properties;
using SatansLilHelper.Types;

namespace SatansLilHelper.InputHandlers;

internal class InventoryInputHandler(GameInputHandler parent) : IInputHandler
{
    private GameInputHandler _parent = parent;
    private VecPair _titleVecs = new(),
        _listVecs = new();
    private Rectangle _shadeShape = Rectangle.Empty;

    public void Draw(
        SpriteBatch spriteBatch,
        Dictionary<TextureID, Texture2D> textureMap,
        Dictionary<EffectID, SoundEffect> effectMap,
        Dictionary<SongID, Song> songMap,
        Dictionary<FontID, SpriteFont> fontMap
    )
    {
        if (_titleVecs.Location == Vector2.Zero)
            CalculateVectors(spriteBatch, fontMap);
        _parent.Draw(spriteBatch, textureMap, effectMap, songMap, fontMap);
        spriteBatch.Draw(textureMap[TextureID.WhitePixel], _shadeShape, Colors.TranslucentBlack);
        spriteBatch.DrawString(
            fontMap[FontID.Menu],
            GameStrings.Inventory_Title,
            _titleVecs.Location,
            Colors.White,
            0f,
            _titleVecs.Origin,
            1f,
            SpriteEffects.None,
            1f
        );

        _listVecs.Location.Y = _titleVecs.Location.Y + fontMap[FontID.Menu].LineSpacing * 1.5f;
        //_listVecs.Location.Y = _shadeShape.Y;
        _listVecs.Location.X = _shadeShape.X + 25;
        foreach (Entity ent in _parent.CurrentMap.Player.GetIncomingLinks<Holder>().Entities)
        {
            if (_listVecs.Location.Y > _shadeShape.Height)
            {
                _listVecs.Location.Y =
                    _titleVecs.Location.Y + fontMap[FontID.Menu].LineSpacing * 1.5f;
                _listVecs.Location.X += (_shadeShape.Width - 50) / 3;
                if (_listVecs.Location.X > _shadeShape.Width)
                    break;
            }
            EntityName entName = ent.GetComponent<EntityName>();
            string displayName = "" + entName.value;
            if (ent.TryGetComponent(out Equipper _))
                displayName += " (e)";
            spriteBatch.DrawString(
                fontMap[FontID.Status],
                displayName,
                _listVecs.Location,
                Colors.White
            );
            _listVecs.Location.Y += fontMap[FontID.Status].LineSpacing * 1.15f;
        }
    }

    public IInputHandler HandleKey(Keys key)
    {
        return key switch
        {
            Keys.Escape => _parent,
            Keys.I => _parent,
            _ => this,
        };
    }

    private void CalculateVectors(SpriteBatch spriteBatch, Dictionary<FontID, SpriteFont> fontMap)
    {
        _titleVecs.Location.X = spriteBatch.GraphicsDevice.Viewport.Width / 2;
        Vector2 textSize = fontMap[FontID.Menu].MeasureString(GameStrings.Inventory_Title);
        _titleVecs.Origin.X = textSize.X / 2;
        _shadeShape.X = spriteBatch.GraphicsDevice.Viewport.Width / 8;
        _shadeShape.Y = spriteBatch.GraphicsDevice.Viewport.Height / 8;
        _shadeShape.Width = spriteBatch.GraphicsDevice.Viewport.Width * 6 / 8;
        _shadeShape.Height = spriteBatch.GraphicsDevice.Viewport.Height * 6 / 8;
        _titleVecs.Location.Y = _shadeShape.Y + 10;
        _listVecs.Location.Y = _titleVecs.Location.Y + textSize.Y * 1.5f;
        _listVecs.Location.X = _shadeShape.X + 25;
    }
}
