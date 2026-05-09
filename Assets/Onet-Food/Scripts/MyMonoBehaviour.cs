using UnityEngine;
using UnityEngine.AI;

public class MyMonoBehaviour : MonoBehaviour
{
    protected DataManager m_dataManager;

    public DataManager DataManager
    {
        get
        {
            if (ReferenceEquals(m_dataManager, null))
            {
                m_dataManager = DataManager.Instance;
            }
            return m_dataManager;
        }
    }

    protected AdManager m_adManager;

    public AdManager AdManager
    {
        get
        {
            if (ReferenceEquals(m_adManager, null))
            {
                m_adManager = AdManager.Instance;
            }
            return m_adManager;
        }
    }

    protected AudioManager m_audioManager;

    public AudioManager AudioManager
    {
        get
        {
            if (ReferenceEquals(m_audioManager, null))
            {
                m_audioManager = AudioManager.Instance;
            }
            return m_audioManager;
        }
    }

    protected VibrationManager m_vibrationManager;

    public VibrationManager VibrationManager
    {
        get
        {
            if (ReferenceEquals(m_vibrationManager, null))
            {
                m_vibrationManager = VibrationManager.Instance;
            }
            return m_vibrationManager;
        }
    }

    protected EnvironmentController m_environmentController;

    public EnvironmentController EnvironmentController
    {
        get
        {
            if (ReferenceEquals(m_environmentController, null))
            {
                m_environmentController = EnvironmentController.Instance;
            }
            return m_environmentController;
        }
    }
}