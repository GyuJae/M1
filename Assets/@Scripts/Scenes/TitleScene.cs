using UnityEngine;

public class TitleScene : MonoBehaviour
{
    void Start()
    {
        Managers.Resource.LoadAllAsync<Object>("PreLoad", (key, count, totalCount) =>
        {
            Debug.Log($"{key} {count} / {totalCount}");

            if (count >= totalCount)
            {
                // TODO
            }
        });
    }

    void Update()
    {
        
    }
}
