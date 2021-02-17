using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class FitCapTest : MonoBehaviour
{
	public string DeviceName = "FitCap1";

	public Text AccelerometerText;
	public Text FitCapStatusText;

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
		new Characteristic { ServiceUUID = "00000000-CC7A-482A-984A-7F2ED5B3E58", CharacteristicUUID = "0000E000-8E22-4541-9D4C-21EDAE82ED19", Found = false },
		new Characteristic { ServiceUUID = "00000000-CC7A-482A-984A-7F2ED5B3E58F", CharacteristicUUID = "00000004-8E22-4541-9D4C-21EDAE82ED19", Found = false },
	};

	public Characteristic SubscribeAccelerometer = Characteristics[0];
	public Characteristic ReadAccelerometer = Characteristics[1];

	public bool AllCharacteristicsFound { get { return !(Characteristics.Where (c => c.Found == false).Any ()); } }
	public Characteristic GetCharacteristic (string serviceUUID, string characteristicsUUID)
	{
		return Characteristics.Where (c => IsEqual (serviceUUID, c.ServiceUUID) && IsEqual (characteristicsUUID, c.CharacteristicUUID)).FirstOrDefault ();
	}

	enum States
	{
		None,
		Scan,
		Connect,
		SubscribeToAccelerometer,
		SubscribingToAccelerometer,
		Disconnect,
		Disconnecting,
	}

	private bool _connected = false;
	private float _timeout = 0f;
	private States _state = States.None;
	private string _deviceAddress;

	private byte[] _accelerometerConfigureBytes = new byte[] { 0x01, 0x01 };

	string FitCapStatusMessages
	{
		set
		{
			Debug.Log(value);

			if (!string.IsNullOrEmpty(value))
				BluetoothLEHardwareInterface.Log (value);
			if (FitCapStatusText != null)
			{
				FitCapStatusText.text = value;
			}
		}
	}

	void Reset ()
	{
		_connected = false;
		_timeout = 0f;
		_state = States.None;
		_deviceAddress = null;

		TopPanel.SetActive (true);
		MiddlePanel.SetActive (true);

        FitCapStatusMessages = "Reset";
	}

	void SetState (States newState, float timeout)
	{
		_state = newState;
		_timeout = timeout;
	}

	void StartProcess ()
	{
		FitCapStatusMessages = "StartProcess";
		Reset ();
		BluetoothLEHardwareInterface.Initialize (true, false, () => {

			SetState (States.Scan, 0.1f);

		}, (error) => {

			BluetoothLEHardwareInterface.Log ("Error: " + error);
		});
	}

	private void OnCharacteristicNotification(string deviceAddress, string characteristric, byte[] rcvd_data)
	{
		// Show ( "received data");
		_state = States.None;
		MiddlePanel.SetActive(true);

		var sBytes = BitConverter.ToString(rcvd_data);
		AccelerometerText.text = "Accelerometer: " + sBytes;
		FitCapStatusMessages = "Accelerometer: " + sBytes;
	}


	// Use this for initialization
	void Start ()
	{
		StartProcess ();
	}

	// Update is called once per frame
	void Update ()
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
						BluetoothLEHardwareInterface.ScanForPeripheralsWithServices (null, (address, deviceName) => {
						FitCapStatusMessages = "Scanning Found: " + deviceName;

						if (deviceName.Contains (DeviceName))
						{
							FitCapStatusMessages = "Found a SensorBug";

							BluetoothLEHardwareInterface.StopScan ();

							TopPanel.SetActive (true);

							// found a device with the name we want
							// this example does not deal with finding more than one
							_deviceAddress = address;
							SetState (States.Connect, 0.5f);
						}

					}, null, true);
					break;

				case States.Connect:
					FitCapStatusMessages = "Connecting to SensorBug...";

					BluetoothLEHardwareInterface.ConnectToPeripheral (_deviceAddress, null, null, (address, serviceUUID, characteristicUUID) => {

						var characteristic = GetCharacteristic (serviceUUID, characteristicUUID);
						if (characteristic != null)
						{
							BluetoothLEHardwareInterface.Log (string.Format ("Found {0}, {1}", serviceUUID, characteristicUUID));

							characteristic.Found = true;

							if (AllCharacteristicsFound)
							{
								_connected = true;
								SetState (States.SubscribeToAccelerometer, 3f);
							}
						}
					}, (disconnectAddress) => {
						FitCapStatusMessages = "Disconnected from SensorBug";
						Reset ();
						SetState (States.Scan, 1f);
					});
					break;

				case States.SubscribeToAccelerometer:
					SetState (States.SubscribingToAccelerometer, 5f);
					FitCapStatusMessages = "Subscribing to SensorBug Accelerometer...";

					BluetoothLEHardwareInterface.SubscribeCharacteristicWithDeviceAddress(_deviceAddress, SubscribeAccelerometer.ServiceUUID, SubscribeAccelerometer.CharacteristicUUID, delegate { }, OnCharacteristicNotification);
						/*

											BluetoothLEHardwareInterface.SubscribeCharacteristicWithDeviceAddress (_deviceAddress, SubscribeAccelerometer.ServiceUUID, SubscribeAccelerometer.CharacteristicUUID, null, (deviceAddress, characteristric, bytes) => {

																		_state = States.None;
																		MiddlePanel.SetActive (true);

																		var sBytes = BitConverter.ToString (bytes);
																		AccelerometerText.text = "Accelerometer: " + sBytes;
																	});
												*/
						break;

				case States.SubscribingToAccelerometer:
					// if we got here it means we timed out subscribing to the accelerometer
					SetState (States.Disconnect, 0.5f);
					break;

				case States.Disconnect:
					SetState (States.Disconnecting, 5f);
					if (_connected)
					{
						BluetoothLEHardwareInterface.DisconnectPeripheral (_deviceAddress, (address) => {
							// since we have a callback for disconnect in the connect method above, we don't
							// need to process the callback here.
						});
					}
					else
					{
						Reset ();
						SetState (States.Scan, 1f);
					}
					break;

				case States.Disconnecting:
					// if we got here we timed out disconnecting, so just go to disconnected state
					Reset ();
					SetState (States.Scan, 1f);
					break;
				}
			}
		}
	}

	bool IsEqual (string uuid1, string uuid2)
	{
		return (uuid1.ToUpper ().CompareTo (uuid2.ToUpper ()) == 0);
	}
}
