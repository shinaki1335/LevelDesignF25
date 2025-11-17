using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Raindrop : MonoBehaviour
{

    [SerializeField] GameObject R;
    [SerializeField] GameObject R1;
    [SerializeField] GameObject R2;
    [SerializeField] GameObject R3;
    [SerializeField] GameObject R4;
    [SerializeField] GameObject R5;
    [SerializeField] GameObject R6;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        StartCoroutine(Entrance());

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public IEnumerator Entrance()
    {


        //A simple timer
        float timer = 0;




        //This is an example of a timer used in a coroutine--it'll always take exactly 0.5 seconds
        while (timer < 12)
        {

            timer += Time.deltaTime / 0.5f;


            yield return null;
        }

        yield return null;




        //Shrink and return to the center of the screen
        //This works just like the very first segment, but in reverse
        timer = 0;



    }


}



