using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TyphoonMonster : HazardController
{
    public override void DoAction(string act, float amt = 0)
    {
        if (act == "ShootSpiral")
        {
            StartCoroutine(ShootSpiralCoroutine((int)amt));
        }
        else if (act == "RingShot")
        {
            StartCoroutine(RingShotCoroutine((int)amt));
        }
        else
        {
            base.DoAction(act, amt);
        }
    }

    private System.Collections.IEnumerator ShootSpiralCoroutine(float duration)
    {
        float endTime = Time.time + duration;
        float angle = 0f;               
        float angleSpeed = 4f;         
        float spiralFireRate = 0.1f;    
        int armCount = 3;               
        float armSpacing = 360f / armCount;

        while (Time.time < endTime)
        {
            Quaternion originalRotation = transform.rotation;

            for (int i = 0; i < armCount; i++)
            {
                float currentAngle = angle + (i * armSpacing);
                transform.rotation = Quaternion.Euler(0, 0, currentAngle);
                Shoot();
            }

            transform.rotation = originalRotation;

            angle += angleSpeed;
            if (angle >= 360f) angle -= 360f;

            yield return new WaitForSeconds(spiralFireRate);
        }
    }

    private System.Collections.IEnumerator RingShotCoroutine(int waveCount)
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
