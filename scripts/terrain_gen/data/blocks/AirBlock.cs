using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrototypeVoxelGame.scripts.terrain_gen.data.blocks
{
    public class AirBlock : Block
    {
        public AirBlock()
        {
            type = BlockType.AIR;
        }
    }
}
