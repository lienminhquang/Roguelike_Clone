using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovableObject : MonoBehaviour
{
    float moveTime = 0.1f;
    public LayerMask blockingLayer;

    private BoxCollider2D boxCollider2D;
    private Rigidbody2D rgd2d;
    private float inverseMoveTime;

    protected virtual void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        rgd2d = GetComponent<Rigidbody2D>();
        inverseMoveTime = 1 / moveTime;
    }

    protected bool Move(int xDir, int yDir, out RaycastHit2D hit)
    {
        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(xDir, yDir);

        //Color a = CompareTag("Player") ? Color.green : Color.red;
        //Debug.DrawRay(start, (new Vector2(xDir, yDir)).normalized, a, 2, false);

        boxCollider2D.enabled = false;
        hit = Physics2D.Linecast(start, end, blockingLayer);
        boxCollider2D.enabled = true;

        if(hit.transform == null)
        {
            StartCoroutine(SmoothMove(end));
            return true;
        }

        return false;
    }

    protected IEnumerator SmoothMove(Vector3 target)
    {
        float sqrtDist = (transform.position - target).sqrMagnitude;
        while (sqrtDist > float.Epsilon)
        {
            Vector3 newPos = Vector3.MoveTowards(transform.position, target, inverseMoveTime * Time.deltaTime);
            rgd2d.MovePosition(newPos);
            sqrtDist = (transform.position - target).sqrMagnitude;
            yield return null;
        }
    }

    protected virtual void AttempMove<T>(int xDir, int yDir)
        where T: Component
    {
        RaycastHit2D hit;
        bool canMove = Move(xDir, yDir, out hit);
        if(hit.transform == null)
        {
            return;
        }
        T hitComponent = hit.transform.GetComponent<T>();
        if(!canMove && hitComponent != null)
        {
            OnCantMove(hitComponent);
        }
    }
    protected abstract void OnCantMove<T>(T component)
        where T : Component;
    // Update is called once per frame
    void Update()
    {
        
    }
}
