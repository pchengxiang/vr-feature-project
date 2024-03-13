using QuickType;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardGenerator : MonoBehaviour 
{
    public static CardGenerator instance;
    public GameObject prefab;
    public DuelDiskEntity disk;
    [SerializeField]
    List<Texture> CardTextures= new List<Texture>();
    int index = 0;
    private void Awake()
    {
        if (instance == null) instance = this;
    }

    public CardEntity Spawn(string name,Card model)
    {
        if (prefab)
        {
            var card = Instantiate(prefab, disk.positions[index]);
            var ui = card.GetComponent<CardEntity>();
            ui.Set(model);
            var texture = CardTextures.Find((texture) => texture.name == model.Commands[0].Prefab);
            if (texture != null)
            {
                ui.Graph.GetComponent<RawImage>().texture = texture;
            }
            index = (index + 1) % disk.positions.Length;
            return ui;
        }

        Debug.LogErrorFormat($"Please input a Card UI.");
        return null;
    }

    public CardEntity Spawn(string name, Card model, Transform position)
    {
        if (prefab)
        {
            var card = Instantiate(prefab, position);
            var ui = card.GetComponent<CardEntity>();
            ui.Set(model);
            var texture = CardTextures.Find((texture) => texture.name == model.Commands[0].Prefab);
            if (texture != null)
            {
                ui.Graph.GetComponent<RawImage>().texture = texture;
            }
            return ui;
        }

        Debug.LogErrorFormat($"Please input a Card UI.");
        return null;
    }
}
