using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHealth : MonoBehaviour, IDamagable
{
    [SerializeField] private int maxHealth;
    [SerializeField] private float invunerableDuration;
    [SerializeField] private Color invunerableColor;
    [SerializeField] private int invunerableBlinkNum = 1;

    PlayerController player;
    SpriteRenderer spriteRenderer;
    Color defaultColor;
    float invunerableTimer = -1;

    public int Health { get; set; }
    public int MaxHealth { get { return maxHealth; } }
    public bool IsAlive { get { return Health > 0; } }

    bool isInvunerable => invunerableTimer > 0;

    void Awake()
    {
        player = GetComponent<PlayerController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultColor = spriteRenderer.color;
    }
    void Start()
    {
        Health = maxHealth;
        UIManager.Instance.MaxHPUI = Health;
        //Debug.Log($"PlayerHealth : {Health}/{maxHealth}");
    }
    void Update()
    {
        if (isInvunerable)
        {
            invunerableTimer -= Time.deltaTime * player.TimeAdjustCoefficient;

            var blinkDuration = invunerableDuration / invunerableBlinkNum;
            var interpolatedValue = ((invunerableDuration - invunerableTimer) % blinkDuration) / blinkDuration;
            if (interpolatedValue > 0.5f)
            {
                interpolatedValue = 1 - interpolatedValue;
            }
            spriteRenderer.color = Color.Lerp(defaultColor, invunerableColor, interpolatedValue);
        }
    }

    public void TakeDamage(int damage)
    {
        if (!isInvunerable)
        {
            Health -= damage;
            UIManager.Instance.UpdateUIHP(Health);

            invunerableTimer = invunerableDuration;
        }
    }

    public void TakeHealing(int healing)
    {
        Health += healing;
        UIManager.Instance.UpdateUIHP(Health);
        //Debug.Log($"Player takes {healing} healing!");
        //Debug.Log($"PlayerHealth : {Health}/{maxHealth}");
    }
}
