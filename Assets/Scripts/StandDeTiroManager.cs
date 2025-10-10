using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandDeTiroManager : MonoBehaviour
{
    public static StandDeTiroManager Instance { get; private set; }
    
    [Header("Configurações do Jogo")]
    public float tempoTotalSegundos = 60f;
    public List<AlvoAirsoft> alvos = new List<AlvoAirsoft>();
    
    [Header("Referências")]
    public FPSController playerController;
    public ArmaAirsoft armaPlayer;
    public UIStandDeTiro uiStand;
    
    // Estado do jogo
    private bool jogoAtivo = false;
    private float tempoRestante;
    private int pontuacaoAtual = 0;
    private int acertosTotal = 0;
    
    public bool JogoAtivo => jogoAtivo;
    public float TempoRestante => tempoRestante;
    public int PontuacaoAtual => pontuacaoAtual;
    public int AcertosTotal => acertosTotal;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        foreach (var alvo in alvos)
        {
            if (alvo != null)
            {
                alvo.gameObject.SetActive(false);
            }
        }
    }
    
    void Update()
    {
        if (jogoAtivo)
        {
            AtualizarTimer();
            
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                FinalizarJogo(true);
            }
        }
    }
    
    void AtualizarTimer()
    {
        tempoRestante -= Time.deltaTime;
        
        if (uiStand != null)
        {
            uiStand.AtualizarTimer(tempoRestante);
        }
        
        if (tempoRestante <= 0)
        {
            FinalizarJogo(false);
        }
    }
    
    public void IniciarJogo()
    {
        Debug.Log("🎮 JOGO INICIADO!");
        
        jogoAtivo = true;
        tempoRestante = tempoTotalSegundos;
        pontuacaoAtual = 0;
        acertosTotal = 0;
        
        if (playerController != null)
        {
            playerController.TravarMovimento(true);
        }

        if (armaPlayer != null)
        {
            armaPlayer.PermitirAtirar(true);
        }

        foreach (var alvo in alvos)
        {
            if (alvo != null)
            {
                alvo.gameObject.SetActive(true);
                alvo.ResetarPosicao();
            }
        }
        

        if (uiStand != null)
        {
            uiStand.MostrarJogo(true);
        }
    }
    
    public void FinalizarJogo(bool cancelado)
    {

        
        jogoAtivo = false;
        

        if (playerController != null)
        {
            playerController.TravarMovimento(false);
        }
        
        if (armaPlayer != null)
        {
            armaPlayer.PermitirAtirar(false);
        }

        foreach (var alvo in alvos)
        {
            if (alvo != null)
            {
                alvo.gameObject.SetActive(false);
            }
        }

        if (uiStand != null)
        {
            uiStand.MostrarResultado(pontuacaoAtual, acertosTotal);
        }
    }
    
    public void AdicionarPontos(int pontos)
    {
        pontuacaoAtual += pontos;
        acertosTotal++;
        
        if (uiStand != null)
        {
            uiStand.AtualizarPontuacao(pontuacaoAtual, acertosTotal);
        }
    }
}
