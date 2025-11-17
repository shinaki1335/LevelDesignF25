using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Typhoon : HazardController
{
    public override void DoAction(string act, float amt = 0)
    {
        if (act == "Spiral")
        {
            StartCoroutine(SpiralCoroutine((int)amt));
        }
        else if (act == "Ring")
        {
            StartCoroutine(RingCoroutine((int)amt));
        }
        else if (act == "S")
        {
            StartCoroutine(SpiralsCoroutine((int)amt));
        }
        else if (act == "S1")
        {
            StartCoroutine(Spirals1Coroutine((int)amt));
        }
        else if (act == "S2")
        {
            StartCoroutine(Spirals2Coroutine((int)amt));
        }
        else if (act == "S3")
        {
            StartCoroutine(Spirals3Coroutine((int)amt));
        }
        else if (act == "S4")
        {
            StartCoroutine(Spirals4Coroutine((int)amt));
        }
        else if (act == "S5")
        {
            StartCoroutine(Spirals5Coroutine((int)amt));
        }
        else
        {
            base.DoAction(act, amt);
        }

    }

    private IEnumerator SpiralCoroutine(float duration)
    {
        float endTime = Time.time + duration;
        float angle = 0f;               // current rotation angle
        float angleSpeed = 9f;          // rotation speed per shot
        float spiralFireRate = 0.24f;    // delay between shots
        int armCount = 3;               // number of spiral arms
        float armSpacing = 360f / armCount; // evenly space arms

        while (Time.time < endTime)
        {
            // Smoothly rotate the sprite visually with the spiral
            transform.rotation = Quaternion.Euler(0, 0, angle);

            // Shoot bullets in all spiral arms
            for (int i = 0; i < armCount; i++)
            {
                float currentAngle = angle + (i * armSpacing);
                Quaternion shootRotation = Quaternion.Euler(0, 0, currentAngle);
                Quaternion originalRotation = transform.rotation;

                // Temporarily rotate to shoot direction
                transform.rotation = shootRotation;
                Shoot();

                // Restore after each shot (so next arm is correct)
                transform.rotation = originalRotation;
            }

            // Advance rotation
            angle += angleSpeed;
            if (angle >= 360f) angle -= 360f;

            yield return new WaitForSeconds(spiralFireRate);
        }

        // Reset rotation after the attack ends (optional)
        transform.rotation = Quaternion.identity;
    }

    private IEnumerator SpiralsCoroutine(float duration)
    {
        float endTime = Time.time + duration;
        float angle = 0f;               // current rotation angle
        float angleSpeed = 6f;          // rotation speed per shot
        float spiralFireRate = 0.1f;    // delay between shots
        int armCount = 1;               // number of spiral arms
        float armSpacing = 360f / armCount; // evenly space arms

        while (Time.time < endTime)
        {
            // Smoothly rotate the sprite visually with the spiral
            transform.rotation = Quaternion.Euler(0, 0, angle);

            // Shoot bullets in all spiral arms
            for (int i = 0; i < armCount; i++)
            {
                float currentAngle = angle + (i * armSpacing);
                Quaternion shootRotation = Quaternion.Euler(0, 0, currentAngle);
                Quaternion originalRotation = transform.rotation;

                // Temporarily rotate to shoot direction
                transform.rotation = shootRotation;
                Shoot();

                // Restore after each shot (so next arm is correct)
                transform.rotation = originalRotation;
            }

            // Advance rotation
            angle += angleSpeed;
            if (angle >= 360f) angle -= 360f;

            yield return new WaitForSeconds(spiralFireRate);
        }

        // Reset rotation after the attack ends (optional)
        transform.rotation = Quaternion.identity;
    }

    private IEnumerator Spirals1Coroutine(float duration)
    {
        float endTime = Time.time + duration;
        float angle = 0f;               // current rotation angle
        float angleSpeed = 7f;          // rotation speed per shot
        float spiralFireRate = 0.1f;    // delay between shots
        int armCount = 2;               // number of spiral arms
        float armSpacing = 360f / armCount; // evenly space arms

        while (Time.time < endTime)
        {
            // Smoothly rotate the sprite visually with the spiral
            transform.rotation = Quaternion.Euler(0, 0, angle);

            // Shoot bullets in all spiral arms
            for (int i = 0; i < armCount; i++)
            {
                float currentAngle = angle + (i * armSpacing);
                Quaternion shootRotation = Quaternion.Euler(0, 0, currentAngle);
                Quaternion originalRotation = transform.rotation;

                // Temporarily rotate to shoot direction
                transform.rotation = shootRotation;
                Shoot();

                // Restore after each shot (so next arm is correct)
                transform.rotation = originalRotation;
            }

            // Advance rotation
            angle += angleSpeed;
            if (angle >= 360f) angle -= 360f;

            yield return new WaitForSeconds(spiralFireRate);
        }

        // Reset rotation after the attack ends (optional)
        transform.rotation = Quaternion.identity;
    }

    private IEnumerator Spirals2Coroutine(float duration)
    {
        float endTime = Time.time + duration;
        float angle = 0f;               // current rotation angle
        float angleSpeed = 8f;          // rotation speed per shot
        float spiralFireRate = 0.1f;    // delay between shots
        int armCount = 3;               // number of spiral arms
        float armSpacing = 360f / armCount; // evenly space arms

        while (Time.time < endTime)
        {
            // Smoothly rotate the sprite visually with the spiral
            transform.rotation = Quaternion.Euler(0, 0, angle);

            // Shoot bullets in all spiral arms
            for (int i = 0; i < armCount; i++)
            {
                float currentAngle = angle + (i * armSpacing);
                Quaternion shootRotation = Quaternion.Euler(0, 0, currentAngle);
                Quaternion originalRotation = transform.rotation;

                // Temporarily rotate to shoot direction
                transform.rotation = shootRotation;
                Shoot();

                // Restore after each shot (so next arm is correct)
                transform.rotation = originalRotation;
            }

            // Advance rotation
            angle += angleSpeed;
            if (angle >= 360f) angle -= 360f;

            yield return new WaitForSeconds(spiralFireRate);
        }

        // Reset rotation after the attack ends (optional)
        transform.rotation = Quaternion.identity;
    }

    private IEnumerator Spirals3Coroutine(float duration)
    {
        float endTime = Time.time + duration;
        float angle = 0f;               // current rotation angle
        float angleSpeed = 9f;          // rotation speed per shot
        float spiralFireRate = 0.1f;    // delay between shots
        int armCount = 4;               // number of spiral arms
        float armSpacing = 360f / armCount; // evenly space arms

        while (Time.time < endTime)
        {
            // Smoothly rotate the sprite visually with the spiral
            transform.rotation = Quaternion.Euler(0, 0, angle);

            // Shoot bullets in all spiral arms
            for (int i = 0; i < armCount; i++)
            {
                float currentAngle = angle + (i * armSpacing);
                Quaternion shootRotation = Quaternion.Euler(0, 0, currentAngle);
                Quaternion originalRotation = transform.rotation;

                // Temporarily rotate to shoot direction
                transform.rotation = shootRotation;
                Shoot();

                // Restore after each shot (so next arm is correct)
                transform.rotation = originalRotation;
            }

            // Advance rotation
            angle += angleSpeed;
            if (angle >= 360f) angle -= 360f;

            yield return new WaitForSeconds(spiralFireRate);
        }

        // Reset rotation after the attack ends (optional)
        transform.rotation = Quaternion.identity;
    }
    private IEnumerator Spirals4Coroutine(float duration)
    {
        float endTime = Time.time + duration;
        float angle = 0f;               // current rotation angle
        float angleSpeed = 10f;          // rotation speed per shot
        float spiralFireRate = 0.1f;    // delay between shots
        int armCount = 5;               // number of spiral arms
        float armSpacing = 360f / armCount; // evenly space arms

        while (Time.time < endTime)
        {
            // Smoothly rotate the sprite visually with the spiral
            transform.rotation = Quaternion.Euler(0, 0, angle);

            // Shoot bullets in all spiral arms
            for (int i = 0; i < armCount; i++)
            {
                float currentAngle = angle + (i * armSpacing);
                Quaternion shootRotation = Quaternion.Euler(0, 0, currentAngle);
                Quaternion originalRotation = transform.rotation;

                // Temporarily rotate to shoot direction
                transform.rotation = shootRotation;
                Shoot();

                // Restore after each shot (so next arm is correct)
                transform.rotation = originalRotation;
            }

            // Advance rotation
            angle += angleSpeed;
            if (angle >= 360f) angle -= 360f;

            yield return new WaitForSeconds(spiralFireRate);
        }

        // Reset rotation after the attack ends (optional)
        transform.rotation = Quaternion.identity;
    }

    private IEnumerator Spirals5Coroutine(float duration)
    {
        float endTime = Time.time + duration;
        float angle = 0f;               // current rotation angle
        float angleSpeed = 10f;          // rotation speed per shot
        float spiralFireRate = 0.1f;    // delay between shots
        int armCount = 6;               // number of spiral arms
        float armSpacing = 360f / armCount; // evenly space arms

        while (Time.time < endTime)
        {
            // Smoothly rotate the sprite visually with the spiral
            transform.rotation = Quaternion.Euler(0, 0, angle);

            // Shoot bullets in all spiral arms
            for (int i = 0; i < armCount; i++)
            {
                float currentAngle = angle + (i * armSpacing);
                Quaternion shootRotation = Quaternion.Euler(0, 0, currentAngle);
                Quaternion originalRotation = transform.rotation;

                // Temporarily rotate to shoot direction
                transform.rotation = shootRotation;
                Shoot();

                // Restore after each shot (so next arm is correct)
                transform.rotation = originalRotation;
            }

            // Advance rotation
            angle += angleSpeed;
            if (angle >= 360f) angle -= 360f;

            yield return new WaitForSeconds(spiralFireRate);
        }

        // Reset rotation after the attack ends (optional)
        transform.rotation = Quaternion.identity;
    }

    private System.Collections.IEnumerator RingCoroutine(int waveCount)
    {
        float bulletCount = 22;

        // Each bullet evenly spaced around 360 degrees
        float angleStep = 360f / bulletCount;

        Quaternion originalRotation = transform.rotation;

        for (int i = 0; i < bulletCount; i++)
        {
            float angle = i * angleStep;

            // Rotate to the firing angle
            transform.rotation = Quaternion.Euler(0, 0, angle);
            Shoot(); // uses ActorController's Shoot()
        }

        // Restore original rotation
        transform.rotation = originalRotation;

        yield return null; // one frame delay, can be adjusted
    }
}
