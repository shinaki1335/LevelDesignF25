using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RayGun : ActorController
{
    public override void DoAction(string act, float amt = 0)
    {
        base.DoAction(act, amt);
        if (act == "RayBeam")
        {
            
        }
        else if (act == "Warning")
        {
            
        }
    }

    
}
