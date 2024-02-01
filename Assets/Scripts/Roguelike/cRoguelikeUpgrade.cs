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
    cRoguelikeUpgradeData[] upgr = new cRoguelikeUpgradeData[3];
    bool[] esUpgradesuper = new bool[3];
    bool waitingForOk = false;
    bool anticipando = false;
    int upgradesNormalesPosibles;
    int upgradesCombinacionesPosibles;
    int cantidadDeMatonesEnParty;
    int upgradesNormalesHechas = 0;
    int upgradesCombinacionesHechas = 0;
    int upgradesMatonHechas = 0;
    int upgradesPJHechas = 0; // permanente

    private void OnEnable()
    {
        upgradesNormalesHechas = 0;
        upgradesCombinacionesHechas = 0;
        upgradesMatonHechas = 0;

        for (int i = 0; i < upgr.Length; i++)
        {
            upgr[i] = new cRoguelikeUpgradeData();
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
        AveriguarMatonesEnParty();
        AveriguarUpgradesNormalesPosibles();

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
            noRepitamos();
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
            noRepitamos();
            esUpgradesuper[1] = true;
            DefinirSuperUpgrade(1);
            CambiarTextoNormal(0, upgr[0]);
            CambiarTextoSuper(1, upgrade[1]);
            CambiarTextoNormal(2, upgr[2]);
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
            noRepitamos();
            for (int i = 0; i < 3; i++)
            {
                CambiarTextoNormal(i, upgr[i]);
            }
        }
    }


    public void DefinirSuperUpgrade(int upgradeSlot)
    {
        Debug.Log("Definiendo super upgrade");
        int opciones = 3 - upgradesPJHechas - upgradesMatonHechas;
        Debug.Log("Opciones: " + opciones);
        int tipo = Random.Range(0, opciones);
        Debug.Log("Tipo: " + tipo);
        if (tipo == 1 && upgradesPJHechas > 0) tipo++;
        Debug.Log("Tipo update: " + tipo);
        switch (tipo)
        {
            case 0:
                //combinacion normales
                Debug.Log("case 0, combinaciones");
                upgrade[upgradeSlot] = Random.Range(0, upgradesCombinacionesPosibles - upgradesCombinacionesHechas++);
                break;
            case 1:
                // pj
                Debug.Log("case 1, pj");
                upgradesPJHechas++;
                upgrade[upgradeSlot] = 1000;
                break;
            case 2:
                // maton
                Debug.Log("case 2, metones");
                upgradesMatonHechas++;
                upgrade[upgradeSlot] = 1001;
                break;
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

    public void AveriguarUpgradesNormalesPosibles()
    {
        if (cantidadDeMatonesEnParty > 0)
            upgradesNormalesPosibles = 10;
        else upgradesNormalesPosibles = 7;
        upgradesNormalesPosibles *= rM.party.Count;
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
        //Hay 2 grandes tipos de upgrades, las que mejoran un miembro de la party y las que agregan un miembro a la party
        //las normales son todas del primer tipo, asi que siempre tienen un objetivo
        //lo primero es decidir cual es
        Debug.Log("upgr length: " + upgr.Length);
        Debug.Log("upgr obj " + upgr[upgradeSlot]);
        upgr[upgradeSlot].objetivoDeUpgrade = Random.Range(0, rM.party.Count);
        //ahora que ta tenemos ojetivo, hay que decidir si aumentamos habs, atrs o cant, si es maton
        bool esMaton = rM.party[upgr[upgradeSlot].objetivoDeUpgrade].esMaton;
        //primero vamos a seleccionar sin wight, pero quiero hacerlo con

        //Problema: si las otras 2 fueron para el mismo obejtivo y ya fueron Habs, no tendria que ser hab
        // y si una ya fue cant, el mismo objetivo no puede volver a tener cant.
        // y se podria extender a: si el mismo objetivo ya tuvo tantas veces esta upgrade como upgrades de este tipo podria etner, no se puede agregar una. cantidad es 1 en general, pero puede ser 0, si el objetivo tiene cantidad 10. hab es 2 en general, pero puede ser 0 o 1 si ya lelgaron a 5. y atr es 5, pero puede bajar si aluno ya llego a 5.
        // onda, podriaas llegar a un punto donde un objetivo ya no es upgradeable, y hasta podria ser que toda tu party no sea upgradeable
        //hay que tener presentes esas posibilidades
        if (esMaton)
        {
            upgr[upgradeSlot].tipoDeUpgrade = Random.Range(0, 3);
        }
        else
        {
            upgr[upgradeSlot].tipoDeUpgrade = Random.Range(0, 2);
        }

        List<int> ilegales = new List<int>();
        switch (upgr[upgradeSlot].tipoDeUpgrade)
        {
            case 0: // habilidaes
                LlenarUpgradesIlegales(ilegales, upgradeSlot);
                upgr[upgradeSlot].elementoAUpgradear = GetRandomHab(ilegales);
                break;
            case 1: // atributos
                LlenarUpgradesIlegales(ilegales, upgradeSlot);
                upgr[upgradeSlot].elementoAUpgradear = GetRandomAtr(ilegales);
                break;
            case 2: // cantidad
                break;
            default:
                break;
        }



        //upgrade[upgradeSlot] = Random.Range(0, upgradesNormalesPosibles - upgradesNormalesHechas++);
        /*
 Upgrades Normales
+1 en una Hab
+1 en un Atr
+1 en cantidad para un maton
 */
    }

    void LlenarUpgradesIlegales(List<int> ilegales, int upgradeSlot)
    {
        for (int i = upgradeSlot-1; i >= 0; i--)
        {
            if (upgr[i].objetivoDeUpgrade != upgr[upgradeSlot].objetivoDeUpgrade) return;
            else if (upgr[i].tipoDeUpgrade != upgr[upgradeSlot].tipoDeUpgrade) return;
            else ilegales.Add(upgr[i].elementoAUpgradear);
        }
    }

    int GetRandomHab(List<int> valoresIlegales)
    {
        int ret = Random.Range(0, 2-valoresIlegales.Count); //hardcodenado 2, en realidad estaria buno ver cuantas habs tienen
        return chequearRepiticion(valoresIlegales, ret);
    }

    int GetRandomAtr(List<int> valoresIlegales)
    {
        int ret = Random.Range(0, 5 - valoresIlegales.Count);
        return chequearRepiticion(valoresIlegales,ret);
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
        rM.party.Add(gameObject.AddComponent(typeof(cPersonajeFlyweight)) as cPersonajeFlyweight);
        rM.party[rM.party.Count - 1].Copiar(rM.rC.templatesPersonajes[GetRandomPersonaje(ilegales)]);
        rM.party[rM.party.Count - 1].equipo = 1;
        rM.party[rM.party.Count - 1].esMaton = false;
        rM.party[rM.party.Count - 1].iA = cAI.PLAYER_CONTROLLED;
    }

    public void CambiarTextoNormal(int index, cRoguelikeUpgradeData aUpgradear)
    {
        Debug.Log("onjetivo upgrade: " + aUpgradear.objetivoDeUpgrade + ", value" + aUpgradear.elementoAUpgradear);
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
                        text = "+1 Músculo para " + rM.party[aUpgradear.objetivoDeUpgrade].nombre;
                        break;
                    case 1:
                        text = "+1 Maña para " + rM.party[aUpgradear.objetivoDeUpgrade].nombre;
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
        if (value == 1000)
        {
            Debug.Log("texto nuevo pj");
            text = "Nuevo PJ!";
        }
        else if (value == 1001)
        {
            Debug.Log("texto nuevo maton");
            text = "Nuevo Maton!";
        }
        else
        {
            int objetivoUpgrade = value / upgradesCombinacionesPosibles;

            switch (value)
            {
                case 0:
                    text = "+1 Ataque Básico y +1 Defensa Básica para " + rM.party[objetivoUpgrade].nombre;
                    break;
                case 1:
                    text = "+1 Ataque Básico y +1 Músculo para " + rM.party[objetivoUpgrade].nombre;
                    break;
                case 2:
                    text = "+1 Ataque Básico y +1 Maña para " + rM.party[objetivoUpgrade].nombre;
                    break;
                case 3:
                    text = "+1 Ataque Básico y +1 Ingenio para " + rM.party[objetivoUpgrade].nombre;
                    break;
                case 4:
                    text = "+1 Ataque Básico y +1 Brío para " + rM.party[objetivoUpgrade].nombre;
                    break;
                case 5:
                    text = "+1 Ataque Básico y +1 Donaire para " + rM.party[objetivoUpgrade].nombre;
                    break;
                case 6:
                    text = "+1 Defensa Básica y +1 Músculo para " + rM.party[objetivoUpgrade].nombre;
                    break;
                case 7:
                    text = "+1 Defensa Básica y +1 Maña para " + rM.party[objetivoUpgrade].nombre;
                    break;
                case 8:
                    text = "+1 Defensa Básica y +1 Ingenio para " + rM.party[objetivoUpgrade].nombre;
                    break;
                case 9:
                    text = "+1 Defensa Básica y +1 Brío para " + rM.party[objetivoUpgrade].nombre;
                    break;
                case 10:
                    text = "+1 Defensa Básica y +1 Donaire para " + rM.party[objetivoUpgrade].nombre;
                    break;
                case 11:
                    text = "+1 Músculo y +1 Maña para " + rM.party[objetivoUpgrade].nombre;
                    break;
                case 12:
                    text = "+1 Músculo y +1 Ingenio para " + rM.party[objetivoUpgrade].nombre;
                    break;
                case 13:
                    text = "+1 Músculo y +1 Brío para " + rM.party[objetivoUpgrade].nombre;
                    break;
                case 14:
                    text = "+1 Músculo y +1 Donaire para " + rM.party[objetivoUpgrade].nombre;
                    break;
                case 15:
                    text = "+1 Maña y +1 Ingenio para " + rM.party[objetivoUpgrade].nombre;
                    break;
                case 16:
                    text = "+1 Maña y +1 Brío para " + rM.party[objetivoUpgrade].nombre;
                    break;
                case 17:
                    text = "+1 Maña y +1 Donaire para " + rM.party[objetivoUpgrade].nombre;
                    break;
                case 18:
                    text = "+1 Ingenio y +1 Brío para " + rM.party[objetivoUpgrade].nombre;
                    break;
                case 19:
                    text = "+1 Ingenio y +1 Donaire para " + rM.party[objetivoUpgrade].nombre;
                    break;
                case 20:
                    text = "+1 Brío y +1 Donaire para " + rM.party[objetivoUpgrade].nombre;
                    break;
                case 21:
                case 22:
                case 23:
                    text = "+1 Cantidad y +1 Ataque Básico para " + rM.party[objetivoUpgrade].nombre;
                    break;
                case 24:
                case 25:
                case 26:
                    text = "+1 Cantidad y +1 Defensa Básica para " + rM.party[objetivoUpgrade].nombre;
                    break;
                case 27:
                case 28:
                case 29:
                    text = "+1 Cantidad y +1 Músculo para " + rM.party[objetivoUpgrade].nombre;
                    break;
                case 30:
                case 31:
                case 32:
                    text = "+1 Cantidad y +1 Maña para " + rM.party[objetivoUpgrade].nombre;
                    break;
                case 33:
                case 34:
                case 35:
                    text = "+1 Cantidad y +1 Ingenio para " + rM.party[objetivoUpgrade].nombre;
                    break;
                case 36:
                case 37:
                case 38:
                    text = "+1 Cantidad y +1 Brío para " + rM.party[objetivoUpgrade].nombre;
                    break;
                case 39:
                case 40:
                case 41:
                    text = "+1 Cantidad y +1 Donaire para " + rM.party[objetivoUpgrade].nombre;
                    break;
                case 42:
                case 43:
                case 44:
                    text = "+2 Cantidad para " + rM.party[objetivoUpgrade].nombre;
                    break;
                default:
                    break;
            }
        }
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
                        rM.party[aUpgradear.objetivoDeUpgrade].atr.musculo++;
                        break;
                    case 1:
                        rM.party[aUpgradear.objetivoDeUpgrade].atr.maña++;
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
        if (esUpgradesuper[eleccion]) {
            UpgradeElegidoSuper(upgrade[eleccion]);
        }
        else
        {
            UpgradeElegido(upgr[eleccion]);
        }
    }

    public void UpgradeElegidoSuper(int valorUpgrade)
    {
        if (valorUpgrade == 1000)
        {
            NuevoPJ();
        }
        else if (valorUpgrade == 1001)
        {
            NuevoMaton();
        }
        else
        {
            int tipoUpgrade = valorUpgrade % upgradesCombinacionesPosibles;
            int objetivoUpgrade = valorUpgrade / upgradesCombinacionesPosibles;
            Debug.Log("Tipo Upgrade: " + tipoUpgrade);
            Debug.Log("Objetivo Upgrade: " + objetivoUpgrade);
            switch (tipoUpgrade)
            {
                case 0:
                    rM.party[objetivoUpgrade].hab.ataqueBasico++;
                    rM.party[objetivoUpgrade].hab.defensaBasica++;
                    break;
                case 1:
                    rM.party[objetivoUpgrade].hab.ataqueBasico++;
                    rM.party[objetivoUpgrade].atr.musculo++;
                    break;
                case 2:
                    rM.party[objetivoUpgrade].hab.ataqueBasico++;
                    rM.party[objetivoUpgrade].atr.maña++;
                    break;
                case 3:
                    rM.party[objetivoUpgrade].hab.ataqueBasico++;
                    rM.party[objetivoUpgrade].atr.ingenio++;
                    break;
                case 4:
                    rM.party[objetivoUpgrade].hab.ataqueBasico++;
                    rM.party[objetivoUpgrade].atr.brio++;
                    break;
                case 5:
                    rM.party[objetivoUpgrade].hab.ataqueBasico++;
                    rM.party[objetivoUpgrade].atr.donaire++;
                    break;
                case 6:
                    rM.party[objetivoUpgrade].hab.defensaBasica++;
                    rM.party[objetivoUpgrade].atr.musculo++;
                    break;
                case 7:
                    rM.party[objetivoUpgrade].hab.defensaBasica++;
                    rM.party[objetivoUpgrade].atr.maña++;
                    break;
                case 8:
                    rM.party[objetivoUpgrade].hab.defensaBasica++;
                    rM.party[objetivoUpgrade].atr.ingenio++;
                    break;
                case 9:
                    rM.party[objetivoUpgrade].hab.defensaBasica++;
                    rM.party[objetivoUpgrade].atr.brio++;
                    break;
                case 10:
                    rM.party[objetivoUpgrade].hab.defensaBasica++;
                    rM.party[objetivoUpgrade].atr.donaire++;
                    break;
                case 11:
                    rM.party[objetivoUpgrade].atr.musculo++;
                    rM.party[objetivoUpgrade].atr.maña++;
                    break;
                case 12:
                    rM.party[objetivoUpgrade].atr.musculo++;
                    rM.party[objetivoUpgrade].atr.ingenio++;
                    break;
                case 13:
                    rM.party[objetivoUpgrade].atr.musculo++;
                    rM.party[objetivoUpgrade].atr.brio++;
                    break;
                case 14:
                    rM.party[objetivoUpgrade].atr.musculo++;
                    rM.party[objetivoUpgrade].atr.donaire++;
                    break;
                case 15:
                    rM.party[objetivoUpgrade].atr.maña++;
                    rM.party[objetivoUpgrade].atr.ingenio++;
                    break;
                case 16:
                    rM.party[objetivoUpgrade].atr.maña++;
                    rM.party[objetivoUpgrade].atr.brio++;
                    break;
                case 17:
                    rM.party[objetivoUpgrade].atr.maña++;
                    rM.party[objetivoUpgrade].atr.donaire++;
                    break;
                case 18:
                    rM.party[objetivoUpgrade].atr.ingenio++;
                    rM.party[objetivoUpgrade].atr.brio++;
                    break;
                case 19:
                    rM.party[objetivoUpgrade].atr.ingenio++;
                    rM.party[objetivoUpgrade].atr.donaire++;
                    break;
                case 20:
                    rM.party[objetivoUpgrade].atr.ingenio++;
                    rM.party[objetivoUpgrade].atr.brio++;
                    break;
                case 21:
                case 22:
                case 23:
                    rM.party[objetivoUpgrade].hab.ataqueBasico++;
                    rM.party[objetivoUpgrade].cantidad++;
                    break;
                case 24:
                case 25:
                case 26:
                    rM.party[objetivoUpgrade].hab.defensaBasica++;
                    rM.party[objetivoUpgrade].cantidad++;
                    break;
                case 27:
                case 28:
                case 29:
                    rM.party[objetivoUpgrade].atr.musculo++;
                    rM.party[objetivoUpgrade].cantidad++;
                    break;
                case 30:
                case 31:
                case 32:
                    rM.party[objetivoUpgrade].atr.maña++;
                    rM.party[objetivoUpgrade].cantidad++;
                    break;
                case 33:
                case 34:
                case 35:
                    rM.party[objetivoUpgrade].atr.ingenio++;
                    rM.party[objetivoUpgrade].cantidad++;
                    break;
                case 36:
                case 37:
                case 38:
                    rM.party[objetivoUpgrade].atr.brio++;
                    rM.party[objetivoUpgrade].cantidad++;
                    break;
                case 39:
                case 40:
                case 41:
                    rM.party[objetivoUpgrade].atr.donaire++;
                    rM.party[objetivoUpgrade].cantidad++;
                    break;
                case 42:
                case 43:
                case 44:
                    rM.party[objetivoUpgrade].cantidad++;
                    rM.party[objetivoUpgrade].cantidad++;
                    break;
                default:
                    break;
            }
        }

        //rogueMenuElegir.SetActive(false);
        //rogueMenuRevisar.SetActive(true);
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
        Debug.Log("Por mander rev");
        uiRU.RevisarParty();
        waitingForOk = true;
    }

    void Anticipar()
    {
        TMP_Text text = anticipacion.GetComponent<TMP_Text>();
        //text = "Toma un respiro y preparate, empieza tu pelea numero " + ++rM.nivel;
        anticipando = true;
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
