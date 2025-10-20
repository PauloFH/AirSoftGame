using UnityEngine;
using UnityEngine.UI;

public class UIStandDeTiro : MonoBehaviour
{
    [Header("UI Prompt")]
    public GameObject panelPrompt;
    public Text textoPrompt;
    
    [Header("UI Jogo")]
    public GameObject panelJogo;
    public Text textoTimer;
    public Text textoPontuacao;
    public Text textoAcertos;
    
    [Header("UI Resultado")]
    public GameObject panelResultado;
    public Text textoResultadoPontos;
    public Text textoResultadoAcertos;
    public Button botaoFecharResultado;
    
    void Start()
    {
        MostrarPrompt(false);
        MostrarJogo(false);
        MostrarResultadoPanel(false);
        
        if (botaoFecharResultado != null)
        {
            botaoFecharResultado.onClick.AddListener(() => MostrarResultadoPanel(false));
        }
        
        if (textoPrompt != null)
        {
            textoPrompt.text = "Pressione [E] para iniciar";
        }
    }
    
    public void MostrarPrompt(bool mostrar)
    {
        if (panelPrompt != null)
        {
            panelPrompt.SetActive(mostrar);
        }
    }
    
    public void MostrarJogo(bool mostrar)
    {
        if (panelJogo != null)
        {
            panelJogo.SetActive(mostrar);
        }
        
        MostrarPrompt(false);
    }
    
    public void AtualizarTimer(float tempoRestante)
    {
        if (textoTimer != null)
        {
            int minutos = Mathf.FloorToInt(tempoRestante / 60f);
            int segundos = Mathf.FloorToInt(tempoRestante % 60f);
            textoTimer.text = $"Tempo: {minutos:00}:{segundos:00}";
        }
    }
    
    public void AtualizarPontuacao(int pontos, int acertos)
    {
        if (textoPontuacao != null)
        {
            textoPontuacao.text = $"Pontos: {pontos}";
        }
        
        if (textoAcertos != null)
        {
            textoAcertos.text = $"Acertos: {acertos}";
        }
    }
    
    public void MostrarResultado(int pontosFinal, int acertosFinal)
    {
        MostrarJogo(false);
        MostrarResultadoPanel(true);
        
        if (textoResultadoPontos != null)
        {
            textoResultadoPontos.text = $"Pontuação Final: {pontosFinal}";
        }
        
        if (textoResultadoAcertos != null)
        {
            textoResultadoAcertos.text = $"Total de Acertos: {acertosFinal}";
        }
    }
    
    void MostrarResultadoPanel(bool mostrar)
    {
        if (panelResultado != null)
        {
            panelResultado.SetActive(mostrar);
        }
    }
}
