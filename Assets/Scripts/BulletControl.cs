using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletControl : MonoBehaviour
{
    public float speed = 30;
    public GameObject effectPrefab;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * speed, ForceMode.Impulse);
        Destroy(gameObject, 1f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 打到敌人
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<EnemyControl>().GetHit(2);
        }

        //如果打到可破坏物体
        if (collision.gameObject.tag == "Destructible")
        {
            Rigidbody rbody = collision.gameObject.GetComponent<Rigidbody>();
            if (rbody == null)
            {
                rbody = collision.gameObject.AddComponent<Rigidbody>();
            }
            rbody.AddForceAtPosition(transform.forward * 100, collision.contacts[0].point, ForceMode.Impulse);
            //Destroy(collision.gameObject.GetComponent<Collider>(), 0.04f);
            Destroy(collision.gameObject, 2f);
        }

        var go = Instantiate(effectPrefab, transform.position, Quaternion.LookRotation(collision.contacts[0].normal));
        Destroy(go, 1f);
        Destroy(gameObject);
    }
}