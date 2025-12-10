using Unity.AI.Navigation;
using UnityEngine;

namespace procedural
{
    public class MapGenerator : MonoBehaviour
    {
        public Terrain terrain;
        public ParamProceduralConfig config; 
        public NavMeshSurface navMeshSurface;

        private void Start()
        {
            if (terrain == null) terrain = GetComponent<Terrain>();
            if (config == null) { Debug.LogError("Falta a Config no MapGenerator!"); return; }

            GenerateTerrain();
            PintarTerreno();
            PintarGrama();
            if (navMeshSurface == null) return;
            terrain.terrainData.SyncHeightmap(); 
            navMeshSurface.BuildNavMesh();
        }

        private void GenerateTerrain()
        {
            var data = terrain.terrainData;
            data.heightmapResolution = config.width + 1;
            data.size = new Vector3(config.width, config.depth, config.height);

            var heights = new float[config.width, config.height];

            for (var x = 0; x < config.width; x++)
            {
                for (var y = 0; y < config.height; y++)
                {
                    heights[x, y] = config.GetNoiseValue(x, y, config.terrainScale, 0, config.width, config.height);
                }
            }
        
            data.SetHeights(0, 0, heights);
        }

        private void PintarTerreno()
        {
            var data = terrain.terrainData;
            var mapX = data.alphamapWidth;
            var mapY = data.alphamapHeight;
            var splatmapData = new float[mapX, mapY, 3];

            for (var y = 0; y < mapY; y++)
            {
                for (var x = 0; x < mapX; x++)
                {
                    var normX = (float)x / (mapX - 1);
                    var normY = (float)y / (mapY - 1);
                    var worldX = normY * data.size.x;
                    var worldZ = normX * data.size.z;
                    var itemVal = config.GetNoiseValue(worldX, worldZ, config.itemNoiseScale, config.itemSeedOffset, data.size.x, data.size.z);
                    var enemyVal = config.GetNoiseValue(worldX, worldZ, config.enemyNoiseScale, config.enemySeedOffset, data.size.x, data.size.z);
                    var splat = new float[3];
                    if (enemyVal > config.enemyThreshold) splat[2] = 1f;
                    else if (itemVal > config.itemThreshold) splat[1] = 1f;
                    else splat[0] = 1f;

                    splatmapData[x, y, 0] = splat[0];
                    splatmapData[x, y, 1] = splat[1];
                    splatmapData[x, y, 2] = splat[2];
                }
            }
            data.SetAlphamaps(0, 0, splatmapData);
        }

        private void PintarGrama()
    {
        var data = terrain.terrainData;
        var detailWidth = data.detailWidth;
        var detailHeight = data.detailHeight;
        var map = new int[detailWidth, detailHeight];

        for (var y = 0; y < detailHeight; y++)
        {
            for (var x = 0; x < detailWidth; x++)
            {
                var normX = (float)x / detailWidth;
                var normY = (float)y / detailHeight;
                var worldX = normY * data.size.x;
                var worldZ = normX * data.size.z;
                var enemyVal = config.GetNoiseValue(worldX, worldZ, config.enemyNoiseScale, config.enemySeedOffset, data.size.x, data.size.z);
                var itemVal = config.GetNoiseValue(worldX, worldZ, config.itemNoiseScale, config.itemSeedOffset, data.size.x, data.size.z);
                if (enemyVal > config.enemyThreshold || itemVal > config.itemThreshold)
                {
                    map[x, y] = 0;
                }
                else
                {
                    var grassNoise = Mathf.PerlinNoise(worldX * 0.1f, worldZ * 0.1f);
                    if (grassNoise > config.grassThreshold)
                    {
                        map[x, y] = (int)(config.grassDensity * 10); 
                    }
                    else
                    {
                        map[x, y] = 0;
                    }
                }
            }
        }
        data.SetDetailLayer(0, 0, 0, map);
    }
    }
}