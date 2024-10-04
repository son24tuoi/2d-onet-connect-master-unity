using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardsProfileSO", menuName = "Scriptable Object/Cards Profile")]
public class CardsProfileSO : ScriptableObject
{
    public List<Sprite> sprites = new List<Sprite>();
    public Sprite defaultBG;
    public Sprite selectBG;

    public Sprite GetSprite(int idx)
    {
        idx = Mathf.Clamp(idx, 0, sprites.Count - 1);
        
        return sprites[idx];
    }
}
