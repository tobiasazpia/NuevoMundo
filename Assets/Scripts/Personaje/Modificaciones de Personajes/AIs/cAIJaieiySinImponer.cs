using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cAIJaieiySinImponer : cAI
{
    public override int ElegirAccion(List<cPersonaje> enemigosEnRango, List<int> zonasLimitrofesConEnemigos, int[] zonasLimitrofes, int faseActual)
    {
        if (faseActual == 10) return BuscaYDestruye(enemigosEnRango, zonasLimitrofesConEnemigos, zonasLimitrofes);
        return cPersonaje.AC_GUARDAR;
    }

    public override bool Reaccion(int atq)
    {
        if (atq > 27) return false;
        if (p.c.faseActual > 5) p.c.reaccionActiva = cPersonaje.DB_NerviosDeAcero;
        else p.c.reaccionActiva = cPersonaje.DB_DefensaBasica;
        return true;
    }
}




