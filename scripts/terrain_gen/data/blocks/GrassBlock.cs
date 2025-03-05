using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrototypeVoxelGame.scripts.terrain_gen.data.blocks
{
    public class GrassBlock : Block
    {
        public GrassBlock()
        {
            type = BlockType.GRASS;
            color = Color.Color8(20, 240, 5);
        }
    }
}
