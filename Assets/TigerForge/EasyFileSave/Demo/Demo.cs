using System.Collections.Generic;
using UnityEngine;
using TigerForge;

public class Demo : MonoBehaviour
{

    // Instance of Easy File Save

    EasyFileSave myFile;

    // Item class

    [System.Serializable]
    public class Item
    {
        public string name = "";
        public int quantity = 0;
    }

    // Variables for this Demo.

    string character;
    string nickname;
    int age;
    float strenght;
    bool has_sword;
    bool has_bow;
    int[] skills;
    List<string> equipment;
    Vector3 initialLocation;
    Dictionary<string, int> coins;
    List<Item> items;

    void Start()
    {
        // Start a new instance of Easy File Save. The file name is not specified, so a default name will be used.

        myFile = new EasyFileSave();
        myFile.suppressWarning = false;

        // If this file already exists for some reason, I delete it.

        myFile.Delete();

        Debug.Log(">> HELLO! I'M READY!" + "\n");

    }

    void Update()
    {
        
        // When [S] key is pressed: SAVE.
        if (Input.GetKeyUp(KeyCode.S))
        {
            Debug.Log(">> I'M GOING TO SAVE SOME DATA!" + "\n");

            // Some values.

            equipment = new List<string>();
            equipment.Add("Hammer");
            equipment.Add("Knife");
            equipment.Add("Rope");

            initialLocation = new Vector3(101.5f, -30.4f, 22f);

            coins = new Dictionary<string, int>();
            coins.Add("Copper", 1200);
            coins.Add("Silver", 450);
            coins.Add("Gold", 300);

            // Simple data.

            character = "Conan";
            age = 30;
            strenght = 300.5f;
            has_sword = true;
            has_bow = false;
            skills = new int[] { 6, 10, 22, 46, 69 };

            myFile.Add("name", character);
            myFile.Add("age", age);
            myFile.Add("strenght", strenght);
            myFile.Add("has_sword", has_sword);
            myFile.Add("has_bow", has_bow);
            myFile.Add("skills_id", skills);

            // GameObject data.

            myFile.Add("equipment", equipment);
            myFile.Add("coins", coins);
            myFile.Add("initialLocation", initialLocation);
            myFile.Add("player", gameObject.transform);

            // Class data (serialization).

            items = new List<Item>();
            items.Add(new Item { name = "Gold", quantity = 15000 });
            items.Add(new Item { name = "Darts", quantity = 24 });
            items.Add(new Item { name = "Potions", quantity = 10 });

            myFile.AddSerialized("items", items);
            
            // Custom Extension for managing BoxCollider

            myFile.AddCustom("collider", gameObject.GetComponent<BoxCollider>(), "BoxCollider");

            // Save all the collected data. 
            // At the end of the process, stored data is cleared to free memory.

            myFile.Save();

            Debug.Log(">> Data saved in: " + myFile.GetFileName() + "\n");
            ShowData();
        }

        // When [L] key is pressed: LOAD.
        if (Input.GetKeyUp(KeyCode.L))
        {
            // Load data from file.
            if (myFile.Load())
            {

                Debug.Log(">> I'M GOING TO USE LOADED DATA!" + "\n");

                // Simple data.

                character = myFile.GetString("name");
                age = myFile.GetInt("age");
                strenght = myFile.GetFloat("strenght");
                has_sword = myFile.GetBool("has_sword");
                has_bow = myFile.GetBool("has_bow");
                skills = myFile.GetArray<int>("skills_id");

                // In this case, if 'nickname' key doesn't exist, 'user_1234' string is used.

                nickname = myFile.GetString("nickname", "user_1234");

                // GameObject data.

                equipment = myFile.GetList<string>("equipment");
                coins = myFile.GetDictionary<string, int>("coins");
                initialLocation = myFile.GetUnityVector3("initialLocation");

                var tr = myFile.GetUnityTransform("player");
                gameObject.transform.position = tr.position;
                gameObject.transform.rotation = tr.rotation;
                gameObject.transform.localScale = tr.localScale;

                // Class data (serialization).

                items = (List<Item>)myFile.GetDeserialized("items", typeof(List<Item>));

                // Custom Extension for managing BoxCollider.

                var bc = myFile.GetCustom("collider", "BoxCollider");
                var thisBoxColllider = gameObject.GetComponent<BoxCollider>();
                thisBoxColllider.center = new Vector3 { x = bc["centerX"].ToFloat(), y = bc["centerY"].ToFloat(), z = bc["centerZ"].ToFloat() };
                thisBoxColllider.isTrigger = bc["isTrigger"].ToBool();
 
                // Loaded data has been used as needed.
                // Stored data is manually cleared to free memory.

                myFile.Dispose();

                Debug.Log(">> Data loaded from: " + myFile.GetFileName() + "\n");
                ShowData();
            }
        }

        // When [A] key is pressed: APPEND.
        if (Input.GetKeyUp(KeyCode.A))
        {

            // Simple data.

            myFile.Add("nickname", "The Warrior");
            myFile.Add("age", 32);

            // Append this data to the current file content.
            // 'nickname' key is new, so its value is added to the file.
            // 'age' key already exists, so its current value is updated with this new one.

            myFile.Append();

            Debug.Log(">> New data added to: " + myFile.GetFileName() + "\n");
            Debug.Log(">> Age value updated to 32." + "\n");
            Debug.Log(">> Added nickname." + "\n");
        }

        // When [Del] key is pressed: FILE DELETE.
        if (Input.GetKeyUp(KeyCode.Delete))
        {

            // Delete this file.
            // This method clears stored data as well.

            myFile.Delete();

            Debug.Log(">> The file has been deleted." + "\n");
        }

        // When [T] key is pressed: SAVING AND LOADING DATA TEST.
        if (Input.GetKeyUp(KeyCode.T))
        {

            // Perform a test of writing and loading data.
            // -------------------------------------------

            myFile.Delete();

            // Add some values to the internal storage for saving and loading test.

            equipment = new List<string>();
            equipment.Add("Hammer");
            equipment.Add("Knife");
            equipment.Add("Rope");

            initialLocation = new Vector3(101.5f, -30.4f, 22f);

            character = "Conan";
            age = 30;
            strenght = 300.5f;
            has_sword = true;
            has_bow = false;

            items = new List<Item>();
            items.Add(new Item { name = "Gold", quantity = 15000 });
            items.Add(new Item { name = "Darts", quantity = 24 });
            items.Add(new Item { name = "Potions", quantity = 10 });

            myFile.Add("name", character);
            myFile.Add("age", age);
            myFile.Add("strenght", strenght);
            myFile.Add("has_sword", has_sword);
            myFile.Add("has_bow", has_bow);

            myFile.Add("equipment", equipment);
            myFile.Add("initialLocation", initialLocation);
            myFile.Add("player", gameObject.transform);

            myFile.AddSerialized("items", items);

            myFile.AddCustom("collider", gameObject.GetComponent<BoxCollider>(), "BoxCollider");

            // Test

            if (myFile.TestDataSaveLoad())
            {
                Debug.Log("<color=green>GOOD!</color>\n");
            } else
            {
                Debug.Log("<color=red>OPS! SOMETHING WENT WRONG!</color>\n");
            }

        }

    }

    private void ShowData()
    {
        Debug.Log("Name: " + character + "\n");
        Debug.Log("Nickname: " + nickname + "\n");
        Debug.Log("Age: " + age + "\n");
        Debug.Log("Stregth: " + strenght + "\n");
        Debug.Log("Has a sword: " + has_sword + "\n");
        Debug.Log("Has a Bow: " + has_bow + "\n");
        Debug.Log("Spawn location: x = " + initialLocation.x + ", y = " + initialLocation.y + ", z = " + initialLocation.z + "\n");
        Debug.Log("GameObject position: x = " + gameObject.transform.position.x + ", y = " + gameObject.transform.position.y + ", z = " + gameObject.transform.position.z + "\n");
        Debug.Log("GameObject rotation: x = " + gameObject.transform.rotation.x + ", y = " + gameObject.transform.rotation.y + ", z = " + gameObject.transform.rotation.z + "\n");
        Debug.Log("GameObject scale: x = " + gameObject.transform.localScale.x + ", y = " + gameObject.transform.localScale.y + ", z = " + gameObject.transform.localScale.z + "\n");

        foreach (var n in skills) Debug.Log("Skill ID: " + n + "\n");
        foreach (var item in equipment) Debug.Log("Equipment: " + item + "\n");
        foreach (KeyValuePair<string, int> item in coins) Debug.Log("Coin - Type: " + item.Key + " Quantity: " + item.Value + "\n");
        foreach (var item in items) Debug.Log("Item - Name: " + item.name + " Quantity: " + item.quantity + "\n");
    }

}
