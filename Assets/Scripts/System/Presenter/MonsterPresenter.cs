using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPresenter
{
    private CardEntity ui;
    private Monster model;
    private MonsterPresenter() { }
    public MonsterPresenter(CardEntity ui,Monster model)
    {
        this.ui = ui;
        this.model = model;
    }
}
