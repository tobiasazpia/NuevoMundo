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
        (personaje.arma as cArmasFuego).cargada = true;
        string text = personaje.nombre + " se toma un segundo para recargar su arma.";
        c.personajeActivo.GastarDado(c.faseActual, c.acciones, c.accionesActivas, c.accionesReactivas, text);
        uiC.ActualizarIniciativa(c.personajes);
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
