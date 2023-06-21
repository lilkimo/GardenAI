using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ItemButtonManager : MonoBehaviour
{
    private Button _button;
    public Button button { get => _button; }

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    public void Init(string name, string description, Sprite image)
    {
        transform.GetChild(0).GetComponent<Text>().text = name;
        transform.GetChild(1).GetComponent<RawImage>().texture = image.texture;
        transform.GetChild(2).GetComponent<Text>().text = description;
    }
}
