using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextBox : MonoBehaviour {
    public static TextBox textBox;
    public static bool on = false;
    public static bool ocultar = false;

    Text textTB;

    void Awake()
    {
        if (textBox == null)
            textBox = this;

        textTB = GetComponentInChildren<Text>();
        textBox.gameObject.SetActive(on);
    }



    public static void MuestraTexto(string t, bool combate)
    {
        TextBox.textBox.Escribir(t);
    }



    public void Escribir(string t)
    {
        on = true;
        textBox.gameObject.SetActive(on);
        textBox.textTB.text = t;
        //StartCoroutine(Deletrear(t));

    }



    public IEnumerator Deletrear(string t)
    {
        textBox.textTB.text = "";

        yield return new WaitForSeconds(.1f);

        for (int i = 0; i < t.Length + 1; i++)
        {
            textBox.textTB.text = t.Substring(0, i);
            yield return new WaitForSeconds(.01f);
        }

        if (t == textBox.textTB.text)
        {
            ocultar = true;
        }
    }



    public void OcultarTexto()
    {
        on = false;
        ocultar = false;
        textBox.gameObject.SetActive(on);
    }



    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && ocultar)
        {
            OcultarTexto();
        }
    }



    public static void OcultaTextoFinCombate()
    {
        TextBox.textBox.OcultarTexto();
    }
}
