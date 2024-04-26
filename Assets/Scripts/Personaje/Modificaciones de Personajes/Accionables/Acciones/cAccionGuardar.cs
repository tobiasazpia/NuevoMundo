using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class cAccionGuardar : cAcciones
{
    // Start is called before the first frame update
    protected void Start()
    {
        GetObjets();
        nombre = "Guardar";
        consecuencia = "Reservas tu acción para actuar en el futuro o intervenir en las acciones de otros.";
        reglas = nombre + ": Especial sin tirar. " + consecuencia;
        categoria = cAcciones.AC_CAT_GUARDAR;
        var root = GameObject.Find("UI").GetComponent<UIDocument>().rootVisualElement;
        boton = root.Q<Button>("ButtonGuardar");
        icon = c.GetComponent<cIconos>().Guardar;
        esLegal = true;
    }

    override public void Ejecutar()
    {
        personaje.guardando = true;
        personaje.GuardarGastarDado(c.accionesActivas); // Esto esta para que no pregunten denuevo en la misma fase si alguien hace algo y yo ya guarde
        uiC.SetText(UIInterface.NombreDePersonajeEnNegrita(personaje) + " guarda su accion.");
        c.EsperandoOkOn(true);
        c.stateID = cCombate.BUSCANDO_ACCION;
    }

    override public void RevisarLegalidad()
    {
        esLegal = c.faseActual != 10;
    }
}

        

        


