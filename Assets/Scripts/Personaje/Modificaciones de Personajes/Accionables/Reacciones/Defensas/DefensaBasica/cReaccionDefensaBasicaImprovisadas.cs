using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class cReaccionDefensaBasicaImprovisadas : cReaccionDefensaBasica
{
    void Start()
    {
        GetObjets();
        nombre = "Defensa Básica Improvisada";
        consecuencia = "Usás tu Arma Improvisada para intervenir en otra Zona.";
        reglas = nombre + ": Defensa. " + consecuencia;
        icon = c.GetComponent<cIconos>().Improvisada;
    }

    override protected void Tirando()
    {
        tirada tr = cDieMath.TirarDados(dadosATirar);
        defensa = cDieMath.sumaDe3Mayores(tr);
        c.jugadorDef = defensa;
        string resultado;
        string arma = "";
        string def;
        if (c.atacando)
        {
            exito = defensa >= c.jugadorAtq;
            if (exito)
            {
                def = UIInterface.IntExitoso(defensa);
                resultado = "deteniendo";
                    arma = " El arma improvisada queda destruida.";
                (personaje.arma as cArmasPelea).PerderArmaImprovisada();
            }
            else
            {
                def = UIInterface.IntFallido(defensa);
                resultado = "no pudiendo detener";
                if (personaje.GetZonaActual() != c.personajeActivo.GetZonaActual())
                {
                    arma = " El arma improvisada queda destruida.";
                    (personaje.arma as cArmasPelea).PerderArmaImprovisada();
                }
            }
            uiC.SetText(UIInterface.NombreDePersonajeEnNegrita(personaje) + " saca " + def + ", " + resultado + " el ataque de " + UIInterface.IntEnNegrita(c.jugadorAtq) + "." + arma);
        }
        else
        {
            arma = " Perdio su arma al lanzarla.";
            exito = defensa >= c.personajeActivo.GetGuardia();
            if (exito)
            {
                resultado = "deteniendo";
                def = UIInterface.IntExitoso(defensa);
            }
            else
            {
                def = UIInterface.IntFallido(defensa);
                resultado = "no pudiendo detener";
            }
            uiC.SetText(UIInterface.NombreDePersonajeEnNegrita(personaje) + " saca " + def + ", " + resultado + " el movimiento de " + UIInterface.NombreDePersonajeEnNegrita(c.personajeActivo) + "." + arma);
        }
        if (personaje.Drama && !exito)
        {
            uiC.PedirDrama(); pidiendoDrama = true;
        }
    }
}
