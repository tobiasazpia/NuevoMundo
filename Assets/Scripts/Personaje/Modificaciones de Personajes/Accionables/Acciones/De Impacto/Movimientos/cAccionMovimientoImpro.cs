using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class cAccionMovimientoImpro : cAccionMovimientoAgresivo
{
    void Start()
    {
        GetObjets();
        nombre = "Movimiento Agresivo Improvisado";
        categoria = cAcciones.AC_CAT_MOVIMIENTO;
        var root = GameObject.Find("UI").GetComponent<UIDocument>().rootVisualElement;
        boton = root.Q<Button>("ButtonMoverImpro");
    }

    override public void Defensas()
    {
        Debug.Log("llamamos defensas de mov impro");
        base.Defensas();
        if (c.accionActiva == cPersonaje.AC_ATACAR)
        {
            Debug.Log("activa era atcar y la cambiamos a atacar impro");
            c.accionActiva = cPersonaje.AC_ATACARIMPRO;
        }
    }

    override public void RevisarLegalidad()
    {
        esLegal = c.HayEnemigosVivosEnZonasLimitrofes() && (personaje.arma as cArmasPelea).armaImprovisadaActiva;
    }
}
