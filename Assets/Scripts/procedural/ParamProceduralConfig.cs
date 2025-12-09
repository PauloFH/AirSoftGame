using UnityEngine;

namespace procedural
{
    [CreateAssetMenu(fileName = "ConfigMapa", menuName = "Procedural/Configuração de Mapa")]
    public class ParamProceduralConfig : ScriptableObject
    {
        [Header("Geral")]
        public int seed = 42;
        public bool usarSeedFixa = true;

        [Header("Dimensões do Mapa")]
        public int width = 256;
        public int height = 256;
        public int depth = 6;

        [Header("Relevo (Montanhas)")]
        public float terrainScale = 9f;
        public Vector2 terrainOffset = new Vector2(100f, 100f);

        [Header("Itens (Zona Marrom)")]
        public float itemNoiseScale = 5f;
        [Range(0f, 1f)] public float itemThreshold = 0.6f;
        public float itemSeedOffset = 500f;

        [Header("Inimigos (Zona Preta)")]
        public float enemyNoiseScale = 8f;
        [Range(0f, 1f)] public float enemyThreshold = 0.55f;
        public float enemySeedOffset = 9999f;
    
        [Header("Natureza (Props Grandes)")]
        public float natureScale = 15f;
        public float natureOffset = 300f;
        [Range(0f, 1f)] public float natureThreshold = 0.45f;

        [Header("Detalhes (Grama)")]
        public float grassDensity = 0.5f;

        [Range(0f, 1f)] public float grassThreshold = 0.3f;
        public float GetNoiseValue(float x, float z, float scale, float offsetExtra, float mapWidth, float mapLength)
        {
            var seedOffsetX = 0f;
            var seedOffsetY = 0f;

            if (usarSeedFixa)
            {
                System.Random prng = new System.Random(seed);
                seedOffsetX = prng.Next(-100000, 100000);
                seedOffsetY = prng.Next(-100000, 100000);
            }

            var xCoord = x / mapWidth * scale + offsetExtra + seedOffsetX;
            var zCoord = z / mapLength * scale + offsetExtra + seedOffsetY;

            return Mathf.PerlinNoise(xCoord, zCoord);
        }
    }
}