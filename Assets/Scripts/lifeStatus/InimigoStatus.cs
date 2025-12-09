using UnityEngine;

namespace lifeStatus
{
    public class InimigoStatus : MonoBehaviour
    {
        public float vidaMaxima = 3f;
        private float vidaAtual;

        void Start()
        {
            vidaAtual = vidaMaxima;
        }

        public void ReceberDano(float dano)
        {
            vidaAtual -= dano;
            Debug.Log($"Inimigo atingido! Vida: {vidaAtual}");

            if (vidaAtual <= 0)
            {
                Morrer();
            }
        }

        void Morrer()
        {
            // Aqui depois colocaremos a animação de morte
            Destroy(gameObject);
        }
    }
}