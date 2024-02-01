using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class cAccionable : MonoBehaviour
{
    public string nombre;
    public Button boton;
    public bool esLegal;

    public cPersonaje personaje;

    public UICombate uiC;
    public cCombate c;

    virtual public void Ejecutar() { }

    virtual public void RevisarLegalidad() { }

    public void GetObjets()
    {
        uiC = GameObject.Find("UI").GetComponent<UICombate>();
        c = GetComponentInParent<cCombate>();
    }

    virtual public int DeterminarNumeroDeDados()
    {
        return 0;
    }
}
