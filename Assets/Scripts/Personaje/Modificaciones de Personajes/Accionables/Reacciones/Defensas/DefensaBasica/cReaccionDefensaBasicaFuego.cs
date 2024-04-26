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
        c.jugadorDef = defensa;
        string resultado;
        string def;
        if (c.atacando)
        {
            exito = defensa >= c.jugadorAtq;
            if (c.personajeObjetivo.nombre == personaje.nombre) {
            if (exito)
            {
                def = UIInterface.IntExitoso(defensa);
                resultado = "deteniendo";
            }
            else
            {
                def = UIInterface.IntFallido(defensa);
                resultado = "no pudiendo detener";
            }
            uiC.SetText(UIInterface.NombreDePersonajeEnNegrita(personaje) + " saca " + def + ", " + resultado + " el ataque de " + UIInterface.IntEnNegrita(c.jugadorAtq) + ".");
            }
            else
            {
                string arma = "";
                if (exito)
                {
                    resultado = "deteniendo";
                    def = UIInterface.IntExitoso(defensa);
                    if (!(personaje.arma as cArmasFuego).cargada)
                    {
                        arma = UIInterface.NombreDePersonajeEnNegrita(personaje) + " aprovecha para recargar su arma.";
                        (personaje.arma as cArmasFuego).cargada = true;
                    }
                }
                else
                {
                    resultado = "no pudiendo detener";
                    def = UIInterface.IntFallido(defensa);
                    arma = " El arma de " + UIInterface.NombreDePersonajeEnNegrita(personaje) + " queda descargada.";
                    (personaje.arma as cArmasFuego).cargada = false;
                }
                uiC.SetText(UIInterface.NombreDePersonajeEnNegrita(personaje) + " saca " + def + ", " + resultado + " el ataque de " + UIInterface.NombreDePersonajeEnNegrita(c.personajeActivo) + "." + arma);
            }
        }
        else
        {
            string arma = "";
            Debug.Log("usando estaaao");
            exito = defensa >= c.personajeActivo.GetGuardia();
            if (exito)
            {
                resultado = "deteniendo";
                def = UIInterface.IntExitoso(defensa);
                if (!(personaje.arma as cArmasFuego).cargada)
                {
                    arma = UIInterface.NombreDePersonajeEnNegrita(personaje) + " aprovecha para recargar su arma.";
                    (personaje.arma as cArmasFuego).cargada = true;
                }
            }
            else
            {
                resultado = "no pudiendo detener";
                def = UIInterface.IntFallido(defensa);
                arma = " El arma de " + UIInterface.NombreDePersonajeEnNegrita(personaje) + " queda descargada.";
                (personaje.arma as cArmasFuego).cargada = false;
            }
            uiC.SetText(UIInterface.NombreDePersonajeEnNegrita(personaje) + " saca " + def + ", " + resultado + " el movimiento de " + UIInterface.NombreDePersonajeEnNegrita(c.personajeActivo) + "." + arma);
        }
        if (personaje.Drama && !exito) { uiC.PedirDrama(); pidiendoDrama = true; }
    }
}