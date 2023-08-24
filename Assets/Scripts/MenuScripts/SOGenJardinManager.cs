using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEditor.MemoryProfiler;
using Unity.VisualScripting;

public class SOGenJardinManager : MonoBehaviour
{
     // Valores de la localización del usuario
    [SerializeField] private LocationManager locationManager;

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
    [SerializeField] private List<MockPlant> allPlants = new List<MockPlant>();

    // Constructor de tarjetas de plantas para jardín generado y el contenedor de estas:
    [SerializeField] private PlantaJardinConstructor plantaJardinConstructor;
    [SerializeField] private GameObject VistaPlantasJardin;

    // Struct para almacenar las plantas escogidas para el jardín (panta, cantidad, conflictos presentes)
    private class PlantaInfo
    {
        public MockPlant data;
        public int cantidad;
        public List<string> conflictos;

        public PlantaInfo(MockPlant data, List<string> conflictos){
            this.data = data;
            this.cantidad = 1;
            this.conflictos = conflictos;
        }
    }

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
        
        Dictionary<string, PlantaInfo> plantasParaJardin = PickPlants(costo_v, consumo_v, densidad_v, mantencion_v, flujo_v, blacklist_v);

        Debug.Log("PickPlants finalizado.");

        foreach (var plant in plantasParaJardin)
        {
            Debug.Log(String.Format("Se creará la siguiente planta:\nNombre: {0}\nCantidad: {1}\nUbicación: {2}\nPrecio: {3}\nConsumo: {4}[ml/s]",plant.Key, plant.Value.cantidad, plant.Value.data.Placement, plant.Value.data.Price, plant.Value.data.Consumption));
            PlantaJardinConstructor constructor;
            constructor = Instantiate(plantaJardinConstructor, VistaPlantasJardin.transform);
            constructor.Nombre = plant.Key;
            constructor.Cantidad = plant.Value.cantidad.ToString();
            constructor.Ubicacion = plant.Value.data.Placement;
            constructor.Precio = plant.Value.data.Price.ToString();
            constructor.Consumo = plant.Value.data.Consumption.ToString();

            Debug.Log(String.Format("Los conflictos de {0} son: ",plant));
            plant.Value.conflictos.ForEach(x => Debug.Log(x));
            if(plant.Value.conflictos.Any()) {
                constructor.Conflictos = "Tiene conflictos con: " + String.Join(", ",plant.Value.conflictos);
            }
            else constructor.Conflictos = "";
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
    private Dictionary<string, PlantaInfo> PickPlants(float costoMax, float consumoMax, string densidad, bool mantencion, string flujo, List<string> banned){

        Debug.Log("PickPlants en proceso...");

        float saldoCosto = costoMax;
        float saldoConsumo = consumoMax;

        System.Random rand = new System.Random();
        Dictionary<string, PlantaInfo> plantasJardin = new Dictionary<string, PlantaInfo>();

        List<int> weightedPlants = Enumerable.Range(0, allPlants.Count).ToList();
        int i = 0;
        while(i < allPlants.Count){
            if(allPlants[i].Origin == locationManager.LocationData[0]) weightedPlants.Add(i);
            i++;
        }

        MockPlant planta = allPlants[weightedPlants[rand.Next(weightedPlants.Count)]];
        if (planta != null) Debug.Log("Se escogió una planta aleatoria: "+ planta.Name);

        while((saldoCosto - planta.Price >= 0) && (saldoConsumo - planta.Consumption >= 0) && plantasJardin.Sum(x => x.Value.cantidad) <= 10){
            Debug.Log("Se inicia el proceso de validación...");

            if (IsDensityApropiate(planta.Density, densidad) // Si la densidad es adecuada [cambiar a futuro]
            && !(planta.Maintenance == "Si" && mantencion) // Si la planta no requiere manetnción y se especificó una baja mantención
            && IsFlowApropiate(planta.Resilience, flujo) // Si el flujo al que la planta está acostrumbrada es apropiado
            && !banned.Contains(planta.Name) // Si la planta no fue excluida manualmente
            && planta.Temperature == locationManager.LocationData[1] // Si a planta es adecuada a la temperatura de la ubicación del usuario
            && planta.Soil == locationManager.LocationData[2] // Si la palnta es adecuada al tipo de suelo de la ubicación del usuario
            )
            {
                Debug.Log("La planta "+planta.Name+" cumple con los criterios.");


                if (plantasJardin.ContainsKey(planta.Name)){
                    plantasJardin[planta.Name].cantidad++;
                    Debug.Log("La planta "+planta.Name+" ya se había escogido previamente. Ahora hay "+plantasJardin[planta.Name].cantidad+".");
                }
                else {
                    Debug.Log("La planta "+planta.Name+" no había sido escogida anteriormente. Agregando...");

                    List<string> conflictsFound = CheckConflict(planta.Conflicts, plantasJardin.Keys.ToList());
                    plantasJardin.Add(planta.Name, new PlantaInfo(planta, conflictsFound));
                    
                    foreach (string conflict in conflictsFound)
                    {
                        plantasJardin[conflict].conflictos.Add(planta.Name);
                    }
                }
            }
            else Debug.Log($@"La planta {planta} no cumple con los criterios:
            Densidad escogida: {densidad}. Densidad de la planta: {planta.Density}
            Mantención: {mantencion}. Mantención de la planta: {planta.Maintenance}
            Flujo escogido: {flujo}. Flujo de la planta: {planta.Resilience}
            Temperatura del usuario: {locationManager.LocationData[1]}. Temperatura de la planta: {planta.Temperature}
            Tipo de suelo del usuario: {locationManager.LocationData[2]}. Tiopo de suelo de la planta: {planta.Soil}
            ");

            planta = allPlants[weightedPlants[rand.Next(weightedPlants.Count)]];
            if (planta != null) Debug.Log("Se escogió una planta aleatoria: "+ planta.Name);
        }
        return plantasJardin;
    }
    // Corrobora que sea correcta la densidad.
    bool IsDensityApropiate(string plantaValue, string condition){
        if (condition == "Baja" && (plantaValue == "Media" || plantaValue == "Alta")) return false;
        else if (condition == "Media" && plantaValue == "Alta") return false;
        else return true;
    }
    // Corrobora que la planta sea apta para el flujo
    bool IsFlowApropiate(string plantaValue, string condition){
        if (condition == "Alta" && (plantaValue == "Media" || plantaValue == "Baja")) return false;
        else if (condition == "Media" && plantaValue == "Baja") return false;
        else return true;
    }
    // List<string> CheckConflict(conlfictos, seeccionadas):
    // Busca si plantas con las que la panta seleccionada 
    // tiene conflictos también fueron seleccionadas y las 
    // retorna.
    List<string> CheckConflict(List<string> conflictos, List<string> seleccionadas){
        Debug.Log("Buscando conflictos ...");
        List<string> conflictsFound = new List<string>();
        foreach (string plantaConflicto in conflictos)
        {
            if(seleccionadas.Contains(plantaConflicto)){
                conflictsFound.Add(plantaConflicto);
            }
        }
        if (!conflictos.Any()) Debug.Log("No hay conflictos");
        else Debug.Log("Se encontraron "+conflictsFound.Count+" conflictos");
        return conflictsFound;
    }

    private void Start() {
        foreach (var planta in allPlants)
        {
            PlantaVetableConstructor cons;
            cons = Instantiate(plantaVetableConstructor, whitelist.transform);
            cons.NombrePlanta = planta.Name;
            cons.DescripcionPlanta = planta.Description;
            // cons.ImagenPlanta = planta.Value[n];
        }
    }
    public void ListWLPlants(){
        foreach (var planta in allPlants)
        {
            PlantaVetableConstructor cons;
            cons = Instantiate(plantaVetableConstructor, whitelist.transform);
            cons.NombrePlanta = planta.Name;
            cons.DescripcionPlanta = planta.Description;
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
