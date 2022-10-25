using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    bool isCoinPicked = false;
    [SerializeField] AudioClip coinPickupSFX;
    [SerializeField] int pointsForCoinPickup = 100;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isCoinPicked == false && collision.gameObject.CompareTag("Player"))
        {
            FindObjectOfType<GameSession>().AddToScore(pointsForCoinPickup);
            AudioSource.PlayClipAtPoint(coinPickupSFX, Camera.main.transform.position);
            isCoinPicked = true;
            Destroy(gameObject);
        }
    }
}