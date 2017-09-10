using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;
using System.Xml.Linq;
using System.Text;

#if WINDOWS_UWP
using Windows.Storage;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
#endif


public class CRUDManager : MonoBehaviour
{

    public static CRUDManager Instance { get; private set; }

    public GameObject cursor;
    public GameObject recordMessage;

    private bool isrecording = false;
    private List<Pin> pins = new List<Pin>();
    private static string fileNameXML = "pins.xml";
    private static string fileNameTXT = "pins.txt";
#if WINDOWS_UWP
    private StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
    private static string filePathXML = Path.Combine(ApplicationData.Current.LocalFolder.Path, fileNameXML);
    private static string filePathTXT = Path.Combine(ApplicationData.Current.LocalFolder.Path, fileNameTXT);
#endif

    void Awake()
    {
        Instance = this;
    }

    #region Record
    public void RecordCursorPins()
    {
        isrecording = !isrecording;

        if (isrecording)
        {
            pins.Clear();
            InvokeRepeating("AddPins", 2f, 2f); //repeat rate 2 sec
            recordMessage.GetComponent<TextMesh>().text = "Stop Recording";
        }
        else
        {
            CancelInvoke("AddPins");
            recordMessage.GetComponent<TextMesh>().text = "Start Recording";
        }
    }

    private void AddPins()
    {
        pins.Add(new Pin { posX = cursor.transform.position.x, posY = cursor.transform.position.y, posZ = cursor.transform.position.z });
    }
    #endregion
    
    #region Save
#if WINDOWS_UWP
    public async void SavePins()
    {
        if (isrecording || pins.Count == 0)
        {
            TextMessage.Instance.ChangeTextMessage_Border("Try to record pins.");
            return;
        }
        
        //await WriteTXTToLocalStorage(); //Save to TXT file
    }

    public async Task WriteXMLToLocalStorage()
    {
        XmlSerializer serializer = new XmlSerializer(typeof(List<Pin>));
        StorageFolder storageFolder = ApplicationData.Current.LocalFolder;

        if (File.Exists(filePathXML))
            File.Delete(filePathXML);

        StorageFile storageFile = await storageFolder.CreateFileAsync(fileNameXML);
        using (FileStream fs = new FileStream(storageFile.Path, FileMode.Create))
        {
            serializer.Serialize(fs, pins);
        }
     }

    public async Task WriteTXTToLocalStorage()
    {
        XmlSerializer serializer = new XmlSerializer(typeof(List<Pin>));
        
        if (File.Exists(filePathTXT))
            File.Delete(filePathTXT);

        StorageFile storageFile = await storageFolder.CreateFileAsync(fileNameTXT);
        
        StringBuilder builder = new StringBuilder();
        for (int i = 0; i < pins.Count; i++)
        {
            builder.Append(string.Format("{0} {1} {2}", pins[i].posX.ToString("n5"), pins[i].posY.ToString("n5"), pins[i].posZ.ToString("n5"))).AppendLine();
        }

        //await FileIO.WriteTextAsync(storageFile, builder.ToString());
    }
#endif
    #endregion

    #region Load
    public void LoadPins()
    {
#if WINDOWS_UWP
        if (isrecording || pins.Count == 0)
        {
            TextMessage.Instance.ChangeTextMessage_Border("Try to record pins.");
            return;
        }
        
        if (!File.Exists(filePathXML))
        {
            TextMessage.Instance.ChangeTextMessage_Border("XML file does not exist.");
            return;
        }

        List<Pin> savedPins = ReadXMLFromLocalStorage();
        string pinMessage = string.Empty;
        for(int i = 0; i < savedPins.Count; i++)
        {
            pinMessage = string.Format("{0} {1} {2}{3}", savedPins[i].posX.ToString("n5"), savedPins[i].posY.ToString("n5"), savedPins[i].posZ.ToString("n5"), Environment.NewLine);
        }

        //pinMessage = LoadTXTFromLocalStorage().Result; //Load TXT file

        TextMessage.Instance.ChangeTextMessage_Border(pinMessage);
#endif
    }
#if WINDOWS_UWP
    public List<Pin> ReadXMLFromLocalStorage()
    {

        List<Pin> savedPins;
        XmlSerializer serializer = new XmlSerializer(typeof(List<Pin>));
        
        using (FileStream fs = new FileStream(filePathXML, FileMode.Open, FileAccess.Read))
        {
            savedPins = (List<Pin>)serializer.Deserialize(fs);
        }

        return (savedPins != null)? savedPins : new List<Pin>();
}

    public async Task<string> LoadTXTFromLocalStorage()
    {
        StorageFile storageFile = await storageFolder.GetFileAsync(fileNameTXT);
        
        return await FileIO.ReadTextAsync(storageFile);
    }
#endif
    #endregion
}
