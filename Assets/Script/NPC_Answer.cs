using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Answer : MonoBehaviour
{
    [SerializeField]
    [TextArea(10, 5)]
    public string mensaje;
    public int indice_nombre;

    public bool etiqueta = true;

    void Conversacion()
    {
        if (etiqueta)
        {
            // mensaje = mensaje.Replace("<#Name>", BaseDatos.ObtenerBDActual().stringDB[indice_nombre]);
            etiqueta = false;
        }

        if (!TextBox.on)
            TextBox.MuestraTexto(mensaje, false);
        //print(mensaje);

        //Monstruos m1;

        //m1 = Monstruos.getMonstruo(BaseDatos.BuscarMonstruoIndice(0));
        //print(m1.nombre);
    }
}
