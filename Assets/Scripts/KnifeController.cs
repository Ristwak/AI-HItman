using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KnifeController : MonoBehaviour
{
    private KnifeManager knifeManager;
    private Rigidbody2D kniferb;
    [SerializeField] private float movespeed;
    private bool CanShoot;

    [SerializeField] private string gameSceneName = "GameScene";   // NEW - drag the scene name in Inspector
    [SerializeField] private float loadDelay = 0.5f;

    void Start()
    {
        GetComponents();
    }

    private void Update()
    {
        HandleShoot();
    }

    private void FixedUpdate()
    {
        Shoot();
    }

    private void HandleShoot()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SoundManager.Instance.PlaySound("Shoot");
            CanShoot = true;
            knifeManager.SetDisableKnifeIconColor();
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Circle"))
        {
            // ---------- EXISTING SUCCESS LOGIC ----------
            knifeManager.SetActiveKnife();
            CanShoot = false;
            kniferb.isKinematic = true;
            kniferb.constraints = RigidbodyConstraints2D.FreezeAll;
            transform.SetParent(other.transform);

            // ---------- NEW  : load the gameplay scene ----------
        }

        if (other.gameObject.CompareTag("Knife"))
        {
            StartCoroutine(LoadSceneAfterDelay(gameSceneName, loadDelay));
            // SceneManager.LoadScene("MainScene");   // keep your failure reload
        }
    }

    private IEnumerator LoadSceneAfterDelay(string sceneName, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        //  - change to LoadSceneMode.Additive if you intend to stack scenes
    }

    private void Shoot()
    {
        if (CanShoot)
        {
            kniferb.AddForce(Vector2.up * movespeed * Time.fixedDeltaTime);
        }
    }

    private void GetComponents()
    {
        kniferb = GetComponent<Rigidbody2D>();
        knifeManager = GameObject.FindObjectOfType<KnifeManager>();
    }


}
