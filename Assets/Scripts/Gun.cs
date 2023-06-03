using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Gun : MonoBehaviour
{
    RaycastHit hit;
    public GameObject muzzle;
    LineRenderer beam;

    public float fireDelay;
    bool isFireReady;
    public ParticleSystem muzzleFlash;

    AudioSource audio;
    public AudioSource audio2;

    public GameObject bulletHole;

    public int ammo = 30;
    public int maxAmmo = 30;

    public TextMeshProUGUI text;

    public Transform originalPos;
    public Transform zoomPos;

    public GameObject Sight;
    public GameObject Scope;

    // Start is called before the first frame update
    void Start()
    {
        beam = muzzle.GetComponentInChildren<LineRenderer>();
        isFireReady = true;
        audio = muzzle.GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0) && isFireReady && ammo > 0)
            Shoot();

        if (Input.GetKeyDown(KeyCode.R))
            Reload();

        Zoom();
    }

    void Zoom()
    {
        if(Input.GetMouseButton(1))
        {
            transform.position = zoomPos.position;
            Scope.SetActive(true);
            Sight.SetActive(false);
            transform.GetChild(5).gameObject.SetActive(false);

            /*
            while(this.transform.localPosition != zoomPos.position)
            {
                this.transform.localPosition = Vector3.Lerp(transform.position, zoomPos.position, 0.2f);
            }
            */
        }
        else
        {
            transform.position = originalPos.position;
            Scope.SetActive(false);
            Sight.SetActive(true);
            transform.GetChild(5).gameObject.SetActive(true);

            /*
            while (this.transform.localPosition != originalPos.position)
            {
                this.transform.localPosition = Vector3.Lerp(transform.position, originalPos.position, 0.2f);
            }
            */
        }

    }

    void Reload()
    {
        //1.26��
        ammo = maxAmmo;
        audio2.Play();
        isFireReady = false;
        StartCoroutine(ReloadDelayOn());
    }

    void Shoot()
    {
        ammo -= 1;
        text.text = ammo.ToString();
        muzzleFlash.Play();
        audio.Play();
        beam.enabled = true;
        beam.SetPosition(0, muzzle.transform.position);
        if(Physics.Raycast(muzzle.transform.position, muzzle.transform.forward, out hit, 100))
        {
            //Debug.DrawRay(muzzle.transform.position, muzzle.transform.forward * hit.distance, Color.red);
            beam.SetPosition(1, hit.point);
            if (hit.collider.tag == "Ground")
                Instantiate(bulletHole, hit.point, Quaternion.Euler(90, 0, 0));
            else
                Instantiate(bulletHole, hit.point, hit.transform.rotation);

        }
        else
        {
            beam.SetPosition(1, muzzle.transform.position + muzzle.transform.forward * 100);
        }
        isFireReady = false;
        StartCoroutine("FireDelayOn");
    }

    IEnumerator FireDelayOn()
    {
        yield return new WaitForSeconds(fireDelay);
        isFireReady = true;
        beam.enabled = false;
    }

    IEnumerator ReloadDelayOn()
    {
        yield return new WaitForSeconds(1.5f);
        text.text = maxAmmo.ToString();
        isFireReady = true;
    }
}
