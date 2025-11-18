using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TracyPlayerHitFlash : MonoBehaviour
{
    public float flashDuration = 0.25f;

    private ActorController actor;
    private float lastHealth;

    void Awake()
    {
        actor = GetComponent<ActorController>();   
        if (actor != null) lastHealth = actor.Health;
    }

    void Update()
    {
        if (actor == null) return;

        if (actor.Health < lastHealth && actor.Health > 0f)
        {
            StartCoroutine(actor.Flash(flashDuration));
        }

        lastHealth = actor.Health;
    }
}
