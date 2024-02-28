using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sSingleUpgrade
{
    public int subTipoDeUpgrade;
    public int elementoAUpgradear;
}

public class cRoguelikeUpgradeData
{
    //TIPOS
    public const int RU_SIMPLE = 0;
    public const int RU_DOBLE = 1;
    public const int RU_TRIPLE = 2;
    public const int RU_PJ = 3;
    public const int RU_MARCIAL = 4;
    public const int RU_ARCANA = 5;
    public const int RU_TALENTO = 6;
    public const int RU_DESCANSO = 7;

    //SUBTIPOS
    public const int RUST_HAB = 0;
    public const int RUST_ATR = 1;

    public int tipoDeUpgrade;
    public int objetivoDeUpgrade;
    public List<sSingleUpgrade> upgradesList;
}
