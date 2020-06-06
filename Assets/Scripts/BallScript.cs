using UnityEngine;
using System.Collections;

public class BallScript : MonoBehaviour
{
    public const string VIRUS = "Virus";
    public const string BOUNDARY = "Boundary";
    public const string LINE = "Line";
    public int speedRate = 3;
    public Sprite VirusSprite;
    public CircleCollider2D ballCollider;
    private Vector2 InitialLocation;
    public bool isOriginalVirus;
    // Use this for initialization
    void Start()
    {
        InitialLocation = transform.position;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        ballCollider.GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.CurrentGameState == GameManager.GameState.Playing){
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            Vector2 v = rb.velocity.normalized;
            rb.velocity = v * speedRate;
        } 
    }

    public void StartBall()
    {
        transform.position = InitialLocation;
        GetComponent<Rigidbody2D>().velocity = new Vector2(1.0f, 1.0f);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        SpriteRenderer currentSprite = this.gameObject.GetComponent<SpriteRenderer>();
        bool isBoundary= other.gameObject.CompareTag(BOUNDARY);
        bool isVirus = other.gameObject.CompareTag(VIRUS);
        bool isLine = other.gameObject.CompareTag(LINE);

        if (isOriginalVirus && !isBoundary && !isLine) {
            float x = transform.localScale.x;
            float y = transform.localScale.y;
            transform.localScale = new Vector2(x < 2 ? x * 1.2f : x, y < 2 ? y * 1.2f : y);
        }
        if (!isOriginalVirus && isVirus) {
            transform.gameObject.tag = VIRUS;
            ballCollider.radius = 0.6f;
            currentSprite.sprite = VirusSprite;
        }
    }

}
