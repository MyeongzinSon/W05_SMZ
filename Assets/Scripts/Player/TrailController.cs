using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TrailController : MonoBehaviour
{
    PlayerController player;
    TrailRenderer trail;

    float defaultTime;

    private void Awake()
    {
        player = transform.parent.GetComponent<PlayerController>();
        trail = GetComponent<TrailRenderer>();
        defaultTime = trail.time;
    }

    private void Update()
    {
        trail.time = defaultTime / player.TimeAdjustCoefficient;
    }
}
