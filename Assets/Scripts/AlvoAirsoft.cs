using UnityEngine;

public class AlvoAirsoft : MonoBehaviour
{
    [Header("Configurações do Alvo")]
    public int pontosPorAcerto = 10;
    public bool alvoMovel = false;
    
    [Header("Movimento (se alvoMovel = true)")]
    public float velocidadeMovimento = 2f;
    public float distanciaMovimento = 5f;
    public bool movimentoHorizontal = true; // true = X, false = Z
    
    [Header("Visual")]
    public Color corAcerto = Color.red;
    public float duracaoCorAcerto = 0.2f;
    
    private Vector3 posicaoInicial;
    private float direcao = 1f;
    private Renderer alvoRenderer;
    private Color corOriginal;
    
    void Start()
    {
        posicaoInicial = transform.position;
        alvoRenderer = GetComponent<Renderer>();
        
        if (alvoRenderer != null)
        {
            corOriginal = alvoRenderer.material.color;
        }
    }
    
    void Update()
    {
        if (alvoMovel && StandDeTiroManager.Instance != null && StandDeTiroManager.Instance.JogoAtivo)
        {
            MoverAlvo();
        }
    }
    
    void MoverAlvo()
    {
        Vector3 posicaoAtual = transform.position;
        
        if (movimentoHorizontal)
        {
            posicaoAtual.x += direcao * velocidadeMovimento * Time.deltaTime;
            
            if (Mathf.Abs(posicaoAtual.x - posicaoInicial.x) >= distanciaMovimento)
            {
                direcao *= -1f;
            }
        }
        else
        {
            posicaoAtual.z += direcao * velocidadeMovimento * Time.deltaTime;
            
            if (Mathf.Abs(posicaoAtual.z - posicaoInicial.z) >= distanciaMovimento)
            {
                direcao *= -1f;
            }
        }
        
        transform.position = posicaoAtual;
    }
    
    public void RegistrarAcerto()
    {
        if (StandDeTiroManager.Instance != null)
        {
            StandDeTiroManager.Instance.AdicionarPontos(pontosPorAcerto);
            Debug.Log($"ACERTO! +{pontosPorAcerto} pontos");
            
            if (alvoRenderer != null)
            {
                StartCoroutine(EfeitoAcerto());
            }
        }
    }
    
    System.Collections.IEnumerator EfeitoAcerto()
    {
        alvoRenderer.material.color = corAcerto;
        yield return new WaitForSeconds(duracaoCorAcerto);
        alvoRenderer.material.color = corOriginal;
    }
    
    public void ResetarPosicao()
    {
        transform.position = posicaoInicial;
        direcao = 1f;
    }
}
