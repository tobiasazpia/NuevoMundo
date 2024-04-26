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

        cPersonajeFlyweight hory = gameObject.AddComponent<cPersonajeFlyweight>();
        hory.nombre = "Hory";
        hory.arma = cArma.JAIEIY;
        hory.iA = cAI.PLAYER_CONTROLLED;
        hory.tradicionMarcialId = 2;
        hory.equipo = 1;
        hory.tradicionMarcial[0] = 4;
        hory.tradicionMarcial[1] = 3;
        hory.tradicionMarcial[2] = 3;
        hory.tradicionMarcial[3] = 3;
        hory.tradicionMarcial[4] = 5;
        hory.tradicionMarcial[5] = 3;
        hory.atr.ingenio = 2;
        hory.atr.maña = 1;
        hory.atr.brio = 1;
        hory.atr.donaire = 1;
        hory.atr.musculo = 1;
        combatientes.Add(hory);

        cPersonajeFlyweight marco = gameObject.AddComponent<cPersonajeFlyweight>();
        marco.nombre = "Marco";
        marco.arma = cArma.VOLUNTAD_CREADOR;
        marco.iA = cAI.PLAYER_CONTROLLED;
        marco.tradicionMarcialId = 1;
        marco.equipo = 2;
        marco.tradicionMarcial[0] = 5;
        marco.tradicionMarcial[1] = 4;
        marco.tradicionMarcial[2] = 3;
        marco.tradicionMarcial[3] = 3;
        marco.tradicionMarcial[4] = 3;
        marco.tradicionMarcial[5] = 3;
        marco.maestria = cArma.CalcularMaestria(marco.tradicionMarcial);
        marco.atr.maña = 1;
        marco.atr.musculo = 2;
        marco.atr.ingenio = 1;
        marco.atr.brio = 1;
        marco.atr.donaire = 1;
        combatientes.Add(marco);


        //cPersonajeFlyweight promesaDeAtyYvatevo = gameObject.AddComponent<cPersonajeFlyweight>();
        //promesaDeAtyYvatevo.nombre = "Promesa de Aty Yvatevo";
        //promesaDeAtyYvatevo.arma = cArma.JAIEIY;
        //promesaDeAtyYvatevo.iA = cAI.PLAYER_CONTROLLED;
        //promesaDeAtyYvatevo.equipo = 1;
        //promesaDeAtyYvatevo.tradicionMarcialId = 2;
        //promesaDeAtyYvatevo.tradicionMarcial[0] = 3;
        //promesaDeAtyYvatevo.tradicionMarcial[1] = 2;
        //promesaDeAtyYvatevo.tradicionMarcial[2] = 0;
        //promesaDeAtyYvatevo.tradicionMarcial[3] = 2;
        //promesaDeAtyYvatevo.tradicionMarcial[4] = 0;
        //promesaDeAtyYvatevo.tradicionMarcial[5] = 0;
        //promesaDeAtyYvatevo.maestria = cArma.CalcularMaestria(promesaDeAtyYvatevo.tradicionMarcial);
        //promesaDeAtyYvatevo.atr.donaire = 1;
        //promesaDeAtyYvatevo.atr.musculo = 1;
        //combatientes.Add(promesaDeAtyYvatevo);

        //cPersonajeFlyweight heroeTonatio = gameObject.AddComponent<cPersonajeFlyweight>();
        //heroeTonatio.nombre = "Heroe Tonatio";
        //heroeTonatio.arma = cArma.MEDIAS;
        //heroeTonatio.iA = cAI.FULL_AGGRO;
        //heroeTonatio.equipo = 2;
        //heroeTonatio.tradicionMarcial[0] = 3;
        //heroeTonatio.atr.musculo = 1;
        //heroeTonatio.atr.donaire = 1;
        //combatientes.Add(heroeTonatio);

        //cPersonajeFlyweight guardiaRealDeUrqualia = gameObject.AddComponent<cPersonajeFlyweight>();
        //guardiaRealDeUrqualia.nombre = "Guardia Real de Urqualia";
        //guardiaRealDeUrqualia.arma = cArma.MEDIAS;
        //guardiaRealDeUrqualia.iA = cAI.PLAYER_CONTROLLED;
        //guardiaRealDeUrqualia.equipo = 2;
        //guardiaRealDeUrqualia.esMaton = true;
        //guardiaRealDeUrqualia.tradicionMarcial[0] = 5;
        //guardiaRealDeUrqualia.tradicionMarcial[1] = 5;
        //guardiaRealDeUrqualia.atr.maña = 2;
        //guardiaRealDeUrqualia.atr.musculo = 0;
        //guardiaRealDeUrqualia.atr.ingenio = 2;
        //guardiaRealDeUrqualia.atr.brio = 2;
        //guardiaRealDeUrqualia.atr.donaire = 2;
        //guardiaRealDeUrqualia.cantidad = 10;
        //combatientes.Add(guardiaRealDeUrqualia);

        //cPersonajeFlyweight b = gameObject.AddComponent(typeof(cPersonajeFlyweight)) as cPersonajeFlyweight;
        //b.nombre = "Bart";
        //b.iA = cAI.PLAYER_CONTROLLED;
        //b.arma = cArma.MEDIAS;
        //b.equipo = 2;
        //b.esMaton = true;
        //b.tradicionMarcial[0] = 5;
        //b.tradicionMarcial[1] = 5;
        //b.atr.musculo = 2;
        //b.atr.maña = 2;
        //b.atr.brio = 2;
        //b.modGuardiaDeMaton = 0;
        //b.atr.ingenio = 0;
        //b.cantidad = 10;
        //combatientes.Add(b);


        //cPersonajeFlyweight c = gameObject.AddComponent(typeof(cPersonajeFlyweight)) as cPersonajeFlyweight;
        //c.nombre = "Casio";
        //c.iA = cAI.FULL_AGGRO;
        //c.arma = cArma.MEDIAS;
        //c.equipo = 2;
        //c.esMaton = false;
        //c.tradicionMarcial[0] = 3;
        //c.tradicionMarcial[1] = 3;
        //c.atr.musculo = 2;
        //c.atr.maña = 2;
        //c.atr.ingenio = 2;
        //c.atr.brio = 2;
        //c.atr.donaire = 2;
        //c.modGuardiaDeMaton = 0;
        //c.cantidad = 10;
        //combatientes.Add(c);

        //cPersonajeFlyweight d = gameObject.AddComponent(typeof(cPersonajeFlyweight)) as cPersonajeFlyweight;
        //d.nombre = "Los Dementores";
        //d.iA = cAI.FULL_AGGRO;
        //d.arma = cArma.MEDIAS;
        //d.equipo = 2;
        ////b.esMaton = false;
        //d.esMaton = true;
        //d.tradicionMarcial[0] = 5;
        //d.tradicionMarcial[1] = 5;
        //d.atr.musculo = 2;
        //d.atr.maña = 2;
        //d.modGuardiaDeMaton = -5;
        //d.cantidad = 10;
        //d.atr.ingenio = 2;
        //combatientes.Add(d);

        //cPersonajeFlyweight e = gameObject.AddComponent(typeof(cPersonajeFlyweight)) as cPersonajeFlyweight;
        //e.nombre = "EE";
        //e.iA = cAI.PLAYER_CONTROLLED;
        //e.arma = cArma.ARCO;
        //e.equipo = 1;
        //combatientes.Add(e);

        //cPersonajeFlyweight f = gameObject.AddComponent(typeof(cPersonajeFlyweight)) as cPersonajeFlyweight;
        //f.nombre = "FF";
        //f.iA = cAI.SMART_DEFENSIVO;
        //f.arma = cArma.ARCO;
        //f.equipo = 2;
        //combatientes.Add(f);
    }
}