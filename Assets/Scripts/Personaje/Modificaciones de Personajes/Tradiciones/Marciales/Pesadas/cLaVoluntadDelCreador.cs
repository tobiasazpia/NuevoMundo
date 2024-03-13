using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cLaVoluntadDelCreador : cArmasPesadas
{
    //Principiante: Tiras 1 dado adicional en tus tiradas de Da�o, y tu Base a superar para Matones adicionales se reduce en 1.

    //Sed de Sangre: Pasiva. Cada vez que mates m�s un maton o causes m�s de una herida dram�tica en un solo Ataque, tiras 1 dado Adicional en tus tiradas de Combate, Da�o y Heridas hasta el final de la ronda por cada Rango en Sed de Sangre.
    //para la pasiva... me parece que tengo que transformar su trigger en un evento, escucharlo y ver como resurlvo.

    //Terror de Dios: Defensa. Hasta el final de la ronda, los dados del objetivo no explotan en sus tiradas de Heridas.
    //Ira Divina: Acci�n.Tiras Ira Divina con Brio contra Brio. Hasta el final de la ronda, las tiradas de Da�o del objetivo no explotan, los 9 tambien explotan en tiradas de da�o contra el, o si son Matones su Guardia cuenta como si fuese 3 menos de lo que es.

    public int maestria;

    public int valor_SedDeSangre;
    public int valor_TerrorDeDios;
    public int valor_IraDivina;
    public int valor_EnIntimidar;

    public int dadosDeSedDeSangre;
    // Start is called before the first frame update
    void Start()
    {
        SetUpNombre();

        SetUpNumeros();
        //Principiante
        // Tiras 1 dado adicional en tus tiradas de Da�o, y tu Base a superar para Matones adicionales se reduce en 1.
        basePara2doMaton -= 1;
        p.CalcularExtraParaMatones();
        p.CalcularGuardia();


        if (maestria > 2)
        {
            //Veterano
            //Veterano: Las Explosiones en tus tiradas de Da�o dan 2 dados adicionales en vez de 1, y tus 10 en tiradas de Ataque contra Matones valen 11 (Maximo 30).
            // To Do
        }
        if (maestria > 4)
        {
            //Maestro
            //Maestro: Tu Multiplicador de M�sculo es 4.
            //musMult = 4;
        }
        musMult = 4;
        p.CalcularExtraParaMatones();
        valor_TerrorDeDios = 5;

        SetUpAccionables();
    }

    protected override void SetUpNombre()
    {
        nombre = "Voluntad del Creador";
    }

    protected override void SetUpAccionables()
    {
        acciones.Add(gameObject.AddComponent<cAccionMovimientoAgresivo>());
        acciones.Add(gameObject.AddComponent<cAccionAtaqueBasicoVoluntadDelCreador>());
        acciones.Add(gameObject.AddComponent<cAccionIraDivina>());
        reacciones.Add(gameObject.AddComponent<cReaccionDefensaBasica>());
        reacciones.Add(gameObject.AddComponent<cReaccionTerrorDeDios>());
        AgregarAccionablesAlPersonaje();
    }

    //trigger: Cada vez que mates m�s un maton o causes m�s de una herida dram�tica en un solo Ataque
    void SedDeSangre()
    {
        dadosDeSedDeSangre += valor_SedDeSangre;
        // Principio de ronda, dadosDeSedDeSangre = 0;
        // Sumamos dadosDeSedDeSangre a nuestras tiras de Combate, Da�o y Heridas
    }
}
