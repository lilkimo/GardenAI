using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Networking;
using UnityEditor;

public class LocationManager : MonoBehaviour
{
    private string API_KEY = "AIzaSyD7AGKsaPiLxIHUV0BMea5zgAFJHW58TuU";
    public string direccion;
    [Serializable]
    public class Response 
    {
        public Plus_Code plus_code;
        public string results;
    }
    [Serializable]
    public class Plus_Code
    {
        public string compound_code;
        public string global_code;
    }

    IEnumerator Start()
    {
        Debug.Log("Iniciando la busqueda de localización");
        // Check if the user has location service enabled.
        if (!UnityEngine.Android.Permission.HasUserAuthorizedPermission(UnityEngine.Android.Permission.CoarseLocation)) {
            UnityEngine.Android.Permission.RequestUserPermission(UnityEngine.Android.Permission.CoarseLocation);
        }

        if (!Input.location.isEnabledByUser) {
            // TODO Failure
            Debug.LogFormat("Android and Location not enabled");
            yield break;
        }

        // Starts the location service.
        Input.location.Start(500f, 500f);

        // Waits until the location service initializes
        int maxWait = 40;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSecondsRealtime(1);
            maxWait--;
        }

        // If the service didn't initialize in 20 seconds this cancels location service use.
        if (maxWait < 1)
        {
            print("Timed out");
            yield break;
        }

        // If the connection failed this cancels location service use.
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            print("Unable to determine device location");
            yield break;
        }
        else
        {
            // If the connection succeeded, this retrieves the device's current location and displays it in the Console window.
            print("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);
            var lat = Input.location.lastData.latitude.ToString().Replace(",",".");
            var lon = Input.location.lastData.longitude.ToString().Replace(",",".");

            Debug.Log("Parsing your location...");
            string uri = String.Format("https://maps.googleapis.com/maps/api/geocode/json?latlng={0},{1}&key={2}",lat,lon,API_KEY);
            using(UnityWebRequest request = UnityWebRequest.Get(uri)) 
            {
                yield return request.SendWebRequest();
                if (request.result == UnityWebRequest.Result.ConnectionError)
                    Debug.Log(request.error);
                else
                    Debug.Log(request.downloadHandler.text);
                
                    string json = request.downloadHandler.text;
                    Response response = JsonUtility.FromJson<Response>(json);
                    var data = response.plus_code.compound_code;
                    direccion = data;
            }

        }

        // Stops the location service if there is no need to query location updates continuously.
        Input.location.Stop();
    }

}