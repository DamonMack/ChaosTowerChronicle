using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            gameObject.SetActive(false);
            Destroy(gameObject, 0.5f);
        }
    }

    public void TakeDamage()
    {
        Debug.Log("enemy hit");
        gameObject.SetActive(false);
        Destroy(gameObject, 0.5f);

    }
}
