using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Lost : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(ReturnToLevel3());
    }

    IEnumerator ReturnToLevel3()
    {
        yield return new WaitForSeconds(5f);

        SceneManager.LoadScene("Level 3");
    }
}
