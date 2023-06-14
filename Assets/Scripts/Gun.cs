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

    public GameObject door;

    public GameObject bulletCase;

    Queue<GameObject> poolQueue = new Queue<GameObject>();

    Transform bulletCaseHolder;

    // Start is called before the first frame update
    void Start()
    {
        beam = muzzle.GetComponentInChildren<LineRenderer>();
        isFireReady = true;
        audio = muzzle.GetComponent<AudioSource>();
        bulletCaseHolder = new GameObject().GetComponent<Transform>();
        Init(10);
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
            //Scope.SetActive(true);
            //Sight.SetActive(false);
            //transform.GetChild(5).gameObject.SetActive(false);
        }
        else
        {
            transform.position = originalPos.position;
            //Scope.SetActive(false);
            //Sight.SetActive(true);
            //transform.GetChild(5).gameObject.SetActive(true);
        }

    }

    void Reload()
    {
        //1.26초
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

        //정직하게 머즐에서 나가지 말고, 나가는거 자체는 카메라에서 해야 안이상함.
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 100))
        //if(Physics.Raycast(muzzle.transform.position, muzzle.transform.forward, out hit, 100))
        {
            beam.SetPosition(1, hit.point);
            if (hit.collider.tag == "Enemy")
            {
                Destroy(hit.collider.gameObject);
                door.GetComponent<MeshRenderer>().enabled = false;
                door.GetComponent<BoxCollider>().isTrigger = true;
            }
                

            GameObject temp = Instantiate(bulletHole, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
            Destroy(temp, 3f);

            GameObject bulletCaseTemp = GetObject();
            bulletCaseTemp.transform.position = transform.position;
            StartCoroutine(ReturnObject(bulletCaseTemp));
            
        }
        else
        {
            beam.SetPosition(1, muzzle.transform.position + muzzle.transform.forward * 100);
        }
        isFireReady = false;
        StartCoroutine("FireDelayOn");
    }

    void Init(int num)
    {
        for (int i = 0; i < num; i++)
        {
            poolQueue.Enqueue(CreateBulletCase());
        }
    }

    GameObject CreateBulletCase()
    {
        var obj = Instantiate(bulletCase);
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(bulletCaseHolder);
        return obj;
    }

    public GameObject GetObject()
    {
        if (poolQueue.Count > 0)
        {
            var obj = poolQueue.Dequeue();
            obj.gameObject.SetActive(true);
            return obj;
        }
        else
        {
            var obj = CreateBulletCase();
            obj.gameObject.SetActive(true);
            return obj;
        }
    }

    IEnumerator ReturnObject(GameObject obj)
    {
        yield return new WaitForSeconds(3f);
        obj.gameObject.SetActive(false);
        poolQueue.Enqueue(obj);
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
