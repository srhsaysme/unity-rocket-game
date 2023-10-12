using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformBehaviour : MonoBehaviour
{
    public Material platMaterial;
    public ParticleSystem dissolveEffect;
    private GameManager gameManager;
    public int platformTime;
    public bool touched;

    // Start is called before the first frame update
    void Start()
    {
        platMaterial = GetComponent<Renderer>().material;
        touched = false;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        platformTime = gameManager.platformTime;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision other)
    {

        if (other.gameObject.CompareTag("Sensor"))
        {
            Destroy(gameObject);
        }
    }

    public void ActivateCountdown()
    {
        StartCoroutine(CoroutineCountdown(platformTime));
    }

    public IEnumerator CoroutineCountdown(int countdown)
    {
        yield return new WaitForSeconds(countdown);
        ParticleSystem tempEffect = Instantiate(dissolveEffect, gameObject.transform.position, dissolveEffect.transform.rotation);
        tempEffect.transform.localScale = new Vector3(gameObject.transform.localScale.x / 10.0f, 1, 1);
        Destroy(gameObject);
    }
}
