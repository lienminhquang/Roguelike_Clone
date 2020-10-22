using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MovableObject
{
    public int playerDamage = 1;
    private Animator animator;
    private Transform target;
    private bool skipMove;
    public AudioClip attackClip1;
    public AudioClip attackClip2;


    protected override void OnCantMove<T>(T component)
    {
        Player player = component as Player;
        player.LoseFood(playerDamage);
        animator.SetTrigger("enemyAttack");
        SoundManager.instance.PlayerRandom(attackClip1, attackClip2);
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        GameManager.instance.AddEnemyToList(this);
        animator = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        base.Start();

        rend = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void AttempMove<T>(int xDir, int yDir)
    {
        if(skipMove)
        {
            skipMove = false;
            return;
        }

        base.AttempMove<T>(xDir, yDir);
        skipMove = true;
    }

    public void MoveEnemy()
    {
        int xDir = 0;
        int yDir = 0;

        if(Mathf.Abs(target.position.x - transform.position.x) < 1.0f)
        {
            yDir = target.position.y > transform.position.y ? 1 : -1;
        }
        else
            xDir = target.position.x > transform.position.x ? 1 : -1;
        

        AttempMove<Player>(xDir, yDir);
    }

    Renderer rend;

    // Draws a wireframe sphere in the Scene view, fully enclosing
    // the object.
    void OnDrawGizmosSelected()
    {
        // A sphere that fully encloses the bounding box.
        Vector3 center = rend.bounds.center;
        float radius = rend.bounds.extents.magnitude;

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(center, rend.bounds.size);
    }
}
