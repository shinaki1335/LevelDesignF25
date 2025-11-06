using UnityEngine;

public class JosuePachecoFlashingBullet : ProjectileController
{
    public float flashSpeed = 0.5f;
    private float timer = 0f;

    void Update()
    {
        // Comportamiento de parpadeo
        float scale = 0.5f + Mathf.PingPong(timer * flashSpeed, 1f);
        transform.localScale = Vector3.one * scale;
        timer += Time.deltaTime;
    }
}
