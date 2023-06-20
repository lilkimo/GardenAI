using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MenuManager : MonoBehaviour
{
    public event Action OnPlantas;
    public event Action OnMain;

    // public static MenuManager instance;
    // Start is called before the first frame update
    void Start()
    {
        Main();
    }

    public void Main(){
        OnMain?.Invoke();
        Debug.Log("Main");
    }
    public void Plantas(){
        OnPlantas?.Invoke();
        Debug.Log("Plantas");
    }
}
