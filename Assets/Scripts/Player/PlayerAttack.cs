using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Dash")]
    [SerializeField] private int m_dashPower;
    [SerializeField] private float m_maxStamina;
    [SerializeField] private float m_staminaPerDash;
    [SerializeField] private float m_staminaRegen;
    [SerializeField] private float m_staminaRegenOnGround;
    [Header("Smash")]
    [SerializeField] private int m_smashPower;
    [SerializeField] private float m_smashCooldown;
    [Header("Layer")]
    [SerializeField] private LayerMask m_attackableLayer;
    [SerializeField] private LayerMask m_itemLayer;

    //private UIManager uiManager;
    private PlayerController m_player;
    private PlayerGround m_ground;
    private PlayerHealth m_health;
    private PlayerRayProjector m_rayProjector;
    private TimeField m_timeField;

    private float m_currentStamina;
    private float m_currentSmashCooldown;

    private bool m_isAttacking;

    public bool CanDash { get { return m_currentStamina >= m_staminaPerDash; } }
    public bool CanSmash { get { return m_currentSmashCooldown <= 0; } }
    public float CurrentStamina { get { return m_currentStamina; } }
    float TimeCoefficient => m_player.TimeAdjustCoefficient;

    private void Awake()
    {
        // uiManager = FindObjectOfType<UIManager>();
        m_player = GetComponent<PlayerController>();
        m_ground = GetComponent<PlayerGround>();
        m_health = GetComponent<PlayerHealth>();
        m_rayProjector = GetComponent<PlayerRayProjector>();
        m_timeField = GetComponentInChildren<TimeField>(true);

        m_currentStamina = m_maxStamina;
        m_currentSmashCooldown = -1;
    }
    private void Start()
    { 
        UIManager.Instance.MaxStaminaUI = m_maxStamina;
        UIManager.Instance.MaxCoolDownUI = m_smashCooldown;
    }
    private void Update()
    {
        if (m_currentSmashCooldown > 0)
        {
            m_currentSmashCooldown -= Time.deltaTime * TimeCoefficient;
        }
        
        if (m_currentStamina < m_maxStamina)
        {
            m_currentStamina += (m_ground.GetOnGround() || m_ground.GetOnWall()  ? m_staminaRegenOnGround : m_staminaRegen) * Time.deltaTime * TimeCoefficient;
            if (m_currentStamina > m_maxStamina)
            {
                m_currentStamina = m_maxStamina;
            }
        }
        UIManager.Instance.UpdateUIAtk(CanDash, m_currentStamina, m_currentSmashCooldown);

        if (!m_player.IsSmashInput && m_timeField.OnBulletTime && !CanDash)
        {
            m_timeField.EndBulletTime();
        }
    }

    public void Dash(Vector2 start, Vector2 end)
    {
        m_currentStamina -= m_staminaPerDash;
        var attacked = PerformAttack(start, end, m_dashPower);
        //if (attacked.Count > 0)
        //{
        //    m_timeField.ApplyDashStiff(attacked);
        //}
    }

    public void Smash(Vector2 start, Vector2 end)
    {
        m_currentSmashCooldown = m_smashCooldown;
        var attacked = PerformAttack(start, end, m_smashPower);
        if (attacked.Count > 0)
        {
            m_currentStamina += m_staminaPerDash;
            if (m_currentStamina > m_maxStamina)
            {
                m_currentStamina = m_maxStamina;
            }
            m_timeField.StartBulletTime();
            //m_timeField.ApplyDashStiff(attacked);
            m_timeField.EndBulletTime(attacked.Count);
        }
    }

    List<int> PerformAttack(Vector2 start, Vector2 end, int damage)
    {
        m_rayProjector.SelectTransforms(start, end, m_itemLayer).
            Select(i => i.GetComponent<ItemBase>()).
            ToList().
            ForEach(i => i.OnUse(m_health));

        var attackables = m_rayProjector.SelectTransforms(start, end, m_attackableLayer).
            Select(a => a.GetComponent<IDamagable>()).
            ToList();
        var damages = InflictDamages(attackables, damage);
        return damages;
    }

    List<int> InflictDamages(List<IDamagable> targets, int damage)
    {
        var damages = new List<int>();
        targets.ForEach(a => {
            a.TakeDamage(damage);
            damages.Add(damage);
        });
        return damages;
    }
}
