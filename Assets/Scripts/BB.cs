using System.Collections;
using lifeStatus;
using UnityEngine;

public class Bb : MonoBehaviour
{
    [Header("Propriedades Físicas")]
    public float massa = 0.0002f;
    public float raio = 0.003f;
    
    [Header("Backspin/Magnus")]
    public float backspinDrag = 0.5f;
    
    private Rigidbody rb;
    private float dragCoefficient = 0.47f;
    private float airDensity = 1.225f;
    public bool eTiroDoInimigo = false;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.mass = massa;
        rb.useGravity = true;
        Debug.Log($"Velocidade inicial da BB: {rb.linearVelocity.magnitude} m/s");
        StartCoroutine(DestruirSeCaindo());
        Destroy(gameObject, 10f);
    }
    
    void FixedUpdate()
    {
        AplicarArrasto();
        AplicarEfeituMagnus();
    }
    
    void AplicarArrasto()
    {
        float area = Mathf.PI * raio * raio;
        float velocidade = rb.linearVelocity.magnitude;
        
        if (velocidade > 0.01f)
        {
            float forcaArrasto = 0.5f * dragCoefficient * airDensity * area * velocidade * velocidade;
            Vector3 direcaoArrasto = -rb.linearVelocity.normalized;
            rb.AddForce(direcaoArrasto * forcaArrasto);
        }
    }
    
    void AplicarEfeituMagnus()
    {
        float velocidade = rb.linearVelocity.magnitude;
        
        if (velocidade > 0.1f && backspinDrag > 0.01f)
        {
            float forcaMagnus = velocidade *  backspinDrag * 0.0001f;
            rb.AddForce(Vector3.up * forcaMagnus, ForceMode.Force);
        }
    }
    
    IEnumerator DestruirSeCaindo()
    {
        Vector3 posicaoInicial = transform.position;
        
        while (true)
        {
            float distancia = Vector3.Distance(posicaoInicial, transform.position);
            
            if (distancia > 1000f || rb.linearVelocity.magnitude < 5f)
            {
                Destroy(gameObject);
                yield break;
            }
            
            yield return new WaitForSeconds(0.5f);
        }
    }
    
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Inimigo") && !eTiroDoInimigo)
        {
            var inimigo = collision.gameObject.GetComponent<InimigoStatus>();
            if (!inimigo) inimigo.ReceberDano(1f);
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Player") && eTiroDoInimigo)
        {
            var player = collision.gameObject.GetComponent<PlayerStatus>();
            if (player == null) player = collision.gameObject.GetComponentInParent<PlayerStatus>();
            
            if (player != null) player.ReceberDano(1f);
            Destroy(gameObject);
        }
        else
        {
            // Bateu no chão ou parede
            Destroy(gameObject);
        }
    }
}
