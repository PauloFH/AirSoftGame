using UnityEngine;
using UnityEngine.AI;

namespace AI
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class InimigoAI : MonoBehaviour
    {
        [Header("Alvo")]
        public Transform player;
    
        [Header("Configurações de Combate")]
        public float distanciaDeteccao = 20f;
        public float distanciaAtaque = 10f;
        public float tempoEntreTiros = 1.5f;
        private float _proximoTiro;

        [Header("Arma do Inimigo")]
        public GameObject bbPrefab;
        public Transform bocaDoCano;
        public float energiaDisparo = 1.49f;
        public float massaBb = 0.0002f; 
        private NavMeshAgent _agent;
        private bool _playerNaMira = false;

        private void Start()
        {
            _agent = GetComponent<NavMeshAgent>();
            if (player != null) return;
            var p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }

        private void Update()
        {
            if (!player) return;
            var distancia = Vector3.Distance(transform.position, player.position);
            if (!(distancia <= distanciaDeteccao)) return;
            if (distancia > distanciaAtaque)
            {
                _agent.isStopped = false;
                _agent.SetDestination(player.position);
            }
            else
            {
                _agent.isStopped = true;
                MirarNoPlayer();

                if (!(Time.time >= _proximoTiro)) return;
                Atirar();
                _proximoTiro = Time.time + tempoEntreTiros;
            }
        }

        private void MirarNoPlayer()
        {
            var direcao = (player.position - transform.position).normalized;
            var lookRotation = Quaternion.LookRotation(new Vector3(direcao.x, 0, direcao.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
        private void Atirar()
        {
            if (!bbPrefab || !bocaDoCano) return;
            var bbObj = Instantiate(bbPrefab, bocaDoCano.position, bocaDoCano.rotation);
            var bbScript = bbObj.GetComponent<Bb>();
            if (bbScript)
            {
                bbScript.massa = massaBb;
                bbScript.eTiroDoInimigo = true;
            }
            
            var velocidadeInicial = Mathf.Sqrt(2f * energiaDisparo / massaBb);
        
            var rb = bbObj.GetComponent<Rigidbody>();
            if (!rb) return;
            var direcaoComErro = bocaDoCano.forward + (Random.insideUnitSphere * 0.02f);
            rb.linearVelocity = direcaoComErro.normalized * velocidadeInicial;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, distanciaDeteccao);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, distanciaAtaque);
        }
    }
}