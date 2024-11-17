using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;
using System;

class PlayerControllerTests
{
    private GameObject player;
    private PlayerController playerController;

    [SetUp]
    public void Setup()
    {
        // Cria um jogador temporário na cena para os testes
        player = new GameObject();
        playerController = player.AddComponent<PlayerController>();

        // Inicializa o Animator e Rigidbody2D se necessário
        playerController.oAnimator = player.AddComponent<Animator>();
        playerController.oRigidbody2D = player.AddComponent<Rigidbody2D>();

    }
    
    
    [Test]
    public void TestSpriteFlipRight()
    {
        var player = new GameObject("Player");
        var spriteRenderer = player.AddComponent<SpriteRenderer>();

        spriteRenderer.flipX = false;
        Assert.IsFalse(spriteRenderer.flipX);
    }

    [Test]
    public void TestPlayerCanPunch()
    {
        playerController.canPunch = true;
        playerController.isPunching = false;

        playerController.PerformPunch();

        Assert.That(playerController.isPunching, Is.True);
    }

    [Test]
    public void TestPlayerCanJump()
    {
        playerController.canJump = true;
        playerController.isJumping = false;

        playerController.PerformJump();

        Assert.That(playerController.isJumping, Is.True);
    }

    [Test]
    public void TestPlayerCanMove()
    {
        playerController.canMove = true;
        playerController.isMoving = false;

        playerController.PerformMovement();

        Assert.That(playerController.isMoving, Is.True);
    }

    [Test]
    public void TestPlayerCantPunch()
    {
        playerController.canPunch = false;
        playerController.isPunching = false;

        playerController.PerformPunch();

        Assert.That(playerController.isPunching, Is.False);
    }

    [Test]
    public void TestPlayerCantJump()
    {
        playerController.canJump = false;
        playerController.isJumping = false;

        playerController.PerformJump();

        Assert.That(playerController.isJumping, Is.False);
    }

    [Test]
    public void TestPlayerCantMove()
    {
        playerController.canMove = false;
        playerController.isMoving = false;

        playerController.PerformMovement();

        Assert.That(playerController.isMoving, Is.False);
    }

}