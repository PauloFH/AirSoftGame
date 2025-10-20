using UnityEngine;

public class AlvoSimple : MonoBehaviour
{
    [HideInInspector]
    public ShootingRangeSimple manager;
    
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