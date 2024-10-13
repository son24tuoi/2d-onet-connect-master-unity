using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "LevelProfileSO", menuName = "Scriptable Object/Level Profile")]
public class LevelProfileSO : ScriptableObject
{
    public TextAsset textAsset;

    public StartingCard[] startingCards;

    public AlignmentData alignmentData;

    public TimeSystem timeSystem;

    private void TotalStartingCards()
    {
        int total = 0;
        for (int i = 0; i < startingCards.Length; i++)
        {
            total += startingCards[i].amount;
        }

        Debug.Log("total starting blocks: " + total);
    }

    public void AddStartingBlockToList(List<int> ids)
    {
        for (int i = 0; i < startingCards.Length; i++)
        {
            for (int j = 0; j < startingCards[i].amount; j++)
            {
                ids.Add(startingCards[i].id);
            }
        }
    }

    public List<int> GetStartingCards()
    {
        List<int> idCards = new List<int>();

        for (int i = 0; i < startingCards.Length; i++)
        {
            for (int j = 0; j < startingCards[i].amount; j++)
            {
                idCards.Add(startingCards[i].id);
            }
        }

        return idCards;
    }

    public AlignmentType GetAlignmentType(int index) => alignmentData.GetAlignmentType(index);











#if UNITY_EDITOR
    [CustomEditor(typeof(LevelProfileSO))]
    public class LevelProfileSO_Inspector : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUILayout.Space(20);

            LevelProfileSO target = (LevelProfileSO)base.target;

            if (GUILayout.Button("Total Starting Cards"))
            {
                target.TotalStartingCards();
            }

            if (GUILayout.Button("Total Node Views"))
            {
                if (target.textAsset == null)
                {
                    Debug.LogWarning("LEVELPROFILESO Text Asset is NULL");
                }
                else
                {
                    int count = MapData.GetCount(target.textAsset);
                    Debug.Log("Total node Views: " + count);
                }
            }

            // if (GUILayout.Button("Minus Id 10"))
            // {
            //     for (int i = 0; i < target.startingCards.Length; i++)
            //     {
            //         target.startingCards[i].id -= 10;
            //     }
            //     EditorUtility.SetDirty(this);
            // }

            if (GUILayout.Button("Create id starting cards increase"))
            {
                int count = MapData.GetCount(target.textAsset) / 2;
                List<StartingCard> startingCards = new List<StartingCard>();

                for (int i = 0; i < count; i++)
                {
                    StartingCard startingCard = new StartingCard
                    {
                        id = i,
                        amount = 2,
                    };
                    startingCards.Add(startingCard);
                }

                target.startingCards = startingCards.ToArray();
            }

            if (GUILayout.Button("Create id starting cards decrease"))
            {
                int count = MapData.GetCount(target.textAsset) / 2;
                List<StartingCard> startingCards = new List<StartingCard>();
                int id = 49;

                for (int i = 0; i < count; i++)
                {
                    StartingCard startingCard = new StartingCard
                    {
                        id = id,
                        amount = 2,
                    };
                    startingCards.Add(startingCard);
                    id--;
                }

                target.startingCards = startingCards.ToArray();
            }
        }
    }
#endif
}
