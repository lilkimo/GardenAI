using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DataManager : MonoBehaviour
{
    [SerializeField]
    private List<Item> items = new List<Item>();
    [SerializeField]
    private GameObject buttonContainer;
    [SerializeField]
    private ItemButtonManager itemButtonManager;

    [SerializeField]
    private MenuManager menuManager;
    [SerializeField]
    private PlaceController placeController;

    public string isSelected;

    // Start is called before the first frame update
    void Start()
    {
        if(menuManager == null)
        {
            Debug.Log("DataManager no puede acceder a MenuManager.");
        }
        menuManager.OnPlantas += CreateButtons;
    }

    private void CreateButtons()
    {
        // Habría que hacer que automáticamente se seleccione la primera planta de la lista.
        // (Es más fácil que programar qué pasaría si quieres poner una planta pero no has
        // ninguna seleccionada)
        placeController.SetPlant(items[0].Item3DModel, items[0].Consumption);
        foreach (var item in items)
        {
            ItemButtonManager itemButton = Instantiate(itemButtonManager, buttonContainer.transform);
            itemButton.Init(item.ItemName, item.ItemDescription, item.ItemImage);
            itemButton.button.onClick.AddListener( () => {
                placeController.SetPlant(item.Item3DModel, item.Consumption);
                isSelected = item.ItemName;
                itemButton.changeColor();
            });
        }
        menuManager.OnPlantas -= CreateButtons;
    }
}
