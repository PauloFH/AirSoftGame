using UnityEngine;

public class Alvo : MonoBehaviour
{
    [HideInInspector]
    public TiroAoAlvoManager manager;
    
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Projetil"))
        {

            if (manager != null)
            {
                manager.AdicionarPonto();
            }
            
            Destroy(gameObject);
        }
    }
}