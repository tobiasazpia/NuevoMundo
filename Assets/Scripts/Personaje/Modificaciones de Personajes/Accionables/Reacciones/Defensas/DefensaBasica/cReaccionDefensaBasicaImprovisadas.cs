using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cReaccionDefensaBasicaImprovisadas : cReaccionDefensaBasica
{
    void Start()
    {
        GetObjets();
        nombre = "Defensa Basica Improvisada";
    }

    override protected void Tirando()
    {
        Debug.Log("tirando");
        tirada tr = cDieMath.TirarDados(dadosATirar);
        defensa = cDieMath.sumaDe3Mayores(tr);
        c.jugadorDef = defensa;
        string resultado;
        string arma = "";
        if (c.atacando)
        {
            if (defensa >= c.jugadorAtq)
            {
                exito = true;
                resultado = "deteniendo";
                    arma = " El arma improvisada queda destruida.";
                (personaje.arma as cArmasPelea).PerderArmaImprovisada();
            }
            else
            {
                exito = false;
                resultado = "no pudiendo detener";
                if (personaje.GetZonaActual() != c.personajeActivo.GetZonaActual())
                {
                    arma = " El arma improvisada queda destruida.";
                    (personaje.arma as cArmasPelea).PerderArmaImprovisada();
                }
            }
            uiC.SetText(personaje.nombre + " saca " + defensa + ", " + resultado + " el ataque de " + c.jugadorAtq + "." + arma);
        }
        else
        {
            arma = " Perdio su arma al lanzarla.";
            if (defensa >= c.personajeActivo.GetGuardia())
            {
                resultado = "deteniendo";
                exito = true;
            }
            else
            {
                exito = false;
                resultado = "no pudiendo detener";
            }
            uiC.SetText(personaje.nombre + " saca " + defensa + ", " + resultado + " el movimiento de " + c.personajeActivo.nombre + "." + arma);
        }
    }
}
