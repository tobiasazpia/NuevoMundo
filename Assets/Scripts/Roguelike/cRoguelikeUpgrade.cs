using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class UpgradesHechas
{
    public int upgradeHabs;
    public int upgradeAtrs;
}


public class cRoguelikeUpgrade : MonoBehaviour
{
    public UIRoguelikeUpgrade uiRU;
    public cRoguelikeManager rM;
    public GameObject rogueMenuRevisar, rogueMenuElegir, rogueMenuAnticipar;
    public GameObject personajeInfo, atributos, anticipacion;
    public PlayerInput py;

    bool slotsIdenticos;
    bool recurringEnElmento;
    bool recurringEnSubtipo;
    bool recurringDENUEVOEnSubtipo;

   public cRoguelikeUpgradeData[] upgrades = new cRoguelikeUpgradeData[3];

    List<UpgradesHechas> uHechas = new List<UpgradesHechas>();

    private void OnEnable()
    {
        for (int i = 0; i < upgrades.Length; i++)
        {
            upgrades[i] = new cRoguelikeUpgradeData();
            upgrades[i].upgradesList = new List<sSingleUpgrade>();
        }
    }

    public void LetsUpgrade()
    {

        slotsIdenticos = false;
        recurringEnElmento = false;
        recurringEnSubtipo = false;
        recurringDENUEVOEnSubtipo = false;

        uiRU.InfoBasicaPersonajesUpgrade();

        uHechas.Clear();
        for (int i = 0; i < rM.party.Count; i++)
        {
            UpgradesHechas uH = new UpgradesHechas();
            uH.upgradeAtrs = 0;
            uH.upgradeHabs = 0;
            uHechas.Add(uH);
        }

        foreach (var item in upgrades)
        {
            item.objetivoDeUpgrade = -1;
            item.tipoDeUpgrade = -1;
            item.upgradesList.Clear();
        }

        bool hayEspeacioParaPJS = rM.party.Count < 3;

        const int max = 20;

        bool hayEspeacioParaTriples = false;
        bool hayEspacioParaDobles = false;
        bool hayEspacioParaUnaDoble = false;
        bool hayEspacioParaUna = false;
        bool alguienTieneAlgunaHerida = false;
        bool alguienTieneDosHeridas = false;
        bool alguienNecesitaDrama = false;

        bool espacioUnaDobleParty = RevisarSiHayEspacioEnParty(rM.party, 4);
        bool espacioTresDoblesParty = RevisarSiHayEspacioEnCadaPersonaje(rM.party, 2);
        bool espacioTresTriplesParty = RevisarSiHayEspacioEnCadaPersonaje(rM.party, 3);

        foreach (var item in rM.party)
        {
            // Trate de implementar esto:
            // a) una doble - necesitamoas que se cumplas dos cosas:
            // 1 - que entre toda la party tengamos 4 slots de upgrade disponibles
            // y 2- que por lo menos un persona tenga 2 slots disponibles y que ninguna de las otras upgrades los use
            // b) 3 dobles (sombra) - necesitamoas que se cumpla UNA de dos cosas:
            // 1 - que un personaje tenga 3 slots de upgrade disponibles
            // o 2 - que los 3 personajes tengan 2 slots disponibles
            // c) 3 triples - necesitamoas que se cumpla UNA de dos cosas:
            // 1 - que un personaje tenga 4 slots de upgrade disponibles
            // o 2 - que los 3 personajes tengan 3 slots disponibles
            hayEspeacioParaTriples = RevisarSiHayEspacioEnPersonaje(item, 4) || espacioTresTriplesParty; // se permiten repeticiones meintras las combinaciones sean 
            Debug.Log("espacio triples: " + hayEspeacioParaTriples);
            hayEspacioParaDobles = RevisarSiHayEspacioEnPersonaje(item, 3) || espacioTresDoblesParty; // se permiten repeticiones meintras las combinaciones sean distitnas
            hayEspacioParaUnaDoble = RevisarSiHayEspacioEnPersonaje(item, 2) && espacioUnaDobleParty; // no se permiten repeticiones
            Debug.Log("espacio dobles: " + hayEspacioParaDobles);
            if (CalcularPuntosEnAtributos(item) + CalcularPuntosEnHabilidades(item) < max) hayEspacioParaUna = true;
            Debug.Log("espacio una: " + hayEspacioParaUna);
            if (item.heridas > 0)
            {
                alguienTieneAlgunaHerida = true;
                if (item.heridas > 1) alguienTieneDosHeridas = true;
            }
            Debug.Log("AlgunaHerida: " + alguienTieneAlgunaHerida);
            Debug.Log("dos heridas: " + alguienTieneDosHeridas);
            if (!item.drama) alguienNecesitaDrama = true;
            Debug.Log("falta drama: " + alguienNecesitaDrama);
        }

        uiRU.eligiendoNuevoP = false;
        //cada 5 las tres son pj upgrade
        int pJIntervalo = 5;
        if (rM.nivel % pJIntervalo == 0 && hayEspeacioParaPJS)
        {
            uiRU.eligiendoNuevoP = true;
            int lvl = 1;
            if (rM.nivel > pJIntervalo) lvl = 2;
            Debug.Log("todos pjs");
            List<int> valoresIlegales = new List<int>();
            for (int i = 0; i < 3; i++)
            {
                upgrades[i].tipoDeUpgrade = cRoguelikeUpgradeData.RU_PJ;

                sSingleUpgrade s = new sSingleUpgrade();
                upgrades[i].upgradesList.Add(s);
                upgrades[i].upgradesList[0] = new sSingleUpgrade();
                int elegido = GetRandomPersonaje(lvl, valoresIlegales);
                upgrades[i].upgradesList[0].elementoAUpgradear = elegido;
                valoresIlegales.Add(elegido);
                CambiarTextoUPJ(i, upgrades[i], lvl); //podriamos usar el Subtipo para guardar el nivel?
            }
        }
        //cada 3 las tres son triple upgrade
        else if (hayEspeacioParaTriples && !alguienTieneDosHeridas && Random.Range(0, 4) == 0)
        {
            Debug.Log("triples");
            for (int i = 0; i < 3; i++)
            {
                upgrades[i].tipoDeUpgrade = cRoguelikeUpgradeData.RU_TRIPLE;
            }
            for (int i = 0; i < 3; i++)
            {
                DefinirUpgrade(i, 3);
                uHechas[upgrades[i].objetivoDeUpgrade].upgradeHabs = 0;
                uHechas[upgrades[i].objetivoDeUpgrade].upgradeAtrs = 0;
                CambiarTexto(i, upgrades[i]);
            }
        }
        else if (((alguienNecesitaDrama && alguienTieneAlgunaHerida) || alguienTieneDosHeridas) && hayEspacioParaDobles && Random.Range(0, 5) == 0)
        {
            Debug.Log("sombraaa");
            for (int i = 0; i < 3; i++)
            {
                upgrades[i].tipoDeUpgrade = cRoguelikeUpgradeData.RU_DESCANSOS_Y_DOBLES;
                DefinirUpgrade(i, 2);
                uHechas[upgrades[i].objetivoDeUpgrade].upgradeHabs = 0;
                uHechas[upgrades[i].objetivoDeUpgrade].upgradeAtrs = 0;
                CambiarTexto(i, upgrades[i]);
            }
        }
        //si no, todas son normales
        else if (hayEspacioParaUna)
        {
            Debug.Log("UNA");
            for (int i = 0; i < 3; i++)
            {
                upgrades[i].tipoDeUpgrade = cRoguelikeUpgradeData.RU_SIMPLE;
            }

            bool hayConPesadas = false;
            bool nadieTieneVoluntad = true;
            bool nadieTieneJaieiy = true;
            foreach (var item in rM.party)
            {
                if (item.arma == cArma.PESADAS) { hayConPesadas = true; Debug.Log("haypesadas"); }
                if (item.tradicionMarcialId == 1)
                {
                    nadieTieneVoluntad = false; Debug.Log("ya ha yvoluntad ");
                }
                else if (item.tradicionMarcialId == 2)
                {
                    nadieTieneJaieiy = false; Debug.Log("ya ha yvoluntad ");
                }
            }
            if (Random.Range(0, 2) == 0 && hayEspacioParaUnaDoble)
            {
                upgrades[0].tipoDeUpgrade = cRoguelikeUpgradeData.RU_DOBLE;
                Debug.Log(" + DOS");
            }
            else if (hayConPesadas &&( nadieTieneVoluntad || nadieTieneJaieiy ) && Random.Range(0, 4) == 0)
            {
                Debug.Log("up marcail");
                upgrades[0].tipoDeUpgrade = cRoguelikeUpgradeData.RU_MARCIAL;
                upgrades[0].upgradesList.Add(new sSingleUpgrade());
                if (nadieTieneVoluntad || nadieTieneJaieiy) upgrades[0].upgradesList[0].elementoAUpgradear = Random.Range(0, 2);
                else if (nadieTieneVoluntad) upgrades[0].upgradesList[0].elementoAUpgradear = 0;
                else upgrades[0].upgradesList[0].elementoAUpgradear = 1;
                Debug.Log(upgrades[0].upgradesList[0].elementoAUpgradear);
                ElegirObjetivoDeMarcial(0);
            }

            if ((alguienNecesitaDrama && alguienTieneAlgunaHerida) || alguienTieneDosHeridas)
            {
                upgrades[1].tipoDeUpgrade = cRoguelikeUpgradeData.RU_DESCANSO_COMPLETO;
                Debug.Log(" + DES COMPLE");
            }

            if (alguienTieneAlgunaHerida)
            {
                upgrades[2].tipoDeUpgrade = cRoguelikeUpgradeData.RU_DESCANSO_PARCIAL_Y_SIMPLE;
                Debug.Log(" + DES PARCIAL");
            }

            int amount;
            for (int i = 0; i < 3; i++)
            {
                if (upgrades[i].tipoDeUpgrade == cRoguelikeUpgradeData.RU_DOBLE) amount = 2;
                else amount = 1;

                if (upgrades[i].tipoDeUpgrade != cRoguelikeUpgradeData.RU_DESCANSO_COMPLETO && upgrades[i].tipoDeUpgrade != cRoguelikeUpgradeData.RU_MARCIAL) DefinirUpgrade(i, amount);
                CambiarTexto(i, upgrades[i]);
            }
        }
        else
        {
            Debug.Log("Party Perfecta");
            for (int i = 0; i < 3; i++)
            {
                upgrades[i].tipoDeUpgrade = cRoguelikeUpgradeData.RU_NO_UPGRADE;
                CambiarTexto(i, upgrades[i]);
            }
        }
    }

    bool RevisarSiHayEspacioEnPersonaje(cPersonajeFlyweight per, int espaciosNecesarios)
    {
        const int maxAtr = 2;
        const int maxHabilidad = 5;

        int espaciosEncontrados = 0;

        if (per.atr.brio < maxAtr) espaciosEncontrados++;
        if (per.atr.maña < maxAtr) espaciosEncontrados++;
        if (per.atr.musculo < maxAtr) espaciosEncontrados++;
        if (per.atr.ingenio < maxAtr) espaciosEncontrados++;
        if (per.atr.donaire < maxAtr) espaciosEncontrados++;

        if (per.tradicionMarcial[0] < maxHabilidad) espaciosEncontrados++;
        if (per.tradicionMarcial[1] < maxHabilidad) espaciosEncontrados++;
        if (per.tradicionMarcialId > cArma.FUEGO)
        {
            if (per.tradicionMarcial[2] < maxHabilidad) espaciosEncontrados++;
            if (per.tradicionMarcial[3] < maxHabilidad) espaciosEncontrados++;
            if (per.tradicionMarcial[4] < maxHabilidad) espaciosEncontrados++;
            if (per.tradicionMarcial[5] < maxHabilidad) espaciosEncontrados++;
        }

        return (espaciosEncontrados >= espaciosNecesarios);
    }

    bool RevisarSiHayEspacioEnParty(List<cPersonajeFlyweight> party, int espaciosNecesarios)
    {
        const int maxAtr = 2;
        const int maxHabilidad = 5;

        int espaciosEncontrados = 0;

        foreach (var per in party)
        {
            if (per.atr.brio < maxAtr) espaciosEncontrados++;
            if (per.atr.maña < maxAtr) espaciosEncontrados++;
            if (per.atr.musculo < maxAtr) espaciosEncontrados++;
            if (per.atr.ingenio < maxAtr) espaciosEncontrados++;
            if (per.atr.donaire < maxAtr) espaciosEncontrados++;

            if (per.tradicionMarcial[0] < maxHabilidad) espaciosEncontrados++;
            if (per.tradicionMarcial[1] < maxHabilidad) espaciosEncontrados++;
            if (per.tradicionMarcialId > cArma.FUEGO)
            {
                if (per.tradicionMarcial[2] < maxHabilidad) espaciosEncontrados++;
                if (per.tradicionMarcial[3] < maxHabilidad) espaciosEncontrados++;
                if (per.tradicionMarcial[4] < maxHabilidad) espaciosEncontrados++;
                if (per.tradicionMarcial[5] < maxHabilidad) espaciosEncontrados++;
            }
        }

        return (espaciosEncontrados >= espaciosNecesarios);
    }

    bool RevisarSiHayEspacioEnCadaPersonaje(List<cPersonajeFlyweight> party, int espaciosNecesarios)
    {
        const int maxAtr = 2;
        const int maxHabilidad = 5;

        int espaciosEncontrados;

        foreach (var per in party)
        {
            espaciosEncontrados = 0;

            if (per.atr.brio < maxAtr) espaciosEncontrados++;
            if (per.atr.maña < maxAtr) espaciosEncontrados++;
            if (per.atr.musculo < maxAtr) espaciosEncontrados++;
            if (per.atr.ingenio < maxAtr) espaciosEncontrados++;
            if (per.atr.donaire < maxAtr) espaciosEncontrados++;

            if (per.tradicionMarcial[0] < maxHabilidad) espaciosEncontrados++;
            if (per.tradicionMarcial[1] < maxHabilidad) espaciosEncontrados++;
            if (per.tradicionMarcialId > cArma.FUEGO)
            {
                if (per.tradicionMarcial[2] < maxHabilidad) espaciosEncontrados++;
                if (per.tradicionMarcial[3] < maxHabilidad) espaciosEncontrados++;
                if (per.tradicionMarcial[4] < maxHabilidad) espaciosEncontrados++;
                if (per.tradicionMarcial[5] < maxHabilidad) espaciosEncontrados++;
            }
                if (espaciosEncontrados < espaciosNecesarios) return false;
        }

        return true;
    }


    public void DefinirUpgrade(int upgradeSlot, int amount)
    {
        //si estoy aca, se que al menos un upgrade de este tipo es posible
        ElegirObjetivo(upgradeSlot);
        int tipo = upgrades[upgradeSlot].tipoDeUpgrade;
        if (tipo == cRoguelikeUpgradeData.RU_NO_UPGRADE) return;

        bool exclusivo = true;
        if (tipo == cRoguelikeUpgradeData.RU_DESCANSOS_Y_DOBLES
            || tipo == cRoguelikeUpgradeData.RU_TRIPLE) exclusivo = false;
        if (!exclusivo)
        {
            uHechas[upgrades[upgradeSlot].objetivoDeUpgrade].upgradeHabs = 0;
            uHechas[upgrades[upgradeSlot].objetivoDeUpgrade].upgradeAtrs = 0;
        }

        for (int i = 0; i < amount; i++)
        {
            sSingleUpgrade s = new sSingleUpgrade();
            upgrades[upgradeSlot].upgradesList.Add(s);
            ElegirSubtipo(upgradeSlot, i);
            ElegirElemento(upgradeSlot, i);
        }
    }

    void ElegirObjetivoDeMarcial(int slot)
    {
        int objetivo = 0;
        int guardia = 31;
        for (int i = 0; i < rM.party.Count; i++)
        {
            if(rM.party[i].arma == cArma.PESADAS && rM.party[i].GetGuardia() < guardia)
            {
                guardia = rM.party[i].GetGuardia();
                objetivo = i;
            } 
        }
        upgrades[slot].objetivoDeUpgrade = objetivo;
    }

    void ElegirObjetivo(int slot)
    {
        Debug.Log("Eligiendo Objetivo, slot: " + slot);
        int max;
        List<int> maxs = new List<int>();
        if (rM.party.Count == 1) upgrades[slot].objetivoDeUpgrade = 0; // Si hay un solo P, el objetivo es el. si no...
        else
        {
            List<int> countPoints = new List<int>();
            int count;

            for (int i = 0; i < rM.party.Count; i++)
            {
                if (rM.party[i].arma > cArma.FUEGO) max = 40;
                else max = 20;
                maxs.Add(max);
                count = CalcularPuntosEnAtributos(rM.party[i]);
                count += CalcularPuntosEnHabilidades(rM.party[i]);
                Debug.Log("tipo de upgrae: " + upgrades[slot].tipoDeUpgrade);
                switch (upgrades[slot].tipoDeUpgrade) // Solo hacemos el Add si el Personaje podria recibir las upgrades
                {
                    case cRoguelikeUpgradeData.RU_TRIPLE:
                        if (RevisarSiHayEspacioEnPersonaje(rM.party[i], 3)) countPoints.Add(count);
                        else countPoints.Add(max);
                        break;
                    case cRoguelikeUpgradeData.RU_DOBLE:
                        if (RevisarSiHayEspacioEnPersonaje(rM.party[i], 2)) countPoints.Add(count);
                        else countPoints.Add(max);
                        break;
                    case cRoguelikeUpgradeData.RU_DESCANSOS_Y_DOBLES:
                        if (RevisarSiHayEspacioEnPersonaje(rM.party[i], 2)) countPoints.Add(count);
                        else countPoints.Add(max);
                        break;
                    default:
                        countPoints.Add(count);
                        break;
                }
            }

            List<int> weightedPoints = WeightearElementos(countPoints, maxs);
            //Debug.Log("Hasta este, priemr per " + weightedPoints[0]);
            //Debug.Log("Hasta este, segundo per: " + (weightedPoints[0] + weightedPoints[1]) + " despues 3er per");
            if (weightedPoints.Count > 0)
            {
                upgrades[slot].objetivoDeUpgrade = GetWeightedElement(weightedPoints);
            }
            else upgrades[slot].tipoDeUpgrade = cRoguelikeUpgradeData.RU_NO_UPGRADE;
        }
    }

    void ElegirSubtipo(int slot, int subUpgradeNum)
    {
        List<int> counts = new List<int>();
        List<int> maxs = new List<int>();
        //estos cosnt por ahroa estan hardcodeados, hab total va a cambiar, y por lo tanto el max tambien
        //cuando haya tradiciones en el jueego
        const int atrMax = 10;
        int habMax = 10;
        if (rM.party[upgrades[slot].objetivoDeUpgrade].arma > cArma.FUEGO) habMax = 30;
        int habTotal = 2;
        if (rM.party[upgrades[slot].objetivoDeUpgrade].arma > cArma.FUEGO) habTotal = 6;
        const int atrTotal = 5;
        int objetivo = upgrades[slot].objetivoDeUpgrade;
        if (objetivo == -1)
        {
            Debug.Log("Objetivo: -1");
            objetivo = 0;
        }

        float upgradeHabs = uHechas[objetivo].upgradeHabs;
        float upgradeAtrs = uHechas[objetivo].upgradeAtrs;

        int sum = CalcularPuntosEnHabilidades(rM.party[upgrades[slot].objetivoDeUpgrade]);
        counts.Add(sum + Mathf.FloorToInt((habMax - sum) * (upgradeHabs / habTotal)));
        maxs.Add(habMax);

        sum = CalcularPuntosEnAtributos(rM.party[upgrades[slot].objetivoDeUpgrade]);
        counts.Add(sum + Mathf.FloorToInt((atrMax - sum) * (upgradeAtrs / atrTotal)));
        maxs.Add(atrMax);

        Debug.Log("Eligiendo Subtipo");
        List<int> weighted = WeightearElementos(counts, maxs);
        if (weighted.Count > 0)
        {
            upgrades[slot].upgradesList[subUpgradeNum].subTipoDeUpgrade = GetWeightedElement(weighted);
            if (upgrades[slot].upgradesList[subUpgradeNum].subTipoDeUpgrade == cRoguelikeUpgradeData.RUST_HAB) uHechas[upgrades[slot].objetivoDeUpgrade].upgradeHabs++;
            else uHechas[upgrades[slot].objetivoDeUpgrade].upgradeAtrs++;
        }
        else if (!recurringDENUEVOEnSubtipo)
        {
            upgrades[slot].objetivoDeUpgrade++;
            if (upgrades[slot].objetivoDeUpgrade > 2) upgrades[slot].objetivoDeUpgrade -= 3;
            if (recurringEnSubtipo) recurringDENUEVOEnSubtipo = true;
            else recurringEnSubtipo = true;
            ElegirSubtipo(slot, subUpgradeNum);
        }
        else upgrades[slot].tipoDeUpgrade = cRoguelikeUpgradeData.RU_NO_UPGRADE;
    }

    void ElegirElemento(int slot, int subUpgradeNum)
    {
        Debug.Log("Eligiendo Elemento");
        List<int> counts = new List<int>();
        List<int> maxValues = new List<int>();
        List<int> weighted;
        int max = 0;
        switch (upgrades[slot].upgradesList[subUpgradeNum].subTipoDeUpgrade)
        {
            case 0: // habilidaes
                Debug.Log("case habilidades");
                max = 5;
                if (UpgradeYaSeleccionado(slot, subUpgradeNum, 0, 0)) counts.Add(max);
                else counts.Add(rM.party[upgrades[slot].objetivoDeUpgrade].tradicionMarcial[0]);
                if (UpgradeYaSeleccionado(slot, subUpgradeNum, 0, 1)) counts.Add(max);
                else counts.Add(rM.party[upgrades[slot].objetivoDeUpgrade].tradicionMarcial[1]);
                if (rM.party[upgrades[slot].objetivoDeUpgrade].arma > cArma.FUEGO)
                {
                    for (int i = 0; i < 6; i++)
                    {
                        maxValues.Add(max);
                    }
                    if (UpgradeYaSeleccionado(slot, subUpgradeNum, 0, 2)) counts.Add(max);
                    else counts.Add(rM.party[upgrades[slot].objetivoDeUpgrade].tradicionMarcial[2]);
                    if (UpgradeYaSeleccionado(slot, subUpgradeNum, 0, 3)) counts.Add(max);
                    else counts.Add(rM.party[upgrades[slot].objetivoDeUpgrade].tradicionMarcial[3]);
                    if (UpgradeYaSeleccionado(slot, subUpgradeNum, 0, 4)) counts.Add(max);
                    else counts.Add(rM.party[upgrades[slot].objetivoDeUpgrade].tradicionMarcial[4]);
                    if (UpgradeYaSeleccionado(slot, subUpgradeNum, 0, 5)) counts.Add(max);
                    else counts.Add(rM.party[upgrades[slot].objetivoDeUpgrade].tradicionMarcial[5]);
                }
                else
                {
                    for (int i = 0; i < 2; i++)
                    {
                        maxValues.Add(max);
                    }
                }
                break;
            case 1: // atributos
                Debug.Log("case atributos");
                max = 2;
                for (int i = 0; i < 5; i++)
                {
                    maxValues.Add(max);
                }
                if (UpgradeYaSeleccionado(slot, subUpgradeNum, 1, 0)) counts.Add(max);
                else counts.Add(rM.party[upgrades[slot].objetivoDeUpgrade].atr.maña);
                if (UpgradeYaSeleccionado(slot, subUpgradeNum, 1, 1)) counts.Add(max);
                else counts.Add(rM.party[upgrades[slot].objetivoDeUpgrade].atr.musculo);
                if (UpgradeYaSeleccionado(slot, subUpgradeNum, 1, 2)) counts.Add(max);
                else counts.Add(rM.party[upgrades[slot].objetivoDeUpgrade].atr.ingenio);
                if (UpgradeYaSeleccionado(slot, subUpgradeNum, 1, 3)) counts.Add(max);
                else counts.Add(rM.party[upgrades[slot].objetivoDeUpgrade].atr.brio);
                if (UpgradeYaSeleccionado(slot, subUpgradeNum, 1, 4)) counts.Add(max);
                else counts.Add(rM.party[upgrades[slot].objetivoDeUpgrade].atr.donaire);
                break;
            default:
                break;
        }

        weighted = WeightearElementos(counts, maxValues);

        //switch (upgrades[slot].upgradesList[subUpgradeNum].subTipoDeUpgrade)
        //{
        //    case 0:
        //        Debug.Log("Hasta este, ab " + weighted[0] + " si no DB");
        //        break;
        //    case 1:
        //        Debug.Log("Hasta este, maña " + weighted[0]);
        //        Debug.Log("Hasta este, musuclo " + (weighted[0] + weighted[1]));
        //        Debug.Log("Hasta este, ingenio " + (weighted[0] + weighted[1] + weighted[2]));
        //        Debug.Log("Hasta este, brio " + (weighted[0] + weighted[1] + weighted[2] + weighted[3]) + "si no, donaire");
        //        break;
        //    default:
        //        break;
        //}
        if (weighted.Count > 0)
        {
            upgrades[slot].upgradesList[subUpgradeNum].elementoAUpgradear = GetWeightedElement(weighted);
        }
        else if (slotsIdenticos && !recurringEnElmento)
        {
            if (upgrades[slot].upgradesList[subUpgradeNum].subTipoDeUpgrade == 0) upgrades[slot].upgradesList[subUpgradeNum].subTipoDeUpgrade = 1;
            else upgrades[slot].upgradesList[subUpgradeNum].subTipoDeUpgrade = 0;
            recurringEnElmento = true;
            ElegirElemento(slot, subUpgradeNum);
        }
        else upgrades[slot].tipoDeUpgrade = cRoguelikeUpgradeData.RU_NO_UPGRADE;

    }

    bool UpgradeYaSeleccionado(int slot, int subUpgradeNum, int subTipo, int elemento)
    {
        if (upgrades[slot].tipoDeUpgrade == cRoguelikeUpgradeData.RU_TRIPLE || upgrades[slot].tipoDeUpgrade == cRoguelikeUpgradeData.RU_DESCANSOS_Y_DOBLES)
        {
            int subUpgradesAmount = 1;
            if (upgrades[slot].tipoDeUpgrade == cRoguelikeUpgradeData.RU_TRIPLE) subUpgradesAmount = 2;

            bool ret = UpgradeYaSeleccionadoEstaUpgrade(slot, subUpgradeNum, subTipo, elemento);
            if (!ret && subUpgradeNum == subUpgradesAmount)
            {
                Debug.Log("ya elegimos todas las sub upgrades... quedo igual a otro slot?");
                upgrades[slot].upgradesList[subUpgradeNum].elementoAUpgradear = elemento;
                for (int i = slot - 1; i >= 0; i--)
                {
                    if (SlotsIdenticos(slot, i))
                    {
                        Debug.Log("si quedo, vamos de nuevo? que pasa aca?");
                        //lo que esta pasando es que marca esta subupgrade final como ilegal. pero si ya eligio el subtipo de momento es incapaz de cambiarlo, asi que es posible que ya haya elegio DB en otra subupgrade de este slot, y ahora AB sea ilegal porque quedaria una copia del otro, lo cual tira un soy perfecto. Hay que pesnar soluciones.
                        //Onda, la facil es por un bool aca que diga que estamos rerolleando, y si sale que "soy perfecto", decirle que en vez de eso cambie de subtipo y vuelva a probar
                        slotsIdenticos = true;
                        return true;
                    }
                }
                Debug.Log("no quedo, sale con fritas");
            }
            return ret;
        }

        return UpgradeYaSeleccionadoEnGeneral(slot, subUpgradeNum, subTipo, elemento);
    }

    bool SlotsIdenticos(int slotA, int slotB)
    {
        if (upgrades[slotA].upgradesList.Count != upgrades[slotB].upgradesList.Count) return false;
        Debug.Log("Slot " + slotA + " tiene ");
        foreach (var item in upgrades[slotA].upgradesList)
        {
            Debug.Log("Subtipo " + item.subTipoDeUpgrade + " y Elemento: " + item.elementoAUpgradear);
        }
        Debug.Log("Slot " + slotB + " tiene ");
        foreach (var item in upgrades[slotB].upgradesList)
        {
            Debug.Log("Subtipo " + item.subTipoDeUpgrade + " y Elemento: " + item.elementoAUpgradear);
        }

        for (int i = 0; i < upgrades[slotA].upgradesList.Count; i++)
        {
            if (!UpgradeEstaEnSlot(slotB, upgrades[slotA].upgradesList[i]))
            {
                Debug.Log("No son identicos");
                return false;
            }
        }
        Debug.Log("Son igaules");
        return true;
    }

    bool UpgradeEstaEnSlot(int slot, sSingleUpgrade subUpgrade)
    {
        foreach (var item in upgrades[slot].upgradesList)
        {
            if (item.elementoAUpgradear == subUpgrade.elementoAUpgradear && item.subTipoDeUpgrade == subUpgrade.subTipoDeUpgrade) return true;
        }
        return false;
    }

    bool UpgradeYaSeleccionadoEnGeneral(int slot, int subUpgradeNum, int subTipo, int elemento)
    {
        //Debug.Log("Checkeando ya seleccion en general, Slot: " + slot + ", SubUp: " + subUpgradeNum + ", SubTipo: " + subTipo + "y Elemento: " + elemento);
        for (int i = slot; i >= 0; i--)
        {
            for (int j = 0; j < upgrades[i].upgradesList.Count; j++)
            {
                if (slot != i || subUpgradeNum != j) /// es decir, que no se trata de este elemento
                {
                    if (upgrades[i].upgradesList[j].subTipoDeUpgrade == subTipo && upgrades[i].upgradesList[j].elementoAUpgradear == elemento)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    bool UpgradeYaSeleccionadoEstaUpgrade(int slot, int subUpgradeNum, int subTipo, int elemento)
    {
        for (int i = subUpgradeNum; i >= 0; i--)
        {
            if (subUpgradeNum != i) /// es decir, que no se trata de este elemento
            {
                if (upgrades[slot].upgradesList[i].subTipoDeUpgrade == subTipo && upgrades[slot].upgradesList[i].elementoAUpgradear == elemento)
                {
                    return true;
                }
            }
        }
        return false;
    }

    int GetWeightedElement(List<int> weighted)
    {
        int r = cDieMath.MyRandomUpgrade();
        int weightsSum = 0;
        for (int i = 0; i < weighted.Count; i++)
        {
            if (i + 1 == weighted.Count) return i;//si es la ultima opcion, es el resultado // no que este este mal, peor no tendria que ser necesario... todavia no entiendo que estoy haciendo mal
            weightsSum += weighted[i];
            if (r < weightsSum)
            {
                return i;
            }
        }
        return -9;
    }

    List<int> WeightearElementos(List<int> valores, List<int> maxValues)
    {

        //Debug.Log("valores count: " + valores.Count);
        List<int> ret = new List<int>();
        int total = 0;
        for (int i = 0; i < valores.Count; i++)
        {
            total += maxValues[i] - valores[i];
        }

        if (total == 0) // lista maxeada. Esto puede pasar por ejemplo, si un p todavia puede recibir 1 upgrade, y esta es la 2da de esta "lest upgrade"
        {
            Debug.Log("PERFECTO");
            ret.Clear();
            //int defaultValue = 0;
            //if (valores.Count != 0) defaultValue = 100 / valores.Count; // esto no entiendo porque pasa
            ////en realida si total 0 es porque el per ya esta maxeado, nunca deberia pasar
            //foreach (var item in valores)
            //{
            //    ret.Add(defaultValue);
            //}
            return ret;
        }
        //else
        //foreach (var item in valores)
        //{
        //    ret.Add(100 - item * 100 / total);
        //}
        //Debug.Log("total: " + total);
        for (int i = 0; i < valores.Count; i++)
        {
            ret.Add((maxValues[i] - valores[i]) * 100 / total);
        }

        // if (ret[ret.Count - 1] == 99) ret[ret.Count - 1] = 100;
        return ret;
    }

    int CalcularPuntosEnHabilidades(cPersonajeFlyweight p)
    {
        int amount = 2;
        if (p.arma == cArma.VOLUNTAD_CREADOR) amount = 6;
        int count = 0;
        for (int i = 0; i < amount; i++)
        {
            count += p.tradicionMarcial[i];
        }
        return count;
    }

    int CalcularPuntosEnAtributos(cPersonajeFlyweight p)
    {
        return p.atr.maña + p.atr.musculo + p.atr.ingenio + p.atr.brio + p.atr.donaire;
    }

    int GetRandomPersonaje(int lvl, List<int> valoresIlegales)
    {
        int ret = Random.Range(0, rM.rC.templatesPersonajes[lvl].Count - valoresIlegales.Count);
        return chequearRepiticion(valoresIlegales, ret);
    }

    int chequearRepiticion(List<int> valoresIlegales, int val)
    {
        valoresIlegales.Sort();
        foreach (var item in valoresIlegales)
        {
            if (val >= item) val++;
        }
        return val;
    }

    public void NuevoPJ(int index, int lvl)
    {
        rM.party.Add(gameObject.AddComponent(typeof(cPersonajeFlyweight)) as cPersonajeFlyweight);
        rM.party[rM.party.Count - 1].Copiar(rM.rC.templatesPersonajes[lvl][index]);
        rM.party[rM.party.Count - 1].equipo = 1;
        rM.party[rM.party.Count - 1].esMaton = false;
        rM.party[rM.party.Count - 1].drama = true;
        rM.party[rM.party.Count - 1].iA = cAI.PLAYER_CONTROLLED;
    }

    public void CambiarTexto(int index, cRoguelikeUpgradeData aUpgradear)
    {
        string text = "";
        if (aUpgradear.tipoDeUpgrade == cRoguelikeUpgradeData.RU_NO_UPGRADE) text = "Soy perfecto como soy";
        else if (aUpgradear.tipoDeUpgrade == cRoguelikeUpgradeData.RU_DESCANSO_COMPLETO) text = "¡A recuperar todas nuestras heridas y Drama!";
        else if (aUpgradear.tipoDeUpgrade == cRoguelikeUpgradeData.RU_MARCIAL) text = "¡" + rM.party[aUpgradear.objetivoDeUpgrade].nombre +" aprende " + cArma.GetString(aUpgradear.upgradesList[0].elementoAUpgradear + cArma.VOLUNTAD_CREADOR) + "!";
        else
        {
            for (int i = 0; i < aUpgradear.upgradesList.Count; i++)
            {
                switch (aUpgradear.upgradesList[i].subTipoDeUpgrade)
                {
                    case cRoguelikeUpgradeData.RUST_HAB: // habs
                        if (rM.party[aUpgradear.objetivoDeUpgrade].arma > cArma.FUEGO)
                        {
                            text += "1+ " + cArma.GetHabilidadString(rM.party[aUpgradear.objetivoDeUpgrade].arma, aUpgradear.upgradesList[i].elementoAUpgradear);
                        }
                        else {
                            switch (aUpgradear.upgradesList[i].elementoAUpgradear)
                            {
                                case 0:
                                    text += "+1 Ataque Básico";
                                    break;
                                case 1:
                                    text += "+1 Defensa Básica";
                                    break;
                                default:
                                    break;
                            }
                        }
                        break;
                    case cRoguelikeUpgradeData.RUST_ATR: //atr
                        switch (aUpgradear.upgradesList[i].elementoAUpgradear)
                        {
                            case 0:
                                text += "+1 Maña";
                                break;
                            case 1:
                                text += "+1 Músculo";
                                break;
                            case 2:
                                text += "+1 Ingenio";
                                break;
                            case 3:
                                text += "+1 Brío";
                                break;
                            case 4:
                                text += "+1 Donaire";
                                break;
                            default:
                                break;
                        }
                        break;
                    default:
                        break;
                }
                if (i == aUpgradear.upgradesList.Count - 2) text += " y ";
                else if (i != aUpgradear.upgradesList.Count - 1) text += ", ";
            }
            text += " para " + rM.party[aUpgradear.objetivoDeUpgrade].nombre;
            if (aUpgradear.tipoDeUpgrade == cRoguelikeUpgradeData.RU_DESCANSO_PARCIAL_Y_SIMPLE) text += " y todos nos curamos una Herida.";
            else if (aUpgradear.tipoDeUpgrade == cRoguelikeUpgradeData.RU_DESCANSOS_Y_DOBLES)
            {
                text += " y la Sombra nos sonrie.";
            }
        }
        uiRU.SetUpgradeText(index, text);
        uiRU.UpgradeTooltip(index, aUpgradear);
    }

    public void CambiarTextoUPJ(int index, cRoguelikeUpgradeData upgrade, int lvl)
    {
        cPersonajeFlyweight p = rM.rC.templatesPersonajes[lvl][upgrade.upgradesList[0].elementoAUpgradear];
        //como decidimos el elmento si no sabiamos el nivel?!?!?
        string text = p.nombre + " - " + cArma.GetString(p.arma);

        // modificar tooltip
        uiRU.SetUpgradeText(index, text);
        uiRU.UpgradeTooltip(index, upgrade);

    }

    public void GetEleccion(int eleccion)
    {
        UpgradeElegido(upgrades[eleccion]);
    }

    public void UpgradeElegido(cRoguelikeUpgradeData upgrade)
    {
        switch (upgrade.tipoDeUpgrade)
        {
            case cRoguelikeUpgradeData.RU_NO_UPGRADE:
                break;
            case cRoguelikeUpgradeData.RU_SIMPLE:
                UpgradeIndividual(upgrade, 0);
                if (rM.party[upgrade.objetivoDeUpgrade].arma > cArma.FUEGO)
                {
                    rM.party[upgrade.objetivoDeUpgrade].maestria = cArma.CalcularMaestria(rM.party[upgrade.objetivoDeUpgrade].tradicionMarcial);
                }
                break;
            case cRoguelikeUpgradeData.RU_DOBLE:
                for (int i = 0; i < 2; i++)
                {
                    UpgradeIndividual(upgrade, i);
                }
                if (rM.party[upgrade.objetivoDeUpgrade].arma > cArma.FUEGO)
                {
                    rM.party[upgrade.objetivoDeUpgrade].maestria = cArma.CalcularMaestria(rM.party[upgrade.objetivoDeUpgrade].tradicionMarcial);
                }
                break;
            case cRoguelikeUpgradeData.RU_TRIPLE:
                for (int i = 0; i < 3; i++)
                {
                    UpgradeIndividual(upgrade, i);
                }
                if (rM.party[upgrade.objetivoDeUpgrade].arma > cArma.FUEGO)
                {
                    rM.party[upgrade.objetivoDeUpgrade].maestria = cArma.CalcularMaestria(rM.party[upgrade.objetivoDeUpgrade].tradicionMarcial);
                }
                break;
            case cRoguelikeUpgradeData.RU_PJ:
                UpgradePJ(upgrade);
                if (rM.party[rM.party.Count-1].arma > cArma.FUEGO)
                {
                    rM.party[rM.party.Count - 1].maestria = cArma.CalcularMaestria(rM.party[rM.party.Count - 1].tradicionMarcial);
                }
                break;
            case cRoguelikeUpgradeData.RU_MARCIAL:
                UpgradeMarcial(upgrade);
                if (rM.party[upgrade.objetivoDeUpgrade].arma > cArma.FUEGO)
                {
                    rM.party[upgrade.objetivoDeUpgrade].maestria = cArma.CalcularMaestria(rM.party[upgrade.objetivoDeUpgrade].tradicionMarcial);
                }
                break;
            case cRoguelikeUpgradeData.RU_ARCANA:
                break;
            case cRoguelikeUpgradeData.RU_TALENTO:
                break;
            case cRoguelikeUpgradeData.RU_DESCANSO_PARCIAL_Y_SIMPLE:
                foreach (var item in rM.party)
                {
                    item.DescansoParcial();
                }
                UpgradeIndividual(upgrade, 0);
                if (rM.party[upgrade.objetivoDeUpgrade].arma > cArma.FUEGO)
                {
                    rM.party[upgrade.objetivoDeUpgrade].maestria = cArma.CalcularMaestria(rM.party[upgrade.objetivoDeUpgrade].tradicionMarcial);
                }
                break;
            case cRoguelikeUpgradeData.RU_DESCANSO_COMPLETO:
                foreach (var item in rM.party)
                {
                    item.DescansoCompleto();
                }
                break;
            case cRoguelikeUpgradeData.RU_DESCANSOS_Y_DOBLES:
                foreach (var item in rM.party)
                {
                    item.DescansoCompleto();
                }
                for (int i = 0; i < 2; i++)
                {
                    UpgradeIndividual(upgrade, i);
                }
                if (rM.party[upgrade.objetivoDeUpgrade].arma > cArma.FUEGO)
                {
                    rM.party[upgrade.objetivoDeUpgrade].maestria = cArma.CalcularMaestria(rM.party[upgrade.objetivoDeUpgrade].tradicionMarcial);
                }
                break;
            default:
                break;
        }
        Listorti();
    }

    public void UpgradeMarcial(cRoguelikeUpgradeData upgrade)
    {
        Debug.Log("UM");
        cPersonajeFlyweight per = rM.party[upgrade.objetivoDeUpgrade];
        per.tradicionMarcialId = upgrade.upgradesList[0].elementoAUpgradear;
        per.arma = cArma.VOLUNTAD_CREADOR + per.tradicionMarcialId;
        Debug.Log(per.arma);
        int calidad = per.tradicionMarcial[0] + per.tradicionMarcial[1];
        List<int> ilegales = new List<int>();
        while(calidad > 0)
        {
            calidad--;
            int r = Random.Range(0, 4-ilegales.Count);
            foreach (var item in ilegales)
            {
                if (r >= item) r++;
            }
            if (++per.tradicionMarcial[r + 2] > 4)
            {
                ilegales.Add(r);
                ilegales.Sort();
            }
        }
    }

        public void DBUpgrades()
    {
        for (int i = 0; i < 3; i++)
        {
            Debug.Log("Upgrade " + i);
            DBSingleUpgrade(upgrades[i]);
        }
    }

    public void DBSingleUpgrade(cRoguelikeUpgradeData upgrade)
    {
        string text = "tipo - " + upgrade.tipoDeUpgrade + ", objetivo: " + upgrade.objetivoDeUpgrade;
        foreach (var item in upgrade.upgradesList)
        {
            text += " ----- subtipo: " + item.subTipoDeUpgrade + " y elemento: " + item.elementoAUpgradear;
        }
        Debug.Log(text);
    }

    public void UpgradeIndividual(cRoguelikeUpgradeData upgrade, int subUpgradeNum)
    {
        //DBUpgrades();
        //DBSingleUpgrade(upgrade);
        switch (upgrade.upgradesList[subUpgradeNum].subTipoDeUpgrade)
        {
            case cRoguelikeUpgradeData.RUST_HAB: // habs
                rM.party[upgrade.objetivoDeUpgrade].tradicionMarcial[upgrade.upgradesList[subUpgradeNum].elementoAUpgradear]++;
                break;
            case cRoguelikeUpgradeData.RUST_ATR: //atr
                switch (upgrade.upgradesList[subUpgradeNum].elementoAUpgradear)
                {
                    case 0:
                        rM.party[upgrade.objetivoDeUpgrade].atr.maña++;
                        break;
                    case 1:
                        rM.party[upgrade.objetivoDeUpgrade].atr.musculo++;
                        break;
                    case 2:
                        rM.party[upgrade.objetivoDeUpgrade].atr.ingenio++;
                        break;
                    case 3:
                        rM.party[upgrade.objetivoDeUpgrade].atr.brio++;
                        break;
                    case 4:
                        rM.party[upgrade.objetivoDeUpgrade].atr.donaire++;
                        break;
                    default:
                        break;
                }
                break;
            default:
                break;
        }
    }

    public void UpgradePJ(cRoguelikeUpgradeData upgrade)
    {
        int lvl = 1;
        if (rM.nivel > 5) lvl = 2;
        NuevoPJ(upgrade.upgradesList[0].elementoAUpgradear, lvl);
    }

    void Listorti()
    {
        uiRU.RevisarParty();
    }

    public cPersonajeFlyweight GetPerInUpgrade(int index)
    {
        int pJIntervalo = 5;
        int lvl = 1;
        if (rM.nivel > pJIntervalo) lvl = 2;
        return rM.rC.templatesPersonajes[lvl][upgrades[index].upgradesList[0].elementoAUpgradear];
    }

    public bool EsUpgradeTM(int index)
    {
        if(index >= 0) return upgrades[index].tipoDeUpgrade == cRoguelikeUpgradeData.RU_MARCIAL;
        return false;
    }

    // Update is called once per frame
    void Update()
    {
        //if (waitingForOk && py.actions["Select"].WasPressedThisFrame() && !anticipando)
        //{
        //    rogueMenuRevisar.SetActive(false);
        //    rogueMenuAnticipar.SetActive(true);
        //    Anticipar();
        //}
        //else if (waitingForOk && py.actions["Select"].WasPressedThisFrame() && anticipando)
        //{
        //    waitingForOk = false;
        //    anticipando = false;
        //    rM.EmpezarCombate();
        //    rogueMenuAnticipar.SetActive(false);
        //    gameObject.SetActive(false);
        //}
    }
}
