using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MithrilController : MonoBehaviour
{
    [SerializeField] int direction = 0;
    [SerializeField] bool changingDirection = false;
    [SerializeField] bool attacking = false;
    Animator animator;
    
    void Start() {
        animator = GetComponent<Animator>();
    }

    void FixedUpdate() {
        if (attacking) { 
            attack();
        } else if (!changingDirection) {
            move();
        }
        //animator 
        if (direction==0) {
            animator.SetFloat("Move Y", 1);
            animator.SetFloat("Move X", 0);
        } else if (direction==1) {
            animator.SetFloat("Move Y", 0);
            animator.SetFloat("Move X", -1);
        } else if (direction==2) {
            animator.SetFloat("Move Y", 0);
            animator.SetFloat("Move X", 1);
        } else if (direction==3) {
            animator.SetFloat("Move Y", -1);
            animator.SetFloat("Move X", 0);
        }
    }
    
    void attack() {

    }

    void changeDirection() {
        
    }
    
    void move() {
        
    }
}
