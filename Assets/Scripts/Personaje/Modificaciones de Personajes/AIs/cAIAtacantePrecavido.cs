using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cAIAtacantePrecavido : cAI
{
    bool enPeligro = false;

    public override int ElegirAccion(List<cPersonaje> enemigosEnRango, List<int> zonasLimitrofesConEnemigos, int[] zonasLimitrofes, int faseActual)
    {
        //Tengo que reworkear esta AI en algun momento
        p.c.personajeObjetivo = PersonajeMasDañado(enemigosEnRango);
        if (p.c.personajeObjetivo != null)
        {
            if (p.c.personajeObjetivo.dadosDeAccion[0] > p.dadosDeAccion[0] || p.c.personajeObjetivo.Heridas > 0 || p.BonusPAtqBporDefB > 9 || p.c.personajeObjetivo.dadosDeAccion[0] > 10 || p.c.personajeObjetivo.dadosDeAccion[0] < 0)
            {
                p.uiC.RegistrarAccion(); // esto estaba abajo del return hasta hace poco
                return cPersonaje.AC_ATACAR;
            }
        }
        if (faseActual != 10) return cPersonaje.AC_GUARDAR; // Si nadie estaban tan dañado, ni estamos en la fase 10, guardamos
        p.uiC.RegistrarAccion();
        return BuscaYDestruye(enemigosEnRango, zonasLimitrofesConEnemigos, zonasLimitrofes); // si si estamos en la fase 10 hay que atacar
    }
    
    public override bool Reaccion(int atq)
    {
        if (!enPeligro)
        {
            enPeligro = (p.Heridas > 1);
        }
        int dadosExtrasDefensa = p.atr.ingenio + p.hab.defensaBasica + p.defensaBasicaDadosExtra;
        if (atq < 20 + dadosExtrasDefensa || enPeligro)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}


