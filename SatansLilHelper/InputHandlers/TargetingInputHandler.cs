using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Apos.Camera;
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

internal class TargetingInputHandler : IInputHandler, Interfaces.IUpdateable
{
    private GameInputHandler _parent;
    private int _range;
    private int _radius;
    private List<Point> _visibleTiles;
    private Point _center,
        _offset;

    public TargetingInputHandler(GameInputHandler parent, int range, int radius)
    {
        _parent = parent;
        _range = range;
        _radius = radius;
        _visibleTiles = [];
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
        _visibleTiles = ShadowCast.GetVisibleTiles(_parent.CurrentMap, _center + _offset, _radius);
        foreach (Point cell in _visibleTiles.ToHashSet())
        {
            foreach ((int x, int y) in _parent.CurrentMap.GetNeighbors((cell.X, cell.Y))) { }
            spriteBatch.Draw(
                textureMap[TextureID.WhitePixel],
                new Rectangle(cell.X * 32, cell.Y * 32, 32, 32),
                new Color(0xFF, 0x00, 0x00, 0x1F)
            );
        }
        spriteBatch.End();
        camera.ResetViewport();
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

    public void Update(GameTime gameTime)
    {
        _visibleTiles = ShadowCast.GetVisibleTiles(_parent.CurrentMap, _center, _radius);
    }
}
