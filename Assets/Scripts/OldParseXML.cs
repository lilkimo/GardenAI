using UnityEngine;
using System.Collections;
using System.IO;
using System.Xml;
using UnityEngine.UI;
using Unity.VisualScripting;
using System.Collections.Generic;
using JetBrains.Annotations;

public class OldParseXML : MonoBehaviour{

    private TextAsset xmlRawFile;
    public Dictionary<string,List<string>> infoPlantas;

    void Start(){
        xmlRawFile = Resources.Load<TextAsset>("Info-plantas");
        string data = xmlRawFile.text;
        infoPlantas = parseXmlFile(data);
    }

    public Dictionary<string,List<string>> parseXmlFile(string xmlData){
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(new StringReader(xmlData));
        Dictionary<string,List<string>> infoPlantas = new Dictionary<string,List<string>>();

        string xmlPathPattern = "//info-plantas/planta";
        XmlNodeList MyNodeList = xmlDoc.SelectNodes(xmlPathPattern);
        foreach (XmlNode node in MyNodeList)
        {
            XmlNode nombre = node.FirstChild;
            XmlNode descripcion = nombre.NextSibling;
            XmlNode ubicacion = descripcion.NextSibling;
            XmlNode costo = ubicacion.NextSibling;
            XmlNode consumo = costo.NextSibling;
            XmlNode densidad = consumo.NextSibling;
            XmlNode mantencion = densidad.NextSibling;
            XmlNode resistencia = mantencion.NextSibling;

            List<string> datosPlanta = new List<string>(){
                descripcion.InnerXml,
                ubicacion.InnerXml,
                costo.InnerXml,
                consumo.InnerXml,
                densidad.InnerXml,
                mantencion.InnerXml,
                resistencia.InnerXml};

            infoPlantas.Add(nombre.InnerXml, datosPlanta);
        }
        return infoPlantas;
    }
}