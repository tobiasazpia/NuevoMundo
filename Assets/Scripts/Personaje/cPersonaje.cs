using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using System;


public struct sAtributos
{
    public int maña;
    public int musculo;
    public int ingenio;
    public int brio;
    public int donaire;
}

public struct sHabilidades
{
    public int ataqueBasico;
    public int defensaBasica;
}

public class cPersonaje : MonoBehaviour
{
    public bool mostrandoTactica;
    public bool hovered;
    public bool quiereActuar;
    public List<cAcciones> acciones = new List<cAcciones>();
    public List<cReacciones> reacciones = new List<cReacciones>();
    //Acciones
    public const int AC_ATACAR = 0;
    public const int AC_MOVER = 1;
    public const int AC_GUARDAR = 2;
    public const int AC_MOVAGRE = 3;
    public const int AC_MOVPREC = 4;
    public const int AC_MARCIAL = 5;
    public const int AC_ARCANA = 6;
    public const int AC_RECARGAR = 7;
    public const int AC_ENCONTRAR = 8;
    public const int AC_ATACARIMPRO = 9;
    public const int AC_MOVIMPRO = 10;
    public const int AC_DEFIMPRO = 11;
    public const int AC_ATRAS = 98;
    public const int AC_SINASIGNAR = 99;
    //Reacciones
    public const int DB_DefensaBasica= 0;
    public const int DB_DefensaBasicaImpro = 1;
    public const int DB_ATRAS = 98;
    public const int DB_SINASIGNAR = 99;

    public UICombate uiC;
    public PlayerInput py;
    public bool selected = false;

    public cCombate c;
    public GUIComponent gC;

    public cAI ai;
    public int aiCode;
    public cArma arma;
    public int armaCode;

    //Data de Personaje
    public string nombre; //Se usa como ID, no permitir que 2 personajes tengan el mismo
    public sAtributos atr;
    public sHabilidades hab;
    public int ataqueBasicoDadosExtra;
    public int defensaBasicaDadosExtra;
    public int bonusPAtqBporDefB;
    public int fallamosDefPor;
    public int hSupe;
    public int hDram;
    public int zonaActual;
    public int zonaInicial;

    public bool vivo = true;
    public bool guardando;
    public int equipo;

    //Data de Iniciativa
    public int[] dadosDeAccion;
    public int valorDeIniciativa;
    public bool reaccion1Disponible;
    public bool reaccion2Disponible;

    public int dadosDelAtacantePorPrecavido;
    public int totalDadosDelAtacante;

    //Data Calculable
    protected int guardia;
    private int extraNecesarioPara2doMaton;

    // Start is called before the first frame update
    void Start()
    {
        c = GetComponentInParent<cCombate>();
        gC = GetComponentInParent<GUIComponent>();
        py = GameObject.Find("Sesion").GetComponent<PlayerInput>();
        uiC = GameObject.Find("UI").GetComponent<UICombate>();

        acciones.Add(gameObject.AddComponent<cAccionGuardar>());
        acciones.Add(gameObject.AddComponent<cAccionMovimientoPrecavido>());
        foreach (var item in acciones)
        {
            item.personaje = this;
        }
        foreach (var item in reacciones)
        {
            item.personaje = this;
        }
    }

    public cPersonaje NuevoPersonaje(cPersonajeFlyweight flyweight)
    {
        //imagino que habra que sobrecargar la funcion desde matones para que funque
        nombre = flyweight.nombre;
        aiCode = flyweight.iA;
        armaCode = flyweight.arma;
        equipo = flyweight.equipo;
        atr = flyweight.atr;
        hab = flyweight.hab;
        return this;
    }

    public void SetAI(int aiCode)
    {
        switch (aiCode)
        {
            case cAI.PLAYER_CONTROLLED:
                ai = null;
                break;
            case cAI.FULL_AGGRO:
                ai = gameObject.AddComponent(typeof(cAIFullAggro)) as cAIFullAggro;
                break;
            case cAI.FULL_DEFENSIVO:
                ai = gameObject.AddComponent(typeof(cAIFullDef)) as cAIFullDef;
                break;
            case cAI.SMART_DEFENSIVO:
                ai = gameObject.AddComponent(typeof(cAISmartDef)) as cAISmartDef;
                break;
            case cAI.ATACANTE_PRECAVIDO:
                ai = gameObject.AddComponent(typeof(cAIAtacantePrecavido)) as cAIAtacantePrecavido;
                break;
            default:
                break;
        }
        if (ai != null)
        {
            ai.p = this;
        }
    }

    static public string GetAtritbutoString(int code)
    {
        switch (code)
        {
            case 0:
                return "Maña";
            case 1:
                return "Músculo";
            case 2:
                return "Ingenio";
            case 3:
                return "Brío";
            case 4:
                return "Donaire";
            default:
                return "Error Return String";
        }
    }

    public void SetArma(int armaCode)
    {
        switch (armaCode)
        {
            case cArma.LIGERAS:
                arma = gameObject.AddComponent<cArmasLigeras>();
                break;
            case cArma.MEDIAS:
                arma = gameObject.AddComponent<cArmasMedias>();
                break;
            case cArma.PESADAS:
                arma = gameObject.AddComponent<cArmasPesadas>();
                break;
            case cArma.ARCO:
                arma = gameObject.AddComponent<cArmasArco>();
                break;
            case cArma.FUEGO:
                arma = gameObject.AddComponent<cArmasFuego>();
                break;
            case cArma.PELEA:
                arma = gameObject.AddComponent<cArmasPelea>();
                break;
            default:
                break;
        }
        if (arma != null)
        {
            totalDadosDelAtacante = arma.GetDadosDelAtacanteMod();
            arma.p = this;
        }
    }

    //Set DM
    virtual public void CalcularGuardia()
    {
        guardia = 15 + hab.ataqueBasico + hab.defensaBasica + arma.GetGuardiaMod();
        if (guardia > 30)
        {
            guardia = 30;
        }
    }

    //Get DM
    public int GetGuardia()
    {
        return guardia;
    }

    public void CalcularExtraParaMatones()
    {
        extraNecesarioPara2doMaton = arma.GetBaseParaMatonExtra() - arma.GetMusMult() * atr.musculo;
        if (extraNecesarioPara2doMaton < 1)
        {
            extraNecesarioPara2doMaton = 1;
        }
    }

    public int GetExtraParaMatones()
    {
        return extraNecesarioPara2doMaton;
    }

    //Funcion llamada por combate cuando un Personaje ya decidio que va a actuar y tiene un objetivo decidido
    public void Actuar(cPersonaje defensor, int faseActual, List<sAccion> acciones, List<sAccion> accionesActivas, List<sAccion> accionesReactivas, bool movAgro)
    {
        //Aca seria el punto de conecion para el refactoring
        //onda, lo de arma de fuego estaria genial qeu se encargue el arma misma.
        Debug.Log("Actuamos");
        switch (c.accionActiva)
        {
            case AC_ATACAR:
                Debug.Log("usando esDDASDAto");
                c.ui.m = nombre + " quiere atacar!";
                foreach (var p in c.personajes)
                {
                    Debug.Log("guard false 1");
                    p.guardando = false;
                }
                if (arma is cArmasFuego)
                {
                    if (!(arma as cArmasFuego).cargada)
                    {
                        c.ui.m += "\nPero primero tiene que cargar su arma";
                        (arma as cArmasFuego).cargada = true;
                        Debug.Log("cargada: " + (arma as cArmasFuego).cargada);
                        Debug.Log("DEPRECADO3");
                        //AccionOReaccionGastarDado(faseActual, acciones, accionesActivas, accionesReactivas);
                        c.ContinuarFase();
                        break;
                    }
                }

                dadosDelAtacantePorPrecavido = 0;
                totalDadosDelAtacante = arma.GetDadosDelAtacanteMod() + dadosDelAtacantePorPrecavido;


                //Esta "Logica Ataque" es basicamente Atauqe Basico
                if (LogicaAtaque(defensor, faseActual, acciones, accionesActivas, accionesReactivas, movAgro))
                {
                    c.ui.m += "\nAlguien defendera?";
                    c.stateID = cCombate.CHEQUEANDO_REACCION;
                }
                else
                {
                    c.movAgro = false;
                    c.ContinuarFase();
                }
                break;
            case AC_MOVAGRE:
                foreach (var p in c.personajes)
                {
                    Debug.Log("guard false 2");
                    p.guardando = false;
                }
                c.accionActiva = AC_ATACAR;
                c.movAgro = true;
                c.movPrec = false;
                dadosDelAtacantePorPrecavido = 0;
                totalDadosDelAtacante = arma.GetDadosDelAtacanteMod() + dadosDelAtacantePorPrecavido;
                LogicaMovimiento(faseActual, acciones, accionesActivas, accionesReactivas);
                break;
            case AC_MOVPREC:
                foreach (var p in c.personajes)
                {
                    Debug.Log("guard false 3");
                    p.guardando = false;
                }
                c.movAgro = false;
                c.movPrec = true;
                dadosDelAtacantePorPrecavido = 0;
                totalDadosDelAtacante = arma.GetDadosDelAtacanteMod() + dadosDelAtacantePorPrecavido;
                LogicaMovimiento(faseActual, acciones, accionesActivas, accionesReactivas);
                break;
            case AC_GUARDAR:
                transform.position = new Vector3(transform.position.x, 0, transform.position.z);
                GuardarAccion(accionesActivas);
                guardando = true;
                Debug.Log("guard true 1");
                break;
            default:
                break;
        }
    }

    public void LogicaMovimiento(int faseActual, List<sAccion> acciones, List<sAccion> accionesActivas, List<sAccion> accionesReactivas)
    {
        c.atacando = false;
        //aca se gasta super temprano el dado
        Debug.Log("DEPRECADO2");
        //AccionOReaccionGastarDado(faseActual, acciones, accionesActivas, accionesReactivas);
        c.jugadorAtq = GetGuardia();
        c.ui.m = nombre + " trata moverse de " + c.zonas[zonaActual].nombre + " a " + c.zonas[c.zonaObjetiva].nombre + " con su Guardia de " + GetGuardia();

        c.stateID = cCombate.CHEQUEANDO_REACCION;
        c.enemigosEnRango.Clear();
        List<cPersonaje> temp = new List<cPersonaje>();
        foreach (var p in c.personajes)
        {
            if (p.vivo && p.equipo != equipo)
            {
                if (p.zonaActual == zonaActual) // Cualquiera puede interrumpir
                {
                    temp.Add(p);
                }
                else if (p.arma.GetDeRango()) // solo los de rango pueden defender
                {
                    foreach (var z in c.zonas[p.zonaActual].zonasEnRango)
                    {
                        if (z == zonaActual)
                        {
                            temp.Add(p);
                            break;
                        }
                    }
                }
            }
        }
        temp.Sort((x, y) => y.valorDeIniciativa.CompareTo(x.valorDeIniciativa));
        foreach (var item in temp)
        {
            c.enemigosEnRango.Add(item);
        }
        temp.Clear();
    }

    //Devuelve si el ataque emboco o no
    public bool LogicaAtaque(cPersonaje defensor, int faseActual, List<sAccion> acciones, List<sAccion> accionesActivas, List<sAccion> accionesReactivas, bool movAgro)
    {
        Debug.Log("CREO QUE ESTO YA NO USAMOS");
        c.atacando = true;
        //AccionOReaccionGastarDado(faseActual, acciones, accionesActivas, accionesReactivas);
        c.jugadorAtq = Atacar(defensor);
        int numeroDeDados = 3 + atr.maña + hab.ataqueBasico + arma.GetBonusAtaque() + bonusPAtqBporDefB + defensor.totalDadosDelAtacante;

        //Capaz hay qeu sumarle 1
        //if (arma is cArmasLigeras)
        //{
        //    if (!(arma as cArmasLigeras).yaActuo[defensor.nombre])
        //    {
        //        Debug.Log(nombre + " gana bonus de yaActuo conta " + defensor.nombre);
        //        numeroDeDados++;
        //    }
        //}

        if (arma.GetDeRango())
        {
            if (c.enemigosEnMelee.Count > 0)
            {
                numeroDeDados -= 1;
                if (zonaActual == defensor.zonaActual) numeroDeDados -= 1;
            }
        }
        if (movAgro) numeroDeDados -= 3;
        if (c.jugadorAtq >= defensor.GetGuardia())
        {
            //c.stateID = cCombate.CHEQUEANDO_REACCION;
            c.enemigosEnRango.Clear();
            c.enemigosEnRango.Add(defensor);
            List<cPersonaje> temp = new List<cPersonaje>();
            foreach (var p in c.personajes)
            {
                if (p.vivo && p.equipo == defensor.equipo && p.nombre != defensor.nombre)
                {
                    if (p.zonaActual == defensor.zonaActual || p.zonaActual == zonaActual) // Cualquiera puede defender
                    {
                        temp.Add(p);
                    }
                    else if (p.arma.GetDeRango()) // solo los de rango pueden defender
                    {
                        foreach (var z in c.zonas[p.zonaActual].zonasEnRango)
                        {
                            if (z == zonaActual)
                            {
                                temp.Add(p);
                                break;
                            }
                        }
                    }
                }
            }
            temp.Sort((x, y) => y.valorDeIniciativa.CompareTo(x.valorDeIniciativa));
            foreach (var item in temp)
            {
                c.enemigosEnRango.Add(item);
            }
            temp.Clear();
            return true;
        }
        c.ui.m += "\nAtaca con " + numeroDeDados + " dados contra la Guardia en " + defensor.GetGuardia() + " de " + defensor.nombre + " y saca " + c.jugadorAtq + ", fallando el ataque.";
        c.Continuemos();
        if (arma is cArmasFuego)
        {
            (arma as cArmasFuego).cargada = false;
            Debug.Log("cargada: " + (arma as cArmasFuego).cargada);
            c.ui.m += "\nSu arma de fuego queda descargada.";
        }
        return false;
    }

    int Atacar(cPersonaje defensor)
    {
        int numeroDeDados = 3 + atr.maña + hab.ataqueBasico + arma.GetBonusAtaque() + bonusPAtqBporDefB + defensor.totalDadosDelAtacante;
        //Capaz hay qeu sumarle 1
        //if (arma is cArmasLigeras)
        //{
        //    if (!(arma as cArmasLigeras).yaActuo[defensor.nombre])
        //    {
        //        Debug.Log(nombre + " gana bonus de yaActuo conta " + defensor.nombre);
        //        numeroDeDados++;
        //    }
        //}

        if (c.movAgro) numeroDeDados -= 3;
        if (arma.GetDeRango())
        {
            if (c.enemigosEnMelee.Count > 0)
            {
                numeroDeDados -= 1;
                if (zonaActual == defensor.zonaActual) numeroDeDados -= 1;
            }
        }
        tirada tr = cDieMath.TirarDados(numeroDeDados);
        return cDieMath.sumaDe3Mayores(tr);
    }

    public bool VaAReaccionar()
    {
        if (reaccion1Disponible)
        {
            if (ai is null) // Si es un jugador que decida
            {
                // Capaz estaria cool chequear la "acicon activa" para dar un mensaje mas concreto de si te defendes,
                // defendes a un aliado, o interrumpis movimiento
                c.ui.m = nombre;
                if (c.accionActiva == AC_ATACAR)
                {
                    if (c.personajeObjetivo.nombre == nombre) c.ui.m += " tratas defenderte?\n\nZ - Si\nX - No";
                    else c.ui.m += " tratas defender a " + c.personajeObjetivo.nombre + "?\n\nZ - Si\nX - No";
                }
                else
                {
                    c.ui.m += " tratas detener movimiento?\n\nZ - Si\nX - No";
                }
                c.personajeInterversor = this;
                c.jugadorPuedeReaccionar = true;
                c.esperandoReaccion = true;
                c.auto = false;
                return true;
            }
            else // Si no que decida a AI
            {
                bool exit = false;
                if (arma is cArmasFuego && (!c.atacando || nombre != c.personajeObjetivo.nombre))
                {
                    if (!(arma as cArmasFuego).cargada)
                    {
                        c.ui.m = nombre + " no tiene el arma carga y no puede defender a otros ni detener movimiento";
                        exit = true;
                    }
                }
                if (!exit)
                {
                    if (ai.Reaccion(c.jugadorAtq))
                    {
                        c.ui.m = nombre + ": va a defender!";
                        c.stateID = cCombate.A_DEFENDER;
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public void EmbocarAtaque(cPersonaje defensor, int atq, int def)
    {
        defensor.RecibirGolpe(this, atq, def);
        bonusPAtqBporDefB = 0;
    }

    public virtual void RecibirGolpe(cPersonaje atacante, int atq, int def)
    {
        if (atacante.arma is cArmasFuego)
        {
            HeridoPorArmaDeFuego();
        }
        else Heridas(c.daño);
    }

    public void GuardarAccion(List<sAccion> accionesActivas)
    {
        GuardarGastarDado(accionesActivas);
        c.ContinuarFase();
    }

    public bool Reaccion(int atq, int faseActual, List<sAccion> acciones, List<sAccion> accionesActivas, List<sAccion> accionesReactivas)
    {
        Debug.Log("DEPRECADO4");

        bool def = Defensa(atq);
        if (def)
        {
            //AccionOReaccionGastarDado(faseActual, acciones, accionesActivas, accionesReactivas);
        }
        else
        {
            if (arma is cArmasFuego)
            {
                (arma as cArmasFuego).cargada = false;
                Debug.Log("cargada: " + (arma as cArmasFuego).cargada);
                c.ui.m += "\nSu arma quedo descargada";
            }
            //RetrocederDadoDeAccion(faseActual, acciones, accionesActivas, accionesReactivas);
        }
        return def;
    }

    bool Defensa(int atq)
    {
        if (!c.atacando)
        {
            defensaBasicaDadosExtra = arma.GetBonusDetenerMovimiento();
        }
        else if (nombre == c.personajeObjetivo.nombre)
        {
            defensaBasicaDadosExtra = arma.GetBonusDefensaPropia();
            if (arma is cArmasArco && c.personajeActivo.zonaActual == zonaActual)
            {
                defensaBasicaDadosExtra -= 1;
            }
        }
        else
        {
            defensaBasicaDadosExtra = arma.GetBonusDefensaAjena();
        }
        int numeroDeDados = 3 + atr.ingenio + hab.defensaBasica + defensaBasicaDadosExtra;
        //Capaz hay qeu sumarle 1
        //if (arma is cArmasLigeras)
        //{
        //    if (!(arma as cArmasLigeras).yaActuo[c.personajeActivo.nombre])
        //    {
        //        Debug.Log(nombre + " gana bonus de yaActuo conta " + c.personajeActivo.nombre);
        //        numeroDeDados++;
        //    }
        //}
        if (arma.GetDeRango() && c.enemigosEnMelee.Count > 0)
        {
            numeroDeDados -= 1;
        }
        Debug.Log(" pachamos por aca?");
        tirada tr = cDieMath.TirarDados(numeroDeDados);
        int def = cDieMath.sumaDe3Mayores(tr);
        if (c.atacando)
        {
            c.ui.m = (nombre + " defiende con " + numeroDeDados + " dados, y saca " + def + " contra el ataque de " + atq);
        }
        else
        {
            c.ui.m = (nombre + " interviene con " + numeroDeDados + " dados, y saca " + def + " contra la Guardia de " + atq);
        }
        if (def >= atq)
        {
            bonusPAtqBporDefB += def - atq;
            return true;
        }
        if (c.atacando)
        {
            fallamosDefPor = atq - def;
        }
        else
        {
            fallamosDefPor = c.personajeActivo.GetGuardia() - def;
        }
        return false;
    }

        public void GastarDado(int faseActual, List<sAccion> acciones, List<sAccion> accionesActivas, List<sAccion> accionesReactivas, string text)
    {
        uiC.SetText(text + " " + nombre + " gasta su dado de la fase " + dadosDeAccion[0] + ".");
        //Remover Dado de Accion Valido Menor y determinar la fase que tenia
        int acFase = 11;
        for (int i = 0; i < acciones.Count; i++)
        {
            if (acciones[i].per.nombre == nombre && acciones[i].fase <= faseActual)
            {
                acFase = acciones[i].fase;
                acciones.RemoveAt(i);
                break;
            }
        }

        //Si hay, remueve una accion activa. si no ni entra en el loop
        for (int i = 0; i < accionesActivas.Count; i++)
        {
            if (accionesActivas[i].per.nombre == nombre && accionesActivas[i].fase == acFase)
            {
                accionesActivas.RemoveAt(i);
                break;
            }
        }

        //remueve una accion reactiva
        for (int i = 0; i < accionesReactivas.Count; i++)
        {
            if (accionesReactivas[i].per.nombre == nombre && accionesReactivas[i].fase == acFase)
            {
                accionesReactivas.RemoveAt(i);
                break;
            }
        }

        //Mata dado de acicon, y determina si seguimos teniendo reacciones disponibles 
        bool dieKilled = false;
        for (int i = 1; i <= faseActual; i++)
        {
            for (int j = 0; j < dadosDeAccion.Length; j++)
            {
                if (dadosDeAccion[j] == i)
                {
                    if (!dieKilled)
                    {
                        dadosDeAccion[j] = 11;
                        dieKilled = true;
                        reaccion1Disponible = false;
                    }
                    else
                    {
                        reaccion1Disponible = true;
                    }
                }
            }
        }

        //De momento, se gasta el dado cuando empieza el movimiento agresivo, asi que si se quiere defender el ataque no se gana el bonus. Imaigno que la idea es que si, no?
        //pk, mentira, esto es un problema con todo. apenas tratan de actuar pierden en dado, asi que esto se llama mucho antes. capaz hat que llamarlo en otro lado
        //o capaz hay que gastar el dado despues
        //foreach (var per in c.personajes)
        //{
        //    if (per.arma is cArmasLigeras && per.nombre != nombre)
        //    {
        //        (per.arma as cArmasLigeras).yaActuo[nombre] = true;
        //    }
        //}

        ReOrdenarDados(acciones);
        CalcularTotalDeIniciativa();
    }

    public void RetrocederDadoDeAccion(int faseActual, List<sAccion> acciones, List<sAccion> accionesActivas, List<sAccion> accionesReactivas, string text)
    {
        //Determinar acFase y sumar a dado de accion y tal vez remover
        int acFase = 11;
        sAccion acTemp;
        acTemp.fase = 11;
        for (int i = 0; i < acciones.Count; i++)
        {
            if (acciones[i].per.nombre == nombre && acciones[i].fase <= faseActual)
            {
                acFase = acciones[i].fase;
                acTemp.fase = acFase + fallamosDefPor;
                acTemp.per = this;
                acciones[i] = acTemp;
                text += (acciones[i].per.nombre + " falla en su defensa por " + fallamosDefPor + ", y su dado de " + acFase + " pasa a " + acTemp.fase);
                if (acTemp.fase > 10)
                {
                    acciones.RemoveAt(i);
                    text += (" Ahora vale mas de 10, asi que se pierde.");
                }
                uiC.SetText(text);
                break;
            }
        }

        //Si hay, remueve una accion activa
        bool removed = false;
        for (int i = 0; i < accionesActivas.Count; i++)
        {
            if (accionesActivas[i].per.nombre == nombre && accionesActivas[i].fase == acFase && acTemp.fase > faseActual)
            {
                accionesActivas.RemoveAt(i);
                removed = true;
                break;
            }
        }
        if (!removed)
        {
            accionesActivas.Sort((x, y) => x.fase.CompareTo(y.fase));
        }

        //remueve una accion reactiva
        for (int i = 0; i < accionesReactivas.Count; i++)
        {
            if (accionesReactivas[i].per.nombre == nombre && accionesReactivas[i].fase == acFase && acTemp.fase > faseActual)
            {
                accionesReactivas.RemoveAt(i);
                break;
            }
        }

        //Mueve dado de accion, y determina si seguimos teniendo reacciones disponibles 
        bool dieMoved = false;
        for (int i = 1; i <= faseActual; i++)
        {
            for (int j = 0; j < dadosDeAccion.Length; j++)
            {
                if (dadosDeAccion[j] == i)
                {
                    if (!dieMoved)
                    {
                        dadosDeAccion[j] += fallamosDefPor;
                        dieMoved = true;
                        reaccion1Disponible = false;
                    }
                    else
                    {
                        reaccion1Disponible = true;
                        Debug.Log("aca 4");
                    }
                }
            }
        }

        //foreach (var per in c.personajes)
        //{
        //    if (per.arma is cArmasLigeras)
        //    {
        //        Debug.Log(nombre + " pasa a YA ACTUO");
        //        (per.arma as cArmasLigeras).yaActuo[nombre] = true;
        //    }
        //}

        fallamosDefPor = 0;
        ReOrdenarDados(acciones);
        CalcularTotalDeIniciativa();
    }

    public void ReOrdenarDados(List<sAccion> acciones)
    {
        System.Array.Sort(dadosDeAccion);
        acciones.Sort((x, y) => x.fase.CompareTo(y.fase));
    }

    public void GuardarGastarDado(List<sAccion> accionesActivas)
    {
        for (int i = 0; i < accionesActivas.Count; i++)
        {
            if (accionesActivas[i].per.nombre == nombre)
            {
                accionesActivas.RemoveAt(i);
                break;
            }
        }
    }

    //Tirar Heridas, devuelve si sigue vivo
    public void Heridas(int daño)
    {
        hSupe += daño;
        string text = (nombre + " toma " + daño + " de daño, para un total de " + hSupe + ".");
        int numeroDeDados = 3;
        numeroDeDados += atr.brio * 3;

        tirada tr = cDieMath.TirarDados(numeroDeDados, true);
        int tiradaDeHeridasRes = cDieMath.sumaDe3Mayores(tr);
        text += (" Con " + atr.brio + " en Brio tira " + numeroDeDados + " dados, sacando " + tiradaDeHeridasRes + ".");

        if (tiradaDeHeridasRes < hSupe)
        {
            int dif = hSupe - tiradaDeHeridasRes;
            int hAdicionales = (dif / 30 ) + 1;
            hDram += hAdicionales;
            text += (" Falla la tirada por " + dif + ", y toma " + hAdicionales + " Heridas, para un total de " + hDram + ".");
            hSupe = 0;
        }
        else
        {
            text += " " + nombre + " tuvo exito en la tirada de Heridas!";
        }
        uiC.SetText(text);
    }

    public void HeridoPorArmaDeFuego()
    {
        int hAd = hSupe / 30;
       uiC.SetText(nombre + " recibio un disparo tomando 1 Herida, y por ya tener " + hSupe + " de daño toma " + hAd + " adicionales.");
        hDram += 1 + hAd;
        hSupe = 0;
    }

    public void CapazMori()
    {
        if (hDram >= 3)
        {
            c.RemoverPersonaje(this);
            uiC.ActualizarIniciativa(c.personajes);
            if (!c.MasDeUnEquipoEnPie())
            {
                c.stateID = cCombate.TERMINAR_COMBATE;
            }
            //ESTE MENSAJE NO TAPA AL DE DAÑO?
            uiC.SetText(nombre + " llego a 3 Heridas, se murio!");
        }
    }

    //Tirar Daño
    public int Daño()
    {
        int numeroDeDados = 3;
        numeroDeDados += atr.musculo * arma.GetMusMult() + bonusPAtqBporDefB;
        if (c.movAgro) numeroDeDados -= 3;
        c.movAgro = false;
        //Esto se estaria sumando aunque hagas otro ataque que no sea el basico, hay que arreglarlo
        //Update Primera Escuela
        tirada tr = cDieMath.TirarDados(numeroDeDados, arma.GetDañoExpl());
        int dan = cDieMath.sumaDe3Mayores(tr);
        c.ui.m = (nombre + ", con " + atr.musculo + " en Musculo y " + arma.GetMusMult() + " en su multiplicador de musculo, tira " + numeroDeDados + " dados, sacando " + dan);
        bonusPAtqBporDefB = 0;
        return dan;
    }

    //Tirar Iniciativa
    public void Iniciativa()
    {
        //HacerTirada
        int numeroDeDados = GetDadosBaseIniciativa() + atr.donaire * 3 + arma.GetBonusIniciativa();
        dadosDeAccion = RollDadosdeAccion(numeroDeDados);
        CalcularTotalDeIniciativa();
    }

    public virtual int GetDadosBaseIniciativa()
    {
        return 3;
    }

    public virtual int[] RollDadosdeAccion(int numeroDeDados)
    {
        // Para mostrar al jugador los que no se usaron, tendria que pasarle un arrat de ints vacios de tamaño numedro de dados-3
        // y que lo llenen con todos los que no meten en el array de los 3 menores
        return cDieMath.tomar3Menores(cDieMath.TirarDados(numeroDeDados));
    }

    public void CalcularTotalDeIniciativa()
    {
        valorDeIniciativa = 0;
        for (int i = 0; i < dadosDeAccion.Length; i++)
        {
            if (dadosDeAccion[i] <= 10)
            {
                valorDeIniciativa += dadosDeAccion[i];
            }
        }
    }

    public virtual void ResetHP()
    {
        vivo = true;
        hSupe = 0;
        hDram = 0;
    }

    void OnMouseOver()
    {
        if (py.actions["Select"].WasPressedThisFrame())
        {
            if (c.esperandoObjetivo && c.personajeActivo.equipo != equipo && vivo)
            {
                uiC.OnPersonajeClicked(this);
            }
            else
            {
                foreach (var item in c.personajes)
                {
                    item.selected = false;
                }
                selected = true;
                c.perSeleccionado = true;
            }
        }
    }

    private void OnMouseEnter()
    {
        hovered = true;
        c.uiC.MostrarInfoPerVital(this);
    }

    private void OnMouseDown()
    {
        if (!c.uiC.esperandoPersonaje)
        {
            if (mostrandoTactica)
            {
                c.uiC.MostrarInfoPerCompleta(this);
                mostrandoTactica = false;
            }
            else
            {
                c.uiC.MostrarInfoPerTactica(this);
                mostrandoTactica = true;
            }
        }
    }

    private void OnMouseExit()
    {
        hovered = false;
        c.uiC.EsconderInfoPerVital();     
    }


    ///////////////////////////////////////
    ///////////////////////////////////////
    ///////////////////////////////////////
    ///Consecuencias -- Cosas que le pasan
    public void ConsecuenciaTirarIniciativa() { }
    public void ConsecuenciaTirarHeridas() { }
    public void ConsecuenciaHeridoPorArmaDeFuego() { }
    ///Utilidades -- El resto, funciones menores
    public void Accionar(string accion)
    {
        GetAccionPorNombre(accion).Ejecutar();
    }

    public cAcciones GetAccionPorNombre(string accionNombre)
    {
        foreach (var item in acciones)
        {
            if (item.nombre == accionNombre)
            {
                return item;
            }
        }
        return acciones[0];
    }

    public cAcciones GetAccionPorNumero(int accionNumero)
    {
        return GetAccionPorNombre(c.GetNombreDeAccion(accionNumero));
    }

    public void Reaccionar(string reaccion)
    {
        foreach (var item in reacciones)
        {
            if (item.nombre == reaccion)
            {
                item.Ejecutar();
                break;
            }
        }
    }
}
