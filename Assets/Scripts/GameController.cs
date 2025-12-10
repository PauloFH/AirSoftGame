using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    [Header("Configurações do Jogo")]
    public string nomeCenaVitoria = "Vitoria";
    public string nomeCenaDerrota = "GameOver";
    public int pontosPorInimigo = 100;

    [Header("Estado Atual (Apenas Leitura)")]
    public int pontuacao = 0;
    public int inimigosTotal = 0;
    public int inimigosMortos = 0;
    
    [Header("UI (Opcional)")]
    public Text textoPontos;
    public Text textoInimigos;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Update()
    {
        if (textoPontos) textoPontos.text = $"Pontos: {pontuacao}";
        if (textoInimigos) textoInimigos.text = $"Inimigos: {inimigosMortos}/{inimigosTotal}";
    }


    public void RegistrarTotalInimigos(int quantidade)
    {
        inimigosTotal = quantidade;
        Debug.Log($"Jogo Iniciado! Total de Inimigos: {inimigosTotal}");
    }
    public void InimigoMorreu()
    {
        inimigosMortos++;
        pontuacao += pontosPorInimigo;

        VerificarVitoria();
    }
    public void PlayerMorreu()
    {
        Debug.Log("Game Over!");
        SceneManager.LoadScene(nomeCenaDerrota);
    }

    private void VerificarVitoria()
    {
        if (inimigosMortos < inimigosTotal || inimigosTotal <= 0) return;
        Debug.Log("VITORIA!");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
            
        SceneManager.LoadScene(nomeCenaVitoria);
    }
}