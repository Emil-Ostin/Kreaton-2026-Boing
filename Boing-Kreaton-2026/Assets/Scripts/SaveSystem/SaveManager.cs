using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public List<SavePoint> savePoints;

    public SavePoint currentSavePoint;


    public void SavePointCleaner()
    {
        int currentSave = savePoints.IndexOf(currentSavePoint);

        if (currentSave == 0) return;


        for (int i = 0; i < currentSave; i++)
        {
            savePoints[i] = null;
        }
    }
}
