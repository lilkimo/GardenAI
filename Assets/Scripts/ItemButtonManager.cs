using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]

public class ItemButtonManager : MonoBehaviour
{
    private GameObject DM;
    private DataManager dataManager;

    private Button _button;
    public Button button { get => _button; }

    private void Awake()
    {
        _button = GetComponent<Button>();
        DM = GameObject.Find("DataManager");
        dataManager = DM.GetComponent<DataManager>();
    }

    public void Init(string name, string description, Sprite image)
    {
        transform.GetChild(0).GetComponent<Text>().text = name;
        transform.GetChild(1).GetComponent<RawImage>().texture = image.texture;
        transform.GetChild(2).GetComponent<Text>().text = description;
    }

    void Update(){
        // obtener valor isSelected del datamanager
        if( dataManager.isSelected !=  transform.GetChild(0).GetComponent<Text>().text){
            var colors = _button.colors;
            colors.normalColor = Color.gray;
            GetComponent<Button>().colors = colors; 
        }
    }

    public void changeColor(){
        var colors = GetComponent<Button>().colors;
        colors.normalColor = Color.green;
        GetComponent<Button>().colors = colors; 
    }
}
