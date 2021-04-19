using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MapGen {
	public enum BiomeType { Null, Forest, Desert, Mountain, Tundra };
	public enum MaterialType { Null, Stone, Dirt, Grass, Snow, Sand };
	public enum BlockSides { Null = 0, Up = 1, Down = 2, Left = 4, Right = 8, Front = 16, Back = 32 };

	public class Voxel {
		public Vector3 position;
		public byte enabledSides;
		public BiomeType biome;
		public MaterialType material;

		public Voxel(Vector3 position, BiomeType biome, MaterialType material) {
			this.position = position;
			this.biome = biome;
			this.material = material;
		}
	}
}
