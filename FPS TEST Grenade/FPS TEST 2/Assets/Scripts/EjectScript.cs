using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EjectScript : MonoBehaviour {

    [SerializeField] GameObject grenade;
    [SerializeField] int force=1000;    


    void Update () {
        
        if(Input.GetButtonDown("Fire2"))
        {
           GameObject Go= Instantiate(grenade, transform.position, Quaternion.identity) ;
           Go.GetComponent<Rigidbody>().AddForce(transform.TransformDirection(Vector3.forward) * force);
        }
    }
}