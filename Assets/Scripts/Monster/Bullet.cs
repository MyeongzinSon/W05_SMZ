using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Vector3 m_bulletDir;
    public float m_bulletSpeed;
    public int m_bulletDamage;

    private void Update()
    { 
        transform.position += m_bulletDir * m_bulletSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(m_bulletDamage);
        }

        Destroy(gameObject);
    }
}
