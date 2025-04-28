using System.Collections.Generic;
using System.Linq;
using Apos.Camera;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SatansLilHelper.Constants;
using SatansLilHelper.Utils;

namespace SatansLilHelper.GameComponents;

internal class TargetingOverlay : DrawableGameComponent
{
    private SpriteBatch? batch;
    private Texture2D? pixel;
    private int range,
        radius;
    private Point center,
        offset;
    private Camera? camera;

    public TargetingOverlay(Game game)
        : base(game)
    {
        center = Point.Zero;
        offset = Point.Zero;
        range = radius = 0;
    }

    protected override void LoadContent()
    {
        base.LoadContent();
        batch = new(Game.GraphicsDevice);
        IVirtualViewport defaultViewport = new DefaultViewport(Game.GraphicsDevice, Game.Window);
        camera = new(defaultViewport);
        pixel = new(Game.GraphicsDevice, 1, 1);
        pixel.SetData([Colors.White]);
    }

    public override void Draw(GameTime gameTime)
    {
        if (batch is null || camera is null)
            return;
        camera.SetViewport();
        batch.Begin(transformMatrix: camera.View);

        DrawCenter();
        DrawRange();
        DrawRadius();

        batch.End();
    }

    private void DrawRadius()
    {
        if (Game is not Engine engine || batch is null)
            return;
        List<Point> points = ShadowCast.GetArea(engine.World.CurrentMap, center + offset, radius);
        foreach (Point cell in points.ToHashSet())
        {
            if (!points.Exists((Point point) => point.X == cell.X - 1 && point.Y == cell.Y))
                batch.Draw(
                    pixel,
                    new Rectangle(cell.X * 32 - 1, cell.Y * 32 - 1, 3, 34),
                    Colors.TargetRadius
                );
            if (!points.Exists((Point point) => point.X == cell.X + 1 && point.Y == cell.Y))
                batch.Draw(
                    pixel,
                    new Rectangle((cell.X + 1) * 32 - 1, cell.Y * 32 - 1, 3, 34),
                    Colors.TargetRadius
                );
            if (!points.Exists((Point point) => point.X == cell.X && point.Y == cell.Y - 1))
                batch.Draw(
                    pixel,
                    new Rectangle(cell.X * 32 - 1, cell.Y * 32 - 1, 34, 3),
                    Colors.TargetRadius
                );
            if (!points.Exists((Point point) => point.X == cell.X && point.Y == cell.Y + 1))
                batch.Draw(
                    pixel,
                    new Rectangle(cell.X * 32 - 1, (cell.Y + 1) * 32 - 1, 34, 3),
                    Colors.TargetRadius
                );
        }
    }

    public override void Update(GameTime gameTime)
    {
        if (Game is not Engine engine)
            return;

        if (Settings.Default.Movement.IsPressedAvailable(engine.Handler))
            ProcessMovement();
    }

    private void ProcessMovement()
    {
        if (Game is not Engine engine)
            return;
        int dx = 0,
            dy = 0;
        if (
            Settings.Default.MoveUp.TryConsumePressed(engine.Handler)
            || Settings.Default.MoveUpLeft.IsPressedAvailable(engine.Handler)
            || Settings.Default.MoveUpRight.IsPressedAvailable(engine.Handler)
        )
            dy = -1;
        if (
            Settings.Default.MoveUpRight.TryConsumePressed(engine.Handler)
            || Settings.Default.MoveRight.TryConsumePressed(engine.Handler)
            || Settings.Default.MoveDownRight.IsPressedAvailable(engine.Handler)
        )
            dx = 1;
        if (
            Settings.Default.MoveDownRight.TryConsumePressed(engine.Handler)
            || Settings.Default.MoveDown.TryConsumePressed(engine.Handler)
            || Settings.Default.MoveDownLeft.IsPressedAvailable(engine.Handler)
        )
            dy = 1;
        if (
            Settings.Default.MoveDownLeft.TryConsumePressed(engine.Handler)
            || Settings.Default.MoveLeft.TryConsumePressed(engine.Handler)
            || Settings.Default.MoveUpLeft.TryConsumePressed(engine.Handler)
        )
            dx = -1;
        if (offset.X + dx < radius)
            offset.X += dx;
        if (offset.Y + dy < radius)
            offset.Y += dy;
    }

    private void DrawCenter()
    {
        if (batch is null || pixel is null)
            return;
        List<Rectangle> rects =
        [
            new Rectangle((center.X + offset.X) * 32 - 1, (center.Y + offset.Y) * 32 - 1, 3, 34),
            new Rectangle((center.X + offset.X) * 32 - 1, (center.Y + offset.Y) * 32 - 1, 34, 3),
            new Rectangle(
                (center.X + offset.X + 1) * 32 - 1,
                (center.Y + offset.Y) * 32 - 1,
                3,
                34
            ),
            new Rectangle(
                (center.X + offset.X) * 32 - 1,
                (center.Y + offset.Y + 1) * 32 - 1,
                34,
                3
            ),
        ];
        foreach (Rectangle rect in rects)
        {
            batch.Draw(pixel, rect, Colors.Target);
        }
    }

    private void DrawRange()
    {
        if (Game is not Engine engine || batch is null)
            return;
        List<Point> points = ShadowCast.GetArea(engine.World.CurrentMap, center, range);
        foreach (Point cell in points.ToHashSet())
        {
            if (!points.Exists((Point point) => point.X == cell.X - 1 && point.Y == cell.Y))
                batch.Draw(
                    pixel,
                    new Rectangle(cell.X * 32 - 1, cell.Y * 32 - 1, 3, 34),
                    Colors.TargetRange
                );
            if (!points.Exists((Point point) => point.X == cell.X + 1 && point.Y == cell.Y))
                batch.Draw(
                    pixel,
                    new Rectangle((cell.X + 1) * 32 - 1, cell.Y * 32 - 1, 3, 34),
                    Colors.TargetRange
                );
            if (!points.Exists((Point point) => point.X == cell.X && point.Y == cell.Y - 1))
                batch.Draw(
                    pixel,
                    new Rectangle(cell.X * 32 - 1, cell.Y * 32 - 1, 34, 3),
                    Colors.TargetRange
                );
            if (!points.Exists((Point point) => point.X == cell.X && point.Y == cell.Y + 1))
                batch.Draw(
                    pixel,
                    new Rectangle(cell.X * 32 - 1, (cell.Y + 1) * 32 - 1, 34, 3),
                    Colors.TargetRange
                );
        }
    }
}
