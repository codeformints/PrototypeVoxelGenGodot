using Godot;
using PrototypeVoxelGame.scripts.terrain_gen.data.blocks;
using System;

public class Cube : MeshInstance
{
	Material material;
	//public float size = 0.03125f;
	public float size = 1f / 8;

	public override void _Ready()
	{
		//SurfaceTool st = new SurfaceTool();

		//GenerateCube(st);
	}

	public void GenerateBlock(SurfaceTool st, Vector3 pos)
	{
		AddFront(st, pos);
		AddBack(st, pos);
		AddBottom(st, pos);
		AddRight(st, pos);
		AddLeft(st, pos);
		AddTop(st, pos);
	}

	public void GreedFaces(SurfaceTool st, Vector3 startPos, Vector3 endPos, FaceSide side)
    {
		switch (side)
		{
			case FaceSide.FRONT:
				//Front
				st.AddVertex(new Vector3(startPos.x, startPos.y, startPos.z + size));
				st.AddVertex(new Vector3(startPos.x, startPos.y + size, startPos.z + size));
				st.AddVertex(new Vector3(endPos.x + size, endPos.y, endPos.z + size));

				st.AddVertex(new Vector3(endPos.x + size, endPos.y, endPos.z + size));
				st.AddVertex(new Vector3(startPos.x, startPos.y + size, startPos.z + size));
				st.AddVertex(new Vector3(endPos.x + size, endPos.y + size, endPos.z + size));
				break;

			case FaceSide.BACK:
				//Back
				st.AddVertex(new Vector3(endPos.x + size, endPos.y + size, endPos.z));
				st.AddVertex(new Vector3(startPos.x, startPos.y + size, startPos.z));
				st.AddVertex(new Vector3(endPos.x + size, endPos.y, endPos.z));

				st.AddVertex(new Vector3(endPos.x + size, endPos.y, endPos.z));
				st.AddVertex(new Vector3(startPos.x, startPos.y + size, startPos.z));
				st.AddVertex(new Vector3(startPos.x, startPos.y, startPos.z));
				break;

			case FaceSide.TOP:
				//Top
				st.AddVertex(new Vector3(endPos.x + size, endPos.y + size, endPos.z + size));
				st.AddVertex(new Vector3(startPos.x, startPos.y + size, startPos.z + size));
				st.AddVertex(new Vector3(endPos.x + size, endPos.y + size, endPos.z));

				st.AddVertex(new Vector3(endPos.x + size, endPos.y + size, endPos.z));
				st.AddVertex(new Vector3(startPos.x, startPos.y + size, startPos.z + size));
				st.AddVertex(new Vector3(startPos.x, startPos.y + size, startPos.z));
				break;

			case FaceSide.BOTTOM:
				//Bottom
				st.AddVertex(new Vector3(startPos.x, startPos.y, startPos.z));
				st.AddVertex(new Vector3(startPos.x, startPos.y, startPos.z + size));
				st.AddVertex(new Vector3(endPos.x + size, endPos.y, endPos.z));

				st.AddVertex(new Vector3(endPos.x + size, endPos.y, endPos.z));
				st.AddVertex(new Vector3(startPos.x, startPos.y, startPos.z + size));
				st.AddVertex(new Vector3(endPos.x + size, endPos.y, endPos.z + size));
				break;

			case FaceSide.LEFT:
				//Left
				st.AddVertex(new Vector3(endPos.x, endPos.y + size, endPos.z + size));
				st.AddVertex(new Vector3(endPos.x, endPos.y, endPos.z + size));
				st.AddVertex(new Vector3(startPos.x, startPos.y + size, startPos.z));

				st.AddVertex(new Vector3(startPos.x, startPos.y + size, startPos.z));
				st.AddVertex(new Vector3(endPos.x, endPos.y, endPos.z + size));
				st.AddVertex(new Vector3(startPos.x, startPos.y, startPos.z));
				break;

			case FaceSide.RIGHT:
				//Right
				st.AddVertex(new Vector3(startPos.x + size, startPos.y, startPos.z));
				st.AddVertex(new Vector3(endPos.x + size, endPos.y, endPos.z + size));
				st.AddVertex(new Vector3(startPos.x + size, startPos.y + size, startPos.z));

				st.AddVertex(new Vector3(startPos.x + size, startPos.y + size, startPos.z));
				st.AddVertex(new Vector3(endPos.x + size, endPos.y, endPos.z + size));
				st.AddVertex(new Vector3(endPos.x + size, endPos.y + size, endPos.z + size));
				break;
		}

    }

	public void GenerateCube(SurfaceTool st)
	{
		material = ResourceLoader.Load<Material>("res://materials/cube_mat.tres");

		st.Begin(Mesh.PrimitiveType.Triangles);
		st.SetMaterial(material);
		st.AddColor(Color.Color8(200, 200, 200));

		Vector3 pos = new Vector3(0, 0, 0);

		AddFront(st, pos);
		AddBack(st, pos);
		AddBottom(st, pos);
		AddRight(st, pos);
		AddLeft(st, pos);
		AddTop(st, pos);

		st.Index();
		st.GenerateNormals();

		Mesh = st.Commit();
	}

	public void AddFront(SurfaceTool st, Vector3 pos)
	{
		//Front
		st.AddVertex(new Vector3(pos.x, pos.y, pos.z + size));
		st.AddVertex(new Vector3(pos.x, pos.y + size, pos.z + size));
		st.AddVertex(new Vector3(pos.x + size, pos.y, pos.z + size));

		st.AddVertex(new Vector3(pos.x + size, pos.y, pos.z + size));
		st.AddVertex(new Vector3(pos.x, pos.y + size, pos.z + size));
		st.AddVertex(new Vector3(pos.x + size, pos.y + size, pos.z + size));
	}

	public void AddBack(SurfaceTool st, Vector3 pos)
	{
		//Back
		st.AddVertex(new Vector3(pos.x + size, pos.y + size, pos.z));
		st.AddVertex(new Vector3(pos.x, pos.y + size, pos.z));
		st.AddVertex(new Vector3(pos.x + size, pos.y, pos.z));

		st.AddVertex(new Vector3(pos.x + size, pos.y, pos.z));
		st.AddVertex(new Vector3(pos.x, pos.y + size, pos.z));
		st.AddVertex(new Vector3(pos.x, pos.y, pos.z));		
	}

	public void AddLeft(SurfaceTool st, Vector3 pos)
	{
		//Left
		st.AddVertex(new Vector3(pos.x, pos.y + size, pos.z + size));
		st.AddVertex(new Vector3(pos.x, pos.y, pos.z + size));
		st.AddVertex(new Vector3(pos.x, pos.y + size, pos.z));

		st.AddVertex(new Vector3(pos.x, pos.y + size, pos.z));
		st.AddVertex(new Vector3(pos.x, pos.y, pos.z + size));
		st.AddVertex(new Vector3(pos.x, pos.y, pos.z));		
	}

	public void AddRight(SurfaceTool st, Vector3 pos)
	{
		//Right
		st.AddVertex(new Vector3(pos.x + size, pos.y, pos.z));
		st.AddVertex(new Vector3(pos.x + size, pos.y, pos.z + size));
		st.AddVertex(new Vector3(pos.x + size, pos.y + size, pos.z));

		st.AddVertex(new Vector3(pos.x + size, pos.y + size, pos.z));
		st.AddVertex(new Vector3(pos.x + size, pos.y, pos.z + size));
		st.AddVertex(new Vector3(pos.x + size, pos.y + size, pos.z + size));
	}

	public void AddTop(SurfaceTool st, Vector3 pos)
	{
		//Top
		st.AddVertex(new Vector3(pos.x + size, pos.y + size, pos.z + size));
		st.AddVertex(new Vector3(pos.x, pos.y + size, pos.z + size));
		st.AddVertex(new Vector3(pos.x + size, pos.y + size, pos.z));

		st.AddVertex(new Vector3(pos.x + size, pos.y + size, pos.z));
		st.AddVertex(new Vector3(pos.x, pos.y+ size, pos.z + size));
		st.AddVertex(new Vector3(pos.x, pos.y + size, pos.z));
	}

	public void AddBottom(SurfaceTool st, Vector3 pos)
	{
		//Bottom
		st.AddVertex(new Vector3(pos.x, pos.y, pos.z));
		st.AddVertex(new Vector3(pos.x, pos.y, pos.z + size));
		st.AddVertex(new Vector3(pos.x + size, pos.y, pos.z));

		st.AddVertex(new Vector3(pos.x + size, pos.y, pos.z));
		st.AddVertex(new Vector3(pos.x, pos.y, pos.z + size));
		st.AddVertex(new Vector3(pos.x + size, pos.y, pos.z + size));
	}

}
