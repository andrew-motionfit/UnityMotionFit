using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;

public class FitCapTest : MonoBehaviour
{
    public string DeviceName = "FitCap1";

    public Text AccelerometerText;
    public Text FitCapStatusText;
    public Button StartStopButton;
    public Button DisconnectButton;

    public GameObject TopPanel;
    public GameObject MiddlePanel;

    public class Characteristic
    {
        public string ServiceUUID;
        public string CharacteristicUUID;
        public bool Found;
    }

    public static List<Characteristic> Characteristics = new List<Characteristic>
    {
        new Characteristic { ServiceUUID = "00000000-CC7A-482A-984A-7F2ED5B3E58F", CharacteristicUUID = "0000E000-8E22-4541-9D4C-21EDAE82ED19", Found = false },
        new Characteristic { ServiceUUID = "00000000-CC7A-482A-984A-7F2ED5B3E58F", CharacteristicUUID = "00000004-8E22-4541-9D4C-21EDAE82ED19", Found = false },
        new Characteristic { ServiceUUID = "0000000f-cc7a-482a-984a-7f2ed5b3e58f", CharacteristicUUID = "00000002-8e22-4541-9d4c-21edae82ed19", Found = false },
        new Characteristic { ServiceUUID = "0000180F-0000-1000-8000-00805f9b34fb", CharacteristicUUID = "00002a19-0000-1000-8000-00805f9b34fb", Found = false },
    };

    public Characteristic SubscribeAccelerometer = Characteristics[0];
    public Characteristic ReadAccelerometer = Characteristics[1];
    public Characteristic ConfigureIMU = Characteristics[2];

    public bool AllCharacteristicsFound { get { return !(Characteristics.Where(c => c.Found == false).Any()); } }
    public Characteristic GetCharacteristic(string serviceUUID, string characteristicsUUID)
    {
        return Characteristics.Where(c => IsEqual(serviceUUID, c.ServiceUUID) && IsEqual(characteristicsUUID, c.CharacteristicUUID)).FirstOrDefault();
    }

    private byte[] ConfigureIMU_Bytes = new byte[] { 0x01, 0x10, 0x00, 0x00, 0x00 }; // this is 16 mS 

    enum States
    {
        None,
        Scan,
        Connect,
        ConfigureAccelerometer,
        SubscribeToAccelerometer,
        SubscribingToAccelerometerTimeout,
        Disconnect,
        Disconnecting,
    }

    private bool _connected = false;
    private float _timeout = 0f;
    private States _state = States.None;
    private string _deviceAddress;

    private bool DisplayData = false;
    private bool connectdisconnect = false;

    // path of the file
    private string path = "";

       
    public void OnButtonPress_DisconnectButton()
    {
        if (connectdisconnect == false)
        {
            connectdisconnect = true;
            Text txt = DisconnectButton.GetComponentInChildren<Text>();
            txt.text = "Disconnect";
        }
        else
        {
            connectdisconnect = false;
            Text txt = DisconnectButton.GetComponentInChildren<Text>();
            txt.text = "Connect";
        }
    }

    public void OnButtonPress_StartStopButton()
    {
        if (DisplayData == false)
        {
            DisplayData = true;
            Text txt = StartStopButton.GetComponentInChildren<Text>();
            txt.text = "Stop";

            //string startstring = System.DateTime.Now.ToString();
            System.DateTime theTime = System.DateTime.Now;
            string startstring = theTime.Year + "_" + theTime.Month + "_" + theTime.Day + "_" + theTime.Hour + "_" + theTime.Minute + "_" + theTime.Second;
            //path = Application.dataPath + "/log_" + startstring + ".csv";
            path = Application.persistentDataPath + "/log_" + startstring + ".csv";

            FitCapStatusMessages = path;

            // create file if it doesn't exist
            if (!File.Exists(path))
            {
                // write data to file
                string starttimetag = "Session date: " + theTime.Year + "-" + theTime.Month + "-" + theTime.Day + "-" + theTime.Hour + ":" + theTime.Minute + ":" + theTime.Second + "\n";
                File.WriteAllText(path, starttimetag);
            }
        }
        else
        {
            DisplayData = false;
            Text txt = StartStopButton.GetComponentInChildren<Text>();
            txt.text = "Start";

            path = "";
        }
        // Debug.Log("Button clicked " + DisplayData);
    }

    string FitCapStatusMessages
    {
        set
        {
            Debug.Log(value);

            if (!string.IsNullOrEmpty(value))
                BluetoothLEHardwareInterface.Log(value);
            if (FitCapStatusText != null)
            {
                FitCapStatusText.text = value;
            }
        }
    }

    void Reset()
    {
        _connected = false;
        _timeout = 0f;
        _state = States.None;
        _deviceAddress = null;

        TopPanel.SetActive(true);
        MiddlePanel.SetActive(true);

        FitCapStatusMessages = "Reset";
    }

    void SetState(States newState, float timeout)
    {
        _state = newState;
        _timeout = timeout;
    }

    void StartProcess()
    {
        FitCapStatusMessages = "StartProcess";
        StartStopButton.onClick.AddListener(OnButtonPress_StartStopButton);
        DisconnectButton.onClick.AddListener(OnButtonPress_DisconnectButton);

        Reset();
        BluetoothLEHardwareInterface.Initialize(true, false, () =>
        {

            SetState(States.Scan, 0.1f);

        }, (error) =>
        {

            BluetoothLEHardwareInterface.Log("Error: " + error);
        });
    }

    private void OnCharacteristicNotification(string deviceAddress, string characteristric, byte[] rcvd_data)
    {
        if (connectdisconnect == false)
        {
            SetState(States.Disconnect, 0.5f);
        }
        else
        {
            _state = States.None;
            MiddlePanel.SetActive(true);
        }

        var sBytes = BitConverter.ToString(rcvd_data);


            //        Custom_App_Context.Acc_gyro_mag_char_value[0] = timestamp;
            //        Custom_App_Context.Acc_gyro_mag_char_value[1] = (timestamp >> 8);

            int timestamp = (rcvd_data[0] | (rcvd_data[1] << 8));

            int accAxesXint = (rcvd_data[2] | (rcvd_data[3] << 8));
            if (accAxesXint > 32767) { accAxesXint -= 65536; }

            int accAxesYint = (rcvd_data[4] | (rcvd_data[5] << 8));
            if (accAxesYint > 32767) { accAxesYint -= 65536; }

            int accAxesZint = (rcvd_data[6] | (rcvd_data[7] << 8));
            if (accAxesZint > 32767) { accAxesZint -= 65536; }

            int gyrAxesXint = (rcvd_data[8] | (rcvd_data[9] << 8));
            if (gyrAxesXint > 32767) { gyrAxesXint -= 65536; }

            int gyrAxesYint = (rcvd_data[10] | (rcvd_data[11] << 8));
            if (gyrAxesYint > 32767) { gyrAxesYint -= 65536; }

            int gyrAxesZint = (rcvd_data[12] | (rcvd_data[13] << 8));
            if (gyrAxesZint > 32767) { gyrAxesZint -= 65536; }

            //#define LSM6DSOX_ACC_SENSITIVITY_FS_2G   0.061f  
            float ACCsensitivity = 0.061f;

            //#define LSM6DSOX_ACC_SENSITIVITY_FS_4G   0.122f
            //#define LSM6DSOX_ACC_SENSITIVITY_FS_8G   0.244f
            //#define LSM6DSOX_ACC_SENSITIVITY_FS_16G  0.488f

            //#define LSM6DSOX_GYRO_SENSITIVITY_FS_125DPS    4.375f
            //#define LSM6DSOX_GYRO_SENSITIVITY_FS_250DPS    8.750f
            //#define LSM6DSOX_GYRO_SENSITIVITY_FS_500DPS   17.500f
            //#define LSM6DSOX_GYRO_SENSITIVITY_FS_1000DPS  35.000f
            //#define LSM6DSOX_GYRO_SENSITIVITY_FS_2000DPS  70.000f 
            float GYRsensitivity = 70.000f;

            int accAxesXint32 = (int)((float)((float)accAxesXint * ACCsensitivity));
            int accAxesYint32 = (int)((float)((float)accAxesYint * ACCsensitivity));
            int accAxesZint32 = (int)((float)((float)accAxesZint * ACCsensitivity));

            int gyrAxesXint32 = (int)((float)((float)gyrAxesXint * GYRsensitivity));
            int gyrAxesYint32 = (int)((float)((float)gyrAxesYint * GYRsensitivity));
            int gyrAxesZint32 = (int)((float)((float)gyrAxesZint * GYRsensitivity));

            string stime = timestamp.ToString();

            string display_string = "timestamp: " + timestamp.ToString() + "\n" +
                                    "Acc:\n" +
                                    "X= " + accAxesXint32.ToString() + "\n" +
                                    "Y= " + accAxesYint32.ToString() + "\n" +
                                    "Z= " + accAxesZint32.ToString() + "\n" +
                                    "Gyr:\n" +
                                    "X= " + gyrAxesXint32.ToString() + "\n" +
                                    "Y= " + gyrAxesYint32.ToString() + "\n" +
                                    "Z= " + gyrAxesZint32.ToString();

        if (DisplayData == true)
        {
            AccelerometerText.text = display_string;

            string str = accAxesXint32.ToString() + "," +
                         accAxesYint32.ToString() + "," +
                         accAxesZint32.ToString() + "," +
                         gyrAxesXint32.ToString() + "," +
                         gyrAxesYint32.ToString() + "," +
                         gyrAxesZint32.ToString() + "\n";
            File.AppendAllText(path, str);
        }
        else
        {
            AccelerometerText.text = " ";
        }
        //AccelerometerText.text = "Accelerometer: " + sBytes;
    }


    // Use this for initialization
    void Start()
    {

#if UNITY_ANDROID
        const string perms_activity = "android.permission.ACTIVITY_RECOGNITION";
        if (!Permission.HasUserAuthorizedPermission(perms_activity))
        {
            Permission.RequestUserPermission(perms_activity);
        }

        const string perms_course_location = "android.permission.ACCESS_COARSE_LOCATION";
        if (!Permission.HasUserAuthorizedPermission(perms_course_location))
        {
            Permission.RequestUserPermission(perms_course_location);
        }
#endif
        StartProcess();
    }

    // Update is called once per frame
    void Update()
    {
        if (_timeout > 0f)
        {
            _timeout -= Time.deltaTime;
            if (_timeout <= 0f)
            {
                _timeout = 0f;

                switch (_state)
                {
                    case States.None:
                        break;

                    case States.Scan:
                        FitCapStatusMessages = "Scanning for: " + DeviceName;
                        BluetoothLEHardwareInterface.ScanForPeripheralsWithServices(null, (address, deviceName) =>
                        {

                            FitCapStatusMessages = "Scanning Found: " + deviceName;

                                if (deviceName.Contains(DeviceName))
                                {
                                    FitCapStatusMessages = "Found a FitCap: " + address;

                                    if (connectdisconnect == true)
                                    {
                                        BluetoothLEHardwareInterface.StopScan();

                                        TopPanel.SetActive(true);

                                        // found a device with the name we want
                                        // this example does not deal with finding more than one
                                        _deviceAddress = address;
                                        SetState(States.Connect, 0.5f);
                                    }
                                    else
                                    {
                                        SetState(States.Scan, 0.5f);
                                    }
                                }
                                }, null, true);
                        break;

                    case States.Connect:
                        FitCapStatusMessages = "Connecting to FitCap...";

                        BluetoothLEHardwareInterface.ConnectToPeripheral(_deviceAddress, null, null, (address, serviceUUID, characteristicUUID) =>
                        {
                            FitCapStatusMessages = "Connected to FitCap..." + address;

                            var characteristic = GetCharacteristic(serviceUUID, characteristicUUID);
                            if (characteristic != null)
                            {
                                BluetoothLEHardwareInterface.Log(string.Format("Found {0}, {1}", serviceUUID, characteristicUUID));

                                characteristic.Found = true;

                                if (AllCharacteristicsFound)
                                {
                                    _connected = true;
                                    SetState(States.ConfigureAccelerometer, 3f);
                                }
                            }
                        }, (disconnectAddress) =>
                        {
                            FitCapStatusMessages = "Disconnected from FitCap";
                            Reset();
                            SetState(States.Scan, 1f);
                        });
                        break;

                    case States.ConfigureAccelerometer:
                        FitCapStatusMessages = "Configuring FitCap Accelerometer...";
                        BluetoothLEHardwareInterface.WriteCharacteristic(_deviceAddress, ConfigureIMU.ServiceUUID, ConfigureIMU.CharacteristicUUID, ConfigureIMU_Bytes, ConfigureIMU_Bytes.Length, true, (address) => {
                            FitCapStatusMessages = "Configured FitCap Accelerometer";
                            SetState(States.SubscribeToAccelerometer, 2f);
                        });
                        break;


                    case States.SubscribeToAccelerometer:
                        SetState(States.SubscribingToAccelerometerTimeout, 5f);
                        FitCapStatusMessages = "Subscribing to FitCap Accelerometer...";

                        BluetoothLEHardwareInterface.SubscribeCharacteristicWithDeviceAddress(_deviceAddress, SubscribeAccelerometer.ServiceUUID, SubscribeAccelerometer.CharacteristicUUID, delegate { }, OnCharacteristicNotification);
                        FitCapStatusMessages = "Subscribed to FitCap Accelerometer...";

                        Text txt = StartStopButton.GetComponentInChildren<Text>();
                        txt.text = "Start";

                        break;

                    case States.SubscribingToAccelerometerTimeout:
                        // if we got here it means we timed out subscribing to the accelerometer
                        SetState(States.Disconnect, 0.5f);
                        break;

                    case States.Disconnect:
                        SetState(States.Disconnecting, 5f);
                        if (_connected)
                        {
                            BluetoothLEHardwareInterface.DisconnectPeripheral(_deviceAddress, (address) =>
                            {
                                // since we have a callback for disconnect in the connect method above, we don't
                                // need to process the callback here.
                            });
                        }
                        else
                        {
                            Reset();
                            SetState(States.Scan, 1f);
                        }
                        break;

                    case States.Disconnecting:
                        // if we got here we timed out disconnecting, so just go to disconnected state
                        Reset();
                        SetState(States.Scan, 1f);
                        break;
                }
            }
        }
    }

    bool IsEqual(string uuid1, string uuid2)
    {
        return (uuid1.ToUpper().CompareTo(uuid2.ToUpper()) == 0);
    }
}
