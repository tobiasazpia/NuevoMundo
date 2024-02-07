using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cRoguelikeCombate : MonoBehaviour
{
    //Se encarga de generar un combate con la data roguelike manager entre la party, y unos enemigos generados a partir del nivel en el que estamos
    public cCombate combate;
    public cRoguelikeManager rM;
    List<cPersonajeFlyweight> enemigos;
    public List<cPersonajeFlyweight> templatesMatones;
    public List<cPersonajeFlyweight> templatesPersonajes;

    // Start is called before the first frame update
    void Start()
    {
        combate = FindAnyObjectByType<cCombate>();
        if (enemigos != null) enemigos.Clear();
        enemigos = new List<cPersonajeFlyweight>();
        LlenarTemplateDeMatones();
        LlenarTempatePersonajes();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ArmarCombate(List<cPersonajeFlyweight> party, int nivel)
    {
        //uuh, idea divertida: que haya una base comun para enemigos y aliados, pero cuando reclutes a un tipo de maton / a un personaje, ya no te puede tocar para  que luches contra el
        //manda a generar enemigos para armar el equipo
        List<cPersonajeFlyweight> combatientes = new List<cPersonajeFlyweight>();
        int presupuesto = nivel * 3;
        do
        {
            presupuesto = AgregarEnemigo(presupuesto);
        } while (presupuesto > 5 && enemigos.Count < 3);
        combatientes.AddRange(party);
        combatientes.AddRange(enemigos);
        combate.esRoguelike = true;
        combate.NuevoCombate(combatientes);
        foreach (var item in enemigos)
        {
            Destroy(item);
        }
        enemigos.Clear();
    }

    int AgregarEnemigo(int presupuesto)
    {
        //Agrega un enemigo el team enemigo
        cPersonajeFlyweight nuevoEnemigo = gameObject.AddComponent(typeof(cPersonajeFlyweight)) as cPersonajeFlyweight;
        nuevoEnemigo.equipo = 2;
        if (presupuesto < 14)
        {
            ElegirMaton(nuevoEnemigo, ref presupuesto);
        }
        else
        {
            int tipoEnemigo = Random.Range(0, 2);
            if (tipoEnemigo == 0)
            {
                ElegirMaton(nuevoEnemigo, ref presupuesto);
            }
            else
            {
                ElegirPersonaje(nuevoEnemigo, ref presupuesto);
            }
        }
        return presupuesto;
    }

    void ElegirMaton(cPersonajeFlyweight maton, ref int presupuesto)
    {
        maton.esMaton = true;
        List<int> noDisp = MatonesNoDisponibles();
        int index = Random.Range(0, templatesMatones.Count - noDisp.Count);
        foreach (var item in noDisp)
        {
            if (index >= item) index++;
        }
        maton.cantidad = Random.Range(1, presupuesto / 2 + 1);
        maton.Copiar(templatesMatones[index]);
        enemigos.Add(maton);
        presupuesto -= maton.cantidad * 3;
    }


    void ElegirPersonaje(cPersonajeFlyweight personaje, ref int presupuesto)
    {
        List<int> noDisp = PersonajesNoDisponibles();
        int index = Random.Range(0, templatesPersonajes.Count-noDisp.Count);
        foreach (var item in noDisp)
        {
            if (index >= item) index++;
        }
        personaje.esMaton = false;
        personaje.Copiar(templatesPersonajes[index]);
        enemigos.Add(personaje);
        presupuesto -= 14;
    }

    List<int> PersonajesNoDisponibles()
    {
        List<int> perIndex = new List<int>();
        perIndex.AddRange(PersonajesYaEnEquipo());
        for (int i = 0; i < enemigos.Count; i++)
        {
            if (!enemigos[i].esMaton)
            {
                for (int j = 0; j < templatesPersonajes.Count; j++)
                {
                    if (enemigos[i].nombre == templatesPersonajes[j].nombre) perIndex.Add(j);
                }
            }
        }
        perIndex.Sort();
        return perIndex;
    }

    public List<int> PersonajesYaEnEquipo()
    {
        List<int> perIndex = new List<int>();
        for (int i = 1; i < rM.party.Count; i++)
        {
            if (!rM.party[i].esMaton)
            {
                for (int j = 0; j < templatesPersonajes.Count; j++)
                {
                    if (rM.party[i].nombre == templatesPersonajes[j].nombre) perIndex.Add(j);
                }
            }
        }
        return perIndex;
    }

    public List<int> MatonesNoDisponibles()
    {
        List<int> perIndex = new List<int>();
        perIndex.AddRange(MatonesYaEnEquipo());
        for (int i = 0; i < enemigos.Count; i++)
        {
            if (enemigos[i].esMaton)
            {
                for (int j = 0; j < templatesMatones.Count; j++)
                {
                    if (enemigos[i].nombre == templatesMatones[j].nombre) perIndex.Add(j);
                }
            }
        }
        perIndex.Sort();
        return perIndex;
    }

    public List<int> MatonesYaEnEquipo()
    {
        List<int> perIndex = new List<int>();
        for (int i = 1; i < rM.party.Count; i++)
        {
            if (rM.party[i].esMaton)
            {
                for (int j = 0; j < templatesMatones.Count; j++)
                {
                    if (rM.party[i].nombre == templatesMatones[j].nombre) perIndex.Add(j);
                }
            }
        }
        return perIndex;
    }

        void LlenarTemplateDeMatones()
    {
        cPersonajeFlyweight campesinosLatios = gameObject.AddComponent<cPersonajeFlyweight>();
        campesinosLatios.nombre = "Campesinos Latios";
        campesinosLatios.arma = cArma.PESADAS;
        campesinosLatios.iA = cAI.FULL_AGGRO;
        campesinosLatios.hab.ataqueBasico = 1;
        campesinosLatios.hab.defensaBasica = 2;
        campesinosLatios.atr.maña = 1;
        templatesMatones.Add(campesinosLatios);

        cPersonajeFlyweight pandillerosUrqualianos = gameObject.AddComponent<cPersonajeFlyweight>();
        pandillerosUrqualianos.nombre = "Pandilleros Urqualianos";
        pandillerosUrqualianos.arma = cArma.ARCO;
        pandillerosUrqualianos.iA = cAI.FULL_DEFENSIVO;
        pandillerosUrqualianos.hab.ataqueBasico = 1;
        pandillerosUrqualianos.hab.defensaBasica = 1;
        pandillerosUrqualianos.atr.maña = 2;
        pandillerosUrqualianos.atr.ingenio = 2;
        templatesMatones.Add(pandillerosUrqualianos);

        cPersonajeFlyweight pistolerosGebedenos = gameObject.AddComponent<cPersonajeFlyweight>();
        pistolerosGebedenos.nombre = "Pistoleros Gebedenos";
        pistolerosGebedenos.arma = cArma.FUEGO;
        pistolerosGebedenos.iA = cAI.FULL_AGGRO;
        pistolerosGebedenos.hab.ataqueBasico = 1;
        pistolerosGebedenos.hab.defensaBasica = 1;
        templatesMatones.Add(pistolerosGebedenos);

        cPersonajeFlyweight pibesYvyros = gameObject.AddComponent<cPersonajeFlyweight>();
        pibesYvyros.nombre = "Pibes Yvyros";
        pibesYvyros.arma = cArma.LIGERAS;
        pibesYvyros.iA = cAI.SMART_DEFENSIVO;
        pibesYvyros.atr.maña = 2;
        pibesYvyros.atr.ingenio = 2;
        pibesYvyros.atr.donaire = 1;
        templatesMatones.Add(pibesYvyros);

        cPersonajeFlyweight tonatiosEnEntrenamiento = gameObject.AddComponent<cPersonajeFlyweight>();
        tonatiosEnEntrenamiento.nombre = "Tonatios en Entrenamiento";
        tonatiosEnEntrenamiento.arma = cArma.MEDIAS;
        tonatiosEnEntrenamiento.iA = cAI.SMART_DEFENSIVO;
        tonatiosEnEntrenamiento.hab.ataqueBasico = 1;
        tonatiosEnEntrenamiento.hab.defensaBasica = 1;
        tonatiosEnEntrenamiento.atr.maña = 1;
        tonatiosEnEntrenamiento.atr.ingenio = 1;
        templatesMatones.Add(tonatiosEnEntrenamiento);

        cPersonajeFlyweight pendencierosDeKasur = gameObject.AddComponent<cPersonajeFlyweight>();
        pendencierosDeKasur.nombre = "Pendencieros de Kasur";
        pendencierosDeKasur.arma = cArma.PELEA;
        pendencierosDeKasur.iA = cAI.SMART_DEFENSIVO;
        pendencierosDeKasur.hab.ataqueBasico = 1;
        pendencierosDeKasur.hab.defensaBasica = 1;
        pendencierosDeKasur.atr.maña = 1;
        pendencierosDeKasur.atr.ingenio = 1;
        templatesMatones.Add(pendencierosDeKasur);
    }

    void LlenarTempatePersonajes()
    {
        //Por ahora son personajes medio hechos
        //capaz en el futuro darlos mas en blanco y que el jugador puede ponerles nombre?

        //Couroneo Imprudente
        //Con mi sistema de Cr, vale 14
        cPersonajeFlyweight couroneoImprudente = gameObject.AddComponent<cPersonajeFlyweight>();
        couroneoImprudente.nombre = "Couroneo Imprudente";
        couroneoImprudente.arma = cArma.MEDIAS;
        couroneoImprudente.iA = cAI.FULL_AGGRO;
        couroneoImprudente.hab.ataqueBasico = 2;
        couroneoImprudente.hab.defensaBasica = 1;
        couroneoImprudente.atr.musculo = 1;
        templatesPersonajes.Add(couroneoImprudente);

        //Guardian Yvyro
        //Con mi sistema de Cr, vale 14
        cPersonajeFlyweight guardianDeYsyry = gameObject.AddComponent<cPersonajeFlyweight>();
        guardianDeYsyry.nombre = "Guardian de Ysyry";
        guardianDeYsyry.arma = cArma.ARCO;
        guardianDeYsyry.iA = cAI.SMART_DEFENSIVO;
        guardianDeYsyry.hab.defensaBasica = 2;
        guardianDeYsyry.atr.donaire = 1;
        guardianDeYsyry.atr.ingenio = 1;
        templatesPersonajes.Add(guardianDeYsyry);

        //Guardian Yvyro
        //Con mi sistema de Cr, vale 14
        cPersonajeFlyweight pistoleroDeOzcolto = gameObject.AddComponent<cPersonajeFlyweight>();
        pistoleroDeOzcolto.nombre = "Pistolero de Ozcolto";
        pistoleroDeOzcolto.arma = cArma.FUEGO;
        pistoleroDeOzcolto.iA = cAI.FULL_AGGRO;
        pistoleroDeOzcolto.hab.ataqueBasico = 2;
        pistoleroDeOzcolto.atr.maña = 2;
        templatesPersonajes.Add(pistoleroDeOzcolto);
    }
}
