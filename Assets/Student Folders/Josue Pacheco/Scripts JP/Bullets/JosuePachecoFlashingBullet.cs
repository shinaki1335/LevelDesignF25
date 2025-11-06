using UnityEngine;

public class JosuePachecoFlashingBullet : ProjectileController
{
    private float timer;
    private float flashSpeed;

    void Start()
    {
        flashSpeed = Random.Range(0.3f, 0.9f);
        timer = Random.Range(0f, 0.6f);
    }

    void Update()
    {
        float scale = 0.2f + Mathf.PingPong(timer * flashSpeed, 1.4f);
        transform.localScale = Vector3.one * scale;
        timer += Time.deltaTime;
    }
}