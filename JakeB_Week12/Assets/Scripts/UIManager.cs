using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{

    public void sceneChange(int sceneIndex) {
        SceneManager.LoadScene(sceneIndex);
    }
}
