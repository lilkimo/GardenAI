using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DataManager : MonoBehaviour
{
    [SerializeField] private List<Item> items = new List<Item>();
    [SerializeField] private GameObject buttonContainer;
    [SerializeField] private ItemButtonManager itemButtonManager;

    [SerializeField] private MenuManager Instance;

    // Start is called before the first frame update
    void Start()
    {
        if(Instance == null){
            Debug.Log("DataManager no puede acceder a MenuManager.");
        }
        Instance.OnPlantas += CreateButtons;
    }

    private void CreateButtons(){
        foreach (var item in items){
            ItemButtonManager itemButton;
            itemButton = Instantiate(itemButtonManager, buttonContainer.transform);
            itemButton.ItemName = item.ItemName;
            itemButton.ItemDescription = item.ItemDescription;
            itemButton.ItemImage = item.ItemImage;
            itemButton.Item3DModel = item.Item3DModel;
            itemButton.name = item.ItemName;
        }

        Instance.OnPlantas -= CreateButtons;
    }
}
