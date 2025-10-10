using System.Collections;
using UnityEngine;

public class ArmaAirsoft : MonoBehaviour
{
    [Header("Configurações da Arma")]
    public string modeloArma = "Rifle";
    public TipoCarregador tipoCarregador = TipoCarregador.Rifle;
    
    [Header("Disparo")]
    public Transform bocaDoCano;
    public GameObject bbPrefab;
    public float energiaDisparo = 1.49f; 
    
    [Header("Motor e ROF")]
    public float motorRPM = 30000f;
    public float rateOfFire = 4.5f;
    private float proximoDisparo = 0f;
    
    [Header("Carregador")]
    public Carregador carregadorAtual;
    public int municaoReserva = 100;
    public float tempoRecarga = 2.5f;
    private bool estaRecarregando = false;
    public bool fullAuto = false;
    
    [Header("Hop-up")]
    public float hopUpValue = 0.3f;
    
    [Header("Sistema de Áudio")]
    public AudioSource audioSource;
    public AudioClip somDisparo;
    public AudioClip somCarregadorVazio;
    public AudioClip somRetirarCarregador;
    public AudioClip somInserirCarregador;
    public AudioClip somArmar;
    public AudioClip somModoDisparo;
    
    [Range(0f, 1f)]
    public float volumeDisparo = 0.8f;
    [Range(0f, 1f)]
    public float volumeRecarga = 0.6f;
    [Range(0f, 1f)]
    public float volumeVazio = 0.5f;
    
    void Start()
    {
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.spatialBlend = 1f;
            audioSource.minDistance = 5f;
            audioSource.maxDistance = 50f;
        }
    }
    
    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0f)
        {
            hopUpValue = Mathf.Clamp01(hopUpValue + scroll);
            Debug.Log($"Hop-up ajustado para: {hopUpValue:F2}");
        }
        
        if (Input.GetKeyDown(KeyCode.F))
        {
            fullAuto = !fullAuto;
            Debug.Log($"Full-Auto: {(fullAuto ? "ATIVADO" : "DESATIVADO")}");
            TocarSom(somModoDisparo, volumeRecarga);
        }
        
        if (!estaRecarregando)
        {
            if (fullAuto && Input.GetMouseButton(0))
            {
                if (Time.time >= proximoDisparo)
                {
                    Disparar();
                    proximoDisparo = Time.time + (1f / rateOfFire);
                }
            }
            else if (Input.GetMouseButtonDown(0))
            {
                Disparar();
            }
        }

        if (Input.GetKeyDown(KeyCode.R) && !estaRecarregando)
        {
            IniciarRecarga();
        }
    }
    
    void Disparar()
    {
        if (carregadorAtual == null || carregadorAtual.quantidadeAtual <= 0)
        {
            Debug.Log("Sem munição! Aperte R para recarregar.");
            TocarSom(somCarregadorVazio, volumeVazio);
            return;
        }

        GameObject bbObj = Instantiate(bbPrefab, bocaDoCano.position, bocaDoCano.rotation);
        BB bb = bbObj.GetComponent<BB>();
        
        bb.massa = carregadorAtual.massaBB;
        bb.backspinDrag = hopUpValue;

        // E = 0.5 * m * v²  =>  v = sqrt(2*E/m)
        float velocidadeInicial = Mathf.Sqrt(2f * energiaDisparo / bb.massa);
        
        Debug.Log($"Velocidade inicial: {velocidadeInicial:F2} m/s ({velocidadeInicial * 3.28084f:F0} fps)");
        
        Rigidbody rbBB = bbObj.GetComponent<Rigidbody>();
        rbBB.linearVelocity = bocaDoCano.forward * velocidadeInicial;

        carregadorAtual.quantidadeAtual--;
        
        TocarSom(somDisparo, volumeDisparo);
    }
    
    void IniciarRecarga()
    {
        if (carregadorAtual == null)
        {
            Debug.Log("Nenhum carregador equipado!");
            return;
        }
        
        if (carregadorAtual.quantidadeAtual >= carregadorAtual.capacidade)
        {
            Debug.Log("Carregador já está cheio!");
            return;
        }
        
        if (municaoReserva <= 0)
        {
            Debug.Log("Sem munição reserva!");
            TocarSom(somCarregadorVazio, volumeVazio);
            return;
        }
        
        StartCoroutine(Recarregar());
    }
    
    IEnumerator Recarregar()
    {
        estaRecarregando = true;
        Debug.Log("Recarregando...");
        
        TocarSom(somRetirarCarregador, volumeRecarga);
        
        yield return new WaitForSeconds(tempoRecarga * 0.4f);
        
        TocarSom(somInserirCarregador, volumeRecarga);
        
        yield return new WaitForSeconds(tempoRecarga * 0.4f);
        
        TocarSom(somArmar, volumeRecarga);
        
        yield return new WaitForSeconds(tempoRecarga * 0.2f);
        
        int bbsFaltando = carregadorAtual.capacidade - carregadorAtual.quantidadeAtual;
        int bbsParaAdicionar = Mathf.Min(bbsFaltando, municaoReserva);
        carregadorAtual.quantidadeAtual += bbsParaAdicionar;
        municaoReserva -= bbsParaAdicionar;
        
        estaRecarregando = false;
        
        Debug.Log($"Recarga completa! Carregador: {carregadorAtual.quantidadeAtual}/{carregadorAtual.capacidade} | Reserva: {municaoReserva}");
    }
    
    void TocarSom(AudioClip clip, float volume)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip, volume);
        }
    }
    public void EquiparNovoCarregador(Carregador novoCarregador, TipoCarregador novoTipo)
    {
        carregadorAtual = novoCarregador;
        tipoCarregador = novoTipo;
        Debug.Log($"Novo carregador equipado: {novoTipo} - {novoCarregador.capacidade} BBs de {novoCarregador.massaBB * 1000}g");
    }
    public bool EstaRecarregando()
    {
        return estaRecarregando;
    }
    
    public int GetMunicaoReserva()
    {
        return municaoReserva;
    }
    
}

public enum TipoCarregador
{
    Pistola1911,
    PistolaGlock,
    Rifle,
    Shotgun
}
