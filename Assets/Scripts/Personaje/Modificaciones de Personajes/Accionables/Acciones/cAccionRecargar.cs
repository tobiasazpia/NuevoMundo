using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class cAccionRecargar : cAcciones
{
    // Start is called before the first frame update
    void Start()
    {
        GetObjets();
        nombre = "Recargar";
        categoria = cAcciones.AC_CAT_MARCIAL;
        var root = GameObject.Find("UI").GetComponent<UIDocument>().rootVisualElement;
        boton = root.Q<Button>("ButtonRecargar");
    }

    override public void Ejecutar()
    {
        Debug.Log("usando estoAAA");
        (personaje.arma as cArmasFuego).cargada = true;
        Debug.Log("cargada: " + (personaje.arma as cArmasFuego).cargada);
        personaje.GuardarGastarDado(c.accionesActivas); // Esto esta para que no pregunten denuevo en la misma fase si alguien hace algo y yo ya guarde
        uiC.SetText(personaje.nombre + " se toma  un segundo para recargar su arma.");
        c.EsperandoOkOn(true);
        c.stateID = cCombate.BUSCANDO_ACCION;
    }

    override public void RevisarLegalidad()
    {
        if (personaje.arma is cArmasFuego)
        {
            esLegal = !(personaje.arma as cArmasFuego).cargada;
        }
        else esLegal = false;
    }
}
