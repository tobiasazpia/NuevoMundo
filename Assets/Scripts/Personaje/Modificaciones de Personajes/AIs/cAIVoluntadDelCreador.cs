using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cAIVoluntadDelCreador : cAI
{
    bool enPeligro = false;

    public override int ElegirAccion(List<cPersonaje> enemigosEnRango, List<int> zonasLimitrofesConEnemigos, int[] zonasLimitrofes, int faseActual)
    {
        p.c.personajeObjetivo = PersonajeMasDañado(enemigosEnRango); 
        if (OlemosSangre())
        {
            p.uiC.RegistrarAccion();
            return cPersonaje.AC_ATACAR;
        }
        if (HayEnemigoDebuffeado(enemigosEnRango))
        {
            p.uiC.RegistrarAccion();
            return cPersonaje.AC_ATACAR;
        }
        if (EsMiUltimaAccion())
        {
            p.uiC.RegistrarAccion();
            return BuscaYDestruye(enemigosEnRango, zonasLimitrofesConEnemigos, zonasLimitrofes);
        }
        if (faseActual == 10)
        {
            p.uiC.RegistrarAccion();
            if (enemigosEnRango.Count < 1) return BuscaYDestruye(enemigosEnRango, zonasLimitrofesConEnemigos, zonasLimitrofes);
            // si estmos en la fase 10, no es nuestra ultima accion, no hay enemigos debuffeados, peor hay al menos uno en rango vamos a debufear uno
            return cPersonaje.AC_IRADIVINA;
        }
        return cPersonaje.AC_GUARDAR;
    }

    public override bool Reaccion(int atq)
    {
        enPeligro = (p.c.personajeObjetivo.Heridas > 1) && p.c.personajeObjetivo.Daño > 0;
        int dadosExtrasDB= p.atr.ingenio + p.tradicionMarcial[1];
        int dadosExtrasTerror = p.atr.ingenio + p.tradicionMarcial[3];
        if (enPeligro)
        {
            if (dadosExtrasDB > dadosExtrasTerror) p.c.reaccionActiva = cPersonaje.DB_DefensaBasica;
            else p.c.reaccionActiva = cPersonaje.DB_DefensaTerrorDeDios;
            return true;
        }
        else
        {
            Debug.Log("olemos sangre? " + OlemosSangre());
            Debug.Log("ya tiene debuff? " + AtacanteTieneDebuff());
            if (!OlemosSangre() && !AtacanteTieneDebuff())
            {
                p.c.reaccionActiva = cPersonaje.DB_DefensaTerrorDeDios;
                return true;
            }
        }
        return false;
    }

    public bool EsMiUltimaAccion()
    {
        bool unaAccion = false;
        foreach (var item in p.dadosDeAccion)
        {
            if(item > 0 && item < 11)
            {
                if (unaAccion) return false;
                unaAccion = true;
            } 
        }
        return true;
    }

    public bool HayEnemigoDebuffeado(List<cPersonaje> enemigos)
    {
        foreach (var item in enemigos)
        {
            if(item.tieneIraDivina || item.tieneTerror)
            {
                p.c.personajeObjetivo = item;
                return true;
            }
        }
        return false;
    }

    public bool OlemosSangre()
    {
        if (p.c.personajeObjetivo == null) return false;
        if(p.c.personajeObjetivo is cMatones)
        {
            if ((p.c.personajeObjetivo as cMatones).Cantidad < 2) return true;
        }
        else
        {
            if (p.c.personajeObjetivo.Heridas > 1) return true;
        }
        return false;
    }

    public bool YaSeAplicoDebuff()
    {
        foreach (var item in p.c.personajes)
        {
            if (item.equipo != p.equipo && (item.tieneTerror || item.tieneIraDivina) && item.zonaActual == p.zonaActual) return true;
        }
        return false;
    }

    public bool AtacanteTieneDebuff()
    {
        return p.c.personajeActivo.tieneIraDivina || p.c.personajeActivo.tieneTerror;
    }
}



