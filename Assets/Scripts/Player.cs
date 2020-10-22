using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MovableObject
{
    public float restartLevelDelay = 1.0f;
    public int pointPerFood = 10;
    public int pointPerSoda = 20;
    public int wallDamage = 1;
    private Animator animator;
    private int food;
    public Text foodText;

    public AudioClip fruitClip1;
    public AudioClip fruitClip2;
    public AudioClip chopClip1;
    public AudioClip chopClip2;
    public AudioClip dieClip;
    public AudioClip footStepClip1;
    public AudioClip footStepClip2;
    public AudioClip sodaClip1;
    public AudioClip sodaClip2;

    protected override void OnCantMove<T>(T component)
    {
        Wall wall = component as Wall;
        wall.TakeDamage(wallDamage);
        animator.SetTrigger("playerChop");
        SoundManager.instance.PlayerRandom(chopClip1, chopClip2);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Exit"))
        {
            Invoke("Restart", restartLevelDelay);
            enabled = false;
        }
        else if(other.CompareTag("Food"))
        {
            SoundManager.instance.PlayerRandom(fruitClip1, fruitClip2);
            food += pointPerFood;
            other.gameObject.SetActive(false);
            UpdateText();
        }
        else if(other.CompareTag("Soda"))
        {
            SoundManager.instance.PlayerRandom(sodaClip1, sodaClip2);
            food += pointPerSoda;
            other.gameObject.SetActive(false);
            UpdateText();
        }

    }

    private void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
        print("RESTART");
    }

    public void LoseFood(int loss)
    {
        food -= loss;
        animator.SetTrigger("playerHit");
        CheckIfGameOver();
        UpdateText();
    }

    void UpdateText()
    {
        foodText.text = "Food: " + food;
    }

    // Start is called before the first frame update
    protected override void Start() 
    {
        animator = GetComponent<Animator>();
        food = GameManager.instance.playerFoodPoint;

        base.Start();
        rend = GetComponent<Renderer>();
        UpdateText();
    }

    void OnDisable()
    {
        GameManager.instance.playerFoodPoint = food;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.playerTurn) return;

        int horizontal = (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) ? -1 : 0;
        int vertical = (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) ? 1 : 0;
        horizontal += (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) ? 1 : 0;
        vertical += (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) ? -1 : 0;

        if (horizontal != 0)
        {
            vertical = 0;
        }
        

        if(horizontal != 0 || vertical != 0)
        {
            AttempMove<Wall>(horizontal, vertical);
        }
    }

    protected override void AttempMove<T>(int xDir, int yDir)
    {
        food--;
        base.AttempMove<T>(xDir, yDir);

        RaycastHit2D hit;
        if(Move(xDir, yDir, out hit))
        {
            SoundManager.instance.PlayerRandom(footStepClip1, footStepClip2);
        }

        CheckIfGameOver();
        GameManager.instance.playerTurn = false;
        UpdateText();
    }

    private void CheckIfGameOver()
    {
        if(food <= 0)
        {
            GameManager.instance.GameOver();
            SoundManager.instance.PlayerSingle(dieClip);
        }
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
