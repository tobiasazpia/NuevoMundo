using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class cRoguelikeUpgrade : MonoBehaviour
{
    public UIRoguelikeUpgrade uiRU;
    public cRoguelikeManager rM;
    public GameObject rogueMenuRevisar, rogueMenuElegir, rogueMenuAnticipar;
    public GameObject personajeInfo, atributos, anticipacion;
    public PlayerInput py;

    int[] upgrade = new int[3];
    cRoguelikeUpgradeData[] upgradeNormales = new cRoguelikeUpgradeData[3];
    cRoguelikeUpgradeData[] upgradeNormalesAlternativa = new cRoguelikeUpgradeData[3];
    bool[] esUpgradesuper = new bool[3];

    int upgradesCombinacionesPosibles;
    int cantidadDeMatonesEnParty;

    int upgradesCombinacionesHechas = 0;
    int upgradesMatonHechas = 0; // permanente
    int upgradesPJHechas = 0; // permanente

    private void OnEnable()
    {
        upgradesCombinacionesHechas = 0;
        upgradesMatonHechas = 0;

        for (int i = 0; i < upgradeNormales.Length; i++)
        {
            upgradeNormales[i] = new cRoguelikeUpgradeData();
            upgradeNormalesAlternativa[i] = new cRoguelikeUpgradeData();
        }
        /*
         Upgrades Supers
        primero rand 3 para elegir entre
        Combinando normales 
        nuevo pj
        nuevo Maton

        Combinando cualquiera 2 normales = 
        6+5+4+3+2+1 (21) sin matones
        9+8+7+6+5+4+3+2+1 (o 21+8*3=45) con matones
        o
        nuevo pj
        rand template para elegir cual
        o
        nuevo maton, empieza en 2,
        rand template para elegir cual
         */

        /*
         Despues habria que hacer un weight, para que las cosas sean menos probables cuanto mas tengas de ellas
         */


    }

    public void LetsUpgrade()
    {
        Debug.Log("Lets Upgrade");
        foreach (var item in upgradeNormales)
        {
            Debug.Log("les upgrade reset");
            item.elementoAUpgradear = -1;
            item.objetivoDeUpgrade = -1;
            item.tipoDeUpgrade = -1;
        }
        AveriguarMatonesEnParty();

        //cada 3 las 3 son super upgrade
        int mod = rM.nivel % 3;
        if (mod == 0)
        {
            Debug.Log("Todos Especiales!");
            AveriguarUpgradesCombinacionesPosibles();
            for (int i = 0; i < 3; i++)
            {
                esUpgradesuper[i] = true;
                DefinirSuperUpgrade(i);
            }
            //noRepitamos();
            for (int i = 0; i < 3; i++)
            {
                CambiarTextoSuper(i, upgrade[i]);
            }
        }
        //en todas las demas hay 1/3 de que una sea super
        else if (Random.Range(0, 3) == 0)
        {
            Debug.Log("Un Especial!");
            AveriguarUpgradesCombinacionesPosibles();
            esUpgradesuper[0] = false;
            esUpgradesuper[2] = false;
            DefinirUpgradeNormal(0);
            DefinirUpgradeNormal(2);
            //noRepitamos();
            esUpgradesuper[1] = true;
            DefinirSuperUpgrade(1);
            CambiarTextoNormal(0, upgradeNormales[0]);
            CambiarTextoSuper(1, upgrade[1]);
            CambiarTextoNormal(2, upgradeNormales[2]);
        }
        //si no, todas son normales
        else
        {
            Debug.Log("Todos normales");
            for (int i = 0; i < 3; i++)
            {
                esUpgradesuper[i] = false;
                DefinirUpgradeNormal(i);
            }
            //noRepitamos();
            for (int i = 0; i < 3; i++)
            {
                CambiarTextoNormal(i, upgradeNormales[i]);
            }
        }
    }


    public void DefinirSuperUpgrade(int upgradeSlot)
    {
        Debug.Log("Definiendo super upgrade");
        int opciones = 3 - upgradesPJHechas - upgradesMatonHechas;
        int tipoOpcion = Random.Range(0, opciones);
        if (tipoOpcion == 1 && upgradesPJHechas > 0) tipoOpcion++;
        Debug.Log("Opcion: " + tipoOpcion);
        Debug.Log("Party count: " + rM.party.Count);
        if (rM.party.Count == 3)
        {
            tipoOpcion = 0;
            Debug.Log("Forzando Opcion a 0");
        }
        switch (tipoOpcion)
        {
            case 0:
                //combinacion normales
                upgrade[upgradeSlot] = 0;
                SuperUpgradeCombinacion(upgradeSlot);
                //Con esto ya tenemos asignado en upgradeNormal y upgradeAlt los tipo y elemento
                //falta convertir eso en algo usable para genera el texto y aplciar la upgrade
                break;
            case 1:
                // pj
                upgradesPJHechas++;
                upgrade[upgradeSlot] = 1;
                break;
            case 2:
                // maton
                upgradesMatonHechas++;
                upgrade[upgradeSlot] = 2;
                break;
        }
    }

    public void SuperUpgradeCombinacion(int slot)
    {
        ElegirObjetivo(slot);

        List<int> counts = new List<int>();
        int max = 0;
        int tipo;
        for (int i = 0; i < 2; i++)
        {
            counts.Clear();
            Debug.Log("una opcion de super comb");
            ElegirTipo(slot, i);

            if (i == 0) tipo = upgradeNormales[slot].tipoDeUpgrade;
            else tipo = upgradeNormalesAlternativa[slot].tipoDeUpgrade;
            switch (tipo)
            {
                case 0: // habilidaes
                    max = 5;
                    if (UpgradeYaSeleccionado(0, 0)) counts.Add(max);
                    else counts.Add(rM.party[upgradeNormales[slot].objetivoDeUpgrade].hab.ataqueBasico);
                    if (UpgradeYaSeleccionado(0, 1)) counts.Add(max);
                    else counts.Add(rM.party[upgradeNormales[slot].objetivoDeUpgrade].hab.defensaBasica);
                    break;
                case 1: // atributos
                    max = 2;
                    if (UpgradeYaSeleccionado(1, 0)) counts.Add(max);
                    else counts.Add(rM.party[upgradeNormales[slot].objetivoDeUpgrade].atr.maña);
                    if (UpgradeYaSeleccionado(1, 1)) counts.Add(max);
                    else counts.Add(rM.party[upgradeNormales[slot].objetivoDeUpgrade].atr.musculo);
                    if (UpgradeYaSeleccionado(1, 2)) counts.Add(max);
                    else counts.Add(rM.party[upgradeNormales[slot].objetivoDeUpgrade].atr.ingenio);
                    if (UpgradeYaSeleccionado(1, 3)) counts.Add(max);
                    else counts.Add(rM.party[upgradeNormales[slot].objetivoDeUpgrade].atr.brio);
                    if (UpgradeYaSeleccionado(1, 4)) counts.Add(max);
                    else counts.Add(rM.party[upgradeNormales[slot].objetivoDeUpgrade].atr.donaire);
                    //LlenarUpgradesIlegales(ilegales, upgradeSlot);
                    //upgr[upgradeSlot].elementoAUpgradear = GetRandomAtr(ilegales);
                    break;
                case 2: // cantidad
                    break;
                default:
                    break;
            }
            Debug.Log("A");
            List<int> weighted = WeightearElementos(counts, max);
            switch (tipo)
            {
                case 0:
                    Debug.Log("Hasta este, ab " + weighted[0] + " si no DB");
                    break;
                case 1:
                    Debug.Log("Hasta este, maña " + weighted[0]);
                    Debug.Log("Hasta este, musuclo " + (weighted[0]+weighted[1]));
                    Debug.Log("Hasta este, ingenio " + (weighted[0] + weighted[1]+weighted[2]));
                    Debug.Log("Hasta este, brio " + (weighted[0] + weighted[1] + weighted[2]+weighted[3]) + "si no, donaire");
                    break;
                default:
                    break;
            }

            if (i == 0)
            {
                Debug.Log("finalizando primra mitad");
                upgradeNormales[slot].elementoAUpgradear = GetWeightedElement(weighted);
            }
            else
            {
                Debug.Log("finalizando segunda mitad");
                upgradeNormalesAlternativa[slot].elementoAUpgradear = GetWeightedElement(weighted);
            }
        }        
    }

    public void noRepitamos()
    {
        int masChico;
        int masGrande;
        if (upgrade[1] >= upgrade[0])
        {
            upgrade[1]++;
            masChico = upgrade[0];
            masGrande = upgrade[1];
        }
        else
        {
            masGrande = upgrade[0];
            masChico = upgrade[1];
        }

        if (upgrade[2] >= masChico) upgrade[2]++;
        if (upgrade[2] >= masGrande) upgrade[2]++;
    }

    public void AveriguarUpgradesCombinacionesPosibles()
    {
        if (cantidadDeMatonesEnParty > 0)
            upgradesCombinacionesPosibles = 45;
        else upgradesCombinacionesPosibles = 21;
        upgradesCombinacionesPosibles *= rM.party.Count;
    }

    public void AveriguarMatonesEnParty()
    {
        cantidadDeMatonesEnParty = 0;
        foreach (var item in rM.party)
        {
            if (item.esMaton && item.cantidad < 10) cantidadDeMatonesEnParty++;
        }
    }


    public void DefinirUpgradeNormal(int upgradeSlot)
    {
        Debug.Log("def upgrade normal");

        ElegirObjetivo(upgradeSlot);
        ElegirTipo(upgradeSlot,0);
        ElegirElemento(upgradeSlot);

        /*
         Upgrades Normales
        +1 en una Hab
        +1 en un Atr
        +1 en cantidad para un maton
         */

        /*Super Upgrades
         2 upgrades normales juntas
        nuevo maton
        nuevo pj
        */

        /*Otras Upgrades posibles
         cambiar de arma
        nuevo tradicion marcial
        nueva tradicio narcana
        descanso*/
    }

    void ElegirObjetivo(int slot)
    {
        int max;
        if (rM.party.Count == 1) upgradeNormales[slot].objetivoDeUpgrade = 0;
        else
        {
            Debug.Log("objetivo:");
            // habria que chequear si algun per esta maxeado, y scarlo de las posibiliddaes
            //funcion de chequear atributos maxeado
            //funcion de chequ habilidaes maxeadas
            // funcion de chequear cant maxeada
            // fun cion de cheq per maxeado, que llama estas cuando corresponde y devuelve un bool
            List<int> countPoints = new List<int>();
            int count;

            for (int i = 0; i < rM.party.Count; i++)
            {
                count = CalcularPuntosEnAtributos(rM.party[i]);
                count += CalcularPuntosEnHabilidades(rM.party[i]);
                if (rM.party[i].esMaton)
                {
                    count += rM.party[i].cantidad;
                    countPoints.Add(count / 3);
                }
                else
                {
                    countPoints.Add(count / 2);
                }
            }
            max = 10;
            Debug.Log("B");
            List<int> weightedPoints = WeightearElementos(countPoints, max);
            Debug.Log("Hasta este, priemr per " + weightedPoints[0]);
            Debug.Log("Hasta este, segundo per: " + (weightedPoints[0] + weightedPoints[1]) + " despues 3er per");
            upgradeNormales[slot].objetivoDeUpgrade = GetWeightedElement(weightedPoints);
        }
    }

    void ElegirTipo(int slot, int alt)
    {
        Debug.Log("tipo:");
        bool esMaton = rM.party[upgradeNormales[slot].objetivoDeUpgrade].esMaton;
        List<int> counts = new List<int>();
        int max = 20;
        int upgradeHabs = 0;
        int updradeHabsObj = -1;
        foreach (var item in upgradeNormales)
        {
            if (item.tipoDeUpgrade == 0)
            {
                if(upgradeHabs == 0) { 
                    upgradeHabs++;
                    updradeHabsObj = item.objetivoDeUpgrade;
                }
                else
                {
                    if(updradeHabsObj == item.objetivoDeUpgrade) upgradeHabs++;
                }
            }
        }
        Debug.Log("ya upgradeamos habs este up: " + upgradeHabs + " veces");
        if (upgradeHabs < 2)
        {
            counts.Add(CalcularPuntosEnHabilidades(rM.party[upgradeNormales[slot].objetivoDeUpgrade]));
        }
        else counts.Add(10); //todo esto hardcodeado para ab y DB, el 2 es la cantidad de habs, y el 10 es maxVal*cant
        counts.Add(CalcularPuntosEnAtributos(rM.party[upgradeNormales[slot].objetivoDeUpgrade]));
        if (esMaton)
        {
            counts.Add(rM.party[upgradeNormales[slot].objetivoDeUpgrade].cantidad);
            max = 30;
        }
        Debug.Log("C");
        List<int> weighted = WeightearElementos(counts, max);
        Debug.Log("Hasta este, hab " + weighted[0]);
        Debug.Log("Hasta este, atr: " + (weighted[0] + weighted[1]) + " despues cant");
        if(alt == 0) upgradeNormales[slot].tipoDeUpgrade = GetWeightedElement(weighted);
        else upgradeNormalesAlternativa[slot].tipoDeUpgrade = GetWeightedElement(weighted);
    }

    void ElegirElemento(int slot)
    {
        Debug.Log("upgrade:");
        List<int> counts = new List<int>();
        List<int> weighted;
        int max = 0;
        switch (upgradeNormales[slot].tipoDeUpgrade)
        {
            case 0: // habilidaes
                max = 5;
                if (UpgradeYaSeleccionado(0, 0)) counts.Add(max);
                else counts.Add(rM.party[upgradeNormales[slot].objetivoDeUpgrade].hab.ataqueBasico);
                if (UpgradeYaSeleccionado(0, 1)) counts.Add(max);
                else counts.Add(rM.party[upgradeNormales[slot].objetivoDeUpgrade].hab.defensaBasica);
                break;
            case 1: // atributos
                max = 2;
                if (UpgradeYaSeleccionado(1, 0)) counts.Add(max);
                else counts.Add(rM.party[upgradeNormales[slot].objetivoDeUpgrade].atr.maña);
                if (UpgradeYaSeleccionado(1, 1)) counts.Add(max);
                else counts.Add(rM.party[upgradeNormales[slot].objetivoDeUpgrade].atr.musculo);
                if (UpgradeYaSeleccionado(1, 2)) counts.Add(max);
                else counts.Add(rM.party[upgradeNormales[slot].objetivoDeUpgrade].atr.ingenio);
                if (UpgradeYaSeleccionado(1, 3)) counts.Add(max);
                else counts.Add(rM.party[upgradeNormales[slot].objetivoDeUpgrade].atr.brio);
                if (UpgradeYaSeleccionado(1, 4)) counts.Add(max);
                else counts.Add(rM.party[upgradeNormales[slot].objetivoDeUpgrade].atr.donaire);
                //LlenarUpgradesIlegales(ilegales, upgradeSlot);
                //upgr[upgradeSlot].elementoAUpgradear = GetRandomAtr(ilegales);
                break;
            case 2: // cantidad
                break;
            default:
                break;
        }
        Debug.Log("D");
        weighted = WeightearElementos(counts, max);

        switch (upgradeNormales[slot].tipoDeUpgrade)
        {
            case 0:
                Debug.Log("Hasta este, ab " + weighted[0] + " si no DB");
                break;
            case 1:
                Debug.Log("Hasta este, maña " + weighted[0]);
                Debug.Log("Hasta este, musuclo " + (weighted[0]+weighted[1]));
                Debug.Log("Hasta este, ingenio " + (weighted[0] + weighted[1]+weighted[2]));
                Debug.Log("Hasta este, brio " + (weighted[0] + weighted[1] + weighted[2]+weighted[3]) + "si no, donaire");
                break;
            default:
                break;
        }
        upgradeNormales[slot].elementoAUpgradear = GetWeightedElement(weighted);
    }

        bool UpgradeYaSeleccionado(int tipo, int elemento)
    {
        foreach (var item in upgradeNormales)
        {
            if (item.tipoDeUpgrade == tipo && item.elementoAUpgradear == elemento)
            {
                return true;
            }
        }
        return false;
    }

    int GetWeightedElement(List<int> weighted)
    {
        int r = cDieMath.MyRandomUpgrade();
        Debug.Log("R salio: " + r);
        Debug.Log("weight count: " + weighted.Count);
        int weightsSum = 0;
        for (int i = 0; i < weighted.Count; i++)
        {
            Debug.Log("for loop i: " + i);
            weightsSum += weighted[i];
            Debug.Log("new weight: " + weighted[i]);
            Debug.Log("weightsSum: " + weightsSum);
            if (r < weightsSum)
            {
                return i;
            }
        }
        return -9;
    }

    List<int> WeightearElementos(List<int> valores, int maxValue)
    {
        Debug.Log("valores count: " + valores.Count);
        List<int> ret = new List<int>();
        int total = 0;
        foreach (var item in valores)
        {
            total += maxValue-item;
        }

        if (total == 0)
        {
            int defaulValue = 0;
            if(valores.Count != 0) defaulValue = 100 / valores.Count; // estto no entiendo porque pasa

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
        Debug.Log("total: " + total);
        foreach (var item in valores)
        {
            Debug.Log("Weight: " + (maxValue - item));
            ret.Add((maxValue - item) * 100 / total);
        }
        if (ret[ret.Count - 1] == 99) ret[ret.Count - 1] = 100;
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

    int GetRandomMaton(List<int> valoresIlegales)
    {
        int ret = Random.Range(0, rM.rC.templatesMatones.Count - valoresIlegales.Count);
        return chequearRepiticion(valoresIlegales, ret);
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

    public void NuevoMaton()
    {
        List<int> ilegales = new List<int>();  // si tenemos matones, habria que buscar en que pos de la template estan y agregar esos numeros a la lista de ilegales
        ilegales = rM.rC.MatonesYaEnEquipo();
        rM.party.Add(gameObject.AddComponent(typeof(cPersonajeFlyweight)) as cPersonajeFlyweight);
        rM.party[rM.party.Count - 1].Copiar(rM.rC.templatesMatones[GetRandomMaton(ilegales)]);
        rM.party[rM.party.Count - 1].equipo = 1;
        rM.party[rM.party.Count - 1].esMaton = true;
        rM.party[rM.party.Count - 1].cantidad = 2;
        rM.party[rM.party.Count - 1].iA = cAI.PLAYER_CONTROLLED;
    }

    public void NuevoPJ()
    {
        List<int> ilegales = new List<int>(); // si tenemos otros pjs ademas del inicial, habria que buscar en que pos de la template estan y agregar esos numeros a la lista de ilegales
        ilegales = rM.rC.PersonajesYaEnEquipo();
        rM.party.Add(gameObject.AddComponent(typeof(cPersonajeFlyweight)) as cPersonajeFlyweight);
        rM.party[rM.party.Count - 1].Copiar(rM.rC.templatesPersonajes[GetRandomPersonaje(ilegales)]);
        rM.party[rM.party.Count - 1].equipo = 1;
        rM.party[rM.party.Count - 1].esMaton = false;
        rM.party[rM.party.Count - 1].iA = cAI.PLAYER_CONTROLLED;
    }

    public void CambiarTextoNormal(int index, cRoguelikeUpgradeData aUpgradear)
    {
        string text = "";
        switch (aUpgradear.tipoDeUpgrade)
        {
            case 0: // habs
                switch (aUpgradear.elementoAUpgradear)
                {
                    case 0:
                        text = "+1 Ataque Básico para " + rM.party[aUpgradear.objetivoDeUpgrade].nombre;
                        break;
                    case 1:
                        text = "+1 Defensa Básica para " + rM.party[aUpgradear.objetivoDeUpgrade].nombre;
                        break;
                    default:
                        break;
                }
                break;
            case 1: //atr
                switch (aUpgradear.elementoAUpgradear)
                {
                    case 0:
                        text = "+1 Maña para " + rM.party[aUpgradear.objetivoDeUpgrade].nombre;
                        break;
                    case 1:
                        text = "+1 Músculo para " + rM.party[aUpgradear.objetivoDeUpgrade].nombre;
                        break;
                    case 2:
                        text = "+1 Ingenio para " + rM.party[aUpgradear.objetivoDeUpgrade].nombre;
                        break;
                    case 3:
                        text = "+1 Brío para " + rM.party[aUpgradear.objetivoDeUpgrade].nombre;
                        break;
                    case 4:
                        text = "+1 Donaire para " + rM.party[aUpgradear.objetivoDeUpgrade].nombre;
                        break;
                    default:
                        break;
                }
                break;
            case 2://cant
                text = "+1 Cantidad para " + rM.party[aUpgradear.objetivoDeUpgrade].nombre;
                break;
            default:
                break;
        }
        uiRU.SetUpgradeText(index, text);
    }

    public void CambiarTextoSuper(int index, int value)
    {
        string text = "";
        Debug.Log("cambiando texto super, value: " + value);
        if (value == 1)
        {
            text = "Nuevo PJ!";
        }
        else if (value == 2)
        {
            text = "Nuevo Maton!";
        }
        else
        {
            switch (upgradeNormales[index].tipoDeUpgrade)
            {
                case 0: // habs
                    switch (upgradeNormales[index].elementoAUpgradear)
                    {
                        case 0:
                            text = "+1 Ataque Básico";
                            break;
                        case 1:
                            text = "+1 Defensa Básica";
                            break;
                        default:
                            break;
                    }
                    break;
                case 1: //atr
                    switch (upgradeNormales[index].elementoAUpgradear)
                    {
                        case 0:
                            text = "+1 Maña";
                            break;
                        case 1:
                            text = "+1 Músculo";
                            break;
                        case 2:
                            text = "+1 Ingenio";
                            break;
                        case 3:
                            text = "+1 Brío";
                            break;
                        case 4:
                            text = "+1 Donaire";
                            break;
                        default:
                            break;
                    }
                    break;
                case 2://cant
                    text = "+1 Cantidad";
                    break;
                default:
                    break;
            }

            text += " y ";
            Debug.Log("uN " + upgradeNormales[index]);
            Debug.Log("Obj " + upgradeNormales[index].objetivoDeUpgrade);

            switch (upgradeNormalesAlternativa[index].tipoDeUpgrade)
            {

                case 0: // habs
                    switch (upgradeNormalesAlternativa[index].elementoAUpgradear)
                    {
                        case 0:
                            text += "+1 Ataque Básico para " + rM.party[upgradeNormales[index].objetivoDeUpgrade].nombre;
                            break;
                        case 1:
                            text += "+1 Defensa Básica para " + rM.party[upgradeNormales[index].objetivoDeUpgrade].nombre;
                            break;
                        default:
                            break;
                    }
                    break;
                case 1: //atr
                    switch (upgradeNormalesAlternativa[index].elementoAUpgradear)
                    {
                        case 0:
                            text += "+1 Maña para " + rM.party[upgradeNormales[index].objetivoDeUpgrade].nombre;
                            break;
                        case 1:
                            text += "+1 Músculo para " + rM.party[upgradeNormales[index].objetivoDeUpgrade].nombre;
                            break;
                        case 2:
                            text += "+1 Ingenio para " + rM.party[upgradeNormales[index].objetivoDeUpgrade].nombre;
                            break;
                        case 3:
                            text += "+1 Brío para " + rM.party[upgradeNormales[index].objetivoDeUpgrade].nombre;
                            break;
                        case 4:
                            text += "+1 Donaire para " + rM.party[upgradeNormales[index].objetivoDeUpgrade].nombre;
                            break;
                        default:
                            break;
                    }
                    break;
                case 2://cant
                    text += "+1 Cantidad para " + rM.party[upgradeNormales[index].objetivoDeUpgrade].nombre;
                    break;
                default:
                    break;
            }

        }
        //    int objetivoUpgrade = value / upgradesCombinacionesPosibles;

        //    switch (value)
        //    {
        //        case 0:
        //            text = "+1 Ataque Básico y +1 Defensa Básica para " + rM.party[objetivoUpgrade].nombre;
        //            break;
        //        case 1:
        //            text = "+1 Ataque Básico y +1 Músculo para " + rM.party[objetivoUpgrade].nombre;
        //            break;
        //        case 2:
        //            text = "+1 Ataque Básico y +1 Maña para " + rM.party[objetivoUpgrade].nombre;
        //            break;
        //        case 3:
        //            text = "+1 Ataque Básico y +1 Ingenio para " + rM.party[objetivoUpgrade].nombre;
        //            break;
        //        case 4:
        //            text = "+1 Ataque Básico y +1 Brío para " + rM.party[objetivoUpgrade].nombre;
        //            break;
        //        case 5:
        //            text = "+1 Ataque Básico y +1 Donaire para " + rM.party[objetivoUpgrade].nombre;
        //            break;
        //        case 6:
        //            text = "+1 Defensa Básica y +1 Músculo para " + rM.party[objetivoUpgrade].nombre;
        //            break;
        //        case 7:
        //            text = "+1 Defensa Básica y +1 Maña para " + rM.party[objetivoUpgrade].nombre;
        //            break;
        //        case 8:
        //            text = "+1 Defensa Básica y +1 Ingenio para " + rM.party[objetivoUpgrade].nombre;
        //            break;
        //        case 9:
        //            text = "+1 Defensa Básica y +1 Brío para " + rM.party[objetivoUpgrade].nombre;
        //            break;
        //        case 10:
        //            text = "+1 Defensa Básica y +1 Donaire para " + rM.party[objetivoUpgrade].nombre;
        //            break;
        //        case 11:
        //            text = "+1 Músculo y +1 Maña para " + rM.party[objetivoUpgrade].nombre;
        //            break;
        //        case 12:
        //            text = "+1 Músculo y +1 Ingenio para " + rM.party[objetivoUpgrade].nombre;
        //            break;
        //        case 13:
        //            text = "+1 Músculo y +1 Brío para " + rM.party[objetivoUpgrade].nombre;
        //            break;
        //        case 14:
        //            text = "+1 Músculo y +1 Donaire para " + rM.party[objetivoUpgrade].nombre;
        //            break;
        //        case 15:
        //            text = "+1 Maña y +1 Ingenio para " + rM.party[objetivoUpgrade].nombre;
        //            break;
        //        case 16:
        //            text = "+1 Maña y +1 Brío para " + rM.party[objetivoUpgrade].nombre;
        //            break;
        //        case 17:
        //            text = "+1 Maña y +1 Donaire para " + rM.party[objetivoUpgrade].nombre;
        //            break;
        //        case 18:
        //            text = "+1 Ingenio y +1 Brío para " + rM.party[objetivoUpgrade].nombre;
        //            break;
        //        case 19:
        //            text = "+1 Ingenio y +1 Donaire para " + rM.party[objetivoUpgrade].nombre;
        //            break;
        //        case 20:
        //            text = "+1 Brío y +1 Donaire para " + rM.party[objetivoUpgrade].nombre;
        //            break;
        //        case 21:
        //        case 22:
        //        case 23:
        //            text = "+1 Cantidad y +1 Ataque Básico para " + rM.party[objetivoUpgrade].nombre;
        //            break;
        //        case 24:
        //        case 25:
        //        case 26:
        //            text = "+1 Cantidad y +1 Defensa Básica para " + rM.party[objetivoUpgrade].nombre;
        //            break;
        //        case 27:
        //        case 28:
        //        case 29:
        //            text = "+1 Cantidad y +1 Músculo para " + rM.party[objetivoUpgrade].nombre;
        //            break;
        //        case 30:
        //        case 31:
        //        case 32:
        //            text = "+1 Cantidad y +1 Maña para " + rM.party[objetivoUpgrade].nombre;
        //            break;
        //        case 33:
        //        case 34:
        //        case 35:
        //            text = "+1 Cantidad y +1 Ingenio para " + rM.party[objetivoUpgrade].nombre;
        //            break;
        //        case 36:
        //        case 37:
        //        case 38:
        //            text = "+1 Cantidad y +1 Brío para " + rM.party[objetivoUpgrade].nombre;
        //            break;
        //        case 39:
        //        case 40:
        //        case 41:
        //            text = "+1 Cantidad y +1 Donaire para " + rM.party[objetivoUpgrade].nombre;
        //            break;
        //        case 42:
        //        case 43:
        //        case 44:
        //            text = "+2 Cantidad para " + rM.party[objetivoUpgrade].nombre;
        //            break;
        //        default:
        //            break;
        //    }
        //}
        uiRU.SetUpgradeText(index, text);
    }

    public void UpgradeElegido(cRoguelikeUpgradeData aUpgradear)
    {
        switch (aUpgradear.tipoDeUpgrade)
        {
            case 0: // habs
                switch (aUpgradear.elementoAUpgradear)
                {
                    case 0:
                        rM.party[aUpgradear.objetivoDeUpgrade].hab.ataqueBasico++;
                        break;
                    case 1:
                        rM.party[aUpgradear.objetivoDeUpgrade].hab.defensaBasica++;
                        break;
                    default:
                        break;
                }
                break;
            case 1: //atr
                switch (aUpgradear.elementoAUpgradear)
                {
                    case 0:
                        rM.party[aUpgradear.objetivoDeUpgrade].atr.maña++;
                        break;
                    case 1:
                        rM.party[aUpgradear.objetivoDeUpgrade].atr.musculo++;
                        break;
                    case 2:
                        rM.party[aUpgradear.objetivoDeUpgrade].atr.ingenio++;
                        break;
                    case 3:
                        rM.party[aUpgradear.objetivoDeUpgrade].atr.brio++;
                        break;
                    case 4:
                        rM.party[aUpgradear.objetivoDeUpgrade].atr.donaire++;
                        break;
                    default:
                        break;
                }
                break;
            case 2://cant
                AumentarCantidad();
                break;
            default:
                break;
        }
        Listorti();
    }

    public void GetEleccion(int eleccion)
    {
        if (esUpgradesuper[eleccion])
        {
            UpgradeElegidoSuper(upgrade[eleccion],eleccion);
        }
        else
        {
            UpgradeElegido(upgradeNormales[eleccion]);
        }
    }

    public void UpgradeElegidoSuper(int valorUpgrade, int index)
    {
        //if (rM.party.Count > 2) valorUpgrade = 0;
        //lo saco para claridad, en teoria eso nunca tendria que pasar, nos encargamos antes de que no sea una opcion
        Debug.Log("opcion elegida: " + valorUpgrade);
        if (valorUpgrade == 1)
        {
            NuevoPJ();
        }
        else if (valorUpgrade == 2)
        {
            NuevoMaton();
        }
        else
        {
            //ACA UPDATEAR
            int objetivoUpgrade = upgradeNormales[index].objetivoDeUpgrade;

            switch (upgradeNormales[index].tipoDeUpgrade)
            {
                case 0: // habs
                    switch (upgradeNormales[index].elementoAUpgradear)
                    {
                        case 0:
                            rM.party[objetivoUpgrade].hab.ataqueBasico++;
                            break;
                        case 1:
                            rM.party[objetivoUpgrade].hab.defensaBasica++;
                            break;
                        default:
                            break;
                    }
                    break;
                case 1: //atr
                    switch (upgradeNormales[index].elementoAUpgradear)
                    {
                        case 0:
                            rM.party[objetivoUpgrade].atr.maña++;
                            break;
                        case 1:
                            rM.party[objetivoUpgrade].atr.musculo++;
                            break;
                        case 2:
                            rM.party[objetivoUpgrade].atr.ingenio++;
                            break;
                        case 3:
                            rM.party[objetivoUpgrade].atr.brio++;
                            break;
                        case 4:
                            rM.party[objetivoUpgrade].atr.donaire++;
                            break;
                        default:
                            break;
                    }
                    break;
                case 2://cant
                    rM.party[objetivoUpgrade].cantidad++;
                    break;
                default:
                    break;
            }

            switch (upgradeNormalesAlternativa[index].tipoDeUpgrade)
            {
                case 0: // habs
                    switch (upgradeNormalesAlternativa[index].elementoAUpgradear)
                    {
                        case 0:
                            rM.party[objetivoUpgrade].hab.ataqueBasico++;
                            break;
                        case 1:
                            rM.party[objetivoUpgrade].hab.defensaBasica++;
                            break;
                        default:
                            break;
                    }
                    break;
                case 1: //atr
                    switch (upgradeNormalesAlternativa[index].elementoAUpgradear)
                    {
                        case 0:
                            rM.party[objetivoUpgrade].atr.maña++;
                            break;
                        case 1:
                            rM.party[objetivoUpgrade].atr.musculo++;
                            break;
                        case 2:
                            rM.party[objetivoUpgrade].atr.ingenio++;
                            break;
                        case 3:
                            rM.party[objetivoUpgrade].atr.brio++;
                            break;
                        case 4:
                            rM.party[objetivoUpgrade].atr.donaire++;
                            break;
                        default:
                            break;
                    }
                    break;
                case 2://cant
                    rM.party[objetivoUpgrade].cantidad++;
                    break;
                default:
                    break;
            }         
        }
        Listorti();
    }

    public void AumentarCantidad()
    {
        int indexMaton = Random.Range(0, cantidadDeMatonesEnParty);
        int matonesPrevios = 0;
        foreach (var item in rM.party)
        {
            if (item.esMaton)
            {
                if (matonesPrevios < indexMaton) matonesPrevios++;
                else
                {
                    item.cantidad++;
                    break;
                }
            }
        }
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
