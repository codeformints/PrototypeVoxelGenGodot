using Godot;
using PrototypeVoxelGame.scripts.terrain_gen.data;
using PrototypeVoxelGame.scripts.terrain_gen.data.blocks;
using System;

public class Chunk : MeshInstance
{
	public bool chunkLoaded = false;

	bool startChunkGen = false;
	bool chunkGenerating = false;
	bool creatingMesh = false;

	bool chunkGenerated = false;
	bool chunkGenFinished = false;
	Thread chunkThread = new Thread();
	//Thread meshThread = new Thread();
	//Thread optimizeThread = new Thread();

	Material material;
	Cube cube = new Cube();
	OpenSimplexNoise noise;
	ChunkData data;
	SurfaceTool st;

	public Vector2 position;

	public Vector3 chunkSize = new Vector3(64, 64, 64);
	public Vector2 chunkPos = new Vector2(0, 0);

   public void StartChunkGen(Vector3 size, OpenSimplexNoise noisy, Vector2 pos, Material mat)
   {
		noise = noisy;
		chunkSize = size;
		chunkPos = pos;
		startChunkGen = true;
		material = mat;
		GD.Print("Creating Chunk data...");
		chunkThread.Start(this, "GenerateChunk");
	}

	public override void _Ready()
	{
		try
		{
			st = new SurfaceTool();
		}
		catch(Exception e)
		{
			GD.Print("Something went wrong! Exception: " + e);
		}
	}

	private void CreateMesh()
	{
		st.Begin(Mesh.PrimitiveType.Triangles);
		st.SetMaterial(material);
		st.AddColor(Color.Color8(200, 200, 200));

		GenerateMesh();

		st.Index();
		st.GenerateNormals();
		Mesh = st.Commit();

		GD.Print("Chunk mesh generated!");
		chunkLoaded = true;
	}

	public void GenerateCollider()
	{
		GD.Print("Creating collider...");
		CreateConvexCollision();
		GD.Print("Collider created!");
	}


   

	public void GenerateChunk()
	{
		Random rand = new Random();

		Int16[,,] blocksc = new Int16[(int)chunkSize.x, (int)chunkSize.y, (int)chunkSize.z];

		float value;
		float offset;

		//X
		for (int x = 0; x < chunkSize.x; x++)
		{
			//Y
			for (int y = 0; y < chunkSize.y; y++)
			{
				//Z
				for (int z = 0; z < chunkSize.z; z++)
				{
					if (y >= 0)
					{
						offset = chunkSize.y / 3;
					}
					else
					{
						offset = chunkSize.y;
					}

					Vector3 pos = new Vector3(x, y, z);

					//value = noise.GetNoise3d() + (pos.y / offset);
					value = noise.GetNoise3d(x + (chunkPos.x * chunkSize.x), y, z + (chunkPos.y * chunkSize.z)) + (y / offset);

					if (value > 0.5f)
					{
						blocksc[x, y, z] = (short)BlockType.AIR;
					}
					else
					{
						blocksc[x, y, z] = (short)BlockType.DIRT;
					}
				}
			}
		}

		for (int x = 0; x < chunkSize.x; x++)
		{
			//Y
			for (int y = 0; y < chunkSize.y; y++)
			{
				//Z
				for (int z = 0; z < chunkSize.z; z++)
				{
					if (GetBlockById(blocksc[x, y, z]).type == BlockType.DIRT && y > chunkSize.y / 14)
					{
						if(y + 1 < chunkSize.y)
						{
							if (GetBlockById(blocksc[x, y + 1, z]).type == BlockType.AIR)
							{
								blocksc[x, y, z] = (short)BlockType.GRASS;

								if (y - 1 > 0)
								{
									if (GetBlockById(blocksc[x, y - 1, z]).type == BlockType.DIRT)
									{
										blocksc[x, y - 1, z] = (short)BlockType.GRASS;
									}
								}
							}
						}
					}

					if (GetBlockById(blocksc[x, y, z]).type == BlockType.AIR && y > chunkSize.y / 17 && y < chunkSize.y / 15)
					{
						blocksc[x, y, z] = (short)BlockType.WATER;
					}
				}
			}
		}

		data = new ChunkData((int)chunkSize.x, (int)chunkSize.y, blocksc, new Vector2(0,0));

		GD.Print("Chunk data generated!");
		try
		{
			CreateMesh();
		}
		catch(Exception e)
		{
			GD.Print("Exception: " + e);
		}
	}

	public void GenerateMesh()
	{
		GD.Print("Generating Chunk Mesh...");
		Vector3 pos;
		Vector3 vertPos;

		bool[,,][] faces = new bool[(int)chunkSize.x, (int)chunkSize.y, (int)chunkSize.z][];

		//X
		for (int x = 0; x < chunkSize.x; x++)
		{
			//Y
			for (int y = 0; y < chunkSize.y; y++)
			{
				//Z
				for (int z = 0; z < chunkSize.z; z++)
				{
					faces[x, y, z] = new bool[12];

					pos = new Vector3(x, y, z);
					vertPos = new Vector3((x * cube.size) + ((chunkPos.x * chunkSize.x) * cube.size), (y * cube.size), (z * cube.size) + ((chunkPos.y * chunkSize.z) * cube.size));

					if (data.Blocks[x, y, z] != -1)
					{
						if (!IsBlockHidden(new Vector3(x, y, z)) && Blocks.blocks[data.Blocks[x, y, z]].type != BlockType.AIR)
						{
							st.AddColor(Blocks.blocks[data.Blocks[x, y, z]].color);

							if (!IsBlockAbove(pos))
							{
								faces[x, y, z][(int)FaceSide.TOP] = true;
								cube.AddTop(st, vertPos);
							}
							else
							{
								faces[x, y, z][10] = true;
							}

							if (!IsBlockBelow(pos))
							{
								faces[x, y, z][(int)FaceSide.BOTTOM] = true;
								cube.AddBottom(st, vertPos);
							}
							else
							{
								faces[x, y, z][11] = true;
							}

							if (!IsBlockBehind(pos))
							{
								faces[x, y, z][(int)FaceSide.BACK] = true;
								cube.AddBack(st, vertPos);
							}
							else
							{
								faces[x, y, z][7] = true;
							}

							if (!IsBlockInFront(pos))
							{
								faces[x, y, z][(int)FaceSide.FRONT] = true;
								cube.AddFront(st, vertPos);
							}
							else
							{
								faces[x, y, z][6] = true;
							}

							if (!IsBlockOnLeft(pos))
							{
								faces[x, y, z][(int)FaceSide.LEFT] = true;
								cube.AddLeft(st, vertPos);
							}
							else
							{
								faces[x, y, z][8] = true;
							}

							if (!IsBlockOnRight(pos))
							{
								faces[x, y, z][(int)FaceSide.RIGHT] = true;
								cube.AddRight(st, vertPos);
							}
							else
							{
								faces[x, y, z][9] = true;
							}
							//cube.GenerateBlock(st, new Vector3((x * cube.size) + ((chunkPos.x * chunkSize.x) * cube.size), (y * cube.size), (z * cube.size) + ((chunkPos.y * chunkSize.z) * cube.size)));
						}
					}
				}
			}
		}

	}

	public override void _ExitTree()
	{
		if (chunkThread.IsActive())
		{
			chunkThread.WaitToFinish();
		}
		
		/*
		if (meshThread.IsActive())
		{
			meshThread.WaitToFinish();
		}
		*/
	}

	public Vector3 GetBlockDir(FaceSide side)
	{
		switch (side)
		{
			case FaceSide.FRONT:
				return new Vector3(0, 0, 1);

			case FaceSide.BOTTOM:
				return new Vector3(0, -1, 0);

			case FaceSide.BACK:
				return new Vector3(0, 0, -1);

			case FaceSide.LEFT:
				return new Vector3(-1, 0, 0);

			case FaceSide.RIGHT:
				return new Vector3(1, 0, 0);

			case FaceSide.TOP:
				return new Vector3(0, 1, 0);

			default:
				return new Vector3(0, 0, 0);
		}
	}

	public Block GetBlockAtSide(FaceSide side, Vector3 pos)
	{
		switch(side)
		{
			case FaceSide.BACK:
				return GetBlockById(GetBlockBehind(pos));

			case FaceSide.BOTTOM:
				return GetBlockById(GetBlockBelow(pos));

			case FaceSide.FRONT:
				return GetBlockById(GetBlockInFront(pos));

			case FaceSide.LEFT:
				return GetBlockById(GetBlockOnLeft(pos));

			case FaceSide.RIGHT:
				return GetBlockById(GetBlockOnRight(pos));

			case FaceSide.TOP:
				return GetBlockById(GetBlockAbove(pos));

			default:
				return null;
		}
	}

	public bool IsBlockHidden(Vector3 pos)
	{
		if(IsBlockAbove(pos) && IsBlockBehind(pos) && IsBlockBelow(pos) && IsBlockInFront(pos) && IsBlockOnLeft(pos) && IsBlockOnRight(pos))
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	public bool[] IsNeighbors(Vector3 pos)
	{
		bool[] faces = new bool[6];

		faces[(int)FaceSide.TOP] = IsBlockAbove(pos);
		faces[(int)FaceSide.BOTTOM] = IsBlockBelow(pos);
		faces[(int)FaceSide.RIGHT] = IsBlockOnRight(pos);
		faces[(int)FaceSide.LEFT] = IsBlockOnLeft(pos);
		faces[(int)FaceSide.FRONT] = IsBlockInFront(pos);
		faces[(int)FaceSide.BACK] = IsBlockBehind(pos);

		return faces;
	}

	public bool IsBlockAbove(Vector3 pos)
	{
		Int16 block = GetBlockAbove(pos);

		if (block != -1)
		{
			if (GetBlockById(block).type != BlockType.AIR)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		else
		{
			return false;
		}
	}

	public bool IsBlockBelow(Vector3 pos)
	{
		Int16 block = GetBlockBelow(pos);

		if (block != -1)
		{
			if (GetBlockById(block).type != BlockType.AIR)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		else
		{
			return false;
		}
	}

	public bool IsBlockInFront(Vector3 pos)
	{
		Int16 block = GetBlockInFront(pos);

		if (block != -1)
		{
			if (GetBlockById(block).type != BlockType.AIR)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		else
		{
			return false;
		}
	}

	public bool IsBlockBehind(Vector3 pos)
	{
		Int16 block = GetBlockBehind(pos);

		if (block != -1)
		{
			if (GetBlockById(block).type != BlockType.AIR)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		else
		{
			return false;
		}
	}
		

	public bool IsBlockOnRight(Vector3 pos)
	{
		Int16 block = GetBlockOnRight(pos);

		if (block != -1)
		{
			if (GetBlockById(block).type != BlockType.AIR)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		else
		{
			return false;
		}
	}

	public bool IsBlockOnLeft(Vector3 pos)
	{
		Int16 block = GetBlockOnLeft(pos);

		if (block != -1)
		{
			if (GetBlockById(block).type != BlockType.AIR)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		else
		{
			return false;
		}
	}

	public Block GetBlockById(Int16 id)
	{
		return Blocks.blocks[id];
	}

	public Int16 GetBlockAbove(Vector3 pos)
	{
		if (pos.y <= chunkSize.y - 2)
		{
			return data.Blocks[(int)pos.x, (int)pos.y + 1, (int)pos.z];
		}
		else
		{
			return -1;
		}
	}

	public Int16 GetBlockBelow(Vector3 pos)
	{
		if (pos.y != 0)
		{
			return data.Blocks[(int)pos.x, (int)pos.y - 1, (int)pos.z];
		}
		else
		{
			return -1;
		}
	}

	public Int16 GetBlockOnRight(Vector3 pos)
	{
		if (pos.x <= chunkSize.x - 2)
		{
			return data.Blocks[(int)pos.x + 1, (int)pos.y, (int)pos.z];
		}
		else
		{
			return -1;
		}
	}

	public Int16 GetBlockOnLeft(Vector3 pos)
	{
		if (pos.x != 0)
		{
			return data.Blocks[(int)pos.x - 1, (int)pos.y, (int)pos.z];
		}
		else
		{
			return -1;
		}
	}

	public Int16 GetBlockBehind(Vector3 pos)
	{
		if (pos.z != 0)
		{
			return data.Blocks[(int)pos.x, (int)pos.y, (int)pos.z - 1];
		}
		else
		{
			return -1;
		}
	}

	public Int16 GetBlockInFront(Vector3 pos)
	{
		if (pos.z <= chunkSize.z - 2)
		{
			return data.Blocks[(int)pos.x, (int)pos.y, (int)pos.z + 1];
		}
		else
		{
			return -1;
		}
	}


}
