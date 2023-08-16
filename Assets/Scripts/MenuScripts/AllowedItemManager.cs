using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AllowedItemManager : MonoBehaviour
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
        botton.onClick.AddListener(BanPlant);
    }

    private void BanPlant(){
        string nombrePlanta = transform.GetChild(1).GetComponent<TextMeshProUGUI>().text;

        var topParent = transform.GetComponentInParent<RectTransform>().GetComponentInParent<RectTransform>().GetComponentInParent<RectTransform>();
        var contenedor = topParent.GetChild(0).GetChild(0).GetChild(0);

        int i = 0;
        while (true)
        {
            // GameObject bannedPlant = contenedor.GetChild(i).GetChild(0);
            // if (bannedPlant.GetComponent<TextMeshProUGUI>().text == nombrePlanta) {
            //     bannedPlant.
            // }
        }
        //Deshabilitar planta de lista blanca y habilitar en la negra
    }
}
