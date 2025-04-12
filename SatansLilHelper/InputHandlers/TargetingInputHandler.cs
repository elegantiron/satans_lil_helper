using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Apos.Camera;
using Friflo.Engine.ECS;
using Friflo.Engine.ECS.Index;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SatansLilHelper.Components;
using SatansLilHelper.Constants;
using SatansLilHelper.Interfaces;
using SatansLilHelper.Utils;

namespace SatansLilHelper.InputHandlers;

internal class TargetingInputHandler : IInputHandler
{
    private GameInputHandler _parent;
    private int _range;
    private int _radius;
    private int _rangeSquared;
    private Entity _entity;
    private Point _center,
        _offset;

    private Color targetCenter = new(0x6F, 0x6F, 0x6F, 0x50);
    private Color targetRange = new(0x80, 0xFF, 0x00, 0x50);
    private Color targetRadius = new(0x00, 0x80, 0xFF, 0x50);

    public TargetingInputHandler(GameInputHandler parent, Entity targetable)
    {
        _entity = targetable;
        _parent = parent;
        Location center = _parent.CurrentMap.Player.GetComponent<Location>();
        _center = new(center.X, center.Y);
        _offset = Point.Zero;
        _rangeSquared = _range * _range;
    }

    public void Draw(
        SpriteBatch spriteBatch,
        Camera camera,
        Dictionary<TextureID, Texture2D> textureMap,
        Dictionary<EffectID, SoundEffect> effectMap,
        Dictionary<SongID, Song> songMap,
        Dictionary<FontID, SpriteFont> fontMap
    )
    {
        _parent.Draw(spriteBatch, camera, textureMap, effectMap, songMap, fontMap);
        camera.SetViewport();
        spriteBatch.Begin(transformMatrix: camera.View);
        List<Point> points = ShadowCast.GetArea(_parent.CurrentMap, _center + _offset, _radius);
        foreach (Point cell in points.ToHashSet())
        {
            if (!points.Exists((Point point) => point.X == cell.X - 1 && point.Y == cell.Y))
                spriteBatch.Draw(
                    textureMap[TextureID.WhitePixel],
                    new Rectangle(cell.X * 32 - 1, cell.Y * 32 - 1, 3, 34),
                    targetRadius
                );
            if (!points.Exists((Point point) => point.X == cell.X + 1 && point.Y == cell.Y))
                spriteBatch.Draw(
                    textureMap[TextureID.WhitePixel],
                    new Rectangle((cell.X + 1) * 32 - 1, cell.Y * 32 - 1, 3, 34),
                    targetRadius
                );
            if (!points.Exists((Point point) => point.X == cell.X && point.Y == cell.Y - 1))
                spriteBatch.Draw(
                    textureMap[TextureID.WhitePixel],
                    new Rectangle(cell.X * 32 - 1, cell.Y * 32 - 1, 34, 3),
                    targetRadius
                );
            if (!points.Exists((Point point) => point.X == cell.X && point.Y == cell.Y + 1))
                spriteBatch.Draw(
                    textureMap[TextureID.WhitePixel],
                    new Rectangle(cell.X * 32 - 1, (cell.Y + 1) * 32 - 1, 34, 3),
                    targetRadius
                );
        }
        DrawCenter(spriteBatch, textureMap);
        DrawRange(spriteBatch, textureMap);
        spriteBatch.End();
        camera.ResetViewport();
    }

    private void DrawCenter(SpriteBatch spriteBatch, Dictionary<TextureID, Texture2D> textureMap)
    {
        List<Rectangle> rects =
        [
            new Rectangle(
                (_center.X + _offset.X) * 32 - 1,
                (_center.Y + _offset.Y) * 32 - 1,
                3,
                34
            ),
            new Rectangle(
                (_center.X + _offset.X) * 32 - 1,
                (_center.Y + _offset.Y) * 32 - 1,
                34,
                3
            ),
            new Rectangle(
                (_center.X + _offset.X + 1) * 32 - 1,
                (_center.Y + _offset.Y) * 32 - 1,
                3,
                34
            ),
            new Rectangle(
                (_center.X + _offset.X) * 32 - 1,
                (_center.Y + _offset.Y + 1) * 32 - 1,
                34,
                3
            ),
        ];
        foreach (Rectangle rect in rects)
        {
            spriteBatch.Draw(textureMap[TextureID.WhitePixel], rect, targetCenter);
        }
    }

    private void DrawRange(SpriteBatch spriteBatch, Dictionary<TextureID, Texture2D> textureMap)
    {
        List<Point> points = ShadowCast.GetArea(_parent.CurrentMap, _center, _range);
        foreach (Point cell in points.ToHashSet())
        {
            if (!points.Exists((Point point) => point.X == cell.X - 1 && point.Y == cell.Y))
                spriteBatch.Draw(
                    textureMap[TextureID.WhitePixel],
                    new Rectangle(cell.X * 32 - 1, cell.Y * 32 - 1, 3, 34),
                    targetRange
                );
            if (!points.Exists((Point point) => point.X == cell.X + 1 && point.Y == cell.Y))
                spriteBatch.Draw(
                    textureMap[TextureID.WhitePixel],
                    new Rectangle((cell.X + 1) * 32 - 1, cell.Y * 32 - 1, 3, 34),
                    targetRange
                );
            if (!points.Exists((Point point) => point.X == cell.X && point.Y == cell.Y - 1))
                spriteBatch.Draw(
                    textureMap[TextureID.WhitePixel],
                    new Rectangle(cell.X * 32 - 1, cell.Y * 32 - 1, 34, 3),
                    targetRange
                );
            if (!points.Exists((Point point) => point.X == cell.X && point.Y == cell.Y + 1))
                spriteBatch.Draw(
                    textureMap[TextureID.WhitePixel],
                    new Rectangle(cell.X * 32 - 1, (cell.Y + 1) * 32 - 1, 34, 3),
                    targetRange
                );
        }
    }

    public IInputHandler HandleKey(Keys key)
    {
        switch (key)
        {
            case Keys.Right:
                if (Math.Pow(_offset.X, 2) + Math.Pow(_offset.Y, 2) < _rangeSquared)
                    _offset.X++;
                break;
            case Keys.Left:
                if (Math.Pow(_offset.X, 2) + Math.Pow(_offset.Y, 2) < _rangeSquared)
                    _offset.X--;
                break;
            case Keys.Up:
                if (_offset.Y > -_range)
                    _offset.Y--;
                break;
            case Keys.Down:
                if (_offset.Y < _range)
                    _offset.Y++;
                break;
            case Keys.Escape:
                return _parent;
        }
        return this;
    }
}
