using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cReaccionDefensaBasicaFuego : cReaccionDefensaBasica
{
    override public int DeterminarNumeroDeDados()
    {
        int numeroDeDados = base.DeterminarNumeroDeDados();
        if (c.HayEnemigosEnMelee(personaje))
        {
            numeroDeDados -= 1;
        }
        return numeroDeDados;
    }

    override protected void Tirando()
    {
        Debug.Log("tirando");
        tirada tr = cDieMath.TirarDados(dadosATirar);
        defensa = cDieMath.sumaDe3Mayores(tr);
        string resultado;
        if (c.atacando)
        {
            if (defensa >= c.jugadorAtq)
            {
                exito = true;
                resultado = "deteniendo";
            }
            else
            {
                exito = false;
                resultado = "no pudiendo detener";
            }
            uiC.SetText(personaje.nombre + " saca " + defensa + ", " + resultado + " el ataque de " + c.jugadorAtq + ".");
        }
        else
        {
            string arma = "";
            Debug.Log("usando estaaao");
            if (defensa >= c.personajeActivo.GetGuardia())
            {
                resultado = "deteniendo";
                exito = true;
                if (!(personaje.arma as cArmasFuego).cargada)
                {
                    arma = personaje.nombre + " aprovecha para recargar su arma.";
                    (personaje.arma as cArmasFuego).cargada = true;
                }
            }
            else
            {
                exito = false;
                resultado = "no pudiendo detener";
                if (c.personajeObjetivo.nombre != personaje.nombre)
                {
                    arma = " El arma de " + personaje.nombre + " queda descargada.";
                    (personaje.arma as cArmasFuego).cargada = false;
                }
            }
            uiC.SetText(personaje.nombre + " saca " + defensa + ", " + resultado + " el movimiento de " + c.personajeActivo.nombre + "." + arma);
        }
    }
}