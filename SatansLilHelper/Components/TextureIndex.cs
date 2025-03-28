using Friflo.Engine.ECS;
using Friflo.Json.Fliox;
using SatansLilHelper.Constants;
using SatansLilHelper.Utils;

namespace SatansLilHelper.Components;

[ComponentKey("texture-index")]
public struct TextureIndex : IComponent
{
    [Serialize]
    public TextureID Index;

    public TextureIndex()
    {
        Index = TextureID.Missing;
    }

    public TextureIndex(TextureID index)
    {
        Index = index;
    }
}
