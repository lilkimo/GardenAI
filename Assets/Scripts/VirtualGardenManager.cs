using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VirtualGardenManager : MonoBehaviour
{
    [SerializeField] private GameObject agua;
    private int totalHidricConsumption;
    private int THC {
        get {
            return totalHidricConsumption;
        }
        set{
            // if (totalHidricConsumption - value < 0) Throw err.
            totalHidricConsumption = value;
            agua.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = totalHidricConsumption.ToString();
        }
    }

    void Start(){
        THC = 0;
    }

    [SerializeField]
    public GameObject terrain;

    public void addPlant(int Consumo){
        THC += Consumo;
        Debug.Log("Se agregó una planta.\nNuevo consumo hídrico del Jardín Virtual: " + totalHidricConsumption);
    }
    public void removePlant(int Consumo){
        THC -= Consumo;
        Debug.Log($"Se quitó una planta (conusmo = {Consumo}).\nNuevo consumo hídrico del Jardín Virtual: " + totalHidricConsumption);
    }
}
