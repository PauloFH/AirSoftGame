using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public Text textoMunicao;
    public Text textoHopUp;
    public ArmaAirsoft arma;
    
    void Update()
    {
        if (arma.carregadorAtual != null)
        {
            float massaGramas = arma.carregadorAtual.massaBB * 1000;
            textoMunicao.text = $"Munição: {arma.carregadorAtual.quantidadeAtual}/{arma.carregadorAtual.capacidade}\nMassa: {massaGramas:F2}g";
        }
        
        textoHopUp.text = $"Hop-up: {arma.hopUpValue:F2}";
    }
}   