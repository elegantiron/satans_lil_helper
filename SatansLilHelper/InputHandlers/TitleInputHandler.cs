using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SatansLilHelper.Constants;
using SatansLilHelper.Interfaces;
using SatansLilHelper.Properties;
using SatansLilHelper.Types;
using SatansLilHelper.Utils;

namespace SatansLilHelper.InputHandlers;

internal class TitleInputHandler : IInputHandler, Interfaces.IUpdateable
{
    private struct Range
    {
        public int Min,
            Max;
    }

    private Range _blinkInterval = new() { Min = 6, Max = 9 };
    private Vector2 titlePosition = Vector2.Zero;
    private VecPair satanVecs = new();
    private MersenneTwister _rng = new();
    private SpriteFontBase _font;
    private FontSystem _fontSystem;
    private bool _eyesOpen = true;
    private double _nextBlink;
    private double _blinkEnd;

    public TitleInputHandler()
    {
        _nextBlink = _rng.Next(_blinkInterval.Min, _blinkInterval.Max);
        FontSystemSettings fontSettings = new()
        {
            FontResolutionFactor = 4.0f,
            KernelHeight = 4,
            KernelWidth = 4,
        };
        _fontSystem = new(fontSettings);
        _fontSystem.AddFont(File.ReadAllBytes(@"Content/Fonts/FairyDustB.ttf"));
        _font = _fontSystem.GetFont(125);
    }

    public void Draw(
        SpriteBatch spriteBatch,
        Dictionary<TextureID, Texture2D> textureMap,
        Dictionary<EffectID, SoundEffect> effectMap,
        Dictionary<SongID, Song> songMap,
        Dictionary<FontID, SpriteFont> fontMap
    )
    {
        if (titlePosition == Vector2.Zero)
        {
            CalculateVectors(spriteBatch, textureMap);
        }
        (int rand1, int rand2, int rand3) = GetOffsets();
        string titleString = string.Format(GameStrings.GameTitle, rand1, rand2, rand3);
        spriteBatch.DrawString(_font, titleString, titlePosition, Colors.AmericanRose);
        List<TextureID> satanTextures = [TextureID.SatanMain, TextureID.SatanMouthClosed];
        if (_eyesOpen)
            satanTextures.Add(TextureID.SatanEyesOpen);
        else
            satanTextures.Add(TextureID.SatanEyesClosed);
        foreach (TextureID index in satanTextures)
        {
            spriteBatch.Draw(
                textureMap[index],
                satanVecs.Location,
                null,
                Colors.White,
                0f,
                satanVecs.Origin,
                1f,
                SpriteEffects.None,
                1f
            );
        }
        spriteBatch.Draw(textureMap[TextureID.KeyboardRight], new Vector2(200, 550), Colors.White);
        spriteBatch.Draw(
            textureMap[TextureID.KeyboardRightOutline],
            new Vector2(300, 550),
            Colors.White
        );
    }

    private void CalculateVectors(
        SpriteBatch spriteBatch,
        Dictionary<TextureID, Texture2D> textureMap
    )
    {
        titlePosition.Y = 5;
        Vector2 width = _font.MeasureString(GameStrings.GameTitle);
        titlePosition.X = (spriteBatch.GraphicsDevice.Viewport.Width - width.X) / 2;
        satanVecs.Location.X = spriteBatch.GraphicsDevice.Viewport.Width / 2;
        satanVecs.Location.Y = spriteBatch.GraphicsDevice.Viewport.Height / 2;
        satanVecs.Origin.X = textureMap[TextureID.SatanMain].Width / 2;
        satanVecs.Origin.Y = textureMap[TextureID.SatanMain].Height / 2;
    }

    public IInputHandler HandleKey(Keys key)
    {
        switch (key)
        {
            case Keys.Escape:
                EventBus.Send(Events.QuitGame, new EventMessage());
                break;
            case Keys.Enter:
                return new MainMenuInputHandler();
        }
        return this;
    }

    public void Update(GameTime gameTime)
    {
        double totalSeconds = gameTime.TotalGameTime.TotalSeconds;
        if (totalSeconds > _nextBlink)
        {
            CalculateBlink(gameTime);
            _eyesOpen = false;
        }
        else if (totalSeconds > _blinkEnd)
            _eyesOpen = true;
    }

    private void CalculateBlink(GameTime gameTime)
    {
        _nextBlink =
            _rng.Next(_blinkInterval.Min, _blinkInterval.Max) + gameTime.TotalGameTime.TotalSeconds;
        _blinkEnd = _rng.Next(1) + _rng.NextDouble() + gameTime.TotalGameTime.TotalSeconds;
    }

    private (int, int, int) GetOffsets()
    {
        return (
            (int)(_rng.Next(25) * (_rng.NextDouble() < 0.5 ? 1 : -1)),
            (int)(_rng.Next(25) * (_rng.NextDouble() < 0.5 ? 1 : -1)),
            (int)(_rng.Next(25) * (_rng.NextDouble() < 0.5 ? 1 : -1))
        );
    }
}
