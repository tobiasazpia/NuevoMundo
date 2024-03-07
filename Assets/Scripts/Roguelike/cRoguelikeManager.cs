using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class cRoguelikeManager : MonoBehaviour
{
    //Guarda la info permanente de la sesion de roguelike: la party y sus stats, y el nivel en el que estan
    public List<cPersonajeFlyweight> party;
    public int nivel;
    public cRoguelikeCombate rC;
    public UIRoguelikeEnd uiRE;

    // Start is called before the first frame update
    void Start()
    {
        nivel = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EmpezarCombate()
    {
        rC.ArmarCombate(party, nivel);
    }

    public void AgregarMiembroALaParty(string nombre, int[] eleccion)
    {
        cPersonajeFlyweight jugador = gameObject.AddComponent(typeof(cPersonajeFlyweight)) as cPersonajeFlyweight;
        jugador.equipo = 1;
        jugador.iA = cAI.PLAYER_CONTROLLED;

        jugador.nombre = nombre;
        jugador.arma = eleccion[0];
        jugador.esMaton = false;
        jugador.drama = true;

        switch (eleccion[1])
        {
            case 0:
                jugador.atr.maña = 1;
                break;
            case 1:
                jugador.atr.musculo = 1;
                break;
            case 2:
                jugador.atr.ingenio = 1;
                break;
            case 3:
                jugador.atr.brio = 1;
                break;
            case 4:
                jugador.atr.donaire = 1;
                break;
            default:
                break;
        }

        switch (eleccion[2])
        {
            case 0:
                jugador.hab.ataqueBasico = 1;
                break;
            case 1:
                jugador.hab.defensaBasica = 1;
                break;
            default:
                break;
        }

        party.Add(jugador);
    }
}
