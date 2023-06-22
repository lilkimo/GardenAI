using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Plant", menuName = "Plant")]
public class Plant : ScriptableObject
{
    public string nombre;
    public string description;
    public string edad;
    public int tamaño;
    public int consumo;

    public void print()
    {
        Debug.Log(nombre);
        Debug.Log(description);
        Debug.Log("Edad: "+edad);
        //Debug.Log("Tamaño:"+tamaño); Aca se printeaba el tamaño hardcodeado, no printear
        Debug.Log("Consumo en ml/dia "+consumo);
    }
}