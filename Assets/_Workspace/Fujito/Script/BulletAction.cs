using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletAction : MonoBehaviour
{
    Rigidbody myRb;
    [SerializeField] int power;
    [SerializeField] float addPower;
    [SerializeField, Tooltip("発射後に消えるまでの秒数")] float destroySeconds;

    public int GetPower
    {
        get { return power; }
    }

    // Start is called before the first frame update

    void Start()
    {
        myRb = GetComponent<Rigidbody>();
        myRb.AddForce(transform.forward * addPower);
        Destroy(gameObject, destroySeconds);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag != "Enemy")
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
