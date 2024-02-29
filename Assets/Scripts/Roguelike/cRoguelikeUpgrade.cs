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

    int[] upgrade = new int[3];
    cRoguelikeUpgradeData[] upgrades = new cRoguelikeUpgradeData[3];

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

        //cada 5 las tres son pj upgrade
        if (rM.nivel % 5 == 0 && rM.nivel < 14)
        {
            List<int> ilegales = rM.rC.PersonajesYaEnEquipo();
            Debug.Log("todos pjs");
            for (int i = 0; i < 3; i++)
            {
                upgrades[i].tipoDeUpgrade = cRoguelikeUpgradeData.RU_PJ;
       
                sSingleUpgrade s = new sSingleUpgrade();
                upgrades[i].upgradesList.Add(s);
                upgrades[i].upgradesList[0] = new sSingleUpgrade();
                upgrades[i].upgradesList[0].elementoAUpgradear = GetRandomPersonaje(ilegales);
                ilegales.Add(upgrades[i].upgradesList[0].elementoAUpgradear);
            }

            for (int i = 0; i < 3; i++)
            {
                CambiarTextoUPJ(i, upgrades[i]);
            }
        }
        //cada 3 las tres son triple upgrade
        else if (rM.nivel % 3 == 0)
        {
            Debug.Log("todas triples");
            for (int i = 0; i < 3; i++)
            {
                upgrades[i].tipoDeUpgrade = cRoguelikeUpgradeData.RU_TRIPLE;
                DefinirUpgrade(i, 3);
            }

            for (int i = 0; i < 3; i++)
            {
                CambiarTexto(i, upgrades[i]);
            }
        }
        //en todas las demas hay 1/3 de que una sea doble
        else if (Random.Range(0, 3) == 0)
        {
            Debug.Log("una doble");
            upgrades[0].tipoDeUpgrade = cRoguelikeUpgradeData.RU_SIMPLE;
            upgrades[1].tipoDeUpgrade = cRoguelikeUpgradeData.RU_DOBLE;
            upgrades[2].tipoDeUpgrade = cRoguelikeUpgradeData.RU_SIMPLE;
            DefinirUpgrade(0, 1);
            DefinirUpgrade(1, 2);
            DefinirUpgrade(2, 1);

            for (int i = 0; i < 3; i++)
            {
                CambiarTexto(i, upgrades[i]);
            }
        }
        //si no, todas son normales
        else
        {
            Debug.Log("todas simples");
            for (int i = 0; i < 3; i++)
            {
                upgrades[i].tipoDeUpgrade = cRoguelikeUpgradeData.RU_SIMPLE;
                DefinirUpgrade(i, 1);
            }
            for (int i = 0; i < 3; i++)
            {
                CambiarTexto(i, upgrades[i]);
            }
        }
    }

    public void DefinirUpgrade(int upgradeSlot, int amount)
    {
        ElegirObjetivo(upgradeSlot);
        for (int i = 0; i < amount; i++)
        {
            sSingleUpgrade s = new sSingleUpgrade();
            upgrades[upgradeSlot].upgradesList.Add(s);
            ElegirSubtipo(upgradeSlot, i);
            ElegirElemento(upgradeSlot, i);
        }
    }

    void ElegirObjetivo(int slot)
    {
        int max;
        if (rM.party.Count == 1) upgrades[slot].objetivoDeUpgrade = 0;
        else
        {
            // habria que chequear si algun per esta maxeado, y scarlo de las posibiliddaes
            //funcion de chequear atributos maxeado
            //funcion de chequ habilidaes maxeadas
            // fun cion de cheq per maxeado, que llama estas cuando corresponde y devuelve un bool
            List<int> countPoints = new List<int>();
            int count;

            for (int i = 0; i < rM.party.Count; i++)
            {
                count = CalcularPuntosEnAtributos(rM.party[i]);
                count += CalcularPuntosEnHabilidades(rM.party[i]);
                countPoints.Add(count / 2);
            }
            max = 10;
            List<int> weightedPoints = WeightearElementos(countPoints, max);
            //Debug.Log("Hasta este, priemr per " + weightedPoints[0]);
            //Debug.Log("Hasta este, segundo per: " + (weightedPoints[0] + weightedPoints[1]) + " despues 3er per");
            upgrades[slot].objetivoDeUpgrade = GetWeightedElement(weightedPoints);
        }
    }

    void ElegirSubtipo(int slot, int subUpgradeNum)
    {
        Debug.Log("eligiendo subtipo");
        List<int> counts = new List<int>();
        //estos cosnt por ahroa estan hardcodeados, hab total va a cambiar, y por lo tanto el max tambien
        //cuando haya tradiciones en el jueego
        const int max = 10;
        const int habTotal = 2;
        const int atrTotal = 5;
        int upgradeHabs = uHechas[upgrades[slot].objetivoDeUpgrade].upgradeHabs;
        int upgradeAtrs = uHechas[upgrades[slot].objetivoDeUpgrade].upgradeAtrs;

        int sum = CalcularPuntosEnHabilidades(rM.party[upgrades[slot].objetivoDeUpgrade]);
        counts.Add(sum + (max - sum) * (upgradeHabs / habTotal));

        sum = CalcularPuntosEnAtributos(rM.party[upgrades[slot].objetivoDeUpgrade]);
        counts.Add(sum + (max - sum) * (upgradeAtrs / atrTotal));

        List<int> weighted = WeightearElementos(counts, max);
        upgrades[slot].upgradesList[subUpgradeNum].subTipoDeUpgrade = GetWeightedElement(weighted);
        if (upgrades[slot].upgradesList[subUpgradeNum].subTipoDeUpgrade == cRoguelikeUpgradeData.RUST_HAB) uHechas[upgrades[slot].objetivoDeUpgrade].upgradeHabs++;
        else uHechas[upgrades[slot].objetivoDeUpgrade].upgradeAtrs++;
    }

    void ElegirElemento(int slot, int subUpgradeNum)
    {
        Debug.Log("eligiendo elemento");
        List<int> counts = new List<int>();
        List<int> weighted;
        int max = 0;
        switch (upgrades[slot].upgradesList[subUpgradeNum].subTipoDeUpgrade)
        {
            case 0: // habilidaes
                max = 5;
                if (UpgradeYaSeleccionado(slot, subUpgradeNum, 0, 0)) counts.Add(max);
                else counts.Add(rM.party[upgrades[slot].objetivoDeUpgrade].hab.ataqueBasico);
                if (UpgradeYaSeleccionado(slot, subUpgradeNum, 0, 1)) counts.Add(max);
                else counts.Add(rM.party[upgrades[slot].objetivoDeUpgrade].hab.defensaBasica);
                break;
            case 1: // atributos
                max = 2;
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
        weighted = WeightearElementos(counts, max);

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
        upgrades[slot].upgradesList[subUpgradeNum].elementoAUpgradear = GetWeightedElement(weighted);
    }

    bool UpgradeYaSeleccionado(int slot, int subUpgradeNum, int subTipo, int elemento)
    {
        if (upgrades[slot].tipoDeUpgrade == cRoguelikeUpgradeData.RU_TRIPLE) return UpgradeYaSeleccionadoEstaUpgrade(slot,  subUpgradeNum, subTipo, elemento);
        return UpgradeYaSeleccionadoEnGeneral(slot, subUpgradeNum, subTipo, elemento);
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
            if (i + 1 == weighted.Count) return i;//si es la ultima opcion, es el resultado
            weightsSum += weighted[i];
            if (r < weightsSum)
            {
                return i;
            }
        }
        return -9;
    }

    List<int> WeightearElementos(List<int> valores, int maxValue)
    {
        //Debug.Log("valores count: " + valores.Count);
        List<int> ret = new List<int>();
        int total = 0;
        foreach (var item in valores)
        {
            total += maxValue - item;
        }

        if (total == 0)
        {
            int defaulValue = 0;
            if (valores.Count != 0) defaulValue = 100 / valores.Count; // esto no entiendo porque pasa
            //en realida si total 0 es porque el per ya esta maxeado, nunca deberia pasar
            foreach (var item in valores)
            {
                ret.Add(defaulValue);
            }
            return ret;
        }
        //else
        //foreach (var item in valores)
        //{
        //    ret.Add(100 - item * 100 / total);
        //}
        //Debug.Log("total: " + total);
        foreach (var item in valores)
        {
            // Debug.Log("Weight: " + (maxValue - item));
            ret.Add((maxValue - item) * 100 / total);
        }
        // if (ret[ret.Count - 1] == 99) ret[ret.Count - 1] = 100;
        return ret;
    }

    int CalcularPuntosEnHabilidades(cPersonajeFlyweight p)
    {
        return p.hab.ataqueBasico + p.hab.defensaBasica;
    }

    int CalcularPuntosEnAtributos(cPersonajeFlyweight p)
    {
        return p.atr.maña + p.atr.musculo + p.atr.ingenio + p.atr.brio + p.atr.donaire;
    }

    int GetRandomPersonaje(List<int> valoresIlegales)
    {
        int ret = Random.Range(0, rM.rC.templatesPersonajes.Count - valoresIlegales.Count);
        return chequearRepiticion(valoresIlegales, ret);
    }

    int chequearRepiticion(List<int> valoresIlegales, int val)
    {
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
        rM.party[rM.party.Count - 1].iA = cAI.PLAYER_CONTROLLED;
    }

    public void CambiarTexto(int index, cRoguelikeUpgradeData aUpgradear)
    {
        string text = "";
        for (int i = 0; i < aUpgradear.upgradesList.Count; i++)
        {
            switch (aUpgradear.upgradesList[i].subTipoDeUpgrade)
            {
                case cRoguelikeUpgradeData.RUST_HAB: // habs
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
        uiRU.SetUpgradeText(index, text);
    }

    public void CambiarTextoUPJ(int index, cRoguelikeUpgradeData upgrade)
    {
        int lvl = 2;
        if (rM.nivel < 7) lvl = 1;
        cPersonajeFlyweight p = rM.rC.templatesPersonajes[lvl][upgrade.upgradesList[0].elementoAUpgradear];
        string text = p.nombre + " ( " + cArma.GetString(p.arma) + " )";
        uiRU.SetUpgradeText(index, text);
    }

    public void CambiarTextoUpgrades()
    {
        for (int i = 0; i < 3; i++)
        {
            switch (upgrades[i].tipoDeUpgrade)
            {
                case cRoguelikeUpgradeData.RU_SIMPLE:
                    CambiarTexto(i, upgrades[i]);
                    break;
                case cRoguelikeUpgradeData.RU_DOBLE:
                    CambiarTexto(i, upgrades[i]);
                    break;
                case cRoguelikeUpgradeData.RU_TRIPLE:
                    CambiarTexto(i, upgrades[i]);
                    break;
                case cRoguelikeUpgradeData.RU_PJ:
                    CambiarTextoUPJ(i, upgrades[i]);
                    break;
                case cRoguelikeUpgradeData.RU_MARCIAL:
                    break;
                case cRoguelikeUpgradeData.RU_ARCANA:
                    break;
                case cRoguelikeUpgradeData.RU_TALENTO:
                    break;
                default:
                    break;
            }
        }
    }

    public void GetEleccion(int eleccion)
    {
        UpgradeElegido(upgrades[eleccion]);
    }

    public void UpgradeElegido(cRoguelikeUpgradeData upgrade)
    {
        switch (upgrade.tipoDeUpgrade)
        {
            case cRoguelikeUpgradeData.RU_SIMPLE:
                UpgradeIndividual(upgrade, 0);
                break;
            case cRoguelikeUpgradeData.RU_DOBLE:
                for (int i = 0; i < 2; i++)
                {
                    UpgradeIndividual(upgrade, i);
                }
                break;
            case cRoguelikeUpgradeData.RU_TRIPLE:
                for (int i = 0; i < 3; i++)
                {
                    UpgradeIndividual(upgrade, i);
                }
                break;
            case cRoguelikeUpgradeData.RU_PJ:
                UpgradePJ(upgrade);
                break;
            case cRoguelikeUpgradeData.RU_MARCIAL:
                break;
            case cRoguelikeUpgradeData.RU_ARCANA:
                break;
            case cRoguelikeUpgradeData.RU_TALENTO:
                break;
            default:
                break;
        }
        Listorti();
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
                switch (upgrade.upgradesList[subUpgradeNum].elementoAUpgradear)
                {
                    case 0:
                        rM.party[upgrade.objetivoDeUpgrade].hab.ataqueBasico++;
                        break;
                    case 1:
                        rM.party[upgrade.objetivoDeUpgrade].hab.defensaBasica++;
                        break;
                    default:
                        break;
                }
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
        int lvl;
        if (rM.nivel < 7) lvl = 1;
        else lvl = 2;
        NuevoPJ(upgrade.upgradesList[0].elementoAUpgradear,lvl);
    }

    void Listorti()
    {
        uiRU.RevisarParty();
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
