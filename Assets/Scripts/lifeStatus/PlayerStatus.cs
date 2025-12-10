using UnityEngine;

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
            Debug.Log($"Vida do Player: {vidaAtual}");

            if (!(vidaAtual <= 0)) return;
            if (GameController.instance != null)
            {
                GameController.instance.PlayerMorreu();
            }
            else
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("GameOver");
            }
        }
    }
}