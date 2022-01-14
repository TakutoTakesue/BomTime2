using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletAction : MonoBehaviour
{
    Rigidbody myRb;
    [SerializeField] float addPower;
    [SerializeField, Tooltip("発射後に消えるまでの秒数")] float destroySeconds;

    // Start is called before the first frame update
    void Start()
    {
        myRb = GetComponent<Rigidbody>();
        myRb.AddForce(transform.forward * addPower);
        Destroy(gameObject, destroySeconds);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
