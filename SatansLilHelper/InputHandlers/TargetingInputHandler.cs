using System.Collections.Generic;
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
    private Point _center;

    public TargetingInputHandler(GameInputHandler parent, int range, int radius)
    {
        _parent = parent;
        _range = range;
        _radius = radius;
        _visibleTiles = [];
        Location center = _parent.CurrentMap.Player.GetComponent<Location>();
        _center = new(center.X, center.Y);
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
        foreach (Point cell in _visibleTiles.ToHashSet())
        {
            spriteBatch.Draw(
                textureMap[TextureID.WhitePixel],
                new Rectangle(cell.X * 32, cell.Y * 32, 32, 32),
                new Color(0x70, 0x10, 0x10, 0x0F)
            );
        }
        spriteBatch.End();
        camera.ResetViewport();
    }

    public IInputHandler HandleKey(Keys key)
    {
        return this;
    }

    public void Update(GameTime gameTime)
    {
        _visibleTiles = ShadowCast.GetVisibleTiles(_parent.CurrentMap, _center, _radius);
    }
}
