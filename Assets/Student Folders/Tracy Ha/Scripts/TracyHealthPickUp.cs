using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TracyHealthPickUp : ActorController
{
    public float healAmount = 1f;
    public GnomeScript HealGnome;

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController p = other.GetComponentInParent<PlayerController>();
        if (p == null) return;

        if (HealGnome != null)
            Instantiate(HealGnome, transform.position, Quaternion.identity);

        p.Health += healAmount;

        Destroy(gameObject);
    }
}
