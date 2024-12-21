using System.Linq;
using UnityEngine;

public class CoinScript : MonoBehaviour
{
    private static int coinScore; 
    private float minOffset = 40f; // мін. відстань від країв "світу"
    private float minDistance = 100f; // мін. відстань від попереднього положення
    private Animator animator;
    private Collider[] colliders;
    private AudioSource catchSound;
    
    [SerializeField]
    private TMPro.TextMeshProUGUI coinScoreText;

    void Start()
    {
        animator = GetComponent<Animator>();
        colliders = GetComponents<Collider>();
        catchSound = GetComponent<AudioSource>();
        coinScore = 0;
    }

    void Update() { }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Character")
        {
            if (colliders[0].bounds.Intersects(other.bounds))
            {
                animator.SetInteger("AnimationState", 2);
                catchSound.Play();
                coinScore++;
                coinScoreText.text = coinScore.ToString();
                ReplaceCoin();
            }
            else
            {
                animator.SetInteger("AnimationState", 1);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Character")
        {
            if (colliders[1].bounds.Intersects(other.bounds))
            {
                animator.SetInteger("AnimationState", 0);
            }
        }
    }

    public void ReplaceCoin()
    {
        Vector3 newPosition;
        do
        {
            newPosition =
                this.transform.position
                + new Vector3(
                    Random.Range(-minDistance, minDistance),
                    this.transform.position.y,
                    Random.Range(-minDistance, minDistance)
                );
        } while (
            Vector3.Distance(newPosition, this.transform.position) < minDistance
            || newPosition.x < minOffset
            || newPosition.z < minOffset
            || newPosition.x > 1000 - minOffset
            || newPosition.z > 1000 - minOffset
        );
        float terrainHeight = Terrain.activeTerrain.SampleHeight(newPosition);
        newPosition.y = terrainHeight + Random.Range(2f, 20f - terrainHeight);
        this.transform.position = newPosition;
        animator.SetInteger("AnimationState", 0);
    }
}
/* Д.З. Створити анімацію (кліп) пульсації монети
 * Реалізувати переходи між усіма станами аніматора
 * (не між кожною парою доцільні переходи).
 * * Впровадити переходи при наближенні персонажа.
 */
