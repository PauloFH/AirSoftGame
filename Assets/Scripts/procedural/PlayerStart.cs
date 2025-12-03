using UnityEngine;

namespace procedural
{
    public class PlayerStart : MonoBehaviour
    {
        public Terrain terrain;
        public float alturaExtra = 2f;

        private void Start()
        {
            Invoke(nameof(PosicionarPlayer), 0.1f);
        }

        private void PosicionarPlayer()
        {
            if (terrain == null) return;

            var posAtual = transform.position;
            var alturaTerreno = terrain.SampleHeight(posAtual) + terrain.transform.position.y;
            var novaPosicao = new Vector3(posAtual.x, alturaTerreno + alturaExtra, posAtual.z);
            var cc = GetComponent<CharacterController>();
            if (cc != null) cc.enabled = false;
        
            transform.position = novaPosicao;
        
            if (cc != null) cc.enabled = true;
        }
    }
}