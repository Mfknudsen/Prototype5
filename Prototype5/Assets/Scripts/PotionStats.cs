using System.Reflection;
using UnityEngine;

public class PotionStats : MonoBehaviour
{
    public float strength = 0.0f;
    public float luck = 0.0f;
    public float amnesia = 0.0f;
    
    public void ResetAllToDefault()
    {
        strength = 0.0f;
        luck = 0.0f;
        amnesia = 0.0f;
    }

}
