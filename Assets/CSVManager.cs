using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;

public class CSVManager : MonoBehaviour
{
	public TextAsset csvFile; // Reference of CSV file
	//public InputField rollNoInputField;// Reference of rollno input field
	//public InputField nameInputField; // Reference of name input filed
	//public Text contentArea; // Reference of contentArea where records are displayed

	private char lineSeperater = '\n'; // It defines line seperate character
	private char fieldSeperator = ','; // It defines field seperate chracter
	private int indexer = 0;
	public AccelerometerObjectControl AOC;
	public int rdIndexX, rdIndexY, rdIndexZ;
	void Start()
	{

		print("Reading will start in 5 Sec wait");
	    Invoke("readData",5);
		//InvokeRepeating("waitforCall",2,2);

	}

	// Read data from CSV file
	private void readData()
	{
		string[] records = csvFile.text.Split("\n"[0]);
		for(int i = 0; i < records.Length; i++)
		{
			
		string[] temprecords	=	records[i].Split(","[0]);
			print("Adding data to CSV");
			Vector3 FliteredValues = AOC.filterPos(new Vector3(float.Parse(temprecords[rdIndexX]),float.Parse(temprecords[rdIndexY]),float.Parse(temprecords[rdIndexZ])));
			addData(FliteredValues.x.ToString(),FliteredValues.y.ToString(),FliteredValues.z.ToString(),"1");
          
		}
	}

	public void waitforCall()
    {
		addData(indexer.ToString(),"1a","1b","1c");
		indexer += 1;
    }
	// Add data to CSV file
	public void addData(string Xmg,string Ymg,string zmg, string rep)
	{
		// Following line adds data to CSV file
		File.AppendAllText(getPath() + "/Resources/ExerciseData.csv",lineSeperater+ Xmg + fieldSeperator + Ymg + fieldSeperator+ zmg + fieldSeperator+ rep);
		// Following lines refresh the edotor and print data
	//	rollNoInputField.text = "";
	//	nameInputField.text = "";
	//	contentArea.text = "";
#if UNITY_EDITOR
		UnityEditor.AssetDatabase.Refresh();
#endif
		readData();
	}

	// Get path for given CSV file
	private static string getPath()
	{
#if UNITY_EDITOR
		return Application.dataPath;
#elif UNITY_ANDROID
		return Application.persistentDataPath;// +fileName;
#elif UNITY_IPHONE
		return GetiPhoneDocumentsPath();// +"/"+fileName;
#else
		return Application.dataPath;// +"/"+ fileName;
#endif
	}
	// Get the path in iOS device
	private static string GetiPhoneDocumentsPath()
	{
		string path = Application.dataPath.Substring(0, Application.dataPath.Length - 5);
		path = path.Substring(0, path.LastIndexOf('/'));
		return path + "/Documents";
	}
}
