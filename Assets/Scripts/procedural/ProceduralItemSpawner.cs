using UnityEngine;

namespace procedural
{
    public class ProceduralItemSpawner : MonoBehaviour
    {
        [Header("Referências")]
        public GameObject[] prefabsCarregadores;
        public Terrain terrain;
        public Transform playerTransform; 

        [Header("Configuração de Densidade (Noise)")]
        public int gridSize = 10;
        [Range(0f, 1f)]
        public float spawnThreshold = 0.6f;
        public float noiseScale = 5f; 
        public float seedOffset = 500f;

        private void Start()
        {
            SpawnItems();
        }

        private void SpawnItems()
        {
            foreach (Transform child in transform) Destroy(child.gameObject);

            if (prefabsCarregadores.Length == 0) return;

            var mapWidth = terrain.terrainData.size.x;
            var mapLength = terrain.terrainData.size.z;
            for (float x = 0; x < mapWidth; x += gridSize)
            {
                for (float z = 0; z < mapLength; z += gridSize)
                {
                    var xCoord = x / mapWidth * noiseScale + seedOffset;
                    var zCoord = z / mapLength * noiseScale + seedOffset;
                    var noiseValue = Mathf.PerlinNoise(xCoord, zCoord);

                    if (!(noiseValue > spawnThreshold)) continue;
                    var y = terrain.SampleHeight(new Vector3(x, 0, z)) + terrain.transform.position.y;
                    var spawnPos = new Vector3(x, y + 1.0f, z);
                    if(playerTransform && Vector3.Distance(spawnPos, playerTransform.position) < 5f) continue;
                    var prefabEscolhido = prefabsCarregadores[Random.Range(0, prefabsCarregadores.Length)];
                    Instantiate(prefabEscolhido, spawnPos, Quaternion.identity, transform);
                }
            }
        }
    }
}