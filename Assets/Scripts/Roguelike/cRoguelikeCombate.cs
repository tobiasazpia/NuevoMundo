using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cRoguelikeCombate : MonoBehaviour
{
    //Se encarga de generar un combate con la data roguelike manager entre la party, y unos enemigos generados a partir del nivel en el que estamos
    const int MAX_LVL = 3;
    public cCombate combate;
    public cRoguelikeManager rM;
    List<cPersonajeFlyweight> enemigos;
    public List<List<cPersonajeFlyweight>> templatesMatones;
    public List<List<cPersonajeFlyweight>> templatesPersonajes;

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
        int presupuesto = nivel * 5;
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
        int cantidaDeAcciones = CalcularCantidadAcciones();
        int accionesMaximas = 13;
        int accionesAdicionalesPosibles = accionesMaximas - cantidaDeAcciones;
        int lvl = ElegirNivel(presupuesto);
        // el problema esta es el edge case en el que elegimos un nivel y un tipo (maton o personaje), solo para darnos cuenta despues de que no hay ninguna eleccion valida con esos parametros
        //es bastante comun porque solo hay un pj y un maton lvl 0
        Debug.Log("Agregando Enemigo - lvl: " + lvl + ", mas acciones posibles: " + accionesAdicionalesPosibles + " y presupuesto: " + presupuesto);

        if (presupuesto < 8*(lvl+1))
        {
            Debug.Log("Presupuesto solo alcanza para matones");
            ElegirMaton(nuevoEnemigo, ref presupuesto, lvl, accionesAdicionalesPosibles);
        }
        else
        {
            int tipoEnemigo = Random.Range(0, 2);
            if (tipoEnemigo == 0 || accionesAdicionalesPosibles < 3)
            {
                Debug.Log("salio matoens");
                ElegirMaton(nuevoEnemigo, ref presupuesto, lvl, accionesAdicionalesPosibles);
            }
            else
            {
                Debug.Log("salio pjs");
                ElegirPersonaje(nuevoEnemigo, ref presupuesto, lvl);
            }
        }
        return presupuesto;
    }

    int CalcularCantidadAcciones()
    {
        int ret = 0;
        foreach (var item in enemigos)
        {
            if (item.esMaton) ret += item.cantidad;
            else ret += 3;
        }
        return ret;
    }

    int ElegirNivel(int presupuesto)
    {
        return Mathf.Min(Random.Range(0, (presupuesto / 10) + 1),MAX_LVL);
    }

    void ElegirMaton(cPersonajeFlyweight maton, ref int presupuesto, int lvl, int accFaltantes)
    {
        maton.esMaton = true;
        List<int> noDisp = MatonesNoDisponibles(lvl);

        foreach (var item in noDisp)
        {
            Debug.Log("en NoDisp: " + noDisp);
        }

        int initialLvl = lvl;
        bool bajando = true;
        while(noDisp.Count == templatesMatones[lvl].Count)
        {
            //entonces lvl no valido, hay que cambiarlo
            if (bajando)
            {
                if (lvl - 1 >= 0)
                {
                    lvl -= 1;
                    noDisp = MatonesNoDisponibles(lvl);
                }
                else
                {
                    bajando = false;
                    lvl = initialLvl + 1;
                }
            }
            else
            {
                lvl += 1;
                noDisp = MatonesNoDisponibles(lvl);
            }
        }

        noDisp.Sort();
        Debug.Log("lvl matones count: " + templatesMatones[lvl].Count + ", noDisp Count: " + noDisp.Count);
        int index = Random.Range(0, templatesMatones[lvl].Count - noDisp.Count);
        Debug.Log("index pre adjust: " + index);
        foreach (var item in noDisp)
        {
            if (index >= item)
            {
                Debug.Log("adjusted");
                index++;
            }
        }

        //index valido check

        Debug.Log("index post adjust: " + index);
        int valorDeUnMaton = 1 + 2 * (lvl + 1);
        Debug.Log("valorDeUnMaton: " + valorDeUnMaton);
        maton.cantidad = Random.Range(1, Mathf.Min((presupuesto / valorDeUnMaton) + 1, 11, accFaltantes + 1));
        Debug.Log("cantidad: " + maton.cantidad);
        maton.Copiar(templatesMatones[lvl][index]);
        enemigos.Add(maton);
        presupuesto -= maton.cantidad * valorDeUnMaton;
        Debug.Log("Nuevo Presupuesto: " + presupuesto);
    }


    void ElegirPersonaje(cPersonajeFlyweight personaje, ref int presupuesto, int lvl)
    {
        List<int> noDisp = PersonajesNoDisponibles(lvl);

        int initialLvl = lvl;
        bool bajando = true;
        while (noDisp.Count == templatesMatones[lvl].Count)
        {
            //entonces lvl no valido, hay que cambiarlo
            if (bajando)
            {
                if (lvl - 1 >= 0)
                {
                    lvl -= 1;
                    noDisp = PersonajesNoDisponibles(lvl);
                }
                else
                {
                    bajando = false;
                    lvl = initialLvl + 1;
                }
            }
            else
            {
                lvl += 1;
                noDisp = PersonajesNoDisponibles(lvl);
            }
        }

        noDisp.Sort();
        Debug.Log("lvl personajes count: " + templatesPersonajes[lvl].Count + ", noDisp Count: " + noDisp.Count);
        int index = Random.Range(0, templatesPersonajes[lvl].Count - noDisp.Count);
        Debug.Log("index pre adjust: " + index);
        foreach (var item in noDisp)
        {
            if (index >= item)
            {
                Debug.Log("adjusted");
                index++;
            }
        }
        Debug.Log("index post adjust: " + index);
        personaje.esMaton = false;
        personaje.Copiar(templatesPersonajes[lvl][index]);
        enemigos.Add(personaje);
        presupuesto -= 4 + 8 * (lvl + 1);
        Debug.Log("Nuevo Presupuesto: " + presupuesto);
    }

    List<int> PersonajesNoDisponibles(int lvl)
    {
        List<int> perIndex = PersonajesYaEnEquipo(lvl);
        for (int i = 0; i < enemigos.Count; i++)
        {
            if (!enemigos[i].esMaton)
            {
                for (int j = 0; j < templatesPersonajes[lvl].Count; j++)
                {
                    if (enemigos[i].nombre == templatesPersonajes[lvl][j].nombre) perIndex.Add(j);
                }
            }
        }
        perIndex.Sort();
        return perIndex;
    }

    public List<int> PersonajesYaEnEquipo(int lvl)
    {
        List<int> perIndex = new List<int>();

        for (int i = 1; i < rM.party.Count; i++) // Cada miembro de la party
        {
                for (int k = 0; k < templatesPersonajes[lvl].Count; k++) // Cada personaje
                {
                if (rM.party[i].nombre == templatesPersonajes[lvl][k].nombre)
                {
                    Debug.Log("agregamos a " + templatesPersonajes[lvl][k].nombre + ", indice: " + k);
                    perIndex.Add(k);
                }
                }
        }
        return perIndex;
    }

    public List<int> MatonesNoDisponibles(int lvl)
    {
        List<int> perIndex = new List<int>();
        for (int i = 0; i < enemigos.Count; i++)
        {
            if (enemigos[i].esMaton)
            {
                for (int j = 0; j < templatesMatones[lvl].Count; j++)
                {
                    if (enemigos[i].nombre == templatesMatones[lvl][j].nombre) perIndex.Add(j);
                }
            }
        }
        perIndex.Sort();
        return perIndex;
    }

    void LlenarTemplateDeMatones()
    {
        templatesMatones = new List<List<cPersonajeFlyweight>>();
        List<cPersonajeFlyweight> l;
        int lvl;

        lvl = 0;
        l = new List<cPersonajeFlyweight>();
        templatesMatones.Add(l);

        cPersonajeFlyweight satanitos = gameObject.AddComponent<cPersonajeFlyweight>();
        satanitos.nombre = "Bandidos Novatos";
        satanitos.arma = cArma.MEDIAS;
        satanitos.iA = cAI.FULL_AGGRO;
        templatesMatones[lvl].Add(satanitos);


        lvl = 1;
        l = new List<cPersonajeFlyweight>();
        templatesMatones.Add(l);

        cPersonajeFlyweight campesinosLatios = gameObject.AddComponent<cPersonajeFlyweight>();
        campesinosLatios.nombre = "Campesinos Latios";
        campesinosLatios.arma = cArma.PESADAS;
        campesinosLatios.iA = cAI.FULL_AGGRO;
        campesinosLatios.hab.ataqueBasico = 1;
        campesinosLatios.hab.defensaBasica = 2;
        campesinosLatios.atr.maña = 1;
        templatesMatones[lvl].Add(campesinosLatios);

        cPersonajeFlyweight pandillerosUrqualianos = gameObject.AddComponent<cPersonajeFlyweight>();
        pandillerosUrqualianos.nombre = "Pandilleros Urqualianos";
        pandillerosUrqualianos.arma = cArma.ARCO;
        pandillerosUrqualianos.iA = cAI.FULL_DEFENSIVO;
        pandillerosUrqualianos.hab.ataqueBasico = 1;
        pandillerosUrqualianos.hab.defensaBasica = 1;
        pandillerosUrqualianos.atr.maña = 1;
        pandillerosUrqualianos.atr.ingenio = 2;
        templatesMatones[lvl].Add(pandillerosUrqualianos);

        cPersonajeFlyweight piratasGebedenos = gameObject.AddComponent<cPersonajeFlyweight>();
        piratasGebedenos.nombre = "Piratas Gebedenos";
        piratasGebedenos.arma = cArma.FUEGO;
        piratasGebedenos.iA = cAI.FULL_AGGRO;
        piratasGebedenos.hab.ataqueBasico = 1;
        piratasGebedenos.hab.defensaBasica = 1;
        piratasGebedenos.atr.maña = 2;
        piratasGebedenos.atr.donaire = 1;
        templatesMatones[lvl].Add(piratasGebedenos);

        cPersonajeFlyweight pibesYvyros = gameObject.AddComponent<cPersonajeFlyweight>();
        pibesYvyros.nombre = "Pibes Yvyros";
        pibesYvyros.arma = cArma.LIGERAS;
        pibesYvyros.iA = cAI.SMART_DEFENSIVO;
        pibesYvyros.hab.defensaBasica = 1;
        pibesYvyros.atr.maña = 2;
        pibesYvyros.atr.ingenio = 2;
        pibesYvyros.atr.donaire = 1;
        templatesMatones[lvl].Add(pibesYvyros);

        cPersonajeFlyweight tonatiosEnEntrenamiento = gameObject.AddComponent<cPersonajeFlyweight>();
        tonatiosEnEntrenamiento.nombre = "Tonatios en Entrenamiento";
        tonatiosEnEntrenamiento.arma = cArma.MEDIAS;
        tonatiosEnEntrenamiento.iA = cAI.ATACANTE_PRECAVIDO;
        tonatiosEnEntrenamiento.hab.ataqueBasico = 1;
        tonatiosEnEntrenamiento.hab.defensaBasica = 2;
        tonatiosEnEntrenamiento.atr.maña = 1;
        tonatiosEnEntrenamiento.atr.ingenio = 1;
        templatesMatones[lvl].Add(tonatiosEnEntrenamiento);

        cPersonajeFlyweight pendencierosDeKasur = gameObject.AddComponent<cPersonajeFlyweight>();
        pendencierosDeKasur.nombre = "Pendencieros de Kasur";
        pendencierosDeKasur.arma = cArma.PELEA;
        pendencierosDeKasur.iA = cAI.FULL_AGGRO;
        pendencierosDeKasur.hab.ataqueBasico = 3;
        pendencierosDeKasur.atr.maña = 1;
        pendencierosDeKasur.atr.musculo = 1;
        templatesMatones[lvl].Add(pendencierosDeKasur);

        lvl = 2;
        l = new List<cPersonajeFlyweight>();
        templatesMatones.Add(l);

        cPersonajeFlyweight defensaDeUmivarko = gameObject.AddComponent<cPersonajeFlyweight>();
        defensaDeUmivarko.nombre = "Defensa de Umivarko";
        defensaDeUmivarko.arma = cArma.ARCO;
        defensaDeUmivarko.iA = cAI.SMART_DEFENSIVO;
        defensaDeUmivarko.hab.ataqueBasico = 1;
        defensaDeUmivarko.hab.defensaBasica = 5;
        defensaDeUmivarko.atr.maña = 1;
        defensaDeUmivarko.atr.ingenio = 1;
        defensaDeUmivarko.atr.donaire = 1;
        templatesMatones[lvl].Add(defensaDeUmivarko);

        cPersonajeFlyweight batallonLatio = gameObject.AddComponent<cPersonajeFlyweight>();
        batallonLatio.nombre = "Batallon Latio";
        batallonLatio.arma = cArma.FUEGO;
        batallonLatio.iA = cAI.FULL_AGGRO;
        batallonLatio.hab.ataqueBasico = 4;
        batallonLatio.hab.defensaBasica = 2;
        batallonLatio.atr.maña = 2;
        batallonLatio.atr.donaire = 1;
        templatesMatones[lvl].Add(batallonLatio);

        cPersonajeFlyweight silvidosDeLaEstepa = gameObject.AddComponent<cPersonajeFlyweight>();
        silvidosDeLaEstepa.nombre = "Silvidos de la Estepa";
        silvidosDeLaEstepa.arma = cArma.MEDIAS;
        silvidosDeLaEstepa.iA = cAI.FULL_AGGRO;
        silvidosDeLaEstepa.hab.ataqueBasico = 4;
        silvidosDeLaEstepa.hab.defensaBasica = 2;
        silvidosDeLaEstepa.atr.brio = 2;
        silvidosDeLaEstepa.atr.donaire = 1;
        templatesMatones[lvl].Add(silvidosDeLaEstepa);


        lvl = 3;
        l = new List<cPersonajeFlyweight>();
        templatesMatones.Add(l);

        cPersonajeFlyweight guradiaRealDeUrqualia = gameObject.AddComponent<cPersonajeFlyweight>();
        guradiaRealDeUrqualia.nombre = "Guardia Real de Urqualia";
        guradiaRealDeUrqualia.arma = cArma.MEDIAS;
        guradiaRealDeUrqualia.iA = cAI.SMART_DEFENSIVO;
        guradiaRealDeUrqualia.hab.ataqueBasico = 5;
        guradiaRealDeUrqualia.hab.defensaBasica = 5;
        guradiaRealDeUrqualia.atr.maña = 2;
        guradiaRealDeUrqualia.atr.musculo = 2;
        guradiaRealDeUrqualia.atr.ingenio = 2;
        guradiaRealDeUrqualia.atr.brio = 2;
        guradiaRealDeUrqualia.atr.donaire = 2;  
        templatesMatones[lvl].Add(guradiaRealDeUrqualia);
    }

    void LlenarTempatePersonajes()
    {
        //Por ahora son personajes medio hechos
        //capaz en el futuro darlos mas en blanco y que el jugador puede ponerles nombre?
        templatesPersonajes = new List<List<cPersonajeFlyweight>>();
        List<cPersonajeFlyweight> l;
        int lvl;

        lvl = 0;
        l = new List<cPersonajeFlyweight>();
        templatesPersonajes.Add(l);

        cPersonajeFlyweight satan = gameObject.AddComponent<cPersonajeFlyweight>();
        satan.nombre = "Mr. Satan";
        satan.arma = cArma.MEDIAS;
        satan.iA = cAI.FULL_AGGRO;
        templatesPersonajes[lvl].Add(satan);


        lvl = 1;
        l = new List<cPersonajeFlyweight>();
        templatesPersonajes.Add(l);

        cPersonajeFlyweight sombraEnEntrenamiento = gameObject.AddComponent<cPersonajeFlyweight>();
        sombraEnEntrenamiento.nombre = "Sombra en Entrenamiento";
        sombraEnEntrenamiento.arma = cArma.LIGERAS;
        sombraEnEntrenamiento.iA = cAI.SMART_DEFENSIVO;
        sombraEnEntrenamiento.hab.defensaBasica = 3;
        sombraEnEntrenamiento.atr.donaire = 1;
        sombraEnEntrenamiento.atr.ingenio = 1;
        templatesPersonajes[lvl].Add(sombraEnEntrenamiento);

        cPersonajeFlyweight heroeTonatio = gameObject.AddComponent<cPersonajeFlyweight>();
        heroeTonatio.nombre = "Heroe Tonatio";
        heroeTonatio.arma = cArma.MEDIAS;
        heroeTonatio.iA = cAI.FULL_AGGRO;
        heroeTonatio.hab.ataqueBasico = 3;
        heroeTonatio.atr.musculo = 1;
        heroeTonatio.atr.maña = 1;
        templatesPersonajes[lvl].Add(heroeTonatio);

        cPersonajeFlyweight aspiranteAMartillo = gameObject.AddComponent<cPersonajeFlyweight>();
        aspiranteAMartillo.nombre = "Aspirante a Martillo";
        aspiranteAMartillo.arma = cArma.PESADAS;
        aspiranteAMartillo.iA = cAI.ATACANTE_PRECAVIDO;
        aspiranteAMartillo.hab.defensaBasica = 3;
        aspiranteAMartillo.atr.musculo = 2;
        templatesPersonajes[lvl].Add(aspiranteAMartillo);

        cPersonajeFlyweight arqueroSikuo = gameObject.AddComponent<cPersonajeFlyweight>();
        arqueroSikuo.nombre = "Arquero Sikuo";
        arqueroSikuo.arma = cArma.ARCO;
        arqueroSikuo.iA = cAI.FULL_AGGRO;
        arqueroSikuo.hab.ataqueBasico = 3;
        arqueroSikuo.atr.brio = 1;
        arqueroSikuo.atr.maña = 1;
        templatesPersonajes[lvl].Add(arqueroSikuo);

        cPersonajeFlyweight tiradorDeCaulcaycaja = gameObject.AddComponent<cPersonajeFlyweight>();
        tiradorDeCaulcaycaja.nombre = "Tirador de Caulcaycaja";
        tiradorDeCaulcaycaja.arma = cArma.FUEGO;
        tiradorDeCaulcaycaja.iA = cAI.ATACANTE_PRECAVIDO;
        tiradorDeCaulcaycaja.hab.ataqueBasico = 2;
        tiradorDeCaulcaycaja.hab.defensaBasica = 1;
        tiradorDeCaulcaycaja.atr.ingenio = 1;
        tiradorDeCaulcaycaja.atr.maña = 1;
        templatesPersonajes[lvl].Add(tiradorDeCaulcaycaja);

        cPersonajeFlyweight marineroGebedeno = gameObject.AddComponent<cPersonajeFlyweight>();
        marineroGebedeno.nombre = "Marinero Gebedeno";
        marineroGebedeno.arma = cArma.PELEA;
        marineroGebedeno.hab.ataqueBasico = 3;
        marineroGebedeno.atr.musculo = 2;
        marineroGebedeno.iA = cAI.FULL_AGGRO;
        templatesPersonajes[lvl].Add(marineroGebedeno);


        lvl = 2;
        l = new List<cPersonajeFlyweight>();
        templatesPersonajes.Add(l);

        cPersonajeFlyweight cazarecompensasCouroneo = gameObject.AddComponent<cPersonajeFlyweight>();
        cazarecompensasCouroneo.nombre = "Cazarecompensas Couroneo";
        cazarecompensasCouroneo.arma = cArma.MEDIAS;
        cazarecompensasCouroneo.iA = cAI.FULL_AGGRO;
        cazarecompensasCouroneo.hab.ataqueBasico = 3;
        cazarecompensasCouroneo.hab.defensaBasica = 3;
        cazarecompensasCouroneo.atr.musculo = 1;
        cazarecompensasCouroneo.atr.maña = 1;
        cazarecompensasCouroneo.atr.brio = 1;
        templatesPersonajes[lvl].Add(cazarecompensasCouroneo);

        cPersonajeFlyweight guardianDeYsyry = gameObject.AddComponent<cPersonajeFlyweight>();
        guardianDeYsyry.nombre = "Guardian de Ysyry";
        guardianDeYsyry.arma = cArma.ARCO;
        guardianDeYsyry.iA = cAI.SMART_DEFENSIVO;
        guardianDeYsyry.hab.defensaBasica = 4;
        guardianDeYsyry.hab.ataqueBasico = 2;
        guardianDeYsyry.atr.donaire = 1;
        guardianDeYsyry.atr.ingenio = 2;
        templatesPersonajes[lvl].Add(guardianDeYsyry);

        cPersonajeFlyweight pistoleroDeOzcolto = gameObject.AddComponent<cPersonajeFlyweight>();
        pistoleroDeOzcolto.nombre = "Pistolero de Ozcolto";
        pistoleroDeOzcolto.arma = cArma.FUEGO;
        pistoleroDeOzcolto.iA = cAI.FULL_AGGRO;
        pistoleroDeOzcolto.hab.ataqueBasico = 3;
        pistoleroDeOzcolto.hab.defensaBasica = 3;
        pistoleroDeOzcolto.atr.maña = 2;
        pistoleroDeOzcolto.atr.brio = 1;
        templatesPersonajes[lvl].Add(pistoleroDeOzcolto);


        lvl = 3;
        l = new List<cPersonajeFlyweight>();
        templatesPersonajes.Add(l);

        cPersonajeFlyweight terminator = gameObject.AddComponent<cPersonajeFlyweight>();
        terminator.nombre = "Terminator";
        terminator.arma = cArma.FUEGO;
        terminator.iA = cAI.FULL_AGGRO;
        terminator.hab.ataqueBasico = 5;
        terminator.hab.defensaBasica = 5;
        terminator.atr.maña = 2;
        terminator.atr.musculo = 2;
        terminator.atr.ingenio = 2;
        terminator.atr.brio = 2;
        terminator.atr.donaire = 2;
        templatesPersonajes[lvl].Add(terminator);
    }
}
