using UnityEngine;

public class Collectible : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Предмет собран!");
            Destroy(gameObject); // Удаляем объект после сбора
        }
    }
}