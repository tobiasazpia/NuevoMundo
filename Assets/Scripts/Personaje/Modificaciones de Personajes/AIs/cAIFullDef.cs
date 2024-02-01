using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cAIFullDef : cAI
{
    public override int ElegirAccion(List<cPersonaje> enemigosEnRango, List<int> zonasLimitrofesConEnemigos, int[] zonasLimitrofes, int faseActual)
    {
        if (faseActual != 10) return cPersonaje.AC_GUARDAR;
        p.uiC.RegistrarAccion();
        return BuscaYDestruye(enemigosEnRango, zonasLimitrofesConEnemigos, zonasLimitrofes);   
        //Es un poco extraño que en la fase 10 priorice movimientos agresivos sobre precavidos, pero bueno.
        //Capaz seria mala idea de todas formas
    }

    public override bool Reaccion(int atq)
    {
        return true;
    }
}
