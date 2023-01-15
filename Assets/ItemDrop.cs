using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    public float dropRadius = 2f; // radius around the item that the player must be in to pick it up
    public float dropSpeed = 2f; // speed at which the item moves towards the player

    private Transform player; // reference to the player's transform
    private Rigidbody2D rb; // reference to the item's rigidbody

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform; // find the player object by tag
        rb = GetComponent<Rigidbody2D>(); // get the item's rigidbody
    }

    void Update()
    {
        // if the player is within the drop radius
        if (Vector2.Distance(transform.position, player.position) < dropRadius)
        {
            // move the item towards the player
            rb.velocity = (player.position - transform.position).normalized * dropSpeed;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // if the item collides with the player
        if (other.CompareTag("Player"))
        {
            
            Destroy(gameObject); // destroy the item
        }
    }
}
