using UnityEngine;
using UnityEngine.InputSystem; 

public class PlayerController : MonoBehaviour
{
    public float speed = 5f; // переменная, отвечающая за скорость движения персонажа
    public float jumpHeight = 2f; // переменная, отвечающая за высоту прыжка
    public float gravity = -9.81f; // переменная, отвечающая за силу гравитации

    public float mouseSensitivity = 50f; // отвечает за чувствительность мыши.
    //private float xRotation = 1f; // скорость вращения персонажа

    public int maxJumps = 2; // задаёт количество прыжков (двойной прыжок)
    private int jumpsLeft; // отслеживает оставшиеся прыжки


    private CharacterController controller; // компонент, отвечающий за перемещение.
    private Vector3 velocity; // хранит скорость персонажа, включая движение вверх и вниз.
    private bool isGrounded; // флаг, указывающий, стоит ли персонаж на земле
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<CharacterController>(); // получает ссылку на компонент CharacterController, который отвечает за физику и управление персонажем, при старте игры — void Start() и записывает
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = controller.isGrounded; // все также задается состояние игрока, стоит он на земле или нет
        if (isGrounded) // проверяет, находится игрок на земле или нет.
        {
            jumpsLeft = maxJumps; // Если игрок на земле, то конструкция сбрасывает jumpsLeft до maxJumps при приземлении
            if (velocity.y < 0) // если вертикальная скорость меньше нуля, то происходит сброс вертикальной скорости — velocity.y = -2f; 
            {
                velocity.y = -2f;
            }
        }

        // Переменные для хранения направления движения (-1, 0 или 1)
        float moveX = 0f; // движение влево-вправо
        float moveZ = 0f; // движение вперед-назад

        // Обработка нажатий клавиш через новую систему ввода (Input System)
        if (Keyboard.current.dKey.isPressed) moveX = 1f;  // D или стрелка вправо
        if (Keyboard.current.aKey.isPressed) moveX = -1f; // A или стрелка влево
        if (Keyboard.current.wKey.isPressed) moveZ = 1f;  // W или стрелка вверх
        if (Keyboard.current.sKey.isPressed) moveZ = -1f; // S или стрелка вниз

        // Формируем вектор движения в локальных координатах персонажа
        // transform.right - ось X (влево-вправо)
        // transform.forward - ось Z (вперед-назад)
        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        
        // Перемещаем персонажа с учетом скорости и времени кадра
        controller.Move(move * speed * Time.deltaTime);

        

        // Применяем гравитацию (увеличиваем скорость падения каждый кадр)
        velocity.y += gravity * Time.deltaTime;
        
        // Применяем вертикальное движение (прыжок или падение)
        controller.Move(velocity * Time.deltaTime);

        // Получаем движение мыши через новую систему ввода
        Vector2 mouseDelta = Mouse.current.delta.ReadValue();

        // Горизонтальный поворот персонажа (влево-вправо)
        float mouseX = mouseDelta.x * mouseSensitivity * Time.deltaTime;
        transform.Rotate(Vector3.up * mouseX);

        // Вертикальный поворот камеры (вверх-вниз) — если камера отдельно
        // Пока закомментировано, так как у тебя камера управляется через CameraFollow
        // float mouseY = mouseDelta.y * mouseSensitivity * Time.deltaTime;
        // xRotation -= mouseY;
        // xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        // cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        
        if (Keyboard.current.spaceKey.wasPressedThisFrame && jumpsLeft > 0)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            jumpsLeft--; 
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.CompareTag("Slippery"))
        {
            speed = 2f; 
            velocity.x *= 1.2f; 
        }
        else
        {
            speed = 5f; 
        }
    }

}
