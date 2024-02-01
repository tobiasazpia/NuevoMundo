using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cAIFullAggro : cAI
{
    public override int ElegirAccion(List<cPersonaje> enemigosEnRango, List<int> zonasLimitrofesConEnemigos, int[] zonasLimitrofes, int faseActual)
    {
        p.uiC.RegistrarAccion();
        return BuscaYDestruye(enemigosEnRango, zonasLimitrofesConEnemigos, zonasLimitrofes);
    }
    
    public override bool Reaccion(int atq)
    {
        return false;
    }
}
