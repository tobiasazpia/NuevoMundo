using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class cLaVoluntadDelCreador : cArmasPesadas
{
    //Principiante: Tiras 1 dado adicional en tus tiradas de Daño, y tu Base a superar para Matones adicionales se reduce en 1.

    //Sed de Sangre: Pasiva. Cada vez que mates más un maton o causes más de una herida dramática en un solo Ataque, tiras 1 dado Adicional en tus tiradas de Combate, Daño y Heridas hasta el final de la ronda por cada Rango en Sed de Sangre.
    //para la pasiva... me parece que tengo que transformar su trigger en un evento, escucharlo y ver como resurlvo.

    //Terror de Dios: Defensa. Hasta el final de la ronda, los dados del objetivo no explotan en sus tiradas de Heridas.
    //Ira Divina: Acción.Tiras Ira Divina con Brio contra Brio. Hasta el final de la ronda, las tiradas de Daño del objetivo no explotan, los 9 tambien explotan en tiradas de daño contra el, o si son Matones su Guardia cuenta como si fuese 3 menos de lo que es.

    private int m_DadosDeSedDeSangre;
    public int DadosDeSedDeSangre
    {
        get { return m_DadosDeSedDeSangre; }
        set
        {
            if (m_DadosDeSedDeSangre == value) return;
            m_DadosDeSedDeSangre = value;
            if (OnDadosDeSedDeSangreChange != null)
                OnDadosDeSedDeSangreChange(m_DadosDeSedDeSangre);
        }
    }
    public delegate void OnDadosDeSedDeSangreChangeDelegate(int newVal);
    public event OnDadosDeSedDeSangreChangeDelegate OnDadosDeSedDeSangreChange;
    //Sed de Sangre: Pasiva. Cada vez que mates más un maton o causes más de una herida dramática en un solo Ataque, tiras 1 dado Adicional en tus tiradas de Combate, Daño y Heridas hasta el final de la ronda por cada Rango en Sed de Sangre.

    // Start is called before the first frame update
    void Start()
    {
        habilidadesNombres[0] = "Sed de Sangre";
        habilidadesNombres[1] = "Terror de Dios";
        habilidadesNombres[2] = "Ira Divina";
        habilidadesNombres[3] = "Intimidar";

        SetUpHabilidades();
        maestria = CalcularMaestria(habilidades);
        Debug.Log("maestria: " + maestria);

        p.tieneTradicionMarcial = true;
        SetUpNombre();

        SetUpNumeros();
        //Principiante
        // Tiras 1 dado adicional en tus tiradas de Daño, y tu Base a superar para Matones adicionales se reduce en 1.
        basePara2doMaton -= 1; // el dado extra esta en AB

        //Veterano: Las Explosiones en tus tiradas de Daño dan 2 dados adicionales en vez de 1, y tus 10 en tiradas de Ataque contra Matones valen 11 (Maximo 30).
        // Ya aplicado en AB
        if (maestria > 4)
        {
            //Maestro: Tu Multiplicador de Músculo es 4.
            musMult = 4;
        }

        p.CalcularExtraParaMatones();
        p.CalcularGuardia();

        SetUpAccionables();
    }

    static public string GetNombreDeHabilidad(int index)
    {
        switch (index)
        {
            case 0:
                return "Ataque Básico";
            case 1:
                return "Defensa Básica";
            case 2:
                return "Sed de Sangre";
            case 3:
                return "Terror de Dios";
            case 4:
                return "Ira Divina";
            case 5:
                return "Intimidar";
            default:
                return "Error, index no era habilidad";
        }
    }

    static public string GetDescripcionDeHabilidad(int index)
    {
        switch (index)
        {
            case 0:
                return "Ataque Básico Desc";
            case 1:
                return "Defensa Básica Desc";
            case 2:
                return "Pasiva. Cada vez que vos o un aliado derribe más de un Matón o cause más de una Herida en un solo Ataque a alguien bajo el efecto de una de tus Habilidades de Combate, tirás 1 dado Adicional en tus tiradas de Combate, Daño y Heridas hasta el final de la ronda por cada Punto en Sed de Sangre.";
            case 3:
                return "Defensa. Hasta el final de la ronda, los dados del objetivo no explotan en sus tiradas de Heridas o se derriba un Matón adicional contra él.";
            case 4:
                return "Acción. Con Brío contra Brío. Hasta el final de la ronda, los Ataques del objetivo no generan tiradas de Heridas, y en las tiradas de Daño contra el, los 9 también explotan o se derriba un Matón adicional.";
            case 5:
                return "Habilidad Marcial Civil; al repetir una tirada, tira tantos dados adicionales como puntos tenga " + GetNombreDeHabilidad(index) + ".";
            default:
                return "error descripcion habilidad";
        }
    }

    protected override void SetUpNombre()
    {
        nombre = "V. del Creador";
    }

    protected override void SetUpAccionables()
    {
        acciones.Add(gameObject.AddComponent<cAccionMovimientoAgresivo>());
        acciones.Add(gameObject.AddComponent<cAccionAtaqueBasicoVoluntadDelCreador>());
        acciones.Add(gameObject.AddComponent<cAccionIraDivina>());
        reacciones.Add(gameObject.AddComponent<cReaccionDefensaBasicaVoluntadDelCreador>());
        reacciones.Add(gameObject.AddComponent<cReaccionTerrorDeDios>());
        AgregarAccionablesAlPersonaje();
    }

    //trigger: Cada vez que mates más un maton o causes más de una herida dramática en un solo Ataque
    void SedDeSangre() // de momento no esta siendo usada, suma directo el numero en las situaciones de trigger
    {
        p.c.uiC.perCambio = p.nombre;
        DadosDeSedDeSangre += habilidades[2];
        // Principio de ronda, dadosDeSedDeSangre = 0;
        // Sumamos dadosDeSedDeSangre a nuestras tiras de Combate, Daño y Heridas
    }

    new static public void FillInfo(VisualElement vE)
    {
        VisualElement contenedor = vE.ElementAt(0);

        VisualElement encabezado = contenedor.ElementAt(0);
        VisualElement tecnicas = contenedor.ElementAt(1);
        VisualElement habilidades = contenedor.ElementAt(2);

        Label titulo = encabezado.ElementAt(0) as Label;
        Label desc = encabezado.ElementAt(1) as Label;

        titulo.text = "<b>" + GetNombre();
        desc.text = "Armas Pesadas, Latia, Intimidar - Derribar Matones y Daño";

        (tecnicas.ElementAt(1).ElementAt(0) as Label).text = "<b>Principiante:</b> Tirás 1 dado adicional en tus tiradas de Daño, y tu Base a superar para Matones adicionales se reduce en 1.";
        (tecnicas.ElementAt(1).ElementAt(1) as Label).text = "<b>Veterano:</b> Las Explosiones en tus tiradas de Daño dan 2 dados adicionales en vez de 1, y tus 10 en tiradas de Ataque contra Matones suman 1 más (Máximo 30).";
        (tecnicas.ElementAt(1).ElementAt(2) as Label).text = "<b>Maestro:</b> Tu Multiplicador de Músculo es 4.";

        (habilidades.ElementAt(1).ElementAt(0) as Label).text = "<b>Sed de Sangre:</b> Pasiva. Cada vez que vos o un aliado derribe más de un Matón o cause más de una Herida en un solo Ataque a alguien bajo el efecto de una de tus Habilidades de Combate, tirás 1 dado Adicional en tus tiradas de Combate, Daño y Heridas hasta el final de la ronda por cada Punto en Sed de Sangre.";
        (habilidades.ElementAt(1).ElementAt(1) as Label).text = "<b>Ira Divina:</b> Especial con Brío contra Brío. Hasta el final de la ronda, los Ataques del objetivo no generan tiradas de Heridas, y en las tiradas de Daño contra el, los 9 también explotan o se derriba un Matón adicional.";
        (habilidades.ElementAt(1).ElementAt(2) as Label).text = "<b>Terror de Dios:</b> Defensa. Hasta el final de la ronda, los dados del objetivo no explotan en sus tiradas de Heridas o se derriba un Matón adicional contra él.";
    }

    static public string GetNombre()
    {
        return "La Voluntad del Creador";
    }
    }
