using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class cAccionAtaqueBasicoImprovisadas : cAccionAtaqueBasico
{
    new void Start()
    {
        GetObjets();
        nombre = "Ataque Basico Improvisado";
        categoria = cAcciones.AC_CAT_MARCIAL;
        var root = GameObject.Find("UI").GetComponent<UIDocument>().rootVisualElement;
        boton = root.Q<Button>("ButtonAtacarImpro");
    }

    override public int DeterminarNumeroDeDados()
    {
        int numeroDeDados = base.DeterminarNumeroDeDados();
        numeroDeDados -= (personaje.arma as cArmasPelea).tamañoDeArmaImprovisada;
        return numeroDeDados;
    }


    override public void RevisarLegalidad()
    {
        esLegal = c.HayEnemigosVivosEnRango() && (personaje.arma as cArmasPelea).armaImprovisadaActiva;
    }

    override protected void Tirando()
    {
        mostrarMensaje1 = true;
        tirada tr = cDieMath.TirarDados(dadosATirar);
        c.jugadorAtq = cDieMath.sumaDe3Mayores(tr);
        string resultado;
        string pierdeArma = "";
        if (c.jugadorAtq >= c.personajeObjetivo.GetGuardia())
        {
            resultado = "acierta";
            LlenarReaccionesPosibles();
            pierdeArma = " Su arma improvisada se destruye en el impacto.";
            (personaje.arma as cArmasPelea).PerderArmaImprovisada();
        }
        else
        {
            resultado = "fallando";
            if (personaje.zonaActual != c.personajeObjetivo.zonaActual)
            {
                pierdeArma = " Pierde su arma improvisada por usarla a rango.";
                (personaje.arma as cArmasPelea).PerderArmaImprovisada();
            }
            ab_state = AB_TERMINADO - 1;
        }
        uiC.SetText(c.personajeActivo.nombre + " saca " + c.jugadorAtq + ", " + resultado + " el ataque contra la guardia de " + c.personajeObjetivo.GetGuardia() + " de " + c.personajeObjetivo.nombre + "." + pierdeArma);
    }

}
