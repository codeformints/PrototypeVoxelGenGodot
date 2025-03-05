using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;

namespace PrototypeVoxelGame.scripts.terrain_gen.data
{
    public struct ChunkData
    {
        public Vector2 Region;
        public int Size;
        public int Depth;

        public Int16[,,] Blocks;

        public ChunkData(int size, int depth, Int16[,,] blocks, Vector2 region)
        {
            Size = size;
            Depth = depth;
            Blocks = blocks;
            Region = region;
        }
    }
}
