using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SatansLilHelper.Constants;
using SatansLilHelper.Interfaces;
using SatansLilHelper.Utils;

namespace SatansLilHelper.InputHandlers;

internal class TargetingInputHandler(GameInputHandler parent, int range, int radius)
    : IInputHandler,
        Interfaces.IUpdateable
{
    private GameInputHandler _parent = parent;
    private int _range = range;
    private int _radius = radius;
    private List<Point> _visibleTiles = [];
    private Point _center = Point.Zero;

    public void Draw(
        SpriteBatch spriteBatch,
        Dictionary<TextureID, Texture2D> textureMap,
        Dictionary<EffectID, SoundEffect> effectMap,
        Dictionary<SongID, Song> songMap,
        Dictionary<FontID, SpriteFont> fontMap
    )
    {
        _parent.Draw(spriteBatch, textureMap, effectMap, songMap, fontMap);

        spriteBatch.Begin();
        foreach (Point cell in _visibleTiles)
        {
            spriteBatch.Draw(
                textureMap[TextureID.WhitePixel],
                new Rectangle(cell.X * 32, cell.Y * 32, 32, 32),
                Colors.TranslucentBlack
            );
        }
        spriteBatch.End();
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
