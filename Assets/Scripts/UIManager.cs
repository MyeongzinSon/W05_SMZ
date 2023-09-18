using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    #region UI_Object
    [SerializeField] private Image m_hpBarImg;
    [SerializeField] private Image m_staminaBarImg;
    [SerializeField] private Image m_smashCoolDownImg;
    [SerializeField] private GameObject m_enemyInfo;
    [SerializeField] private Text m_enemyText;
    [SerializeField] private Image m_bulletTimeEffectImg;
    #endregion

    #region UI_Use Variable
    [HideInInspector] public int MaxHPUI;
    [HideInInspector] public int MaxEnemy;
    [HideInInspector] public float MaxStaminaUI;
    [HideInInspector] public float MaxCoolDownUI;
    #endregion

    float bulletTimeEffectInitialAlpha;

    #region MonoBehaviour Methods
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        bulletTimeEffectInitialAlpha = m_bulletTimeEffectImg.color.a;
        UpdateUIBulletTime(0);
    }
    #endregion

    public void UpdateUIAtk(bool CanDash, float m_currentStamina, float m_currentSmashCooldown)
    {
        m_staminaBarImg.fillAmount = m_currentStamina / MaxStaminaUI;
        m_smashCoolDownImg.fillAmount = (m_currentSmashCooldown / MaxCoolDownUI);
        Color tmp = m_staminaBarImg.color;

        if (CanDash) { tmp.a = 1.0f;}
        else { tmp.a = 0.38f; }
        m_staminaBarImg.color = tmp;
    }

    public void UpdateUIHP(int  m_currentHP)
    {
        m_hpBarImg.fillAmount = m_currentHP / (float)MaxHPUI;
    }

    public void UpdateUIEnemyFirst(int m_enemyCount, bool m_isPlayerEnter)
    {
        m_enemyInfo.SetActive(m_isPlayerEnter);
        MaxEnemy = m_enemyCount;
        m_enemyText.text = ($"{m_enemyCount} / {MaxEnemy}");
    }

    public void UpdateUIEnemy(int m_enemyCount)
    {
        m_enemyText.text = ($"{m_enemyCount} / {MaxEnemy}");
    }

    public void UpdateUIBulletTime(float value)
    {
        var color = m_bulletTimeEffectImg.color;
        color.a = bulletTimeEffectInitialAlpha * value;
        m_bulletTimeEffectImg.color = color;
    }
}
