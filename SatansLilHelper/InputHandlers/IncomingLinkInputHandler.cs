using System;
using System.Collections.Generic;
using System.Linq;
using Apos.Camera;
using Friflo.Engine.ECS;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SatansLilHelper.Actions;
using SatansLilHelper.Components;
using SatansLilHelper.Constants;
using SatansLilHelper.Extensions;
using SatansLilHelper.Interfaces;

namespace SatansLilHelper.InputHandlers;

internal class IncomingLinkInputHandler<TComponent>(GameInputHandler parent, string title)
    : IInputHandler
    where TComponent : struct, ILinkComponent
{
    private GameInputHandler _parent = parent;
    private string _title = title;
    private int _index = 0;

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

        spriteBatch.Begin();
        Viewport viewport = spriteBatch.GraphicsDevice.Viewport;
        Rectangle shadeShape = new(
            viewport.Width / 10,
            viewport.Height / 10,
            viewport.Width * 8 / 10,
            viewport.Height * 8 / 10
        );
        spriteBatch.Draw(textureMap[TextureID.WhitePixel], shadeShape, Colors.TranslucentBlack);
        Vector2 size = fontMap[FontID.Menu].MeasureString(_title);
        spriteBatch.DrawString(
            fontMap[FontID.Menu],
            _title,
            new Vector2(viewport.Width / 2.0f, shadeShape.Y + size.Y),
            Colors.White,
            size / 2
        );

        Vector2 textDest = new(shadeShape.X + 25, shadeShape.Y + (2.5f * size.Y));
        EntityLinks<TComponent> entities = _parent.CurrentMap.Player.GetIncomingLinks<TComponent>();

        if (_index < 0)
            _index = entities.Count - 1;
        else if (_index >= entities.Count)
            _index %= entities.Count;

        if (entities.Count > 0)
        {
            foreach (int idx in Enumerable.Range(0, entities.Count))
            {
                if (textDest.Y > shadeShape.Height)
                {
                    textDest.Y = shadeShape.Y + (2.5f * size.Y);
                    textDest.X += (shadeShape.Width - 50) / 3;
                    if (textDest.X > shadeShape.Width)
                        break;
                }

                string entName = entities.Entities[idx].Name.value;
                if (entities.Entities[idx].TryGetComponent(out Equipper _))
                {
                    entName += " (e)";
                }

                spriteBatch.DrawString(fontMap[FontID.Status], entName, textDest, Colors.White);
                if (idx == _index)
                {
                    spriteBatch.Draw(
                        textureMap[TextureID.ArrowBlue],
                        new Vector2(textDest.X - 30, textDest.Y + 3),
                        Colors.White
                    );
                }
                textDest.Y += fontMap[FontID.Status].LineSpacing * 1.15f;
            }
        }
        spriteBatch.End();
    }

    public IInputHandler HandleKey(Keys key)
    {
        switch (key)
        {
            case Keys.Escape:
                return _parent;
            case Keys.Up:
                _index--;
                break;
            case Keys.Down:
                _index++;
                break;
            case Keys.Enter:
                return UseItem();
        }
        return this;
    }

    private IInputHandler UseItem()
    {
        Entity ent = _parent.CurrentMap.Player.GetIncomingLinks<TComponent>().Entities[_index];
        if (ent.Tags.Has<Equippable>())
        {
            EquipAction action = new(_parent.CurrentMap.Player, ent);
            action.Perform();
            _parent.PlayerTurn.AddAction(action);
            return this;
        }
        else if (ent.Tags.Has<Activatable>())
        {
            if (ent.Tags.Has<Targetable>())
            {
                return new TargetingInputHandler(_parent, ent);
            }
            else
            {
                Activate(ent);
                return this;
            }
        }
        return this;
    }

    private void Activate(Entity ent)
    {
        Components.Effect effect = ent.GetComponent<Components.Effect>();
        switch (effect.Type)
        {
            case ItemEffect.None:
                break;
            case ItemEffect.Heal:
                HealAction action = new(_parent.CurrentMap.Player, ent);
                action.Perform();
                _parent.PlayerTurn.AddAction(action);
                break;
            case ItemEffect.Harm:
                break;
            case ItemEffect.Cloud:
                break;
        }
    }
}
