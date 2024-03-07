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
    public const int RU_NO_UPGRADE = -1;
    public const int RU_SIMPLE = 0;
    public const int RU_DOBLE = 1;
    public const int RU_TRIPLE = 2;
    public const int RU_PJ = 3;
    public const int RU_MARCIAL = 4;
    public const int RU_ARCANA = 5;
    public const int RU_TALENTO = 6;
    public const int RU_DESCANSO_COMPLETO = 7;
    public const int RU_DESCANSO_PARCIAL_Y_SIMPLE = 8;
    public const int RU_DESCANSOS_Y_DOBLES = 9;

    //SUBTIPOS
    public const int RUST_HAB = 0;
    public const int RUST_ATR = 1;

    public int tipoDeUpgrade;
    public int objetivoDeUpgrade;
    public List<sSingleUpgrade> upgradesList;
}
