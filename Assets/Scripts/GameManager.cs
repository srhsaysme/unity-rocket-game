using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject platform;
    public GameObject rocket;
    public GameObject background;
    public GameObject titleScreen;
    public TextMeshProUGUI scoreText;
    public GameObject gameOverScreen;

    public ParticleSystem dissolveEffect;

    public float score = 0;
    private float lastPlatX = 0;
    private float edgeRange = 13;
    private float platXDev = 12;
    private float newX;
    private float leftBound;
    private float rightBound;
    public int platformTime;
    private float platSize = 10.0f;
    private float sizeDecreaseRate;
    private float minSize;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame(int newPlatTime, float newRate, float newMinSize)
    {
        lastPlatX = 0;
        platSize = 10.0f;
        platformTime = newPlatTime;
        sizeDecreaseRate = newRate;
        minSize = newMinSize;
        Instantiate(rocket, new Vector3(0, -2.5f, 5), rocket.transform.rotation);
        Instantiate(platform, new Vector3(0, -4, 5), platform.transform.rotation);
        CreatePlatform(4.0f, platSize);
        titleScreen.gameObject.SetActive(false);
        UpdateScore();
    }

    void CreatePlatform(float yValue, float size)
    {
        leftBound = lastPlatX - platXDev;
        if (leftBound < -edgeRange)
        {
            leftBound = -edgeRange;
        }

        rightBound = lastPlatX + platXDev;
        if (rightBound > edgeRange)
        {
            rightBound = edgeRange;
        }

        do {
            newX = Random.Range(leftBound, rightBound);
        } while (newX > (lastPlatX - 2) && newX < (lastPlatX + 2));
        lastPlatX = newX;
        GameObject createdPlat = Instantiate(platform, new Vector3(newX, yValue, 5), platform.transform.rotation);
        createdPlat.transform.localScale = new Vector3(platSize, 0.5f, 3.0f);
        platSize -= sizeDecreaseRate;
        if (platSize < minSize)
        {
            platSize = minSize;
        }
    }

    public void CallMoveDown()
    {
        CreatePlatform(12.0f, platSize);
        GameObject[] objList = GameObject.FindGameObjectsWithTag("Platform");
        foreach (GameObject tempPlat in objList)
        {
            if (tempPlat.GetComponent<PlatformBehaviour>().touched == false)
            {
                 StartCoroutine(MoveDown(tempPlat));
            }
            else
            {
                ParticleSystem tempEffect = Instantiate(dissolveEffect, gameObject.transform.position, dissolveEffect.transform.rotation);
                tempEffect.transform.localScale = gameObject.transform.localScale;
                Destroy(tempPlat);
            }
        }

        GameObject[] rocketList = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject rocket in rocketList)
        {
            StartCoroutine(MoveDown(rocket));
        }
        StartCoroutine(MoveDown(background));

        GameObject[] particleList = GameObject.FindGameObjectsWithTag("Particles");
        foreach (GameObject particleObject in particleList)
        {
            ParticleSystem PS = particleObject.GetComponent<ParticleSystem>();
            int particleCount = PS.particleCount;
            ParticleSystem.Particle[] particles = new ParticleSystem.Particle[particleCount];
            PS.GetParticles(particles);

            for (int i = 0; i < particles.Length; i++)
            {
                particles[i].velocity += new Vector3(0, -3, 0);
            }

            PS.SetParticles(particles, particleCount);
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    IEnumerator MoveDown(GameObject movingObj)
    {
        float elapsedTime = 0;

        while (elapsedTime < 0.5f)
        {
            movingObj.transform.Translate(Vector3.down * Time.deltaTime * 16); 
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

    public void UpdateScore()
    {
        scoreText.text = "Score: " + score;
    }
}
