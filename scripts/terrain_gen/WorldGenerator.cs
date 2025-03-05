using Godot;
using PrototypeVoxelGame.scripts.terrain_gen.data.blocks;
using System.Collections.Generic;

public class WorldGenerator : Node
{
	Spatial camera;

	PackedScene chunkScene;
	PackedScene rigidBall;

	Vector3 chunkSize = new Vector3(64, 160, 64);
	OpenSimplexNoise noise = new OpenSimplexNoise();

	int maxJobs = 4;
	int renderDistance = 24;

	Chunk[,] chunks;
	Queue<Chunk> chunkJobs = new Queue<Chunk>();
	Queue<Chunk> chunkJobsInProgress = new Queue<Chunk>();

	Material material;

	public override void _Ready()
	{
		camera = (Spatial)GetParent().GetChild(0);

		Blocks.CreatBlockRefs();

		material = ResourceLoader.Load<Material>("res://materials/cube_mat.tres");
		rigidBall = GD.Load<PackedScene>("res://testing/RigidBall.tscn");

		noise.Seed = 4728459;
		noise.Octaves = 8;
		noise.Period = 160;
		noise.Persistence = 0.3f;
		noise.Lacunarity = 1.5f;

		chunkScene = GD.Load<PackedScene>("res://world/Chunk.tscn");
		chunks = new Chunk[renderDistance,renderDistance];

		for(int x = 0; x < renderDistance; x++)
		{
			for(int y = 0; y < renderDistance; y++)
			{
				Chunk chunk = (Chunk)chunkScene.Instance();
				chunks[x, y] = chunk;

				chunk.position = new Vector2(x, y);

				chunkJobs.Enqueue(chunk);
				AddChild(chunk);
			}
		}
	}

	public override void _Process(float delta)
	{

		if(Input.IsActionJustPressed("ui_accept"))
		{
			Spatial inst = (Spatial)rigidBall.Instance();
			inst.Translation = camera.Translation;
			GetParent().AddChild(inst);
		}

		if(chunkJobs.Count > 0 && chunkJobsInProgress.Count == 0)
		{
			for(int i = 0; i < maxJobs; i++)
			{
				if (chunkJobs.Count != 0)
				{
					Chunk chunk = chunkJobs.Dequeue();
					chunk.StartChunkGen(chunkSize, noise, chunk.position, material);
					chunkJobsInProgress.Enqueue(chunk);
				}
				else
				{
					break;
				}
			}
		}
		else
		{
			if (chunkJobsInProgress.Count != 0)
			{
				if (chunkJobsInProgress.Peek().chunkLoaded)
				{
					//GD.Print("Chunk Job finished");
					Chunk chunk = chunkJobsInProgress.Dequeue();
					chunk.GenerateCollider();
				}
			}
		}

	}


}
