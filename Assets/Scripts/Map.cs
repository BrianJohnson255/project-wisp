using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using MapGen;

public class Map : MonoBehaviour
{
	public int length;
	public int width;
	public int height;
	public int numBiomes;
	public string seed;

	private Queue<Voxel> biomeGrowQueue;
	private System.Random rng;
	private Voxel[,,] terrainMap;
	private Voxel[,] biomeMap;

	private void Start() {
		GenerateMap();
	}

	private void OnDrawGizmos() {
		if (biomeMap != null) {
			for (int i = 0; i < length; i++) {
				for (int j = 0; j < width; j++) {
					Color c = Color.white;

					switch (biomeMap[i, j].biome) {
						case BiomeType.Forest:
							c = Color.green;
							break;
						case BiomeType.Desert:
							c = Color.yellow;
							break;
						case BiomeType.Mountain:
							c = Color.red;
							break;
						case BiomeType.Tundra:
							c = Color.blue;
							break;
					}

					Gizmos.color = c;
					Gizmos.DrawCube(new Vector3(i, -1, j), Vector3.one);
				}
			}
		}
	}

	void GenerateMap() {
		rng = new System.Random(seed.GetHashCode());
		GenerateBiomes();
		GenerateTerrain();
	}

	void GenerateBiomes() {
		biomeMap = new Voxel[length, width];
		biomeGrowQueue = new Queue<Voxel>();

		for (int i = 0; i < length; i++) {
			for (int j = 0; j < width; j++) {
				biomeMap[i, j] = new Voxel(new Vector3(i, -1, j), BiomeType.Null, MaterialType.Null);
			}
		}

		PlaceBiomeSeeds();
		GrowBiomes();
	}

	void GenerateTerrain() {
		for (int i = 0; i < length; i++) {
			for (int j = 0; j < width; j++) {
				for (int k = 0; k < height; k++) {
					terrainMap[i, j, k] = new Voxel(new Vector3(i, j, k), biomeMap[i, j].biome, MaterialType.Stone);
				}
			}
		}

		MeshGenerator.GenerateMesh(terrainMap);
	}

	void PlaceBiomeSeeds() {
		BiomeType currentBiome = BiomeType.Forest;
		for (int i = 0; i < numBiomes; i++) {
			int randX = rng.Next(length);
			int randZ = rng.Next(width);
			biomeMap[randX, randZ].biome = currentBiome;
			biomeGrowQueue.Enqueue(biomeMap[randX, randZ]);

			if (currentBiome == BiomeType.Tundra) {
				currentBiome = BiomeType.Forest;
			}

			else {
				currentBiome += 1;
			}
		}
	}

	void GrowBiomes() {
		while (biomeGrowQueue.Count != 0) {
			int voxelsThisIteration = biomeGrowQueue.Count;

			List<Voxel> growList = biomeGrowQueue.ToList();
			Util.ShuffleList(ref growList, rng);
			biomeGrowQueue = Util.ListToQueue(growList);

			for (int i = 0; i < voxelsThisIteration; i++) {
				Voxel biomeVoxel = biomeGrowQueue.Dequeue();
				ExpandBiome(biomeVoxel);
			}
		}
	}

	void ExpandBiome(Voxel coreVoxel) {
		int i = (int)coreVoxel.position.x;
		int j = (int)coreVoxel.position.z;

		if (i - 1 >= 0 && biomeMap[i - 1, j].biome == BiomeType.Null) {
			biomeMap[i - 1, j].biome = coreVoxel.biome;
			biomeGrowQueue.Enqueue(biomeMap[i - 1, j]);
		}

		if (i + 1 < length && biomeMap[i + 1, j].biome == BiomeType.Null) {
			biomeMap[i + 1, j].biome = coreVoxel.biome;
			biomeGrowQueue.Enqueue(biomeMap[i + 1, j]);
		}

		if (j - 1 >= 0 && biomeMap[i, j - 1].biome == BiomeType.Null) {
			biomeMap[i, j - 1].biome = coreVoxel.biome;
			biomeGrowQueue.Enqueue(biomeMap[i, j - 1]);
		}

		if (j + 1 < width && biomeMap[i, j + 1].biome == BiomeType.Null) {
			biomeMap[i, j + 1].biome = coreVoxel.biome;
			biomeGrowQueue.Enqueue(biomeMap[i, j + 1]);
		}
	}
}
