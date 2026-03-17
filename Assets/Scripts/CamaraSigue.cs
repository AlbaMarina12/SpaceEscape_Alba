using UnityEngine;

public class CamaraSigue : MonoBehaviour
{
    [Header("Configuración de Seguimiento")]
    public Transform objetivo; // Debería aparecer un cuadro aquí
    public Vector3 desfase = new Vector3(0, 0, -10);

    void LateUpdate()
    {
        if (objetivo != null)
        {
            transform.position = objetivo.position + desfase;
        }
    }
}