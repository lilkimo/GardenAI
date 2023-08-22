using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BlackListManager : MonoBehaviour
{
    [SerializeField] private GameObject whiteList;
    [SerializeField] private GameObject blackList;

    public void BanPlant(){
        // Itero sobre cada planta en la whitelist
        foreach(Transform child in whiteList.transform)
        {
            if (child.GetComponent<Toggle>().isOn) {

                Debug.Log("Planta baneada: "+ child);

                child.GetComponent<Toggle>().isOn = false;

                GameObject baneado = Instantiate(child.gameObject, blackList.transform);
                baneado.transform.SetParent(blackList.transform, false);
                
                child.gameObject.SetActive(false);
            }
        }

    }

    public void UnbanPlant(){
        foreach(Transform child in blackList.transform){
            if(!child.GetComponent<Toggle>().isOn) continue;
            
            string nombre = child.GetChild(0).GetComponent<TextMeshProUGUI>().text;
            foreach(Transform plant in whiteList.transform){
                if(plant.GetChild(0).GetComponent<TextMeshProUGUI>().text == nombre){
                    plant.gameObject.SetActive(true);
                    Destroy(child.gameObject);
                }
            }
        }
    }
}