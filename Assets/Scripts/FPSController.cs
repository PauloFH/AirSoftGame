using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FPSController : MonoBehaviour
{
    [Header("Movimento")]
    public float velocidadeCaminhada = 5f;
    public float velocidadeCorrida = 8f;
    public float forcaPulo = 2f;
    public float gravidade = -20f;
    
    [Header("Mouse Look")]
    public float sensibilidadeMouse = 2f;
    public Transform cameraTransform;
    public float limiteVerticalMin = -90f;
    public float limiteVerticalMax = 90f;
    
    private CharacterController controller;
    private Vector3 velocidade;
    private float rotacaoX = 0f;
    
   
    private bool movimentoTravado = false;
    
    void Start()
    {
        controller = GetComponent<CharacterController>();
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }
    }
    
    void Update()
    {
        OlharComMouse();
        
        if (!movimentoTravado)
        {
            Movimentar();
        }
        
        // ESC para destravar cursor (debug)
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
    
    void Movimentar()
    {
        bool estaNoChao = controller.isGrounded;
        
        if (estaNoChao && velocidade.y < 0)
        {
            velocidade.y = -2f;
        }
        
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        
        Vector3 direcao = transform.right * moveX + transform.forward * moveZ;
        
        float velocidadeAtual = Input.GetKey(KeyCode.LeftShift) ? velocidadeCorrida : velocidadeCaminhada;
        
        controller.Move(direcao * (velocidadeAtual * Time.deltaTime));
        
        if (Input.GetButtonDown("Jump") && estaNoChao)
        {
            velocidade.y = Mathf.Sqrt(forcaPulo * -2f * gravidade);
        }
        
        velocidade.y += gravidade * Time.deltaTime;
        controller.Move(velocidade * Time.deltaTime);
    }
    
    void OlharComMouse()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensibilidadeMouse;
        float mouseY = Input.GetAxis("Mouse Y") * sensibilidadeMouse;
        
        transform.Rotate(Vector3.up * mouseX);
        
        rotacaoX -= mouseY;
        rotacaoX = Mathf.Clamp(rotacaoX, limiteVerticalMin, limiteVerticalMax);
        cameraTransform.localRotation = Quaternion.Euler(rotacaoX, 0f, 0f);
    }
}
