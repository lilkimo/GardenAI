using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ObjectPrintInfo : MonoBehaviour
{
    public Plant plant;
    private float tamaño;
    // Start is called before the first frame update
    void Start()
    {
        plant.print();
        MeshRenderer mesh;
        mesh = GetComponent<MeshRenderer>();
        tamaño = mesh.bounds.size.y;
        
        Debug.Log("Tamaño: "+Math.Round(tamaño,2) +" Metros");
    }
}