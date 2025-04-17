using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Friflo.Engine.ECS;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SatansLilHelper.GameComponents;

internal class IncomingLinks : DrawableGameComponent
{
    private Type? _type;
    private SpriteBatch? batch;

    public IncomingLinks(Game game)
        : base(game) { }

    public override void Initialize()
    {
        base.Initialize();
    }

    protected override void LoadContent()
    {
        batch = new(Game.GraphicsDevice);
    }

    public void SetType(Type type)
    {
        if (type is not ILinkComponent)
            throw new ArgumentException("Type must be an ILinkComponent", nameof(type));
        _type = type;
    }
}
