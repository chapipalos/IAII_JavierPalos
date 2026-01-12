using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using TMPro;

public class TestComandsController : MonoBehaviour
{
    private float life = 0;
    private float timer = 0;

    private bool ToDestroy = false;

    private GameObject m_TestSpawnPrefab;

    public TextMeshProUGUI m_NameText;

    void Update()
    {
        if(ToDestroy)
        {
            timer += Time.deltaTime;
            if (timer >= life)
            {
                Destroy(gameObject);
            }
        }
    }

    public void StartLifetime(float time)
    {
        ToDestroy = true;
        life = time;
    }

    public void SpawnTestObject(int qty, float minSpeed, float maxSpeed)
    {
        var handle = Addressables.LoadAssetAsync<GameObject>("Sphere");
        handle.Completed += h => OnAssetLoaded(h, qty, minSpeed, maxSpeed);
    }

    private void OnAssetLoaded(UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<GameObject> handle, int qty, float minSpeed, float maxSpeed)
    {
        if (handle.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
        {
            m_TestSpawnPrefab = handle.Result;
            for (int i = 0; i < qty; i++)
            {
                GameObject obj = Instantiate(m_TestSpawnPrefab, transform.position, Quaternion.identity);
                SphereController sphereController = obj.GetComponent<SphereController>();
                sphereController.m_MinSpeed = minSpeed;
                sphereController.m_MaxSpeed = maxSpeed;
                sphereController.m_Radius = i + 1;
                sphereController.m_Start = true;
            }
        }
    }

    public void SetName(string name)
    {
        m_NameText.text = name;
    }
}
