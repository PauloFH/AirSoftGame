using UnityEngine;

[System.Serializable]
public class Carregador
{
    public TipoCarregador tipo;
    public int capacidade;
    public int quantidadeAtual;
    public float massaBB;//(0.0002 = 0.2g)
    
    public Carregador(TipoCarregador tipo, int capacidade, float massaBB)
    {
        this.tipo = tipo;
        this.capacidade = capacidade;
        this.quantidadeAtual = capacidade;
        this.massaBB = massaBB;
    }
}