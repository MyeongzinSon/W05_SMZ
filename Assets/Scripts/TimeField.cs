using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TimeField : MonoBehaviour
{
    [SerializeField] AnimationCurve distanceToCoefficientCurve;

    List<ITimeAdjustable> timeAdjustables;
    float scaleValue;

    public bool OnBulletTime
    {
        get => gameObject.activeSelf;
        private set => gameObject.SetActive(value);
    }

    private void Awake()
    {
        scaleValue = transform.lossyScale.x;
        timeAdjustables = new();
    }
    private void FixedUpdate()
    {
        foreach (var a in timeAdjustables)
        {
            var distance = Vector3.Distance(a.Position, transform.position);
            var radius = scaleValue / 2;
            a.TimeAdjustCoefficient = distanceToCoefficientCurve.Evaluate(distance / radius);
        }
    }

    public void SetActive(bool value)
    {
        gameObject.SetActive(value);
    }
    public void StartBulletTime()
    {
        if (!OnBulletTime)
        {
            OnBulletTime = true;
        }
    }
    public void EndBulletTime(int attackedNum = 0)
    {
        if (attackedNum > 0)
        {
            StartCoroutine(EndBulletTimeAfterDelay(2 + attackedNum * 0.25f));
        }
        else
        {
            ExecuteEndBulletTime();
        }
    }
    IEnumerator EndBulletTimeAfterDelay(float delay)
    {
        Debug.Log($"Coroutine with {delay} seconds started. : {Time.realtimeSinceStartup}");
        yield return new WaitForSecondsRealtime(delay);
        Debug.Log($"Bullet time ended. : {Time.realtimeSinceStartup}");
        ExecuteEndBulletTime();
    }
    void ExecuteEndBulletTime()
    {
        if (OnBulletTime)
        {
            OnBulletTime = false;
        }
    }
    public void SetScaleValue(float value)
    {
        transform.localScale = Vector3.one * value;
        scaleValue = transform.lossyScale.x;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<ITimeAdjustable>(out var a))
        {
            timeAdjustables.Add(a);
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
}
