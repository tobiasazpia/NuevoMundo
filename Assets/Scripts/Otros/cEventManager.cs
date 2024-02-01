using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class cEventManager : MonoBehaviour
{
    public static event Action<cPersonaje> PersonajeActuoEvent;
    public static event Action FinDeRondaEvent;

    public static void StartPersonajeActuoEvent(cPersonaje p)
    {
        PersonajeActuoEvent?.Invoke(p);
    }

    public static void StartFinDeRondaEvent()
    {
        FinDeRondaEvent?.Invoke();
    }
}