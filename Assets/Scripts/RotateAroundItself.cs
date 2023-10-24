using UnityEngine;

public class RotacionObjeto : MonoBehaviour
{
    public float velocidadRotacion = 30.0f; // Velocidad de rotación en grados por segundo

    // Update se llama una vez por fotograma
    void Update()
    {
        // Rotamos el objeto alrededor del eje Y
        transform.Rotate(Vector3.up * velocidadRotacion * Time.deltaTime);
    }
}
