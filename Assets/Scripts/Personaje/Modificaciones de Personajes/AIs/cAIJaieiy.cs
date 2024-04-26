using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cAIJaieiy : cAI
{

    public override int ElegirAccion(List<cPersonaje> enemigosEnRango, List<int> zonasLimitrofesConEnemigos, int[] zonasLimitrofes, int faseActual)
    {
        if (faseActual == 10) return BuscaYDestruye(enemigosEnRango, zonasLimitrofesConEnemigos, zonasLimitrofes);
        if (enemigosEnRango.Count == 0) return cPersonaje.AC_GUARDAR;
        if (EsMiPrimeraAccion() && p.c.faseActual < 5) return cPersonaje.AC_IMPONER;
        return cPersonaje.AC_GUARDAR;
    }

    public override bool Reaccion(int atq)
    {
        if (atq > 27) return false;
        if (p.c.faseActual > 5) p.c.reaccionActiva = cPersonaje.DB_NerviosDeAcero;
        else p.c.reaccionActiva = cPersonaje.DB_DefensaBasica;
        return true;
    }

    public bool EsMiPrimeraAccion()
    {
        int acciones = 0;
        foreach (var item in p.dadosDeAccion)
        {
            if (item > 0 && item < 11)
            {
                acciones++;
            }
        }
        return acciones == 3;
    }
}



