using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class cAccionEncontrarImprovisada : cAcciones
{
    protected void Start()
    {
        GetObjets();
        nombre = "Encontrar Improvisada";
        var root = GameObject.Find("UI").GetComponent<UIDocument>().rootVisualElement;
        boton = root.Q<Button>("ButtonConseguirImpro");
    }

    override public void Ejecutar()
    {
        int armaI = Random.Range(0, 3);
        string armaINombre = "";
        switch (armaI)
        {
            case 0:
                armaINombre = "arma improvisada pequeña";
                break;
            case 1:
                armaINombre = "arma improvisada mediana";
                break;
            case 2:
                armaINombre = "arma improvisada grande";
                break;
            default:
                break;
        }
        string text = (personaje.nombre + " busca un arma improvisada, y encuentra un " + armaINombre + ".");
        (personaje.arma as cArmasPelea).AdquirirArmaImprovisada(armaI);
        c.EsperandoOkOn(true);
        c.stateID = cCombate.BUSCANDO_ACCION;
        c.personajeActivo.GastarDado(c.faseActual, c.acciones, c.accionesActivas, c.accionesReactivas, text);
        uiC.ActualizarIniciativa(c.personajes);
    }

    override public void RevisarLegalidad()
    {
        esLegal = !(personaje.arma as cArmasPelea).armaImprovisadaActiva;
    }
}
