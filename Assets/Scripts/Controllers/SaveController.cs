using UnityEngine;

public class SaveController : MonoBehaviour
{
    #region FIELDS

    public Data DataPlayer;

    #endregion

    #region METODS

    public void Save()
    {
        string jsonDataString = JsonUtility.ToJson(DataPlayer, true);
        PlayerPrefs.SetString(Constants.DATA, jsonDataString);
    }

    public void Load()
    {
        if (PlayerPrefs.HasKey(Constants.DATA))
        {
            string loadedString = PlayerPrefs.GetString(Constants.DATA);
            DataPlayer = JsonUtility.FromJson<Data>(loadedString);
        }
        else
        {
            DataPlayer = new Data();
        }
    }

    [ContextMenu("Reset save")]
    public void ResetSave()
    {
        PlayerPrefs.DeleteAll();
    }

    #endregion
}
