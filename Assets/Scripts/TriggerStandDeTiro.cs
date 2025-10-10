using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class TriggerStandDeTiro : MonoBehaviour
{
    public UIStandDeTiro uiStand;
    private bool playerDentro = false;
    
    void Start()
    {
        GetComponent<BoxCollider>().isTrigger = true;
    }
    
    void Update()
    {
        if (playerDentro && Input.GetKeyDown(KeyCode.E))
        {
            if (StandDeTiroManager.Instance != null && !StandDeTiroManager.Instance.JogoAtivo)
            {
                StandDeTiroManager.Instance.IniciarJogo();
            }
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerDentro = true;
            
            if (uiStand != null)
            {
                uiStand.MostrarPrompt(true);
            }
        }
    }
    
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerDentro = false;
            
            if (uiStand != null)
            {
                uiStand.MostrarPrompt(false);
            }
        }
    }
}