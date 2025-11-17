using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerUno : PlayerController
{
    //A static variable to make the player easy to find
    public static PlayerUno Player1;
    


    public override void OnAwake()
    {
        base.OnAwake();
        Player1 = this;
    }

}
