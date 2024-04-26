using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class cAccionable : MonoBehaviour
{
    public string nombre;
    public Button boton;
    public bool esLegal;
    public int acc_state;
    public int reroleandoState;
    public bool reroleando = false;
    public int dadosExtrasParaReroll = 0;
    public int index;

    public string reglas;
    public string consecuencia;

    public cPersonaje personaje;

    public UICombate uiC;
    public cCombate c;

    public Texture2D icon;

    virtual public void Ejecutar() { }

    virtual public void SetUp() { }

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

    public void UsaDrama()
    {
        uiC.SetText("¡Usamos Drama! Vamos a volver a tirar los dados.");
        acc_state = reroleandoState;
        uiC.perCambio = personaje.nombre;
        personaje.Drama = false;
        reroleando = true;
        if (personaje.tieneTradicionMarcial)
        {
            Debug.Log("reroll true y tradicion amrcail");
            dadosExtrasParaReroll = personaje.tradicionMarcial[5];      
        }
        c.EsperandoOkOn(true);
    }
}
