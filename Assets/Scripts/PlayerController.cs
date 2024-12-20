using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class PlayerController : NetworkBehaviour
{
    [SerializeField] private float speed = 3f;
    private Camera m_Camera;
    private Vector3 _mouseInput = Vector3.zero;
    private Bullet shooting;
    private int Heart = 4;
    private int Laugh = 3;
    private int MiddleFinger = 6;

   
    public float deSpawnTime = 3;
    public GameObject _arrowPrefab;
    public GameObject a_SpawnPrefab;
    private bool emoji_spawned = false;
    private void Initialize()
    {
        m_Camera = Camera.main;
    }

    public override void OnNetworkSpawn()
    {
        
        shooting = GetComponent<Bullet>();

        Initialize();
        if (!IsOwner) return;
        CreateAsteroidSpawnerServerRpc();
    }
    private void Update()
    {
        if (!IsOwner || !Application.isFocused) return;
       
        _mouseInput.x = Input.mousePosition.x;
        _mouseInput.y = Input.mousePosition.y;
        _mouseInput.z = m_Camera.nearClipPlane;
        Vector2 mousePositionScreen = Input.mousePosition;
        Vector3 mouseWorldCoordinates = m_Camera.ScreenToWorldPoint(mousePositionScreen);
        mouseWorldCoordinates.z = 0f;

        if (Input.GetKey(KeyCode.W))
        {
            transform.position = Vector3.MoveTowards(transform.position,
                mouseWorldCoordinates, Time.deltaTime * speed);
        }

       
        if (mouseWorldCoordinates != transform.position)
        {
            Vector3 targetDirection = mouseWorldCoordinates - transform.position;
            targetDirection.z = 0f;
            transform.up = targetDirection;
        }

        if (Input.GetMouseButtonDown(0))
        {
            shooting.ShootServerRpc();
        }

        if (IsOwner && Input.GetKey(KeyCode.E))
        {
            UseEmoji();
            InvokeRepeating("DeSpawnEmoji", 0f, deSpawnTime);
        }
    }
    public void UseEmoji()
    {
      
        EmojiServerRpc();

    }

    [ServerRpc]
    private void EmojiServerRpc()
    {
       
        if (!emoji_spawned)
        {
            GameObject emoji = gameObject.transform.GetChild(Laugh).gameObject;
            emoji.SetActive(true);
            emoji_spawned = true;

          
            EmojiClientRpc(Laugh);

          
            Invoke(nameof(DeactivateEmoji), deSpawnTime);
        }
    }

    [ClientRpc]
    private void EmojiClientRpc(int emojiIndex)
    {
        
        GameObject emoji = transform.GetChild(emojiIndex).gameObject;
        emoji.SetActive(true);

    }
    private void DeactivateEmoji()
    {
       
        GameObject emoji = gameObject.transform.GetChild(Laugh).gameObject;
        emoji.SetActive(false);
        emoji_spawned = false;

        EmojiDeactivationClientRpc(Laugh);
    }

    [ClientRpc]
    private void EmojiDeactivationClientRpc(int emojiIndex)
    {
      
        GameObject emoji = transform.GetChild(emojiIndex).gameObject;
        emoji.SetActive(false);
    }

    [ServerRpc]
    private void CreateAsteroidSpawnerServerRpc()
    {
        if (!IsOwner) return;
        GameObject spawn = Instantiate(a_SpawnPrefab, Vector3.zero, Quaternion.identity);

        spawn.GetComponent<NetworkObject>().Spawn();
        Debug.Log("Spawned Asteroid");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Asteroids")
        {
            Debug.Log("Ouch I got Hit");
        }
    }
}
