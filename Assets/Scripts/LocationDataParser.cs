using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LocationDataParser : MonoBehaviour
{
    public LocationManager locationManager;
    private List<string> locationData;
    public List<string> LocationData 
    {
        get 
        {
            return locationData; 
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        locationManager = GetComponent<LocationManager>();
        string ubicacion = locationManager.UserRegion;

        // Ahora debería revisar una archivo con los datios de las distintas regionaes para poder obetner sus suelos y temperaturas.
        // Pero mientras tanto...
        if(ubicacion == "Región Metropolitana")
            locationData = new List<string>(){"Region Metropolitana", "Templaldo", "Vertisol"};
        
    }
}
