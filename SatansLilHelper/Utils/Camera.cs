using Microsoft.Xna.Framework;

namespace SatansLilHelper.Utils;

public class Camera
{
    private Point _screenSizePixels,
        _screenSizeTiles,
        _center,
        _offset;
    private int _tileSize;

    public Camera(Point screenSizePixels, int tileSize)
    {
        _screenSizePixels = screenSizePixels;
        _tileSize = tileSize;
        _screenSizeTiles = new Point(
            _screenSizePixels.X / _tileSize,
            _screenSizePixels.Y / _tileSize
        );
        _center = _offset = Point.Zero;
    }

    #region SetCenter
    public void SetCenter(Point center)
    {
        _center = center;
        CalculateOffset();
    }

    public void SetCenter(int x, int y)
    {
        _center.X = x;
        _center.Y = y;
        CalculateOffset();
    }

    public void SetCenter(float x, float y)
    {
        _center.X = (int)x;
        _center.Y = (int)y;
        CalculateOffset();
    }

    public void SetCenter(Vector2 center)
    {
        _center.X = (int)center.X;
        _center.Y = (int)center.Y;
        CalculateOffset();
    }
    #endregion SetCenter
    public void CalculateOffset()
    {
        _offset.X = _center.X - (_screenSizeTiles.X / 2);
        _offset.Y = _center.Y - (_screenSizeTiles.Y / 2);
    }

    public Point GetOffset() { return _offset; }
    public int TileWidth => _screenSizeTiles.X;
    public int TileHeight => _screenSizeTiles.Y;
}
