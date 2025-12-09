using UnityEngine;
using UnityEngine.SceneManagement;


namespace lifeStatus
{
    public class PlayerStatus : MonoBehaviour
    {
        public float vidaMaxima = 10f;
        private float vidaAtual;

        private void Start()
        {
            vidaAtual = vidaMaxima;
        }

        public void ReceberDano(float dano)
        {
            vidaAtual -= dano;
            Debug.Log($"PLAYER ATINGIDO! Vida restante: {vidaAtual}");

            if (vidaAtual <= 0)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }
}