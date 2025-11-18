using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TracySplitProjectile : ProjectileController
{
    public int maxBounces = 1;
    public float splitAngle = 20f;
    private int bounceCount = 0;
    public bool canSplit = true;

    public override void HitWall(GameObject obj)
    {
        if (canSplit)
        {
            Vector2 dir = RB != null ? RB.linearVelocity.normalized : (Vector2)transform.right;
            if (dir.sqrMagnitude < 0.001f) dir = transform.right;

            Vector2 normal;
            if (obj.transform.localScale.x > obj.transform.localScale.y)
                normal = Vector2.up;
            else
                normal = Vector2.right;

            Vector2 reflectDir = Vector2.Reflect(dir, normal);

            SpawnChild(reflectDir, -splitAngle);
            SpawnChild(reflectDir, splitAngle);

            Destroy(gameObject);
            return;
        }

        if (bounceCount < maxBounces)
        {
            bounceCount++;
            if (RB != null)
            {
                Vector2 vel = RB.linearVelocity;
                if (obj.transform.localScale.x > obj.transform.localScale.y)
                    vel.y *= -1;
                else
                    vel.x *= -1;
                RB.linearVelocity = vel;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void SpawnChild(Vector2 baseDir, float offsetAngle)
    {
        float baseAngle = Mathf.Atan2(baseDir.y, baseDir.x) * Mathf.Rad2Deg;
        float finalAngle = baseAngle + offsetAngle;
        Quaternion rot = Quaternion.Euler(0f, 0f, finalAngle);

        TracySplitProjectile child = Instantiate(this, transform.position, rot);
        child.canSplit = false;
        child.bounceCount = 0;
        child.maxBounces = maxBounces;
        child.Source = Source;

        child.transform.localScale = transform.localScale * 0.5f;

        if (child.RB != null)
            child.RB.linearVelocity = (Vector2)(rot * Vector3.right) * Speed;
    }

    public override void Setup(ActorController src)
    {
        base.Setup(src);
    }

    public override void OnHit(ActorController act)
    {
        base.OnHit(act);
    }
}
