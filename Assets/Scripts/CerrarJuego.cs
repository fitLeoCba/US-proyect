using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CerrarJuego : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Cerrando el juego...");
            Application.Quit();

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false; // Solo en editor
#endif
        }
    }
}
