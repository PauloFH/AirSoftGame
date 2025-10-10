using UnityEngine;
using UnityEngine.UI;

public class UICarregadorPrompt : MonoBehaviour
{
    [Header("Referências UI")]
    public GameObject painelPrompt;
    public Text textoTipo;
    public Text textoCapacidade;
    public Text textoMassa;
    public Text textoInstrucao;
    
    private CarregadorPickup carregadorAtual;
    private ArmaAirsoft armaAtual;
    
    void Start()
    {
        if (painelPrompt != null)
        {
            painelPrompt.SetActive(false);
        }
    }
    
    void Update()
    {
        if (painelPrompt != null && painelPrompt.activeSelf)
        {
            // Pressiona E para trocar
            if (Input.GetKeyDown(KeyCode.E))
            {
                AceitarTroca();
            }
            
            // Pressiona ESC ou Q para cancelar
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Q))
            {
                RecusarTroca();
            }
        }
    }
    
    public void MostrarPrompt(CarregadorPickup carregador, ArmaAirsoft arma)
    {
        carregadorAtual = carregador;
        armaAtual = arma;
        
        if (painelPrompt != null)
        {
            painelPrompt.SetActive(true);
            
            // Atualiza as informações
            if (textoTipo != null)
            {
                textoTipo.text = $"Tipo: {carregador.tipo}";
            }
            
            if (textoCapacidade != null)
            {
                textoCapacidade.text = $"Capacidade: {carregador.capacidade} BBs";
            }
            
            if (textoMassa != null)
            {
                float massaGramas = carregador.massaBB * 1000;
                textoMassa.text = $"Massa: {massaGramas:F2}g";
            }
            
            if (textoInstrucao != null)
            {
                textoInstrucao.text = "Pressione [E] para trocar ou [Q] para cancelar";
            }
        }
    }
    
    public void EsconderPrompt()
    {
        if (painelPrompt != null)
        {
            painelPrompt.SetActive(false);
        }
        
        carregadorAtual = null;
        armaAtual = null;
    }
    
    void AceitarTroca()
    {
        if (carregadorAtual != null && armaAtual != null)
        {
            carregadorAtual.TrocarCarregador(armaAtual);
            EsconderPrompt();
        }
    }
    
    void RecusarTroca()
    {
        Debug.Log("Troca de carregador cancelada");
        EsconderPrompt();
    }
    
    // Métodos públicos para usar com botões UI (opcional)
    public void BotaoAceitar()
    {
        AceitarTroca();
    }
    
    public void BotaoRecusar()
    {
        RecusarTroca();
    }
}
