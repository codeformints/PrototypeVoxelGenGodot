
using Godot;

namespace PrototypeVoxelGame.scripts.terrain_gen.data.blocks
{
    public class DirtBlock : Block
    {
        public DirtBlock()
        {
            type = BlockType.DIRT;
            color = Color.Color8(140, 60, 30);
        }
    }


}
