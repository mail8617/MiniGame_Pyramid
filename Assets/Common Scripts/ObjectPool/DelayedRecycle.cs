using UnityEngine;
using System.Collections;

public class DelayedRecycle : MonoBehaviour {

    [SerializeField]
    private float delay = 1f;

    private void OnEnable()
    {
        StartCoroutine(RecycleRoutine());
    }

    IEnumerator RecycleRoutine()
    {
        yield return new WaitForSeconds(delay);

        gameObject.Recycle();
    }
}
