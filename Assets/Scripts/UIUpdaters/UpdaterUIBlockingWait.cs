using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdaterUIBlockingWait : MonoBehaviour
{
    [SerializeField] private float secondsToBlockUI = 3f;
    
    private void OnEnable()
    {
        // start blocking wait countdown timer
        StartCoroutine(BlockCountdownCO());
    }
    private void OnDisable() {
        StopAllCoroutines();
    }
    private void OnDestroy() {
        StopAllCoroutines();
    }
    private IEnumerator BlockCountdownCO() {
        yield return new WaitForSecondsRealtime(secondsToBlockUI);
        gameObject.SetActive(false);
    }
}
