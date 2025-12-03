using UnityEngine;

namespace procedural
{
    public class MapGenerator : MonoBehaviour
    {
        [Header("Configurações do Terreno")]
        public Terrain terrain;
        public int width = 256; 
        public int height = 256;
        public int depth = 6;

        [Header("Ruído Perlin")]
        public float scale = 9f; 
        public float offsetX = 100f; 
        public float offsetY = 100f;
    
        [Header("Seed")]
        public bool usarSeed = true;
        public int seed;

        private void Start()
        {
            if(terrain == null) terrain = GetComponent<Terrain>();
            GenerateTerrain();
        }

        private void GenerateTerrain()
        {
            if (usarSeed)
            {
                var prng = new System.Random(seed);
                offsetX = prng.Next(-100000, 100000); 
                offsetY = prng.Next(-100000, 100000);
            }

            terrain.terrainData = GenerateTerrainData(terrain.terrainData);
        }

        private TerrainData GenerateTerrainData(TerrainData terrainData)
        {
            terrainData.heightmapResolution = width + 1;
            terrainData.size = new Vector3(width, depth, height);
            terrainData.SetHeights(0, 0, GenerateHeights());
            return terrainData;
        }

        private float[,] GenerateHeights()
        {
            var heights = new float[width, height];
            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    var xCoord = (float)x / width * scale + offsetX;
                    var yCoord = (float)y / height * scale + offsetY;
                    heights[x, y] = Mathf.PerlinNoise(xCoord, yCoord);
                }
            }
            return heights;
        }
    }
}