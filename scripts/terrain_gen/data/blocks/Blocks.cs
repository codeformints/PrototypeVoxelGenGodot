using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrototypeVoxelGame.scripts.terrain_gen.data.blocks
{
    public enum FaceSide
    {
        FRONT,
        BACK,
        LEFT,
        RIGHT,
        TOP,
        BOTTOM
    }

    public enum BlockType
    {
        AIR,
        DIRT,
        GRASS,
        WATER
    }

    public static class Blocks
    {
        public static Block[] blocks = new Block[4];
        
        public static void CreatBlockRefs()
        {
            blocks[0] = new AirBlock();
            blocks[1] = new DirtBlock();
            blocks[2] = new GrassBlock();
            blocks[3] = new WaterBlock();
        }

    }
}
