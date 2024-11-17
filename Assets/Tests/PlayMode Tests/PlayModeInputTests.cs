using System.Collections;
using NUnit.Framework;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class PlayModeInputTests : InputTestFixture
{
    
    [UnityTest] 
    public IEnumerator PressShiftAndPunch() 
    { 
        SceneManager.LoadScene("SampleScene"); 
        yield return null;
        
        var player = GameObject.FindWithTag("Player"); 
        Assert.IsNotNull(player, "O jogador não foi encontrado na cena de teste.");
        
        var playerController = player.GetComponent<PlayerController>(); 
        var keyboard = InputSystem.AddDevice<Keyboard>();
        
        Press(keyboard.leftShiftKey); 
        yield return new WaitForFixedUpdate();
        
        Assert.IsTrue(playerController.isPunching, "Esperado que o jogador esteja socando.");
        
    }
    
    [UnityTest] 
    public IEnumerator JumpAndTryToPunch() 
    { 
        SceneManager.LoadScene("SampleScene"); 
        yield return null;
        
        var player = GameObject.FindWithTag("Player"); 
        Assert.IsNotNull(player, "O jogador não foi encontrado na cena de teste.");
        
        var playerController = player.GetComponent<PlayerController>(); 
        var keyboard = InputSystem.AddDevice<Keyboard>();
        
        Press(keyboard.spaceKey);
        Press(keyboard.leftShiftKey);
        yield return new WaitForFixedUpdate();
        
        Assert.IsFalse(playerController.isPunching, "Esperado que o jogador nao possa socar no ar.");
        
    }
    
    [UnityTest] 
    public IEnumerator PunchAndTryToJump() 
    { 
        SceneManager.LoadScene("SampleScene"); 
        yield return null;
        
        var player = GameObject.FindWithTag("Player"); 
        Assert.IsNotNull(player, "O jogador não foi encontrado na cena de teste.");
        
        var playerController = player.GetComponent<PlayerController>(); 
        var keyboard = InputSystem.AddDevice<Keyboard>();
        
        Press(keyboard.leftShiftKey);
        Press(keyboard.spaceKey);
        
        yield return new WaitForFixedUpdate();
        
        Assert.IsTrue(playerController.isPunching,"Esperado que o jogador esteja socando");
        Assert.IsFalse(playerController.isJumping, "Esperado que o jogador nao possa pular no meio da animaçao de soco.");
        
    }

    [UnityTest]
    public IEnumerator PressSpaceAndJump()
    {
        SceneManager.LoadScene("SampleScene");
        yield return null;

        var player = GameObject.FindWithTag("Player");
        Assert.IsNotNull(player, "O jogador não foi encontrado na cena de teste.");

        var playerController = player.GetComponent<PlayerController>();
        var keyboard = InputSystem.AddDevice<Keyboard>();

        Assert.IsTrue(playerController.canJump);
        Assert.IsFalse(playerController.isJumping);
        Press(keyboard.spaceKey);
        yield return new WaitForSeconds(0.3f);
        
        Assert.IsTrue(playerController.isJumping, "Esperado que o jogador esteja pulando.");
        Assert.IsFalse(playerController.canJump);
    }

    [UnityTest]
    public IEnumerator WalkThenJump()
    {
        SceneManager.LoadScene("SampleScene");
        yield return null;

        var player = GameObject.FindWithTag("Player");
        Assert.IsNotNull(player, "O jogador não foi encontrado na cena de teste.");

        var playerController = player.GetComponent<PlayerController>();
        var keyboard = InputSystem.AddDevice<Keyboard>();

        Press(keyboard.dKey);
        yield return new WaitForSeconds(0.3f);
        Assert.IsTrue(playerController.isMoving, "Esperado que o jogador esteja andando");
        Assert.IsTrue(playerController.canJump);
        
        Release(keyboard.dKey);
        Press(keyboard.spaceKey);
        yield return new WaitForFixedUpdate();
        Assert.IsTrue(playerController.isJumping, "Esperado que o jogador esteja pulando.");
        Assert.IsFalse(playerController.canJump);

    }
    
    [UnityTest]
    public IEnumerator SpriteFlipTest()
    {
        SceneManager.LoadScene("SampleScene"); 
        yield return null;
        
        var player = GameObject.FindWithTag("Player");

        var playerController = player.GetComponent<PlayerController>();
        var transform = player.GetComponent<Transform>();
        var keyboard = InputSystem.AddDevice<Keyboard>();

        Press(keyboard.dKey);
        yield return new WaitForSeconds(0.3f);
        Assert.IsTrue(transform.localScale.x > 0);

        Release(keyboard.dKey);
        Press(keyboard.aKey);
        yield return new WaitForSeconds(0.3f); 
        Assert.IsTrue(transform.localScale.x < 0);

    }
    
}