using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class FindTheSpecific : Minigame
{
    public ObjectCollection[] Collections;

    public float SpawnNumber = 80;

    private RectBounds bounds;

    public string TargetName;

	// Use this for initialization
	void Start ()
    {
        bounds = Camera.main.GetCamWorldBounds(1.0f);

        ObjectCollection col = this.Collections.GetRandom();
        
        List<ClickyObject> objects = col.GetChildren<ClickyObject>().ToList();
        ClickyObject target = objects.GetRandom();
        objects.RemoveAll(a => a.name == target.name);

        this.TargetName = target.name;

        for (int i = 0; i < SpawnNumber - 1; i++)
        {
            CreateObj(objects.GetRandom());
        }

        CreateObj(target);
	}

    private void CreateObj(ClickyObject obj)
    {
        float x = UnityEngine.Random.Range(bounds.LeftBound, bounds.RightBound);
        float y = UnityEngine.Random.Range(bounds.LowerBound, bounds.UpperBound);
        ClickyObject newObj = Instantiate(obj);
        newObj.transform.position = new Vector3(x, y);
    }

    // Update is called once per frame
    void Update () {		
	}

    public override string GetGameName()
    {
        throw new NotImplementedException();
    }

    public override string GetInstructions()
    {
        throw new NotImplementedException();
    }

    public override string InitializeGame()
    {
        throw new NotImplementedException();
    }
}
