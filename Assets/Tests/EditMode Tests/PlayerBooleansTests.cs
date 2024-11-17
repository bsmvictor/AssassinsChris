using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;
using System;
using UnityEngine.InputSystem;

class PlayerBooleansTests
{
    private GameObject player;
    private PlayerController playerController;

    [SetUp]
    public void SetUp()
    {
        player = new GameObject();
        playerController = player.AddComponent<PlayerController>();
        playerController.oRigidbody2D = player.AddComponent<Rigidbody2D>();
        playerController.oAnimator = player.AddComponent<Animator>();
    }
    
    [Test]
    public void PlayerMovesHorizontallyWhenCanMoveIsTrue()
    {
        playerController.canMove = true;
        playerController.OnMove(new InputAction.CallbackContext());
        playerController.moveAmount = new Vector2(1, 0);
        playerController.PerformMovement();
        Assert.AreNotEqual(0, playerController.oRigidbody2D.linearVelocityX);
    }
    
    
    [Test]
    public void Player_DoesNotMove_WhenCanMoveIsFalse()
    {
        playerController.canMove = false;
        playerController.OnMove(new InputAction.CallbackContext());

        playerController.PerformMovement();

        Assert.AreEqual(0, playerController.oRigidbody2D.linearVelocityX);
    }

    [Test]
    public void Player_Jumps_WhenCanJumpIsTrue()
    {
        playerController.canJump = true;
        playerController.PerformJump();
        
        Assert.IsTrue(playerController.isJumping);
    }
    
    [Test]
    public void TestResetPunchRestoresPlayerState()
    {
       
        playerController.canMove = true;
        playerController.canJump = true;
        playerController.canPunch = true;
        
        playerController.PerformPunch();
        
        Assert.IsFalse(playerController.canMove);
        Assert.IsFalse(playerController.canJump);
        Assert.IsFalse(playerController.canPunch);
        
        playerController.StartCoroutine(WaitAndCheck(1.0f, playerController));

        
    }

    private IEnumerator WaitAndCheck(float waitTime, PlayerController player)
    {
        yield return new WaitForSeconds(waitTime);
        
        Assert.IsTrue(playerController.canMove);
        Assert.IsTrue(playerController.canJump);
        Assert.IsTrue(playerController.canPunch);
    }
    
    
}