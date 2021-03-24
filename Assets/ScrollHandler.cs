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
    public GameObject startworkout;
    [Header("POPUP")]
    public GameObject PopupPanel1;
    public GameObject PopupPanel2;
    public TextMeshProUGUI Poptitle;
    public Transform PopupList;
    public GameObject PopClone;
    private List<GameObject> tempSetList = new List<GameObject>();
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
            clone.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(delegate { preaddExercise(clone.GetComponentInChildren<TextMeshProUGUI>().text); });
        }

        
    }
    public void startWorkout()
    {
        string tempstring = "";
            for (int i = 0; i < selectedExercise.Count; i++)
           {
            for (int z = 0; z < AnimatorParameters.Count; z++)
            {
                if(selectedExercise[i] == AnimatorParameters[z])
                {
                    if (tempstring.Length > 0)
                    {
                        tempstring += "," + z;
                    }
                    else
                    {
                        tempstring += z;
                    }
                }
            }
           }
        LocalDatabase.instance.saveWorkout(tempstring);
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

    public void preaddExercise(string a)
    {
        for (int i = 0; i < createExercise.Count; i++)
        {
            if (a == createExercise[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text)
            {
                if (createExercise[i].transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>().color.a <= 0)
                {

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
                            if (a.Length > 1)
                            {
                                ani.SetBool(a, false);
                                a = "";
                            }

                        }
                    }
                    return;
                }
            }
        }
        Poptitle.text = a;
        PopupPanel1.SetActive(true);

    }

    public void enterSet(TMP_InputField settxt)
    {
        if (settxt.text.Length == 0)
            return;

       

        int tempset = int.Parse(settxt.text);
       
        for (int i = 0; i < tempset; i++)
        {
           GameObject clone = Instantiate(PopClone,Vector3.zero,Quaternion.identity);
            clone.transform.SetParent(PopupList);
            clone.transform.localScale = Vector3.one;
            clone.SetActive(true);
            int temp = i + 1;
            clone.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text =temp.ToString();
            tempSetList.Add(clone);
        }
       
        PopupPanel2.SetActive(true);
    }

    public void postaddExercise()
    {
        foreach(GameObject objs in tempSetList)
        {
            Destroy(objs);
        }
        print(Poptitle.text);
        addExercise(Poptitle.text);
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
        }
         
        if(selectedExercise.Count > 0)
        {
            startworkout.gameObject.SetActive(true);
        }
        else
        {
            startworkout.gameObject.SetActive(false);
        }
      
    }
}
