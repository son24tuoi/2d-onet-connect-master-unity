using UnityEngine;

[CreateAssetMenu(fileName = "LevelManagerProfileSO", menuName = "Scriptable Object/Level Manager Profile")]
public class LevelManagerProfileSO : ScriptableObject
{
    public LevelProfileSO[] levelProfiles;

    public LevelProfileSO[] bonusLevelProfiles;

    public int MaxLevelIndex => levelProfiles.Length - 1;

    public LevelProfileSO GetLevelProfileSO(int index)
    {
        index = Mathf.Clamp(index, 0, levelProfiles.Length - 1);

        return levelProfiles[index];
    }

    public LevelProfileSO GetBonusLevelProfileSO()
    {
        if (Random.Range(0, 2) == 0)
        {
            return levelProfiles[Random.Range(0, levelProfiles.Length - 1)];
        }
        else
        {
            return bonusLevelProfiles[Random.Range(0, bonusLevelProfiles.Length - 1)];
        }
    }
}