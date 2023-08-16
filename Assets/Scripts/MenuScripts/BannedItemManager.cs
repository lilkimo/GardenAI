using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BannedItemManager : MonoBehaviour
{
    private string nombrePlanta;
    private string descripcionPlanta;
    private Sprite imagenPlanta;

    public string NombrePlanta { set => nombrePlanta = value;}
    public string DescripcionPlanta { set => descripcionPlanta = value;}
    public Sprite ImagenPlanta { set => imagenPlanta = value;}

    void Start()
    {
        transform.GetChild(0).GetComponent<Image>().sprite =  imagenPlanta;
        transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = nombrePlanta;
        transform.GetChild(2).GetComponent<TextMeshProUGUI>().text =  descripcionPlanta;

        var botton = transform.GetChild(0).GetComponent<Button>();
        botton.onClick.AddListener(UnbanPlant);
    }

    private void UnbanPlant(){
        //Habilitar planta de lista negra y deshabilitar en la blanca
    }
}
