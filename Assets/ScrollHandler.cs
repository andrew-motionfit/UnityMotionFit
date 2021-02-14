using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollHandler : MonoBehaviour
{
    [SerializeField]
    private GameObject parent,prefab,selectedParet,selectedprefab;
    public List<string> buttonname = new List<string>();
    public List<string> selectedExercise = new List<string>();
    public List<GameObject> createExercise = new List<GameObject>();
    public List<GameObject> selectedExerciseobj = new List<GameObject>();
    public List<string> AnimatorParameters = new List<string>();
    public Animator ani;
    public Image circle;
    public Button closebutton;
    private void Start()
    {
       
        for(int i = 0; i < buttonname.Count; i++)
        {
            GameObject clone = Instantiate(prefab,parent.transform.position,Quaternion.identity);
            clone.transform.SetParent(parent.transform);
            clone.transform.localScale = Vector3.one;
            clone.GetComponentInChildren<TextMeshProUGUI>().text = buttonname[i];
            clone.gameObject.SetActive(true);
            createExercise.Add(clone);
            clone.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(delegate { addExercise(clone.GetComponentInChildren<TextMeshProUGUI>().text); });
        }

        
    }

    public void closeAnimation()
    {
        for (int y = 0; y < AnimatorParameters.Count; y++)
        {
            ani.SetBool(AnimatorParameters[y], false);
        }
        closebutton.gameObject.SetActive(false);
        circle.gameObject.SetActive(false);
    }
    public void addExercise(string a)
    {
        for (int y = 0; y < AnimatorParameters.Count; y++)
        {
            ani.SetBool(AnimatorParameters[y], false);
        }

            string tempparameter = "";
        for(int x = 0; x< AnimatorParameters.Count; x++)
        {
            if(a == AnimatorParameters[x])
            {
                tempparameter = a;
            }
        }


        for (int i = 0; i < createExercise.Count; i++)
        {

            if (a == createExercise[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text)
            {
                if(createExercise[i].transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>().color.a <= 0)
                {
                    selectedExercise.Add(a);
                    
                    GameObject clone = Instantiate(selectedprefab, selectedParet.transform.position, Quaternion.identity);
                    clone.transform.SetParent(selectedParet.transform);
                    clone.transform.localScale = Vector3.one;
                    clone.GetComponentInChildren<TextMeshProUGUI>().text = a;
                    clone.gameObject.SetActive(true);
                    selectedExerciseobj.Add(clone);
                    closebutton.gameObject.SetActive(true);
                    circle.gameObject.SetActive(true);
                    if (tempparameter.Length > 1)
                    {
                        ani.SetBool(tempparameter, true);
                        tempparameter = "";
                    }
                }
                else
                {
                    selectedExercise.Remove(a);
                    for (int z = 0; z < selectedExerciseobj.Count; z++)
                    {
                       
                        if (a == selectedExerciseobj[z].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text)
                        {
                            GameObject temp = selectedExerciseobj[z];
                            Destroy(temp);
                            selectedExerciseobj.Remove(selectedExerciseobj[z]);
                            closebutton.gameObject.SetActive(false);
                            circle.gameObject.SetActive(false);
                            if (tempparameter.Length > 1)
                            {
                                ani.SetBool(tempparameter, false);
                                tempparameter = "";
                            }

                        }
                    }
                   
                }
            }
        }
      
    }
}
