using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour, ITimeAdjustable
{
    public Vector3 m_bulletDir;
    public float m_bulletSpeed;
    public int m_bulletDamage;

    public float TimeAdjustCoefficient { get; set; }

    public Vector3 Position => transform.position;

    private void Update()
    { 
        transform.position += m_bulletDir * m_bulletSpeed * TimeAdjustCoefficient * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(m_bulletDamage);
            Destroy(gameObject);
        }

    }
}
