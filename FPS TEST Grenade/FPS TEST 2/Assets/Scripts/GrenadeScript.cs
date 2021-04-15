using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeScript : MonoBehaviour {

    public float sec = 2f;
    public bool boom = false;
    public GameObject cible;
    [SerializeField] GameObject particle;

    
    
    IEnumerator Explosion()
    {
        yield return new WaitForSeconds(sec);
        boom = true;

    }

    private void OnCollisionEnter(Collision collision)
    {
        StartCoroutine(Explosion());
    }
   

    private void OnTriggerStay (Collider other)
    {
        if (!boom) return;

        if (other.CompareTag("Player") )
        {            
            Destroy(other.gameObject);            
        }

        Instantiate(particle, transform.position, Quaternion.identity);
        Destroy(gameObject);
    
    }
}