using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cAccionAtaque : cAccionDeImpacto
{
    override protected void MensajeDeExito()
    {
        uiC.SetText("¡Nadie detuvo el ataque, da en blanco!");
    }
}
