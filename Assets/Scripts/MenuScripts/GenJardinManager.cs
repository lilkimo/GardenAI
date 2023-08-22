using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class GenJardinManager : MonoBehaviour
{
    // Valores de la localización del usuario

    // Criterios definidos por el usuario:
    [SerializeField] private GameObject costo;
    [SerializeField] private GameObject consumo;
    [SerializeField] private GameObject densidad;
    [SerializeField] private GameObject mantencion;
    [SerializeField] private GameObject flujo;
    [SerializeField] private GameObject blacklist;
    [SerializeField] private GameObject whitelist;
    [SerializeField] private PlantaVetableConstructor plantaVetableConstructor;

    // Data de todas las plantas:
    [SerializeField] private OldParseXML parseXml;

    // Constructor de tarjetas de plantas para jardín generado y el contenedor de estas:
    [SerializeField] private PlantaJardinConstructor plantaJardinConstructor;
    [SerializeField] private GameObject VistaPlantasJardin;

    // void GenerarJardin():
    // La función recupera todos los criterios ingresados por el usuario y 
    // luego ejecuta la función que genera el listado de plantas a colocar.
    public void GenerarJardin(){
        int tamañoJardin = 200; //En m^2

        float costo_v = costo.GetComponentInChildren<Slider>().value * 300000;
        float consumo_v = consumo.GetComponentInChildren<Slider>().value * tamañoJardin * 20000; //Paso de m^2 a cm^3 == ml
        string densidad_v = GetSelectedToggle(densidad).GetComponentInChildren<Text>().text;
        bool mantencion_v = mantencion.GetComponentInChildren<Toggle>().isOn;
        string flujo_v = GetSelectedToggle(flujo).GetComponentInChildren<Text>().text;
        List<string> blacklist_v = GetAllBlacklist(blacklist);
        
        Dictionary<string, List<string>> plantasParaJardin = PickPlants(costo_v, consumo_v, densidad_v, mantencion_v, flujo_v, blacklist_v);

        Debug.Log("PickPlants finalizado.");

        foreach (var plant in plantasParaJardin)
        {
            Debug.Log(String.Format("Se creará la siguiente planta:\nNombre: {0}\nCantidad: {1}\nUbicación: {2}\nPrecio: {3}\nConsumo: {4}[ml/s]",plant.Key, plant.Value[0], plant.Value[1], plant.Value[2], plant.Value[3]));
            PlantaJardinConstructor constructor;
            constructor = Instantiate(plantaJardinConstructor, VistaPlantasJardin.transform);
            constructor.Nombre = plant.Key;
            constructor.Cantidad = plant.Value[0];
            constructor.Ubicacion = plant.Value[1];
            constructor.Precio = plant.Value[2];
            constructor.Consumo = plant.Value[3];
        }
    }
    
    // Devuelve el toggle encendido dentro de un grupo.
    Toggle GetSelectedToggle(GameObject parent) {
        Toggle[] toggles = parent.GetComponentsInChildren<Toggle>();
        foreach (var t in toggles)
        if (t.isOn) return t;  //returns selected toggle
        return null;           // if nothing is selected return null
    }

    // Devuelve los nombres de todas las platnas betadas manualmente por el usuario.
    List<string> GetAllBlacklist(GameObject container){
        List<string> nombres = new List<string>();
        foreach(Transform child in container.transform){
            nombres.Add(child.GetChild(0).GetComponent<TextMeshProUGUI>().text);
        }
        return nombres;
    }
    
    // Dictionary<string, List<string>> PickPlants():
    // La función selecciona plantas en un bucle limitado por 2 cantidades: el saldo del costo del jardín y 
    // el saldo del consumo de agua del jardín. Debido a que aún está en etapa inicial, por ahora recomienda 
    // plantas aleatorias que cumplan con los criterios del usuario y los de la ubicación:
    // - Temperatura y tipo de suelo son condiciones booleanas
    // - Conflicto interespecie agrega una advertencia por ahora. Más adelante determianra un radio que las 
    //   plantas conflictivas no podrán sobreponer.
    // - Flora local aumenta el peso de la planta para la selección aleatoria.
    public Dictionary<string, List<string>> PickPlants(float costoMax, float consumoMax, string densidad, bool mantencion, string flujo, List<string> banned){

        Debug.Log("PickPlants en proceso...");

        float saldoCosto = costoMax;
        float saldoConsumo = consumoMax;
        System.Random rand = new System.Random();
        Dictionary<string, List<string>> plantasJardin = new Dictionary<string, List<string>>();

        var planta = parseXml.infoPlantas.ElementAt(rand.Next(0, parseXml.infoPlantas.Count));
        if (planta.Key != null) Debug.Log("Se escogió una planta aleatoria: "+ planta.Key);

        var count = 5;
        while((saldoCosto - int.Parse(planta.Value[2]) >= 0) && (saldoConsumo - int.Parse(planta.Value[3]) >= 0) && count >= 0){
            Debug.Log("Se inicia el proceso de validación...");

            if (IsDensityApropiate(planta.Value[4], densidad) 
            && !(planta.Value[5] == "Si" && mantencion)
            && IsFlowApropiate(planta.Value[6], flujo)
            && !banned.Contains(planta.Key))
            {
                Debug.Log("La planta "+planta.Key+" cumple con los criterios.");
                if (plantasJardin.ContainsKey(planta.Key)){
                    plantasJardin[planta.Key][0] = (int.Parse(plantasJardin[planta.Key][0]) + 1).ToString();

                    Debug.Log("La planta "+planta.Key+" ya se había escogido previamente. Ahora hay "+plantasJardin[planta.Key][0]+".");
                }
                else {
                    plantasJardin.Add(planta.Key, 
                    new List<string>{
                        "1",
                        planta.Value[1],// Ubicacion
                        planta.Value[2],// Precio
                        planta.Value[3] // Consumo
                    });

                    Debug.Log("La planta "+planta.Key+" no había sido escogida anteriormente. Agregando...");
                }
            }
            
            count-=1;

            planta = parseXml.infoPlantas.ElementAt(rand.Next(0, parseXml.infoPlantas.Count));
            if (planta.Key != null) Debug.Log("Se escogió otra planta aleatoria: "+ planta.Key);
        }
        return plantasJardin;
    }
    bool IsDensityApropiate(string plantaValue, string condition){
        if (condition == "Baja" && (plantaValue == "Media" || plantaValue == "Alta")) return false;
        else if (condition == "Media" && plantaValue == "Alta") return false;
        else return true;
    }
    bool IsFlowApropiate(string plantaValue, string condition){
        if (condition == "Alta" && (plantaValue == "Media" || plantaValue == "Baja")) return false;
        else if (condition == "Media" && plantaValue == "Baja") return false;
        else return true;
    }


    public void ListWLPlants(){
        foreach (var planta in parseXml.infoPlantas)
        {
            PlantaVetableConstructor cons;
            cons = Instantiate(plantaVetableConstructor, whitelist.transform);
            cons.NombrePlanta = planta.Key;
            cons.DescripcionPlanta = planta.Value[0];
            // cons.ImagenPlanta = planta.Value[n];
        }
    }
    public void DeleteWLPlants(){
        foreach(Transform child in whitelist.transform)
        {
            Destroy(child.gameObject);
        }
        foreach(Transform child in blacklist.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
