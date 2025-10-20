using UnityEngine;

public class CarregadorPickup : MonoBehaviour {
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
    public float cooldownPegar = 3f;
    public LayerMask playerLayer;
    
    private Vector3 posicaoInicial;
    private Transform playerProximo = null;
    private float proximoTempoDisponivel = 0f;
    private bool podeInteragir = true;
    
    [Header("Feedback Visual")]
    public Material materialDisponivel;
    public Material materialCooldown;
    private Renderer meshRenderer;
    
    void Start() {
        posicaoInicial = transform.position;
        meshRenderer = GetComponentInChildren<Renderer>();
        SphereCollider trigger = GetComponent<SphereCollider>();
        if (trigger == null) {
            trigger = gameObject.AddComponent<SphereCollider>();
        }
        trigger.isTrigger = true;
        trigger.radius = raioDeteccao;
    }

    void Update() {
        AnimarCarregador();
        if (meshRenderer != null) {
            meshRenderer.material = podeInteragir ? materialDisponivel : materialCooldown;
        }
    }
    void AnimarCarregador() {
        transform.Rotate(Vector3.up, velocidadeRotacao * Time.deltaTime);
        float novaY = posicaoInicial.y + Mathf.Sin(Time.time * velocidadeFlutuar) * amplitudeFlutuar;
        transform.position = new Vector3(posicaoInicial.x, novaY, posicaoInicial.z);
    }
    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player") && podeInteragir) {
            playerProximo = other.transform;
            ArmaAirsoft arma = other.GetComponentInChildren<ArmaAirsoft>();

            if (arma != null) {
                TentarPegarCarregador(arma);
            }
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            playerProximo = null;

            UICarregadorPrompt ui = FindFirstObjectByType<UICarregadorPrompt>();
            if (ui != null) {
                ui.EsconderPrompt();
            }
        }
    }

    void TentarPegarCarregador(ArmaAirsoft arma) {
        if (arma.tipoCarregador == tipo) {
            RecarregarAutomatico(arma);
        }
        else {
            MostrarPromptTroca(arma);
        }
    }

    void RecarregarAutomatico(ArmaAirsoft arma) {
        int municaoRecebida = 100;
        arma.municaoReserva += municaoRecebida;

        Debug.Log($"Munição recarregada! +{municaoRecebida} BBs na reserva. Total: {arma.municaoReserva}");

        podeInteragir = false;
        proximoTempoDisponivel = Time.time + cooldownPegar;

        StartCoroutine(EfeitoRecarregar());
    }

    void MostrarPromptTroca(ArmaAirsoft arma) {
        UICarregadorPrompt ui = FindFirstObjectByType<UICarregadorPrompt>();
        if (ui != null) {
            ui.MostrarPrompt(this, arma);
        }
        else {
            Debug.LogWarning("UICarregadorPrompt não encontrado na cena!");
        }
    }

    public void TrocarCarregador(ArmaAirsoft arma) {
        Carregador novoCarregador = new Carregador(tipo, capacidade, massaBB);
        arma.carregadorAtual = novoCarregador;
        arma.tipoCarregador = tipo;
        arma.TrocarMeshArma(tipo);
        arma.municaoReserva = 100;
        podeInteragir = false;
        proximoTempoDisponivel = Time.time + cooldownPegar;
        StartCoroutine(EfeitoRecarregar());
    }

    System.Collections.IEnumerator EfeitoRecarregar() {
        Vector3 escalaOriginal = transform.localScale;
        float duracao = 0.3f;
        float tempo = 0f;
        while (tempo < duracao) {
            tempo += Time.deltaTime;
            float fator = 1f + Mathf.Sin(tempo / duracao * Mathf.PI) * 0.2f;
            transform.localScale = escalaOriginal * fator;
            yield return null;
        }
        transform.localScale = escalaOriginal;
        yield return new WaitForSeconds(cooldownPegar);
        podeInteragir = true;
    }
    void OnDrawGizmosSelected() {
        Gizmos.color = podeInteragir ? Color.yellow : Color.red;
        Gizmos.DrawWireSphere(transform.position, raioDeteccao);
    }
}