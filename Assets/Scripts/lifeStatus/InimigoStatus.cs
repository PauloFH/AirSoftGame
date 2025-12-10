using UnityEngine;

namespace lifeStatus
{
    public class InimigoStatus : MonoBehaviour
    {
        public float vidaMaxima = 3f;
        private float vidaAtual;
        private void Start()
        {
            vidaAtual = vidaMaxima;
        }
        public void ReceberDano(float dano)
        {
            vidaAtual -= dano;
            if (vidaAtual <= 0)
            {
                Morrer();
            }
        }
        private void Morrer()
        {
            if (GameController.instance != null)
            {
                GameController.instance.InimigoMorreu();
            }
            Destroy(gameObject);
        }
    }
}