using UnityEngine;

public class LifeimeController : MonoBehaviour
{
    private float life = 0;
    private float timer = 0;

    public bool ToDestroy = false;

    // Update is called once per frame
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
}
