using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cAcciones : cAccionable
{
    public bool intentaronDetenerlo;

    public int categoria;
    public const int AC_CAT_MARCIAL = 0;
    public const int AC_CAT_ARCANA = 1;
    public const int AC_CAT_MOVIMIENTO = 2;
    public const int AC_CAT_GUARDAR = 3;

    virtual public void ResetState()
    {

    }

    virtual public void ResetMensaje()
    {

    }
}
