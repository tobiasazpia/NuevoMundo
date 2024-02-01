using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cAccionMovimiento : cAccionDeImpacto
{
    override protected void MensajeDeExito()
    {
        uiC.SetText("¡Nadie nos detuvo, nos movemos!");
    }
}
