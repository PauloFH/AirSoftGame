using UnityEngine;

public class CarregadorPickup : MonoBehaviour
{
    [Header("Configurações do Carregador")]
    public TipoCarregador tipo;
    public int capacidade = 30;
    public float massaBB = 0.0002f; // 0.2g
    
    [Header("Visual")]
    public float velocidadeRotacao = 50f;
    public float amplitudeFlutuar = 0.3f;
    public float velocidadeFlutuar = 2f;
    
    [Header("Interação")]
    public float raioDeteccao = 3f;
    public LayerMask playerLayer;
    
    private Vector3 posicaoInicial;
    private bool foiPego = false;
    private Transform playerProximo = null;
    
    void Start()
    {
        posicaoInicial = transform.position;
        
        SphereCollider trigger = GetComponent<SphereCollider>();
        if (trigger == null)
        {
            trigger = gameObject.AddComponent<SphereCollider>();
        }
        trigger.isTrigger = true;
        trigger.radius = raioDeteccao;
    }
    
    void Update()
    {
        if (!foiPego)
        {
            AnimarCarregador();
        }
    }
    
    void AnimarCarregador()
    {
        transform.Rotate(Vector3.up, velocidadeRotacao * Time.deltaTime);
        
        float novaY = posicaoInicial.y + Mathf.Sin(Time.time * velocidadeFlutuar) * amplitudeFlutuar;
        transform.position = new Vector3(posicaoInicial.x, novaY, posicaoInicial.z);
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !foiPego)
        {
            playerProximo = other.transform;
            ArmaAirsoft arma = other.GetComponentInChildren<ArmaAirsoft>();
            
            if (arma != null)
            {
                TentarPegarCarregador(arma);
            }
        }
    }
    
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerProximo = null;
            
            UICarregadorPrompt ui = FindObjectOfType<UICarregadorPrompt>();
            if (ui != null)
            {
                ui.EsconderPrompt();
            }
        }
    }
    
    void TentarPegarCarregador(ArmaAirsoft arma)
    {

        if (arma.tipoCarregador == tipo)
        {
            RecarregarAutomatico(arma);
        }
        else
        {
            MostrarPromptTroca(arma);
        }
    }
    
    void RecarregarAutomatico(ArmaAirsoft arma)
    {
        if (arma.carregadorAtual != null && arma.carregadorAtual.quantidadeAtual < arma.carregadorAtual.capacidade)
        {
            int bbsFaltando = arma.carregadorAtual.capacidade - arma.carregadorAtual.quantidadeAtual;
            int bbsNovo = Mathf.Min(capacidade, bbsFaltando);
            
            arma.carregadorAtual.quantidadeAtual += bbsNovo;
            capacidade -= bbsNovo;
            
            Debug.Log($"Carregador recarregado! +{bbsNovo} BBs");
            
            if (capacidade <= 0)
            {
                foiPego = true;
                Destroy(gameObject, 0.5f);
            }
        }
    }
    
    void MostrarPromptTroca(ArmaAirsoft arma)
    {
        UICarregadorPrompt ui = FindObjectOfType<UICarregadorPrompt>();
        if (ui != null)
        {
            ui.MostrarPrompt(this, arma);
        }
        else
        {
            Debug.LogWarning("UICarregadorPrompt não encontrado na cena!");
        }
    }
    
    public void TrocarCarregador(ArmaAirsoft arma)
    {
        Carregador novoCarregador = new Carregador(tipo, capacidade, massaBB);
        arma.carregadorAtual = novoCarregador;
        arma.tipoCarregador = tipo;
        
        Debug.Log($"Carregador trocado! Novo tipo: {tipo}, Massa: {massaBB * 1000}g");
        
        foiPego = true;
        Destroy(gameObject);
    }
    
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, raioDeteccao);
    }
}
