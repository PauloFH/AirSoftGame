using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace procedural
{
    public class ProceduralSpawner : MonoBehaviour
    {
        public enum TipoSpawn { Itens, Inimigos, Natureza }

        [Header("Configuração Geral")]
        public ParamProceduralConfig config;
        public Terrain terrain;
        public Transform playerTransform;
    
        [Header("O que Spawnar?")]
        public TipoSpawn tipoDeSpawn;
        public GameObject[] prefabs; 
        public int gridSize = 2;
        public bool randomizarRotacao = true;
        public bool randomizarEscala = true;
        public Transform containerOrganizador;

        private void Start()
        {
            Invoke(nameof(Spawnar), 0.2f);
        }

        private void Spawnar()
        {
            if (config == null || terrain == null) return;
        
            foreach (Transform child in transform) Destroy(child.gameObject);
            
            if (prefabs == null || prefabs.Length == 0)
            {
                Debug.LogWarning($"[Spawner] Lista de prefabs vazia para: {tipoDeSpawn}");
                return;
            }
            var mapWidth = terrain.terrainData.size.x;
            var mapLength = terrain.terrainData.size.z;
            var terrainPos = terrain.transform.position;
            var scale = config.itemNoiseScale;
            var threshold = config.itemThreshold;
            var offset = config.itemSeedOffset;
            var alturaExtra = 0f;

            switch (tipoDeSpawn)
            {
                case TipoSpawn.Itens:
                    scale = config.itemNoiseScale;
                    threshold = config.itemThreshold;
                    offset = config.itemSeedOffset;
                    alturaExtra = 1.0f;
                    break;
                case TipoSpawn.Inimigos:
                    scale = config.enemyNoiseScale;
                    threshold = config.enemyThreshold;
                    offset = config.enemySeedOffset;
                    alturaExtra = 0.5f;
                    break;
                case TipoSpawn.Natureza:
                    scale = config.natureScale;
                    threshold = config.natureThreshold;
                    offset = config.natureOffset;
                    alturaExtra = -0.5f;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
          var spawnsCount = 0;
            for (float x = 0; x < mapWidth; x += gridSize)
            {
                for (float z = 0; z < mapLength; z += gridSize)
                {
                    var noiseVal = config.GetNoiseValue(x, z, scale, offset, mapWidth, mapLength);

                    if (!(noiseVal > threshold)) continue;
                    var worldX = x + terrainPos.x;
                    var worldZ = z + terrainPos.z;
                    var yTerreno = terrain.SampleHeight(new Vector3(worldX, 0, worldZ)) + terrainPos.y;
                    var posProvisoria = new Vector3(worldX, yTerreno, worldZ);
                    if (playerTransform && Vector3.Distance(posProvisoria, playerTransform.position) < 15f) continue;
                    var posicaoFinal = posProvisoria;
                    if (tipoDeSpawn == TipoSpawn.Inimigos)
                    {
                        UnityEngine.AI.NavMeshHit hit;
                        if (UnityEngine.AI.NavMesh.SamplePosition(posProvisoria, out hit, 5.0f, UnityEngine.AI.NavMesh.AllAreas))
                        {
                            posicaoFinal = hit.position;
                        }
                        else
                        {
                            continue; 
                        }
                    }
                    else
                    {
                        posicaoFinal.y += alturaExtra;
                    }

                    var prefab = prefabs[Random.Range(0, prefabs.Length)];
                    var pai = (containerOrganizador != null) ? containerOrganizador : transform;
                    var obj = Instantiate(prefab, posicaoFinal, Quaternion.identity, pai);
                    if (randomizarRotacao) 
                        obj.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
                    
                    if (randomizarEscala) 
                    {
                        float randomScale = Random.Range(0.8f, 1.5f);
                        obj.transform.localScale *= randomScale;
                    }
                    
                    spawnsCount++;
                }
            }
            Debug.Log($"[Spawner {tipoDeSpawn}] Gerou {spawnsCount} objetos com GridSize {gridSize}.");
        }
        
    }
}