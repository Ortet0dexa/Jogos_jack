using UnityEngine;

public class CameraSwicthed : MonoBehaviour
{
    public Camera camera1st;
    public Camera camera3rd;

    private bool isFirstPerson = false;

    void Start()
    {
        EnableFirstPerson(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            isFirstPerson = !isFirstPerson;
            EnableFirstPerson(isFirstPerson);
        }
    }

    void EnableFirstPerson(bool enable)
    {
        camera1st.gameObject.SetActive(enable);
        camera3rd.gameObject.SetActive(!enable);

        Debug.Log("Cam 1st active = " + enable);
    }
}
