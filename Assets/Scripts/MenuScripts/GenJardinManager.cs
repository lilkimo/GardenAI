using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GenJardinManager : MonoBehaviour
{
    [SerializeField] private GameObject costo;
    [SerializeField] private GameObject consumo;
    [SerializeField] private GameObject densidad;
    [SerializeField] private GameObject mantencion;
    [SerializeField] private GameObject flujo;
    [SerializeField] private GameObject blacklist;

    public void iniciarEscaneo(){
        var costo_v = costo.GetComponentInChildren<Slider>().value;
        var consumo_v = consumo.GetComponentInChildren<Slider>().value;
        var densidad_v = GetSelectedToggle(densidad).GetComponentInChildren<Text>().text;
        var mantencion_v = mantencion.GetComponentInChildren<Toggle>().isOn;
        var flujo_v = GetSelectedToggle(flujo).GetComponentInChildren<Text>().text;
        
    }
    
    Toggle GetSelectedToggle(GameObject parent) {
        Toggle[] toggles = parent.GetComponentsInChildren<Toggle>();
        foreach (var t in toggles)
        if (t.isOn) return t;  //returns selected toggle
        return null;           // if nothing is selected return null
    }
}
