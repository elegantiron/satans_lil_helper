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
    private Entity _entity;
    private Point _center,
        _offset;

    public TargetingInputHandler(GameInputHandler parent, Entity targetable)
    {
        _entity = targetable;
        Targetable tComp = _entity.GetComponent<Targetable>();
        _parent = parent;
        _range = tComp.Range;
        _radius = tComp.Radius;
        Location center = _parent.CurrentMap.Player.GetComponent<Location>();
        _center = new(center.X, center.Y);
        _offset = Point.Zero;
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
                    Colors.Targeting
                );
            if (!points.Exists((Point point) => point.X == cell.X + 1 && point.Y == cell.Y))
                spriteBatch.Draw(
                    textureMap[TextureID.WhitePixel],
                    new Rectangle((cell.X + 1) * 32 - 1, cell.Y * 32 - 1, 3, 34),
                    Colors.Targeting
                );
            if (!points.Exists((Point point) => point.X == cell.X && point.Y == cell.Y - 1))
                spriteBatch.Draw(
                    textureMap[TextureID.WhitePixel],
                    new Rectangle(cell.X * 32 - 1, cell.Y * 32 - 1, 34, 3),
                    Colors.Targeting
                );
            if (!points.Exists((Point point) => point.X == cell.X && point.Y == cell.Y + 1))
                spriteBatch.Draw(
                    textureMap[TextureID.WhitePixel],
                    new Rectangle(cell.X * 32 - 1, (cell.Y + 1) * 32 - 1, 34, 3),
                    Colors.Targeting
                );
        }
        DrawCenter(spriteBatch, textureMap);
        spriteBatch.End();
        camera.ResetViewport();
    }

    private void DrawCenter(SpriteBatch spriteBatch, Dictionary<TextureID, Texture2D> textureMap)
    {
        Color targetCenter = new(0xFF, 0xFF, 0xFF, 0xFF);
        List<Rectangle> rects =
        [
            new Rectangle(_center.X * 32 - 1, _center.Y * 32 - 1, 3, 34),
            new Rectangle(_center.X * 32 - 1, _center.Y * 32 - 1, 34, 3),
            new Rectangle((_center.X + 1) * 32 - 1, _center.Y * 32 - 1, 3, 34),
            new Rectangle(_center.X * 32 - 1, (_center.Y + 1) * 32 - 1, 34, 3),
        ];
        foreach (Rectangle rect in rects)
        {
            spriteBatch.Draw(textureMap[TextureID.WhitePixel], rect, targetCenter);
        }
    }

    public IInputHandler HandleKey(Keys key)
    {
        switch (key)
        {
            case Keys.Right:
                if (_offset.X < _range)
                    _offset.X++;
                break;
            case Keys.Left:
                if (_offset.X > -_range)
                    _offset.X -= 1;
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
