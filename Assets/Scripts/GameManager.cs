using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

//This script handles the game's backend
//You really shouldn't mess with it
//Just make sure to set MainNPC and JSON in the editor
public class GameManager : MonoBehaviour
{
    [Header("Put walls here: ")]
    public GameObject Walls;
    [Header("Set To Your Main NPC")]
    public ActorController MainNPC;
    [Header("Add Disabled-At-Start NPCs Here")]
    public List<ActorController> SideNPCs;
    [Header("Drag Your JSON File Here")]
    public TextAsset JSON;
    [Header("Check To View Time While Testing")]
    public bool TestMode;
    public static GameManager Singleton;
    [Header("Ignore These")]
    public TextMeshPro HealthDisplay;
    public TextMeshPro DialogueDisplay;
    public TextMeshPro TimeDisplay;
    public SpriteRenderer Fader;
    public AudioSource AS;
    public LevelJSON Script;
    public List<ActorController> Actors;
    public Dictionary<string, List<ActorController>> ActorDict = new Dictionary<string, List<ActorController>>();
    public float Clock;
    public List<EventJSON> Queue = new List<EventJSON>();
    private bool RoundBegun = false;
    public float ScaleSpeed = 0.1f;
    public Vector3 targetScale = new Vector3(0.6f, 0.6f, 0.6f);
    public float BreakTime = 9f;
    private bool TimeStamp = false;
    public Vector3 NewTargetScale = new Vector3(1.18f, 1.1f, 1f);
    public float NewScaleSpeed = 5f;
    private void Awake()
    {
        GameManager.Singleton = this;
    }

    void Start()
    {
        AS.Stop();
        foreach (ActorController a in SideNPCs)
            AddActor(a);
        Script = JSONReader.ParseJSON(JSON.text);
        foreach(EventJSON e in Script.Events)
            Queue.Add(e);
        StartCoroutine(GameMaster.Fade(Fader,false,0.5f));
        DialogueDisplay.text = Script.Title + "\n" + Script.Author;
    }

    // Update is called once per frame
    void Update()
    {
        if (!RoundBegun)
        {
            if (Input.anyKey && Time.time > 0.5f)
            {
                RoundBegun = true;
                DialogueDisplay.text = "";
                AS.Play();
            }
            return;
        }


        if (!TimeStamp && Walls != null)
        {
            Walls.transform.localScale = Vector3.MoveTowards(
                Walls.transform.localScale,
                targetScale,
                ScaleSpeed * Time.deltaTime
            );

            if (Clock >= BreakTime)
            {
                TimeStamp = true;
            }
        }

        if (TimeStamp && Walls != null)
        {
            Walls.transform.localScale = Vector3.MoveTowards(
                Walls.transform.localScale,
                NewTargetScale,
                NewScaleSpeed * Time.deltaTime
            );
        }

        Clock += Time.deltaTime;
        if (TestMode)
        {
            float timeRemaining = AS.clip.length - Clock;
            timeRemaining = Mathf.Max(0f, timeRemaining); // avoid negative numbers

            int minutes = Mathf.FloorToInt(timeRemaining / 60);
            int seconds = Mathf.FloorToInt(timeRemaining % 60);

            TimeDisplay.text = $"{minutes:0}:{seconds:0}";
        }
        while (Queue.Count > 0 && Queue[0].Time <= Clock)
        {
            EventJSON e = Queue[0];
            Queue.RemoveAt(0);
            ResolveEvent(e);
        }

        if (Queue.Count == 0)
        {
            StartCoroutine(LevelComplete());
        }

        
        if (PlayerController.Player != null)
        {
            HealthDisplay.text = "Health: " + Mathf.Ceil(PlayerController.Player.Health);
        }
        else
        {
            HealthDisplay.text = "YOU ARE DEAD";
        }
    }

    public void AddActor(ActorController a)
    {
        if (a == null || Actors.Contains(a)) return;
        Actors.Add(a);
        if(!ActorDict.ContainsKey(a.gameObject.name))
            ActorDict.Add(a.gameObject.name,new List<ActorController>());
        ActorDict[a.gameObject.name].Add(a);
    }

    public void RemoveActor(ActorController a)
    {
        Actors.Remove(a);
        ActorDict.Remove(a.gameObject.name);
    }

    public void ResolveEvent(EventJSON e)
    {
        List<ActorController> who = new List<ActorController>();
        if (!string.IsNullOrEmpty(e.Who)) who.AddRange(ActorDict[e.Who]);
        else who.Add(MainNPC);
        foreach(ActorController w in who)
            w.TakeEvent(e);
        if (e.Dialogue != null)
        {
           HandleDialogue(e.Dialogue); 
        }
    }

    public void HandleDialogue(string d, float duration=0)
    {
        DialogueDisplay.text = d;
    }

    public IEnumerator LevelComplete()
    {
        yield return StartCoroutine(GameMaster.Fade(Fader));
        yield return new WaitForSeconds(0.5f);
        GameMaster.NextStage(); 
    }
}
