using UnityEngine;

class UIManager : MonoBehaviour
{
  private static UIManager m_instance;

  private GameObject activeUI;
  public bool isInputCaptured = false;

  public static UIManager GetInstance()
  {
    return m_instance;
  }

  public void SetUI(GameObject UIobject)
  {
    if (activeUI == UIobject)
    {
      return;
    }
    if (activeUI != null)
    {
      activeUI.SetActive(false);
    }

    activeUI = UIobject;
  }

  public static Vector3 GetMousePositionWorldSpace()
  {
    Vector3 mousePosition = Input.mousePosition;
    mousePosition.z = -Camera.main.transform.position.z;
    Vector3 mousePoint = Camera.main.ScreenToWorldPoint(mousePosition);
    return mousePoint;
  }

  void Awake()
  {
    if (m_instance != null)
    {
      Debug.LogError("Attempt to create second singleton of type UIManager");
      return;
    }

    m_instance = this;
  }

  void Update()
  {
    if (Input.GetKeyDown(KeyCode.Escape))
    {
      SetUI(null);
    }
  }
}