using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apos.Camera;
using Friflo.Engine.ECS;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SatansLilHelper.Constants;
using SatansLilHelper.ECSComponents;
using SatansLilHelper.Extensions;
using SatansLilHelper.Utils;

namespace SatansLilHelper.GameComponents;

internal class StatusWindow : DrawableGameComponent
{
    private SpriteBatch? _spriteBatch;
    private SpriteFont? _font;
    private Texture2D? _whitePixel;

    public StatusWindow(Game game)
        : base(game) { }

    public override void Initialize()
    {
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new(Game.GraphicsDevice);
        _font = Game.Content.Load<SpriteFont>(FilePaths.StatusFont);

        _whitePixel = new(Game.GraphicsDevice, 1, 1);
        _whitePixel.SetData([Colors.White]);
    }

    public override void Draw(GameTime gameTime)
    {
        if (Game is not Engine engine)
            return;
        if (_spriteBatch is null || _font is null)
            return;
        _spriteBatch.Begin();

        _spriteBatch.Draw(
            _whitePixel,
            new Rectangle(
                Game.GraphicsDevice.Viewport.Width * 4 / 5,
                0,
                Game.GraphicsDevice.Viewport.Width / 5,
                Game.GraphicsDevice.Viewport.Height
            ),
            Colors.TranslucentBlack
        );

        Vector2 size,
            textLoc;
        AbilityStat playerResource;

        size = _font.MeasureString(Properties.GameStrings.StatusTitle);
        _spriteBatch.DrawString(
            _font,
            Properties.GameStrings.StatusTitle,
            new Vector2(Game.GraphicsDevice.Viewport.Width * 9 / 10, 15),
            Colors.White,
            new Vector2(size.X / 2, 0)
        );

        textLoc = new(Game.GraphicsDevice.Viewport.Width * 4 / 5 + 10, 2 * _font.LineSpacing);
        Location playerLoc = engine.World.Player.GetComponent<Location>();
        _spriteBatch.DrawString(
            _font,
            String.Format(Properties.GameStrings.StatusLocation, playerLoc.X, playerLoc.Y),
            textLoc,
            Colors.White
        );

        textLoc.Y += 2 * _font.LineSpacing;
        playerResource = engine.World.CurrentMap.Player.GetRelation<AbilityStat, AbilityID>(
            AbilityID.Health
        );
        _spriteBatch.DrawString(
            _font,
            String.Format(
                Properties.GameStrings.StatusHealth,
                playerResource.Cur,
                EntityCalcs.GetStat(engine.World.CurrentMap.Player, AbilityID.Health)
            ),
            textLoc,
            Colors.White
        );

        textLoc.Y += _font.LineSpacing;
        playerResource = engine.World.CurrentMap.Player.GetRelation<AbilityStat, AbilityID>(
            AbilityID.Mana
        );
        _spriteBatch.DrawString(
            _font,
            String.Format(
                Properties.GameStrings.StatusMana,
                playerResource.Cur,
                EntityCalcs.GetStat(engine.World.CurrentMap.Player, AbilityID.Mana)
            ),
            textLoc,
            Colors.White
        );

        textLoc.Y += 2 * _font.LineSpacing;
        _spriteBatch.DrawString(_font, Properties.GameStrings.StatusTurn, textLoc, Colors.White);

        textLoc.Y += _font.LineSpacing;
        _spriteBatch.DrawString(
            _font,
            String.Format(
                Properties.GameStrings.StatusMoves,
                engine.PlayerTurn.MovesUsed,
                engine.PlayerTurn.MovesMax
            ),
            textLoc,
            Colors.White
        );

        textLoc.Y += _font.LineSpacing;
        _spriteBatch.DrawString(
            _font,
            String.Format(
                Properties.GameStrings.StatusAttacks,
                engine.PlayerTurn.AttacksUsed,
                engine.PlayerTurn.AttacksMax
            ),
            textLoc,
            Colors.White
        );

        _spriteBatch.End();
    }

    public override void Update(GameTime gameTime)
    {
        if (Game is not Engine engine)
            return;
        if (DrawOrder == engine.GameScreen.DrawOrder)
            DrawOrder++;
    }
}
