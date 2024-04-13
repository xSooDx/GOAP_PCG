using PCC.ContentRepresentation.Features;
using PCC.ContentRepresentation.Sample;
using PCC.CurationMethod;
using PCC.CurationMethod.Binary;
using PCC.Utility.Range;
using System.Collections.Generic;
using UnityEngine;

public class PCPCGManager : MonoBehaviour
{
    public const string BRANCH_FEATURE = "BranhingFactor";
    public const string ENEMY_DENSITY_FEATURE = "EnemyDensity";
    public static PCPCGManager Instance { get; private set; }
    public ICurationMethod Curator;
    Sample currentSample;
    int sampleCount;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
        List<Feature> features = new List<Feature>
        {
            new Feature(BRANCH_FEATURE, new FloatRange(0f, 1f)),
            new Feature(ENEMY_DENSITY_FEATURE, new FloatRange(0f, 1f))
        };
        Curator =  new BRSCurator(features);
        sampleCount = 0;
    }

    public Sample GetSample()
    {
        SampleGenerationMethod method;
        if(sampleCount < 2 || sampleCount % 5 == 0)
        {
            method = SampleGenerationMethod.RANDOM;
        }
        else
        {
            method = SampleGenerationMethod.RANDOM_FROM_KNOWNS;
        }
        currentSample = Curator.GenerateSamples(1, method)[0];

        sampleCount++; 
        return currentSample;
    }

    public void RecordSample(bool val)
    {
        Curator.RecordSample(currentSample, val ? 1 : -1);
    }
}
