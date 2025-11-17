using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class MachineGun : HazardController
{
     public override void DoAction(string act, float amt = 0)
    {
        if (act == "Barrage")
        {
            StartCoroutine(BarrageCoroutine((int)amt));
        }
        else
        {
            base.DoAction(act, amt);
        }
    }

    private IEnumerator BarrageCoroutine(float duration)
    {
        float barrageLength = Time.time + duration;

        while (Time.time < barrageLength)
        {
            int bulletAmount = Random.Range(2, 4);

            for (int x = 0; x < bulletAmount; x++)
            {
                Vector3 lastPosition = transform.position;
                Quaternion lastRotation = transform.rotation;

                float offset = Random.Range(-2f, 2f);
                Vector3 spawnPos = transform.position + new Vector3(0.5f, offset, 0);

                transform.position = spawnPos;

                float rotation = Random.Range(-90f, 90f);
                transform.rotation = Quaternion.Euler(0, 0, 90f + rotation);

                Shoot();

                transform.position = lastPosition;
                transform.rotation = lastRotation;

                yield return new WaitForSeconds(Random.Range(0.02f, 0.08f));
            }

            yield return new WaitForSeconds(Random.Range(0.1f, 0.4f));
        }
    }

}