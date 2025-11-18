using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.EventSystems;
using TMPro;


using System.Text;


public static class SaveManager
{
    private static readonly string SaveFolder = Path.Combine(Application.persistentDataPath, "Saves");

    public static void SaveToFile<T>(T data, string fileName)
    {
        if (!Directory.Exists(SaveFolder))
        {
            Directory.CreateDirectory(SaveFolder);
        }

        string json = JsonUtility.ToJson(data, true);
        string fullPath = Path.Combine(SaveFolder, fileName);

        File.WriteAllText(fullPath, json);
        Debug.Log($"Saved to {fullPath}");
    }

    public static T LoadFromFile<T>(string fileName)
    {
        string fullPath = Path.Combine(SaveFolder, fileName);

        if (File.Exists(fullPath))
        {
            string json = File.ReadAllText(fullPath);
            return JsonUtility.FromJson<T>(json);
        }
        else
        {
            Debug.LogWarning($"File not found: {fullPath}");
            return default;
        }
    }

    public static bool FileExists(string fileName)
    {
        return File.Exists(Path.Combine(SaveFolder, fileName));
    }

    public static void DeleteSave(string fileName)
    {
        string fullPath = Path.Combine(SaveFolder, fileName);
        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }
    }

}

public static class UserIdManager
{
    private const string Key = "userId";

    public static string GetOrCreateUserId()
    {
        if (PlayerPrefs.HasKey(Key))
            return PlayerPrefs.GetString(Key);

        string baseId = SystemInfo.deviceUniqueIdentifier;

        if (string.IsNullOrEmpty(baseId) || baseId == "00000000000000000000000000000000")
        {
            baseId = "unknown";
        }

        string randomPart = Guid.NewGuid().ToString();
        string combinedId = $"{baseId}_{randomPart}";

        PlayerPrefs.SetString(Key, combinedId);
        PlayerPrefs.Save();

        return combinedId;
    }
}


public class SaveController: MonoBehaviour 
{
    public EnvironmentController environmentController;

    public AnimalController animalController;

    public PlantController plantController;

    public InnovationTracker innovationTracker;

    //public GraphLinesController graphController;
    public GameObject graphController;

    //public SetEnvButton applyEnvironmentButton;

    //public SetAnimalButton applyAnimalButton;

    //public SetPlantButton applyPlantButton;

    
    public GameObject applyEnvironmentButton;

    public GameObject applyAnimalButton;

    public GameObject applyPlantButton;


    public Button newGameButton;

    public Button loadGameButton;

    public Button loadFromServerButton;

    public Button saveToServerButton;

    public Button saveButton;

    public Button clearButton;

    public GameObject sizeLimitErrorPanel;

    bool isSimulationActive = true; //For autosaves and graphs?

    public void Awake(){
        //loadFromServerButton.interactable = false;
        //saveToServerButton.interactable = false;
        

        environmentController = GameObject.Find("EnvironmentController").GetComponent<EnvironmentController>();//        
        animalController = GameObject.Find("AnimalController").GetComponent<AnimalController>();
        plantController = GameObject.Find("PlantController").GetComponent<PlantController>();
        //graphController = GameObject.Find("MenuController").GetComponent<GraphLinesController>();
        innovationTracker = GameObject.Find("InnovationTracker").GetComponent<InnovationTracker>();

        //applyEnvironmentButton = GameObject.Find("ApplyEnvironmentButton").GetComponent<SetEnvButton>();
        //applyAnimalButton = GameObject.Find("ApplyAnimalSettingsButton").GetComponent<SetAnimalButton>();
        //applyPlantButton = GameObject.Find("ApplyPlantSettingsButton").GetComponent<SetPlantButton>();
        
    }

    public void Start(){

        if(applyEnvironmentButton == null)
        applyEnvironmentButton = GameObject.Find("ApplyEnvironmentButton"); //.GetComponent<SetEnvButton>();
        if(applyAnimalButton == null)
        applyAnimalButton = GameObject.Find("ApplyAnimalSettingsButton"); //.GetComponent<SetAnimalButton>();

        if(applyPlantButton == null)
        applyPlantButton = GameObject.Find("ApplyPlantSettingsButton"); //.GetComponent<SetPlantButton>();

        if(graphController == null){
            graphController = GameObject.Find("MenuController");
        }

        loadFromServerButton.interactable = false;
        saveToServerButton.interactable = false;

        SwitchButtons();
        StartCoroutine(CheckServerAvailability(UpdateServerButtons));
    }


    void SwitchButtons(){
        isSimulationActive = !isSimulationActive;
        newGameButton.interactable = !isSimulationActive;
        loadGameButton.interactable = !isSimulationActive;

        if(isAvailable){
            loadFromServerButton.interactable = !isSimulationActive;
            saveToServerButton.interactable = isSimulationActive;
        } else {
            loadFromServerButton.interactable = false;
            saveToServerButton.interactable = false;
        }

        saveButton.interactable  = isSimulationActive;
        clearButton.interactable = isSimulationActive;
    }

    private void UpdateServerButtons(bool serverAvailable)
        {
            loadFromServerButton.interactable = serverAvailable && !isSimulationActive;
            saveToServerButton.interactable = serverAvailable && isSimulationActive;
        }

    public PopulationData CollectData(){
        List<GameObject> herbivores = animalController.GetHerbivores();
        List<GameObject> carnivores = animalController.GetCarnivores();
        List<GameObject> plants = plantController.GetPlants();

        List<AnimalSaveData> resAnimal = GetAnimalSaveData(herbivores, carnivores);
        List<PlantSaveData> resPlant = GetPlantSaveData(plants);

        PopulationData data = new PopulationData();
        data.animals = resAnimal;
        data.plants = resPlant;

        data.innovationTrackerData = innovationTracker.ToData();

        data.environmentSaveData = environmentController.ToData();

        data.plantControllerData = plantController.ToData();

        data.animalControllerData = animalController.ToData();

        return data;
    }
    


    public void CollectDataAndCreateSave(){

        PopulationData data = CollectData();

        SaveManager.SaveToFile(data, "newSave");
    }


    public static List<AnimalSaveData> GetAnimalSaveData(List<GameObject> herbivorePopulation, List<GameObject> carnivorePopulation)
    {
        List<AnimalSaveData> saveDataList = new List<AnimalSaveData>();

        List<GameObject> population = new List<GameObject>();

        population.AddRange(herbivorePopulation);
        population.AddRange(carnivorePopulation);

        foreach (GameObject obj in population)
        {
            Animal animal = obj.GetComponent<Animal>();
            if (animal == null)
                continue;

            AnimalSaveData data = animal.ToData();

            data.objectName = obj.name;

            NeatNN neatNN = obj.GetComponent<NeatNN>();

            data.neatNN = neatNN.ToData();

            CreatureMover mover = obj.GetComponent<CreatureMover>();

            data.mover = mover.ToData();

            DNAstats statsMod = obj.GetComponent<DNAstats>();

            data.statsMod = statsMod.ToData();

            saveDataList.Add(data);
        }

        return saveDataList;
    }

    public static List<PlantSaveData> GetPlantSaveData(List<GameObject> population)
    {
        List<PlantSaveData> saveDataList = new List<PlantSaveData>();

        foreach (GameObject obj in population)
        {
            Plant plant = obj.GetComponent<Plant>();
            if (plant == null)
                continue;

            PlantSaveData data = plant.ToData();

            data.objectName = obj.name;

            PlantDNAstats statsMod = obj.GetComponent<PlantDNAstats>();

            data.statsMod = statsMod.ToData();

            saveDataList.Add(data);
        }
        return saveDataList;
    }


    private const string uploadUrl = "http://localhost:3000/upload";
    private const string downloadUrl = "http://localhost:3000/download";
    private const string pingUrl = "http://localhost:3000/ping";

    bool isAvailable = false;

    public IEnumerator CheckServerAvailability(Action<bool> callback)
    {
        while (true) {

            UnityWebRequest request = UnityWebRequest.Get(pingUrl);

            request.timeout = 3;
         
            if(isAvailable){
                yield return new WaitForSeconds(3);
            }

            yield return request.SendWebRequest();

            isAvailable = request.result == UnityWebRequest.Result.Success 
            && request.responseCode == 200;

            callback(isAvailable);
        }
    }

    public IEnumerator UploadJson<T>(T data)
    {
        UnityWebRequest request = new UnityWebRequest(uploadUrl, "POST");

        string deviceId = UserIdManager.GetOrCreateUserId();

        string jsonData = JsonUtility.ToJson(data, true);

        byte[] jsonToSend = Encoding.UTF8.GetBytes(jsonData);

        const int MaxSizeBytes = 16 * 1024 * 1024; // 16 МБ

        if (jsonToSend.Length > MaxSizeBytes)
            {
                Debug.Log($"Upload failed: JSON size is {jsonToSend.Length / (1024f * 1024f):F2} MB, too big.");
                ShowError("Размер сохранения превышает допустимый для сервера!");
                yield break; 
            }

        request.uploadHandler = new UploadHandlerRaw(jsonToSend);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("user-id", deviceId);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Upload successful!");
        }
        else
        {
            Debug.LogError("Upload failed: " + request.error);
            ShowError("Ошибка сохранения");
        }
    }


    private void ShowError(string error)
    {
        sizeLimitErrorPanel.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = error;
        sizeLimitErrorPanel.SetActive(true);
        StartCoroutine(HideAfterDelay(sizeLimitErrorPanel, 3f));
    }


    private IEnumerator HideAfterDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        obj.SetActive(false);
    }


    public IEnumerator DownloadSaveFromServer()
    {
        string deviceId = UserIdManager.GetOrCreateUserId();

        UnityWebRequest request = UnityWebRequest.Get(downloadUrl);
        request.SetRequestHeader("user-id", deviceId);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string json = request.downloadHandler.text;
            PopulationData data = JsonUtility.FromJson<PopulationData>(json);

            Debug.Log("Загрузка успешна");

            environmentController.FromData(data.environmentSaveData);

            innovationTracker.FromData(data.innovationTrackerData);

            plantController.FromData(data.plantControllerData);

            animalController.FromData(data.animalControllerData);

            foreach (var animal in data.animals)
            {
                animalController.FromSave(animal);
            }

            foreach(var plant in data.plants){
                plantController.FromSave(plant);
            }

            if(data.animals.Count > 0 || data.plants.Count > 0){
                graphController.GetComponent<GraphLinesController>().StartGraph();
            }

            applyEnvironmentButton.GetComponent<SetEnvButton>().GameStarted();
            applyPlantButton.GetComponent<SetPlantButton>().GameStarted();
            applyAnimalButton.GetComponent<SetAnimalButton>().GameStarted();

            SwitchButtons();
        }
        else
        {
            Debug.LogError("Ошибка при загрузке с сервера: " + request.error);
            ShowError("Ошибка загрузки");
        }
    }

    public void CreateSaveOnServer(){

        PopulationData data = CollectData();
        StartCoroutine(UploadJson<PopulationData>(data));
    }

    public void LoadSaveFromServer()
    {
        StartCoroutine(DownloadSaveFromServer());
    }


    public void LoadSave(){
        PopulationData data = SaveManager.LoadFromFile<PopulationData>("NewSave");

        if (data == null)
        {
            ShowError("Ошибка загрузки");
            return;
        }

        List<AnimalSaveData> animalDataList = data.animals;
        List<PlantSaveData> plantDataList = data.plants;

        environmentController.FromData(data.environmentSaveData);

        innovationTracker.FromData(data.innovationTrackerData);

        plantController.FromData(data.plantControllerData);

        animalController.FromData(data.animalControllerData);

        foreach(var animal in animalDataList){
            animalController.FromSave(animal);
        }
        foreach(var plant in plantDataList){
            plantController.FromSave(plant);
        }

        if(data.animals.Count > 0){
            graphController.GetComponent<GraphLinesController>().StartGraph();
        }

        applyEnvironmentButton.GetComponent<SetEnvButton>().GameStarted();
        applyPlantButton.GetComponent<SetPlantButton>().GameStarted();
        applyAnimalButton.GetComponent<SetAnimalButton>().GameStarted();

        SwitchButtons();
    }


    public void NewGame(){
        environmentController.FromNewGame();
        animalController.FromNewGame();
        plantController.FromNewGame();

        //if(animalController.GetPopulation() > 0){ //TODO: add carnivorepopulation
            graphController.GetComponent<GraphLinesController>().StartGraph();
        //}
        applyEnvironmentButton.GetComponent<SetEnvButton>().GameStarted();
        applyPlantButton.GetComponent<SetPlantButton>().GameStarted();
        applyAnimalButton.GetComponent<SetAnimalButton>().GameStarted();
        //graphController.StartGraph();
        SwitchButtons();
    }

    public void EndGame(){
        environmentController.EndGame();
        animalController.EndGame();
        plantController.EndGame();
        innovationTracker.EndGame();
        graphController.GetComponent<GraphLinesController>().EndGame();
        applyEnvironmentButton.GetComponent<SetEnvButton>().GameEnded();
        applyPlantButton.GetComponent<SetPlantButton>().GameEnded();
        applyAnimalButton.GetComponent<SetAnimalButton>().GameEnded();
        SwitchButtons();
    }
    

}
