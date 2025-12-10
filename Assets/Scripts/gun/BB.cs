using System.Collections;
using lifeStatus;
using UnityEngine;

namespace gun
{
    public class Bb : MonoBehaviour
    {
        [Header("Propriedades Físicas")]
        public float massa = 0.0002f;
        public float raio = 0.003f;
    
        [Header("Backspin/Magnus")]
        public float backspinDrag = 0.5f;
    
        private Rigidbody _rb;
        private const float DragCoefficient = 0.47f;
        private const float AirDensity = 1.225f;

        public bool eTiroDoInimigo;

        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
            _rb.mass = massa;
            _rb.useGravity = true;
            _rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
            if (Physics.Raycast(transform.position, transform.forward, out var hit, 1.0f))
            {
                TentarAplicarDano(hit.collider.gameObject);
            }

            StartCoroutine(DestruirSeCaindo());
            Destroy(gameObject, 10f);
        }

        private void FixedUpdate()
        {
            if (!this) return;
            AplicarArrasto();
            AplicarEfeituMagnus();
        }
        
        void TentarAplicarDano(GameObject alvo)
        {
            if (alvo.CompareTag("Inimigo") && !eTiroDoInimigo)
            {
                var inimigo = alvo.GetComponentInParent<InimigoStatus>();

                if (!inimigo) return;
                inimigo.ReceberDano(1f);
                Debug.Log("Acertou Inimigo (Raycast/Colisão)");
                Destroy(gameObject);
            }
            else if (alvo.CompareTag("Player") && eTiroDoInimigo)
            {
                var player = alvo.GetComponentInParent<PlayerStatus>();

                if (player == null) return;
                player.ReceberDano(1f);
                Debug.Log("Acertou Player (Raycast/Colisão)");
                Destroy(gameObject);
            }
            else
            {
                var bateuNoDono = (eTiroDoInimigo && alvo.CompareTag("Inimigo")) || (!eTiroDoInimigo && alvo.CompareTag("Player"));
            
                if (!bateuNoDono && !alvo.CompareTag("Arma") && !alvo.CompareTag("Projetil"))
                {
                    Destroy(gameObject);
                }
            }
        }

        void OnCollisionEnter(Collision collision)
        {
            TentarAplicarDano(collision.gameObject);
        }

        private void AplicarArrasto()
        {
            if(!_rb) return;
            var area = Mathf.PI * raio * raio;
            var velocidade = _rb.linearVelocity.magnitude;

            if (!(velocidade > 0.01f)) return;
            var forcaArrasto = 0.5f * DragCoefficient * AirDensity * area * velocidade * velocidade;
            var direcaoArrasto = -_rb.linearVelocity.normalized;
            _rb.AddForce(direcaoArrasto * forcaArrasto);
        }

        private void AplicarEfeituMagnus()
        {
            if(!_rb) return;
            var velocidade = _rb.linearVelocity.magnitude;

            if (!(velocidade > 0.1f) || !(backspinDrag > 0.01f)) return;
            var forcaMagnus = velocidade * backspinDrag * 0.0001f;
            _rb.AddForce(Vector3.up * forcaMagnus, ForceMode.Force);
        }

        private IEnumerator DestruirSeCaindo()
        {
            var posicaoInicial = transform.position;
        
            while (true)
            {
                if (!this) yield break;

                var distancia = Vector3.Distance(posicaoInicial, transform.position);
            
                if (distancia > 1000f || (_rb && _rb.linearVelocity.magnitude < 2f))
                {
                    Destroy(gameObject);
                    yield break;
                }
            
                yield return new WaitForSeconds(0.5f);
            }
        }
    }
}