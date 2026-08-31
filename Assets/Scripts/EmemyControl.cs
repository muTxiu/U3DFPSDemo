using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    public int hp = 10;
    public GameObject bombEffect;

    void Start()
    {

    }

    public void GetHit(int damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            //爆炸
            Instantiate(bombEffect, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}