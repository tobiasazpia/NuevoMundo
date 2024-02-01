using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class cAI : MonoBehaviour
{
    public cPersonaje p;

    //IA de Perssonajes // Mandar a AI?
    public const int PLAYER_CONTROLLED = 0;
    public const int FULL_AGGRO = 1;                // ATACA SIEMPRE
    public const int FULL_DEFENSIVO = 2;      // SE DEFIENDE SI PUEDE GASTAR 1 SOLO DADO
    public const int SMART_DEFENSIVO = 4;           // ATACA SI EL ENEMIGO TIENE 2 HERIDAS DRAMATICAS y NO DEFIENDE >25
    public const int ATACANTE_PRECAVIDO = 5;        // ATACA SI EL ENEMIGO TIENE 2 HERIDAS DRAMATICAS y NO DEFIENDE >25

    public abstract int ElegirAccion(List<cPersonaje> enemigosEnRango, List<int> zonasLimitrofesConEnemigos, int[] zonasLimitrofes, int faseActual);
    public abstract bool Reaccion(int atq);

    public virtual int BuscaYDestruye(List<cPersonaje> enemigosEnRango, List<int> zonasLimitrofesConEnemigos, int[] zonasLimitrofes)
    {
        // Trata de atacar siempre que puede, decidiendo al azar a quien.
        // Si no puede trata de hacer un movimiento agresivo, y si tampco puede hace un movimiento precavido a una zona al azar
        if (enemigosEnRango.Count > 0)
        {
            p.c.personajeObjetivo = enemigosEnRango[Random.Range(0, enemigosEnRango.Count)];
            return cPersonaje.AC_ATACAR;
        }

        // Si seguimos aca es porque no habia nadie para atacar en nuestra zona, pasamos a movimiento agresivo.
        enemigosEnRango.Clear();
        int numeroDeZonasConEnemigos = zonasLimitrofesConEnemigos.Count;
        if (numeroDeZonasConEnemigos > 0 && !p.arma.GetDeRango())
        {
            p.c.movAgro = true;
            p.c.movPrec = false;
            p.c.zonaObjetiva = zonasLimitrofesConEnemigos[Random.Range(0, numeroDeZonasConEnemigos)];
            foreach (var per in p.c.personajes)
            {
                if (per.vivo && per.equipo != p.equipo && per.zonaActual == p.c.zonaObjetiva)
                {
                    enemigosEnRango.Add(per);
                    break;
                }
            }
            p.c.personajeObjetivo = enemigosEnRango[Random.Range(0, enemigosEnRango.Count)];
            return cPersonaje.AC_MOVAGRE;
        }

        //Si seguimos aca es porque no habia ninguna zona limitrofe con objeticos validos, pasamos a movimiento precavido
        p.c.zonaObjetiva = zonasLimitrofes[Random.Range(0, zonasLimitrofes.Length)];
        p.c.movPrec = true;
        p.c.movAgro = false;
        return cPersonaje.AC_MOVPREC;

        //no return 4 - Nunca guarda
    }

    public virtual cPersonaje PersonajeMasDaņado(List<cPersonaje> listaPersonajes)
    {
        cPersonaje menosVida = null; // Enemigo mas daņado
        foreach (var per in listaPersonajes)
        {
            if (menosVida == null)
            {
                menosVida = per;
            }
            else
            {
                if (menosVida.hDram < per.hDram)
                {
                    menosVida = per;
                }
                else if (menosVida.hDram == per.hDram)
                {
                    if (menosVida.hSupe < per.hSupe)
                    {
                        menosVida = per;
                    }
                    else if (menosVida.hSupe == per.hSupe)
                    {
                        cPersonaje[] pl = { menosVida, per };
                        menosVida = pl[Random.Range(0, 2)];
                    }
                }
            }
        }
        return menosVida;
    }

}
