using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TimeField : MonoBehaviour
{
    [Header("Bullet Time")]
    [SerializeField] float bulletTimeDuration;
    [SerializeField] float bulletTimeBonusPerHit;
    [Header("Time Field")]
    [SerializeField] float standardScale;
    [SerializeField] AnimationCurve distanceToCoefficientCurve;

    List<ITimeAdjustable> timeAdjustables;
    float scaleValue;
    float bulletTimeCounter;

    public bool OnBulletTime
    {
        get => gameObject.activeSelf;
        private set => gameObject.SetActive(value);
    }
    public float BulletTimeProgressValue { get; private set; }

    private void Awake()
    {
        scaleValue = transform.lossyScale.x;
        timeAdjustables = new();
    }
    private void Update()
    {
        if (bulletTimeCounter > 0)
        {
            bulletTimeCounter -= Time.deltaTime;
            SetProgressValue(bulletTimeCounter / bulletTimeDuration);
            SetScaleValueWithT(BulletTimeProgressValue);
            if (bulletTimeCounter <= 0)
            {
                EndBulletTime();
            }
        }
    }
    private void FixedUpdate()
    {
        foreach (var a in timeAdjustables.ToList())
        {
            if (a != null && (MonoBehaviour)a != null)
            {
                var distance = Vector3.Distance(a.Position, transform.position);
                var maxRadius = standardScale / 2;
                var currentRadius = scaleValue / 2;
                var value = 1 + (distance - currentRadius) / maxRadius;
                a.TimeAdjustCoefficient = distanceToCoefficientCurve.Evaluate(value);
            }
            else
            {
                timeAdjustables.Remove(a);
            }
        }
    }

    public void SetActive(bool value)
    {
        gameObject.SetActive(value);
    }
    public void StartBulletTime(bool withDuration = false)
    {
        if (!OnBulletTime)
        {
            OnBulletTime = true;
            if (withDuration)
            {
                bulletTimeCounter = bulletTimeDuration;
            }
            else
            {
                bulletTimeCounter = -1;
            }
            SetScaleValue(standardScale);
            SetProgressValue(1);
        }
    }
    public void EndBulletTime()
    {
        if (OnBulletTime)
        {
            OnBulletTime = false;
            SetProgressValue(0);
        }
    }
    public void AddBonusBulletTime(int num)
    {
        if (bulletTimeCounter >= 0)
        {
            bulletTimeCounter += num * bulletTimeBonusPerHit;
        }
    }
    public void SetScaleValue(float value)
    {
        transform.localScale = Vector3.one * value;
        scaleValue = transform.lossyScale.x;
    }

    void SetScaleValueWithT(float t)
    {
        var value = standardScale * Mathf.Sqrt(Mathf.Max(t, 0));
        SetScaleValue(value);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<ITimeAdjustable>(out var a))
        {
            a.AddThisToList(timeAdjustables);
            a.InTimeField = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<ITimeAdjustable>(out var a) && timeAdjustables.Contains(a))
        {
            timeAdjustables.Remove(a);
            a.TimeAdjustCoefficient = 1;
            a.InTimeField = false;
        }
    }
    void SetProgressValue(float value)
    {
        BulletTimeProgressValue = value;
        UIManager.Instance.UpdateUIBulletTime(BulletTimeProgressValue);
    }
}
