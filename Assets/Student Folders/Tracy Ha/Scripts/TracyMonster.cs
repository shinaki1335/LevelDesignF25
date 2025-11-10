using System.Collections;
using UnityEngine;

public class TracyMonster : ExampleMonster
{
    public float spiralMaxRadius = 4.8f;
    public float spiralTurns = 1.5f;
    public Vector3 spiralCenter = Vector3.zero;
    public float arenaHalfX = 5.5f;
    public float arenaHalfY = 2.5f;
    public float edgeMargin = 0.2f;

    public float quadSpinDurationDefault = 0.8f;
    public float quadSpinShotInterval = 0.2f;
    public float quadSpinDegreesPerSecond = 1080f;

    private Coroutine spiralOutRoutine;
    private Coroutine spiralInRoutine;
    private Coroutine quadSpinRoutine;

    public override void DoAction(string act, float amt = 0)
    {
        base.DoAction(act, amt);

        if (act == "SpiralOut")
        {
            if (spiralOutRoutine != null)
                StopCoroutine(spiralOutRoutine);
            spiralOutRoutine = StartCoroutine(CoSpiralOut(amt));
        }
        else if (act == "SpiralIn")
        {
            if (spiralInRoutine != null)
                StopCoroutine(spiralInRoutine);
            spiralInRoutine = StartCoroutine(CoSpiralIn(amt));
        }
        else if (act == "SetSpiralRadius")
        {
            spiralMaxRadius = Mathf.Max(0f, amt);
        }
        else if (act == "SetSpiralTurns")
        {
            spiralTurns = Mathf.Max(0f, amt);
        }
        else if (act == "SetArenaX")
        {
            arenaHalfX = Mathf.Max(0f, amt);
        }
        else if (act == "SetArenaY")
        {
            arenaHalfY = Mathf.Max(0f, amt);
        }
        else if (act == "NWayShoot")
        {
            int count = Mathf.Max(1, Mathf.RoundToInt(amt));
            NWayShoot(count);
        }
        else if (act == "QuadSpinShoot")
        {
            float duration = amt > 0f ? amt : quadSpinDurationDefault;
            if (quadSpinRoutine != null)
                StopCoroutine(quadSpinRoutine);
            quadSpinRoutine = StartCoroutine(CoQuadSpinShoot(duration));
        }
    }

    private Vector3 ClampToArena(Vector3 p)
    {
        float x = Mathf.Clamp(p.x, -arenaHalfX + edgeMargin, arenaHalfX - edgeMargin);
        float y = Mathf.Clamp(p.y, -arenaHalfY + edgeMargin, arenaHalfY - edgeMargin);
        return new Vector3(x, y, p.z);
    }

    private IEnumerator CoSpiralOut(float duration)
    {
        if (duration <= 0f) yield break;
        Vector3 origin = transform.position;
        float t = 0f;
        float maxR = Mathf.Min(arenaHalfX, arenaHalfY) - edgeMargin;
        float R = Mathf.Min(spiralMaxRadius, maxR);

        while (t < duration)
        {
            float u = t / duration;
            float radius = Mathf.Lerp(0f, R, u);
            float angle = u * spiralTurns * Mathf.PI * 2f;
            Vector3 p = origin + new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0f);
            p = ClampToArena(p);
            Quaternion rot = Quaternion.Euler(0f, 0f, angle * Mathf.Rad2Deg);
            transform.SetPositionAndRotation(p, rot);
            t += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator CoSpiralIn(float duration)
    {
        if (duration <= 0f) yield break;
        Vector3 startPos = transform.position;
        Vector3 rel = startPos - spiralCenter;
        float r0 = rel.magnitude;
        float a0 = Mathf.Atan2(rel.y, rel.x);
        float t = 0f;
        float maxR = Mathf.Min(arenaHalfX, arenaHalfY) - edgeMargin;
        r0 = Mathf.Min(r0, maxR);

        while (t < duration)
        {
            float u = t / duration;
            float radius = Mathf.Lerp(r0, 0f, u);
            float angle = a0 + u * spiralTurns * Mathf.PI * 2f;
            Vector3 p = spiralCenter + new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0f);
            p = ClampToArena(p);
            Quaternion rot = Quaternion.Euler(0f, 0f, angle * Mathf.Rad2Deg);
            transform.SetPositionAndRotation(p, rot);
            t += Time.deltaTime;
            yield return null;
        }

        Vector3 finalPos = ClampToArena(spiralCenter);
        transform.position = finalPos;
    }

    private void NWayShoot(int count)
    {
        if (count < 1) count = 1;

        float baseAngle = transform.rotation.eulerAngles.z;
        float step = 360f / count;

        for (int i = 0; i < count; i++)
        {
            float angle = baseAngle + step * i;
            Shoot(FindProjectile(0), transform.position, new Vector3(0f, 0f, angle));
        }
    }

    private IEnumerator CoQuadSpinShoot(float duration)
    {
        if (Anim != null)
        {
            Anim.Play("SpinShoot");
            StartCoroutine(TrackAnim("SpinShoot"));
        }

        float elapsed = 0f;
        float shotTimer = 0f;
        float angle = transform.rotation.eulerAngles.z;

        while (elapsed < duration)
        {
            float dt = Time.deltaTime;
            elapsed += dt;
            shotTimer += dt;

            angle += quadSpinDegreesPerSecond * dt;
            Quaternion rot = Quaternion.Euler(0f, 0f, angle);
            transform.rotation = rot;

            if (shotTimer >= quadSpinShotInterval)
            {
                shotTimer -= quadSpinShotInterval;
                FireQuadBurst(angle);
            }

            yield return null;
        }
    }

    private void FireQuadBurst(float baseAngle)
    {
        float step = 90f;
        for (int i = 0; i < 4; i++)
        {
            float angle = baseAngle + step * i;
            Shoot(FindProjectile(0), transform.position, new Vector3(0f, 0f, angle));
        }
    }
}
