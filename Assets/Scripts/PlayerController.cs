using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private int jumps = 0;
    private Rigidbody playerRb;
    private float jumpStrength = 14;
    private float moveSpeed = 5;
    private float rotateSpeed = 20;
    private float horizontalInput;
    private AudioSource playerAudio;
    public AudioClip jumpSound;
    public AudioClip lastJumpSound;
    public AudioClip jumpRecharge;
    public ParticleSystem launchPS;
    public ParticleSystem trailPS;
    public ParticleSystem fallPS;
    private GameManager gameManager;

    private ParticleSystem rocketTrail;

    // Start is called before the first frame update
    void Start()
    {
        jumps = 0;
        playerRb = GetComponent<Rigidbody>();
        playerAudio = GetComponent<AudioSource>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        rocketTrail = Instantiate(trailPS, gameObject.transform, false);
        rocketTrail.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        rocketTrail.transform.position = gameObject.transform.GetChild(3).gameObject.transform.position + new Vector3(0, -0.4f, 0);
        horizontalInput = Input.GetAxis("Horizontal");

        if (jumps < 3)
        {
            transform.Translate(Vector3.left * Time.deltaTime * moveSpeed * horizontalInput);
            transform.Rotate(Vector3.forward * Time.deltaTime * rotateSpeed * horizontalInput);
            if (!rocketTrail.isPlaying)
            {
                rocketTrail.Play();
            }

        }
        else if (rocketTrail.isPlaying)
        {
            rocketTrail.Stop();
        }

        if (Input.GetKeyDown(KeyCode.Space) && (jumps > 0))
        {
            playerRb.AddForce(Vector3.up * jumpStrength, ForceMode.Impulse);
            playerAudio.PlayOneShot(jumpSound, 0.5f);
            jumps--;
            Instantiate(launchPS, gameObject.transform.GetChild(3).gameObject.transform.position, rocketTrail.transform.rotation);
            if (jumps == 0)
            {
                StartCoroutine(OutOfFuelSound());
            }
        }
    }

    private IEnumerator OutOfFuelSound()
    {
        playerAudio.PlayOneShot(lastJumpSound, 1);
        yield return new WaitForSeconds(lastJumpSound.length);
        playerAudio.PlayOneShot(lastJumpSound, 1);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Platform"))
        {
            PlatformBehaviour platScript = other.gameObject.GetComponent<PlatformBehaviour>();
            if (platScript.touched == false)
            {
                if (other.gameObject.transform.position.y > -4.0f)
                {
                    gameManager.score++;
                    gameManager.UpdateScore();
                    gameManager.CallMoveDown();
                }
                other.gameObject.GetComponent<PlatformBehaviour>().platMaterial.color = Color.red;
                playerAudio.PlayOneShot(jumpRecharge, 0.75f);
                jumps = 3;
                other.gameObject.GetComponent<PlatformBehaviour>().touched = true;
                other.gameObject.GetComponent<PlatformBehaviour>().ActivateCountdown();
            }
            transform.eulerAngles = new Vector3(0, 180, 0);
            rocketTrail.transform.eulerAngles = new Vector3(90, 90, 90);
        }

        if (other.gameObject.CompareTag("Sensor"))
        {
            Instantiate(fallPS, gameObject.transform.position, fallPS.transform.rotation);
            gameManager.gameOverScreen.SetActive(true);
            Destroy(gameObject);
        }
    }
}
