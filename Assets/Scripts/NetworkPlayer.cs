using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;
public class NetworkPlayer : MonoBehaviourPun, IPunObservable
{
    [SerializeField] Transform cannonPivot;
    [SerializeField] Transform bulletPosition;
    [SerializeField] float rotationSpeed;
    [SerializeField] Rigidbody bulletPrefab;

    public int maxHealth;
    [SerializeField] float movementSpeed;
    [SerializeField] float maxSpeed;
    public int health;
    Rigidbody rb;
    [SerializeField] Image hp;
    private ExitGames.Client.Photon.Hashtable value = new ExitGames.Client.Photon.Hashtable();

    public float speed = 10.0f;
    [SerializeField] TextMeshProUGUI playername;
    [SerializeField] GameObject target;
    int damage = 10;
    bool isDead = false;

   
    // Start is called before the first frame updat
    void Start()
    {
        if (photonView.IsMine)
        {
            health = maxHealth;
            rb = GetComponent<Rigidbody>();
            //PhotonNetwork.LocalPlayer.TagObject = this;
            photonView.RPC("Updatename", RpcTarget.All);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine &&isDead ==false)
        {
          

            //Movement
            Vector2 input;
            input.x = Input.GetAxis("Horizontal");
            input.y = Input.GetAxis("Vertical");
            Vector3 currentVelocity = rb.velocity;
            currentVelocity.x = Mathf.Clamp(currentVelocity.x + (input.x * movementSpeed * Time.deltaTime), -maxSpeed, maxSpeed);
            currentVelocity.z = Mathf.Clamp(currentVelocity.z + (input.y * movementSpeed * Time.deltaTime), -maxSpeed, maxSpeed);

            rb.velocity = currentVelocity;

            //Shooting

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                cannonPivot.transform.RotateAround(target.transform.position, Vector3.up, rotationSpeed * Time.deltaTime);


                GetComponent<PhotonView>().RPC("Shoot", RpcTarget.All, 1);
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                cannonPivot.transform.RotateAround(target.transform.position, Vector3.up, -rotationSpeed * Time.deltaTime);

                photonView.RPC("Shoot", RpcTarget.All, -1);
            }

            if (health == 0)
            {
                isDead = true;
                GamePlayUI.Instance.DisplayGameOver();

            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("bullet"))
        {
            photonView.RPC("updateHealth", RpcTarget.All,damage);
        }
    }

    [PunRPC]
    void Shoot(int x)
    {
        if (x > 0)
        {
            //Shoot Right
            shootBullet();
        }
        else if (x < 0)
        {
            //Shoot Left
            shootBullet();
        }

    }


    [PunRPC]
    private void shootBullet()
    {

        Rigidbody clone;
        clone = Instantiate(bulletPrefab, bulletPosition.position, bulletPosition.rotation);
        clone.velocity = bulletPosition.transform.forward * speed;
        Destroy(clone.gameObject,3);
    }

    [PunRPC ]

    private void updateHealth(int Getdamage)
    {
        health = Mathf.Max(health - Getdamage, 0);
        NetworkManager.Instance.Log(health.ToString());
        hp.fillAmount = health * 1.0f / maxHealth;
      
    }
 
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(health);
            stream.SendNext(cannonPivot.position);
            stream.SendNext(cannonPivot.rotation);
        }
        else
        {
            health = (int)stream.ReceiveNext();
            cannonPivot.position =(Vector3) stream.ReceiveNext();
            cannonPivot.rotation = (Quaternion)stream.ReceiveNext();
        }
    }

    [PunRPC]
     void Updatename()
    {
        playername.text = photonView.Owner.NickName;
    }
}
