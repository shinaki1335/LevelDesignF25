using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class NeoMonster : HazardController
{
    public Rigidbody2D play;
    public GameObject baby;

    public Sprite om;
    public Sprite nom;
    public override void DoAction(string act, float amt = 0)
    {
        base.DoAction(act, amt);
        if (act == "FourSquare")
        {
            StartCoroutine(FourSquare());
        }
        if (act == "RandomWalk")
        {
            StartCoroutine(RandomWalk());
        }
        if (act == "Chase")
        {
            StartCoroutine(Chase());
        }
        if (act == "WaveRise")
        {
            StartCoroutine(WaveRise(amt));
        }
        if (act == "WaveSlide")
        {
            StartCoroutine(WaveSlide(amt));
        }
        if (act == "Roam")
        {
            StartCoroutine(Roam());
        }
        if (act == "Fade")
        {
            StartCoroutine(Fade());
        }
        if (act == "Bite")
        {
            StartCoroutine(Bite(amt));
        }
        if (act == "MoveOverTime 1")
        {
            StartCoroutine(MoveOverTime(new Vector2(-5, 4f)));
        }
        if (act == "MoveOverTime 2")
        {
            StartCoroutine(MoveOverTime(new Vector2(0, 4f)));
        }
        if (act == "MoveOverTime 3")
        {
            StartCoroutine(MoveOverTime(new Vector2(5, 4f)));
        }
        if (act == "MoveOverTime 4")
        {
            StartCoroutine(MoveOverTime(new Vector2(-5, -1f)));
        }
        if (act == "MoveOverTime 5")
        {
            StartCoroutine(MoveOverTime(new Vector2(0, -1f)));
        }
        if (act == "MoveOverTime 6")
        {
            StartCoroutine(MoveOverTime(new Vector2(5, -1f)));
        }
        if (act == "SetRandomX")
        {
            StartCoroutine(SetRandomX(amt));
        }
        if (act == "Stop")
        {
            StartCoroutine(Stop());
        }
    }

    //Get big and move to each corner of the screen
    public IEnumerator FourSquare()
    {
        //I use this to track movement speed
        float moveSpeed = 5;
        //These variables all get used repeatedly in different contexts
        //They always mean loosely the same thing, and just get wiped clean before second use
        //A simple timer
        float timer = 0;
        //Tracks what size I was at the start of a segment
        float startSize = transform.localScale.x;
        //Tracks my position at the start of a segment
        Vector3 startPos = transform.position;
        //Tracks what size I was at the end of a segment
        float endSize = 4;
        //Tracks my position at the end of a segment
        Vector3 endPos = new Vector3(0,2.5f);
        //Move to the top of the screen and get big
        //This is an example of a timer used in a coroutine--it'll always take exactly 0.5 seconds
        while (timer < 1)
        {
            //Note that timer is always a number between 0 and 1--if I want time to pass slow or fast
            //  I just divide or multiple timer by another number to make it change at not-deltaTime
            timer += Time.deltaTime / 0.5f;
            //I use Lerp to determine how big I should be each frame
            //Remember--lerp is always a,b,c--start number, end number, % of the way between them
            float size = Mathf.Lerp(startSize, endSize, timer);
            //I plug the size I calculated in
            transform.localScale = new Vector3(size, size, 1);
            //Same deal, but now with position instead of size
            transform.position = Vector3.Lerp(startPos, endPos, timer);
            yield return null;
        }
        //Move to the top right corner
        //This works a little differently from the segment above--I'm using a different type of Lerp
        //  where I refresh the first number each frame. It means I move a % of the remaining distance every time
        //This gives me a nice curve to my movement
        //Update endPos to be the place I'm moving to
        endPos = new Vector3(5.5f, 2.5f);
        //Loop until I get there
        while (transform.position != endPos)
        {
            //Move a percentage of the way there each frame
            transform.position = Vector3.Lerp(transform.position, endPos,moveSpeed*Time.deltaTime);
            //Because Lerp will never actually reach my target, I need to put a tiny MoveTowards in the segment
            //Otherwise I'll end up 0.000001 units away from my target, forever
            transform.position = Vector3.MoveTowards(transform.position, endPos,0.1f*Time.deltaTime);
            yield return null;
        }
        //Move to the bottom right corner. This works exactly like the above block
        endPos = new Vector3(5.5f, -2.5f);
        while (transform.position != endPos)
        {
            transform.position = Vector3.Lerp(transform.position, endPos,moveSpeed*Time.deltaTime);
            transform.position = Vector3.MoveTowards(transform.position, endPos,0.1f*Time.deltaTime);
            yield return null;
        }
        //Move to the bottom left corner. This works exactly like the above block
        endPos = new Vector3(-5.5f, -2.5f);
        while (transform.position != endPos)
        {
            transform.position = Vector3.Lerp(transform.position, endPos,moveSpeed*Time.deltaTime);
            transform.position = Vector3.MoveTowards(transform.position, endPos,0.1f*Time.deltaTime);
            yield return null;
        }
        //Move to the top left corner. This works exactly like the above block
        endPos = new Vector3(-5.5f, 2.5f);
        while (transform.position != endPos)
        {
            transform.position = Vector3.Lerp(transform.position, endPos,moveSpeed*Time.deltaTime);
            transform.position = Vector3.MoveTowards(transform.position, endPos,0.1f*Time.deltaTime);
            yield return null;
        }
        //Return to the center top part of the screen. This works exactly like the above block
        endPos = new Vector3(0, 2.5f);
        while (transform.position != endPos)
        {
            transform.position = Vector3.Lerp(transform.position, endPos,moveSpeed*Time.deltaTime);
            transform.position = Vector3.MoveTowards(transform.position, endPos,0.1f*Time.deltaTime);
            yield return null;
        }
        //Shrink and return to the center of the screen
        //This works just like the very first segment, but in reverse
        timer = 0;
        startPos = transform.position;
        startSize = transform.localScale.x;
        endPos = Vector3.zero;
        endSize = 1;
        while (timer < 1)
        {
            timer += Time.deltaTime / 0.5f;
            float size = Mathf.Lerp(startSize, endSize, timer);
            transform.localScale = new Vector3(size, size, 1);
            transform.position = Vector3.Lerp(startPos, endPos, timer);
            yield return null;
        }
    }
    
    //Go to a random place on the screen
    //Notice how the action after this one plays out different than normal
    public IEnumerator RandomWalk()
    {
        //I use this to track movement speed
        float moveSpeed = 5;
        //This works a lot like the FourSquare movement blocks, but it's just one
        Vector3 endPos = new Vector3(Random.Range(-5.5f,5.5f),Random.Range(-2.5f,2.5f));
        while (transform.position != endPos)
        {
            //Move a percentage of the way there each frame
            transform.position = Vector3.Lerp(transform.position, endPos,moveSpeed*Time.deltaTime);
            //Because Lerp will never actually reach my target, I need to put a tiny MoveTowards in the segment
            //Otherwise I'll end up 0.000001 units away from my target, forever
            transform.position = Vector3.MoveTowards(transform.position, endPos,0.1f*Time.deltaTime);
            yield return null;
        }
    }

    public IEnumerator Chase()
    {
        //I use this to track movement speed
        float moveSpeed = 5;
        //This works a lot like the FourSquare movement blocks, but it's just one
        Vector3 endPos = play.transform.position;
        while (transform.position != endPos)
        {
            //Move a percentage of the way there each frame
            transform.position = Vector3.Lerp(transform.position, endPos, moveSpeed * Time.deltaTime);
            //Because Lerp will never actually reach my target, I need to put a tiny MoveTowards in the segment
            //Otherwise I'll end up 0.000001 units away from my target, forever
            transform.position = Vector3.MoveTowards(transform.position, endPos, 0.1f * Time.deltaTime);
            yield return null;
        }
    }
    public IEnumerator WaveRise(float numb)
    {
        float moveSpeed = 5;
        Vector3 endPos = transform.position + new Vector3(0, 5 * numb, 0);
        while (Math.Abs(transform.position.y - endPos.y) > 0.5f)
        {
            transform.position = Vector3.Lerp(transform.position, endPos, moveSpeed * Time.deltaTime);
            transform.position = Vector3.MoveTowards(transform.position, endPos, 0.1f * Time.deltaTime);
            yield return null;
        }
        moveSpeed = 3;
        endPos = transform.position + new Vector3(0, -5 * numb, 0);
        while (Math.Abs(transform.position.y - endPos.y) > 0.4f)
        {
            transform.position = Vector3.Lerp(transform.position, endPos, moveSpeed * Time.deltaTime);
            transform.position = Vector3.MoveTowards(transform.position, endPos, 0.1f * Time.deltaTime);
            yield return null;
        }
    }
    public IEnumerator WaveSlide(float numb)
    {
        float moveSpeed = 5;
        Vector3 endPos = transform.position + new Vector3(3 * numb, 0, 0);
        while (Math.Abs(transform.position.x - endPos.x) > 0.5f)
        {
            transform.position = Vector3.Lerp(transform.position, endPos, moveSpeed * Time.deltaTime);
            transform.position = Vector3.MoveTowards(transform.position, endPos, 0.1f * Time.deltaTime);
            yield return null;
        }
        moveSpeed = 3;
        endPos = transform.position + new Vector3(-3 * numb,0, 0);
        while (Math.Abs(transform.position.x - endPos.x) > 0.4f)
        {
            transform.position = Vector3.Lerp(transform.position, endPos, moveSpeed * Time.deltaTime);
            transform.position = Vector3.MoveTowards(transform.position, endPos, 0.1f * Time.deltaTime);
            yield return null;
        }
    }
    public IEnumerator Roam()
    {
        //I use this to track movement speed
        float meed = 1;
        while (baby.activeInHierarchy)
        {
            //This works a lot like the FourSquare movement blocks, but it's just one
            Vector3 endPos = new Vector3(Random.Range(-7f, 7f), Random.Range(-4f, 4f));
            while (Math.Abs(transform.position.x - endPos.x) > 0.5f && baby.activeInHierarchy)
            {
                //Move a percentage of the way there each frame
                transform.position = Vector3.Lerp(transform.position, endPos, meed * Time.deltaTime);
                //Because Lerp will never actually reach my target, I need to put a tiny MoveTowards in the segment
                //Otherwise I'll end up 0.000001 units away from my target, forever
                transform.position = Vector3.MoveTowards(transform.position, endPos, 0.1f * Time.deltaTime);
                yield return null;
            }
        }
    }

    public IEnumerator Fade()
    {
        //I use this to track movement speed
        float number = 1;
        while (number > 0.9f)
        {
            number -= 0.05f * Time.deltaTime;
            Body.color = new Color(0, 0, 0, number);
            yield return null;
        }
    }
    
    public IEnumerator MoveOverTime(Vector2 targetPosition)
    {
        Vector2 startPos = transform.position;
        float elapsed = 0f;

        while (elapsed < 0.3f)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / 0.3f);
            transform.position = Vector2.Lerp(startPos, targetPosition, t);
            transform.localScale = Vector2.Lerp(new Vector2(1, 1), new Vector2(2f, 2f), t);
            yield return null;
        }
        // Ensure exact final position
        transform.position = targetPosition;
    }
    public IEnumerator Bite(float numb)
    {
        if (numb == 1)
        {
            Body.sprite = nom;
            //transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            yield return null;
        }
        else if (numb == 2)
        {
            Body.sortingOrder = 12;
        }
        else if (numb == 0)
        {
            Body.sprite = om;
            //transform.localScale = new Vector3(1f, 1f, 1f);
            yield return null;
        }
        else
        {
            Body.sortingOrder = -3;

        }
    }
    public IEnumerator SetRandomX(float numb)
    {
        if (numb == 0)
            transform.position = new Vector2(Random.Range(-4.5f, 4.5f), transform.position.y);
        else
            transform.position = new Vector2(Random.Range(-7, 8)+0.5f, transform.position.y);
        yield return null;

    }
    public IEnumerator Stop()
    {
        StopAllCoroutines();
        yield return null;

    }
}
