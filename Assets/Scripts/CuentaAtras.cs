using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class CuentaAtras : MonoBehaviour
{
    public Button boton;
    public Image imagen;
    public Sprite[] numeros;

    // Start is called before the first frame update
    void Start()
    {
        boton.onClick.AddListener(Empezar);
    }

    void Empezar()
    {
        imagen.gameObject.SetActive(true);
        boton.gameObject.SetActive(false);

        StartCoroutine(CuentaAtrass());
    }

    IEnumerator CuentaAtrass()
    {
        for (int i = 0; i < numeros.Length; i++)
        {
            imagen.sprite = numeros[i];
            yield return new WaitForSeconds(1);
        }
        SceneManager.LoadScene("nivel1");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
