using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrototypeVoxelGame.scripts.terrain_gen.data.blocks
{
    class WaterBlock : Block
    {
        public WaterBlock()
        {
            type = BlockType.WATER;
            color = Color.Color8(71, 126, 245);
        }
    }
}
