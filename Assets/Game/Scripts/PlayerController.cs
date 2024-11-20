using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D oRigidbody2D;
    public Animator oAnimator;
    public PlayerInput playerInput;

    [Header("Stats")]
    public float playerSpeed = 10f;
    public float jumpForce = 50f;
    public float fallMult = 1.5f;
    public Vector2 X_bounds = new(-10, 10);
    public Vector2 Y_bounds = new(-10, 10);

    [Header("Booleans")]
    public bool canMove = true;
    public bool canPunch = true;
    public bool canJump = true;
    public bool isMoving = false;
    public bool isPunching = false;
    public bool isJumping = false;
    public bool isJumpPressed = false;

    public Vector2 moveAmount;

    private void Awake()
    {
        playerInput = new PlayerInput();

        // Adiciona os callbacks para os inputs
        playerInput.PlayerControls.Jump.started += OnJump;
        playerInput.PlayerControls.Jump.canceled += OnJump;

    }

    private void Start()
    {
        oRigidbody2D = GetComponent<Rigidbody2D>();
        oAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (canMove)
            PerformMovement();

        if (canPunch && isPunching)
        {
            PerformPunch();
        }
    }

    // Método que controla a movimentação do jogador
    public void PerformMovement()
    {
        // Verifica se o jogador pode se mover
        if (!canMove)
        {
            isMoving = false; // Garante que isMoving continue falso
            return; // Sai do método se canMove for false
        }

        // Confirma a movimentação do player
        isMoving = true;

        // Usa o movimento do eixo X para o movimento horizontal e preserva a velocidade vertical
        oRigidbody2D.linearVelocity = new Vector2(moveAmount.x * playerSpeed, oRigidbody2D.linearVelocityY);

        // Define a animação dependendo do movimento horizontal
        if (moveAmount.x == 0)
        {
            oAnimator.SetTrigger("isIdle");
        }
        else
        {
            oAnimator.SetTrigger("isWalking");

            // Adiciona o flip da sprite para a direção que o jogador está se movendo
            if (moveAmount.x > 0) // Movendo para a direita
            {
                transform.localScale = new Vector3(1f, 1f, 1f); // Normal
            }
            else if (moveAmount.x < 0) // Movendo para a esquerda
            {
                transform.localScale = new Vector3(-1f, 1f, 1f); // Inverte no eixo X
            }
        }

        // Limita a posição do jogador dentro dos limites definidos para o eixo X
        oRigidbody2D.position = new Vector2(Mathf.Clamp(oRigidbody2D.position.x, X_bounds.x, X_bounds.y), oRigidbody2D.position.y);
    }

    // Função que controla o pulo
    public void PerformJump()
    {
        // Verifica se o jogador pode pular
        if (!canJump)
        {
            isJumping = false; // Garante que isJumping continue falso
            return; // Sai do método se canJump for false
        }

        // Aplica o movimento de pulo e altera o estado de animação
        oRigidbody2D.linearVelocity = new Vector2(oRigidbody2D.linearVelocityX, jumpForce);
        isJumping = true;
        canJump = false;
        canPunch = false;
        oAnimator.SetTrigger("isJumping");

        // Inicia a coroutine para restaurar `canPunch` após o pulo
        StartCoroutine(ResetJump());
    }

    // Coroutine para restaurar `canPunch` após o pulo
    private IEnumerator ResetJump()
    {
        // Espera um pequeno tempo para simular a duração do pulo (ajuste conforme necessário)
        yield return new WaitForSeconds(0.5f);

        // Restaura `canPunch` ao final do pulo
        canPunch = true;
    }


    // Função que controla o soco
    public void PerformPunch()
    {
        // Verifica se o jogador pode socar
        if (!canPunch)
        {
            isPunching = false; // Garante que isPunching continue falso
            return; // Sai do método se canPunch for false
        }

        // Aplica o soco e atualiza o estado
        oAnimator.SetTrigger("isPunching");
        oRigidbody2D.linearVelocity = new Vector2(0, oRigidbody2D.linearVelocityY); // Zera o movimento no eixo X
        isPunching = true;
        isMoving = false;
        canPunch = false;
        canMove = false;
        canJump = false;

        // Inicia a coroutine para resetar o soco
        StartCoroutine(ResetPunch());
    }


    // Coroutine para resetar o soco
    public IEnumerator ResetPunch()
    {
        yield return new WaitForSeconds(1.0f); // Tempo da animação de soco
        isPunching = false;
        canPunch = true;
        canMove = true;
        canJump = true;
    }

    // Input System Callback for Movement
    public void OnMove(InputAction.CallbackContext context)
    {
        // Lê o valor float do eixo horizontal (esquerda/direita)
        float horizontalMovement = context.ReadValue<float>();

        // Define o movimento no eixo X e zera o movimento no eixo Y
        moveAmount = new Vector2(horizontalMovement, 0);
    }

    // Input System Callback for Jump
    public void OnJump(InputAction.CallbackContext context)
    {
        isJumpPressed = context.ReadValueAsButton();

        // Verifica se o botão de pulo está pressionado e se o pulo é permitido
        if (isJumpPressed && canJump)
        {
            PerformJump(); // Chama a função de pulo separada
        }
    }

    // Input System Callback for Punch
    public void OnPunch(InputAction.CallbackContext context)
    {
        // Somente permite o soco se a ação for "performed" e o jogador não estiver atacando
        if (context.performed && !isPunching)
        {
            PerformPunch(); // Chama a função de soco separada
        }
    }


    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            oAnimator.SetTrigger("isIdle");
            isJumping = false;
            canJump = true;
        }
    }

    public void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isJumping = true;
            canJump = false;
        }
    }
}
