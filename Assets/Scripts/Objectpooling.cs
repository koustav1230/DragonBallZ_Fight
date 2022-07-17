using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objectpooling : MonoBehaviour
{
    public static Objectpooling instance;
    private List<GameObject> pooledbeam = new List<GameObject>();
    private int NumberOfBeams = 20;
    [SerializeField] GameObject Beam;


    private List<GameObject> pooledbeam2 = new List<GameObject>();
    private int NumberOfBeams2 = 20;
    [SerializeField] GameObject Beam2;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        for(int i = 0; i < NumberOfBeams; i++)
        {
            GameObject TempBeam = Instantiate(Beam);
            TempBeam.SetActive(false);
            pooledbeam.Add(TempBeam);
        }

        for (int i = 0; i < NumberOfBeams2; i++)
        {
            GameObject TempBeam2 = Instantiate(Beam2);
            TempBeam2.SetActive(false);
            pooledbeam2.Add(TempBeam2);
        }
    }

 
   public GameObject GetBeamObject()
    {
        for(int i =0; i < pooledbeam.Count ; i++)
        {
            if (!pooledbeam[i].activeInHierarchy)
            {

                return pooledbeam[i];

            }
        }

        return null;
    }

    public GameObject GetBeamObject2()
    {
        for (int i = 0; i < pooledbeam2.Count; i++)
        {
            if (!pooledbeam2[i].activeInHierarchy)
            {

                return pooledbeam2[i];

            }
        }

        return null;
    }
}


