using Friflo.Engine.ECS;
using SatansLilHelper.Constants;

namespace SatansLilHelper.ECSComponents;

[ComponentKey("texture-index")]
public struct TextureIndex : IComponent
{
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
