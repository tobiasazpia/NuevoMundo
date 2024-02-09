using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscarmuzaManager : MonoBehaviour
{
    public cCombate combate;
    List<cPersonajeFlyweight> combatientes;

    public List<cPersonaje> personajes;
    public List<string> equipos;

    public void IniciarCombate()
    {
        combate.esRoguelike = false;
        if (combatientes != null)
        {
            if (combatientes.Count == 0)
            {
                MandarCombatientesDefault();
            }
        }
        else
        {
            MandarCombatientesDefault();
        }
        combate.NuevoCombate(combatientes);
    }

    void MandarCombatientesDefault()
    {
        combatientes = new List<cPersonajeFlyweight>();

        cPersonajeFlyweight a = gameObject.AddComponent(typeof(cPersonajeFlyweight)) as cPersonajeFlyweight;
        a.nombre = "Ariel";
        a.iA = cAI.PLAYER_CONTROLLED;
        a.arma = cArma.MEDIAS;
        a.equipo = 1;
        a.esMaton = false;
        //a.esMaton = true;
        a.atr.donaire = 1;
        a.hab.ataqueBasico = 2;
        a.hab.defensaBasica = 2;
        a.atr.musculo = 0;
        a.modGuardiaDeMaton = 0;
        a.atr.ingenio = 1;
        a.cantidad = 10;
        combatientes.Add(a);

        cPersonajeFlyweight b = gameObject.AddComponent(typeof(cPersonajeFlyweight)) as cPersonajeFlyweight;
        b.nombre = "Bart";
        b.iA = cAI.FULL_AGGRO;
        b.arma = cArma.MEDIAS;
        b.equipo = 2;
        b.esMaton = false;
        //b.esMaton = true;
        b.hab.ataqueBasico = 2;
        b.hab.defensaBasica = 2;
        b.atr.musculo = 1;
        b.atr.maña = 1;
        b.atr.donaire = 0;
        b.modGuardiaDeMaton = 0;
        b.atr.ingenio = 0;
        b.cantidad = 10;
        combatientes.Add(b);

        cPersonajeFlyweight c = gameObject.AddComponent(typeof(cPersonajeFlyweight)) as cPersonajeFlyweight;
        c.nombre = "Casio";
        c.iA = cAI.PLAYER_CONTROLLED;
        c.arma = cArma.MEDIAS;
        c.equipo = 1;
        c.esMaton = false;
        //c.esMaton = true;
        c.hab.ataqueBasico = 0;
        c.hab.defensaBasica = 5;
        c.atr.musculo = 2;
        c.atr.maña = 2;
        c.atr.ingenio = 2;
        c.modGuardiaDeMaton = -5;
        c.cantidad = 10;
        combatientes.Add(c);

        cPersonajeFlyweight d = gameObject.AddComponent(typeof(cPersonajeFlyweight)) as cPersonajeFlyweight;
        d.nombre = "Los Dementores";
        d.iA = cAI.FULL_AGGRO;
        d.arma = cArma.MEDIAS;
        d.equipo = 2;
        //b.esMaton = false;
        d.esMaton = true;
        d.hab.ataqueBasico = 5;
        d.hab.defensaBasica = 5;
        d.atr.musculo = 2;
        d.atr.maña = 2;
        d.modGuardiaDeMaton = -5;
        d.cantidad = 10;
        d.atr.ingenio = 2;
        combatientes.Add(d);

        cPersonajeFlyweight e = gameObject.AddComponent(typeof(cPersonajeFlyweight)) as cPersonajeFlyweight;
        e.nombre = "EE";
        e.iA = cAI.PLAYER_CONTROLLED;
        e.arma = cArma.ARCO;
        e.equipo = 1;
        combatientes.Add(e);

        cPersonajeFlyweight f = gameObject.AddComponent(typeof(cPersonajeFlyweight)) as cPersonajeFlyweight;
        f.nombre = "FF";
        f.iA = cAI.SMART_DEFENSIVO;
        f.arma = cArma.ARCO;
        f.equipo = 2;
        combatientes.Add(f);
    }
}