using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBase : MonoBehaviour
{

    protected Collider2D m_collider2D;
    protected Rigidbody2D m_rigidbody2D;

    [SerializeField] protected ItemType m_itemType;
    public ItemType ItemType { get { return m_itemType; } }

    protected virtual void Start()
    {
        m_collider2D = GetComponent<Collider2D>();
        m_rigidbody2D = GetComponent<Rigidbody2D>();
    }

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") == true)
        {
            m_collider2D.isTrigger = true;
            m_rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") == true)
        {
            m_collider2D.isTrigger = false;
            m_rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
        }
    }

    public virtual void OnUse(PlayerHealth player)
    {

    }
}
