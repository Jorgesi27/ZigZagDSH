using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

public class JugadorBola : MonoBehaviour
{
    public Camera camara;
    public GameObject suelo;
    public float velocidad = 5;
    public Canvas canvasGameOver;
    public GameObject estrella;
    public Transform plataformactual;
    public GameObject plataformainicial;

    private Vector3 offset;
    private float ValX, ValZ;
    private Vector3 DireccionActual;
    private bool juegoTerminado = false;
    private int miPuntuacion = -1;
    private int aleatorio2;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private TMP_Text textoPuntuacion;
    [SerializeField] private TMP_Text textoPuntuacionTotal;


    // Start is called before the first frame update
    void Start()
    {
        offset = camara.transform.position;
        CrearSueloInicial();
        DireccionActual = Vector3.forward;
    }

    // Update is called once per frame
    void Update()
    {
        if (!juegoTerminado)
        {
            camara.transform.position = transform.position + offset;
            if (Input.GetKeyUp(KeyCode.Space))
            {
                CambiarDireccion();
            }
            transform.Translate(DireccionActual * velocidad * Time.deltaTime);
            if(transform.position.y < 0.0f)
            {
                GameOver();
            }
            if(miPuntuacion == 25)
            {
                nextLevel();
            }
        }
    }

    void GameOver()
    {
        juegoTerminado = true;
        canvasGameOver.gameObject.SetActive(true);
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.tag == "suelo")
        {
            Debug.Log("Colision con suelo");
            StartCoroutine(BorrarSuelo(other.gameObject));
        }
    }

    IEnumerator BorrarSuelo(GameObject suelo)
    {
        float aleatorio = Random.Range(0.0f, 1.0f);
        if (aleatorio < 0.5f)
        {
            ValX += 6.0f;
        }
        else
        {
            ValZ += 6.0f;
        }

        plataformactual = Instantiate(suelo, new Vector3(ValX, 0, ValZ), Quaternion.identity).transform;
        aleatorio2 = Random.Range(0, 5);
        if(aleatorio2 < 2)
        {
            float offsetX = Random.Range(-2.0f, 2.0f);
            float offsetZ = Random.Range(-2.0f, 2.0f);
            Instantiate(estrella, plataformactual.position + new Vector3(offsetX, 0.5f, offsetZ), estrella.transform.rotation);
        }  
        yield return new WaitForSeconds(2);
        suelo.gameObject.GetComponent<Rigidbody>().isKinematic = false;
        suelo.gameObject.GetComponent<Rigidbody>().useGravity = true;
        yield return new WaitForSeconds(2);
        Destroy(suelo);
    }

    void CambiarDireccion()
    {
        if (DireccionActual == Vector3.forward)
        {
            DireccionActual = Vector3.right;
        }
        else
        {
            DireccionActual = Vector3.forward;
        }
    }

    void CrearSueloInicial()
    {
        for (int i = 0; i < 3; i++)
        {
            ValZ += 6.0f;
            plataformainicial = Instantiate(suelo, new Vector3(ValX, 0, ValZ), Quaternion.identity);
            /*float posY = 0.25f;
            float posZ = ValZ;
            if (Random.value < 0.6f)  // Ajusta el valor según la probabilidad deseada
            {
                InstanciarEstrellas(new Vector3(ValX, posY, posZ));
            }*/
        }
    }

   /* void InstanciarEstrellas(Vector3 posicion)
    {
        for (int i = 0; i < 3; i++)
        {
            float offsetX = Random.Range(-2.0f, 20.0f);  // Ajusta según tus necesidades
            float offsetZ = Random.Range(-2.0f, 50.0f);  // Ajusta según tus necesidades
            Instantiate(estrella, new Vector3(posicion.x + offsetX, posicion.y, posicion.z + offsetZ), Quaternion.identity);
        }
    }*/

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("estrella"))
        {
            Debug.Log("Colision con la estrella");
            Destroy(other.gameObject); // Destruir la estrella

        // Reproducir el sonido asociado al AudioSource
            if (audioSource != null && audioSource.clip != null)
            {
                audioSource.PlayOneShot(audioSource.clip);
            }

            IncrementarPuntuacion(); // Incrementar la puntuación del jugador u realizar otras acciones según sea necesario
        }
    }

    void IncrementarPuntuacion()
    {
        miPuntuacion +=3;
        textoPuntuacion.text = miPuntuacion.ToString();
        textoPuntuacionTotal.text = textoPuntuacion.text;
    }

    void nextLevel()
    {
        int indiceSiguienteEscena = SceneManager.GetActiveScene().buildIndex + 1;
        if (indiceSiguienteEscena < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(indiceSiguienteEscena);
        }
    }

    public void Reiniciar()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Salir()
    {
        Application.Quit();
    }
}
