using UnityEngine;

public class TutorialScript : MonoBehaviour
{
    public int tutorialLevel;
    public GameObject[] instructionBoxes;
    public GameObject firstStar;
    public bool isFirstTime;
    public bool isInStarTutorial;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(isFirstTime)
        {
            tutorialLevel = 0;
            UpdateTutorialLevel(tutorialLevel);    
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(tutorialLevel == instructionBoxes.Length)
        {
            isFirstTime = false;
        }

        if(firstStar != null && isFirstTime)
        {
            Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
            Bounds bounds = firstStar.GetComponent<SpriteRenderer>().bounds;

            if (!GeometryUtility.TestPlanesAABB(planes, bounds))
            {
                Debug.Log("Sprite completamente fora da visão da câmera!");
            }
            else
            {
                UpdateTutorialLevel(6);
                print("ACHOOOOOOOOOOOOOOOOOOOOOOOO");
            }
        }

        if(!isFirstTime)
        {
            foreach (var item in instructionBoxes)
            {
                item.SetActive(false);
            }
        }
    }

    public void UpdateTutorialLevel(int level)
    {
        if(isFirstTime)
        {
            if(level == tutorialLevel || level == 3)
            { 
                if(level == 3)
                {
                    tutorialLevel = 3;
                    isInStarTutorial = true;
                }

                foreach (var item in instructionBoxes)
                {
                    item.SetActive(false);
                }
                instructionBoxes[tutorialLevel].SetActive(true);

                tutorialLevel ++;
            }
        }

        if(level == 100)
        {
            isFirstTime = false;
        }
    }

    bool IsFullyInsideFrustum(Plane[] planes, Bounds bounds)
    {
        Vector3[] corners = {
            bounds.min,
            bounds.max,
            new Vector3(bounds.min.x, bounds.min.y, bounds.max.z),
            new Vector3(bounds.min.x, bounds.max.y, bounds.min.z),
            new Vector3(bounds.max.x, bounds.min.y, bounds.min.z),
            new Vector3(bounds.max.x, bounds.max.y, bounds.min.z),
            new Vector3(bounds.min.x, bounds.max.y, bounds.max.z),
            new Vector3(bounds.max.x, bounds.min.y, bounds.max.z)
        };

        foreach (Plane plane in planes)
        {
            bool allOutside = true;
            foreach (Vector3 corner in corners)
            {
                if (plane.GetDistanceToPoint(corner) > 0)
                {
                    allOutside = false;
                    break;
                }
            }
            if (allOutside)
                return false;
        }

        return true;
    }

}
