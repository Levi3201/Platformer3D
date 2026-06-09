using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // переменная для хранения объекта, за которым должна следовать камера.
    public float smoothSpeed = 0.125f; // переменная, определяющая, насколько плавно камера будет двигаться.
    public Vector3 offset; // смещение камеры относительно персонажа.
    private float currentAngle; // текущий угол поворота камеры.
    
    void LateUpdate()
    {
        // Перемещение камеры
        Vector3 desiredPosition = target.position + target.TransformDirection(offset);
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Плавное закрепление вращения камеры за персонажем
        float targetAngle = target.eulerAngles.y;
        currentAngle = Mathf.LerpAngle(currentAngle, targetAngle, smoothSpeed);
        transform.rotation = Quaternion.Euler(10f, currentAngle, 0f);
    

       
    }
}
