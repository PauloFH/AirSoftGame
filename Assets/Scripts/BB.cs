using System.Collections;
using UnityEngine;

public class BB : MonoBehaviour
{
    [Header("Propriedades Físicas")]
    public float massa = 0.0002f;
    public float raio = 0.003f;
    
    [Header("Backspin/Magnus")]
    public float backspinDrag = 0.5f;
    
    private Rigidbody rb;
    private float dragCoefficient = 0.47f;
    private float airDensity = 1.225f;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.mass = massa;
        rb.useGravity = true;
        
        Debug.Log($"BB criada - Massa: {massa} kg, Gravity: {rb.useGravity}, Kinematic: {rb.isKinematic}");
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
            float escalaRealista = backspinDrag * 0.00009f;
            float forcaMagnus = velocidade * escalaRealista;
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
        rb.linearVelocity = Vector3.zero;
        rb.isKinematic = true;
        Destroy(gameObject, 2f);
    }
}
