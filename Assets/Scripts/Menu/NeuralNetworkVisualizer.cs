using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class NeuralNetworkVisualizer : MonoBehaviour
{
    public RectTransform container; // Панель, где рисовать (Canvas)
    public GameObject neuronPrefab; // Prefab кружка нейрона
    public GameObject connectionPrefab; // Prefab соединения (например, линия)

    public GameObject iconPrefab;
    public Sprite foodIcon; 
    public Sprite waterIcon;
    public Sprite healthIcon; 
    public Sprite energyIcon;
    public Sprite reproductionIcon;
    public Sprite distanceFoodIcon;
    public Sprite distanceWaterIcon;
    public Sprite distanceAnimalIcon;
    public Sprite seeingDangerIcon;
    public Sprite distanceDangerIcon;
    public Sprite wanderIcon;
    public Sprite sleepIcon;
    public Sprite huntFleeIcon;
    public Sprite evaluationIcon;
    public Sprite healthPreyIcon;
    public Sprite distancePreyIcon;


    private Dictionary<int, Sprite> iconMapping;

    AnimalController animalController;

    public GameObject tooltipPanel;
    public TextMeshProUGUI tooltipText;

    void Start(){
        animalController = GameObject.Find("AnimalController").GetComponent<AnimalController>();
    }

    private void InitializeIconMapping()
    {
        iconMapping = new Dictionary<int, Sprite>
        {
            { 0, healthIcon },
            { 1, foodIcon },
            { 2, waterIcon },
            { 3, energyIcon },
            { 4, reproductionIcon },
            { 5, distanceFoodIcon },
            { 6, distanceWaterIcon },
            { 7, distanceAnimalIcon },
            { 8, wanderIcon },
            { 9, foodIcon },
            { 10, waterIcon },
            { 11, reproductionIcon },
            { 12, sleepIcon },
            { 13, huntFleeIcon },
            { 14, seeingDangerIcon },
            { 15, distanceDangerIcon },
            { 16, evaluationIcon },
            { 17, healthPreyIcon },
            { 18, distancePreyIcon },
            { 19, evaluationIcon },
            //ID input/output нейронов
        };
    }



    public void Visualize()
    {
        NeatNN nn = animalController.lastBrain;
        
        if (nn == null) return;

        InitializeIconMapping();

        //Очистка старой визуализации
        foreach (Transform child in container)
        {
            if (child.name != "NeuralNetworkOffButton" && child.name != "PercentageOfMutation")
                Destroy(child.gameObject);
        }

        Dictionary<int, RectTransform> neuronUI = new Dictionary<int, RectTransform>();
        Dictionary<int, Neuron> neurons = nn.GetNeurons();
        List<Connection> connections = nn.GetConnections();

        //глубина слоя
        Dictionary<int, int> neuronDepths = GetNeuronDepths(nn);
        Dictionary<int, List<int>> layers = new Dictionary<int, List<int>>();

        int maxDepth = 0;
        foreach (var pair in neuronDepths)
        {
            int depth = pair.Value;
            maxDepth = Mathf.Max(maxDepth, depth);
            if (!layers.ContainsKey(depth))
                layers[depth] = new List<int>();
            layers[depth].Add(pair.Key);
        }

        float xSpacing = 200f;
        float ySpacing = 60f;

        //визуализация
        foreach (var layer in layers)
        {
            int depth = layer.Key;
            List<int> neuronIds = layer.Value;

            float startY = (neuronIds.Count - 1) * 0.5f * ySpacing;
            for (int i = 0; i < neuronIds.Count; i++)
            {
                int id = neuronIds[i];
                Neuron neuron = neurons[id];

                GameObject go = Instantiate(neuronPrefab, container);
                RectTransform rect = go.GetComponent<RectTransform>();
                neuronUI[id] = rect;


                EventTrigger trigger = go.AddComponent<EventTrigger>();//percentage count

                EventTrigger.Entry entryEnter = new EventTrigger.Entry();
                entryEnter.eventID = EventTriggerType.PointerEnter;
                entryEnter.callback.AddListener((eventData) => ShowTooltip(neuron.Id, nn));

                EventTrigger.Entry entryExit = new EventTrigger.Entry();
                entryExit.eventID = EventTriggerType.PointerExit;
                entryExit.callback.AddListener((eventData) => HideTooltip());

                trigger.triggers.Add(entryEnter);
                trigger.triggers.Add(entryExit);



                Vector2 pos = new Vector2(-maxDepth * 0.5f * xSpacing + depth * xSpacing, startY - i * ySpacing);
                rect.anchoredPosition = pos;

                go.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "<mark=#FFFFFF> " + neuron.Id.ToString() + ".";
                go.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = " " + neuron.Id.ToString();

                if (iconMapping.TryGetValue(neuron.Id, out Sprite iconSprite))
                {
                    GameObject icon = Instantiate(iconPrefab, container);
                    RectTransform iconRect = icon.GetComponent<RectTransform>();
                    icon.GetComponent<Image>().sprite = iconSprite;

                    Vector2 offset = neuron.Type == NodeType.Input
                        ? new Vector2(-50, 0)
                        : new Vector2(50, 0);

                    if(neuron.Id > 4 && neuron.Id < 8 || neuron.Id > 16 && neuron.Id < 19 || neuron.Id > 13 && neuron.Id < 16){
                        icon.GetComponent<RectTransform>().sizeDelta = new Vector2(80, 40);
                        offset *= 1.4f;
                    }

                    iconRect.anchoredPosition = pos + offset;
                    iconRect.SetAsLastSibling(); // чтобы было поверх
                }
            }
        }

        //Визуализация соединений
        foreach (var conn in connections)
        {
            if (!conn.Enabled || !neuronUI.ContainsKey(conn.From) || !neuronUI.ContainsKey(conn.To))
                continue;

            Vector3 fromWorld = neuronUI[conn.From].transform.position;
            Vector3 toWorld = neuronUI[conn.To].transform.position;

            GameObject connObj = Instantiate(connectionPrefab, container);
            LineRenderer lr = connObj.GetComponent<LineRenderer>();

            lr.useWorldSpace = true;
            lr.positionCount = 2;
            lr.SetPosition(0, fromWorld);
            lr.SetPosition(1, toWorld);

            Color color = conn.Weight > 0 ? new Color(0.5f, 1.0f, 0.9f) : new Color(1.0f, 0.64f, 0.0f);
            if (neurons[conn.From].Type == NodeType.Input && neurons[conn.To].Type == NodeType.Output)
                color = conn.Weight > 0 ? Color.green : Color.red;
            if (!conn.Enabled)
                color = Color.grey;

            lr.startColor = lr.endColor = color;
            lr.startWidth = lr.endWidth = 0.2f;
        }
    }


    void ShowTooltip(int neuronId, NeatNN neat) //check if herbivore or carnivore
    {
        int total;
        int withNeuron = 0;
        bool herbivore = true;

        List<GameObject> population;

        if(neat.GetNeurons().ContainsKey(16)){
            population = animalController.GetCarnivores();
            total = population.Count;
            herbivore = false;
        } else {
            population = animalController.GetHerbivores();
            total = population.Count;
        }

        foreach (var animal in population)
        {

            var nn = animal.GetComponent<NeatNN>();
            if (nn.GetNeurons().ContainsKey(neuronId))
                withNeuron++;
        }

        float percentage = total > 0 ? (withNeuron * 100f / total) : 0f;
        if(herbivore){
            tooltipText.text = $"Есть у {percentage:F3}% текущей популяции (трав.)";
        } else {
            tooltipText.text = $"Есть у {percentage:F3}% текущей популяции (хищ.)";
        }


        tooltipPanel.SetActive(true);
        
        //tooltipPanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -Screen.height / 2 + 100);
    }

    void HideTooltip()
    {
        tooltipPanel.SetActive(false);
    }



    private Dictionary<int, int> GetNeuronDepths(NeatNN nn)
    {
        Dictionary<int, int> depths = new Dictionary<int, int>();
        Dictionary<int, Neuron> neurons = nn.GetNeurons();

        //Глубина входных = 0
        foreach (var n in neurons.Values)
        {
            if (n.Type == NodeType.Input)
                depths[n.Id] = 0;
        }

        // Распространим глубины на hidden 
        bool changed = true;
        while (changed)
        {
            changed = false;
            foreach (var conn in nn.GetConnections())
            {
                if (!conn.Enabled) continue;

                if (!neurons.ContainsKey(conn.From) || !neurons.ContainsKey(conn.To))
                    continue;

                var fromNeuron = neurons[conn.From];
                var toNeuron = neurons[conn.To];

                //Входы-выходы в игнор
                if (toNeuron.Type == NodeType.Input || fromNeuron.Type == NodeType.Output)
                    continue;

                if (!depths.ContainsKey(conn.From)) continue;

                int proposedDepth = depths[conn.From] + 1;
                if (!depths.ContainsKey(conn.To) || depths[conn.To] < proposedDepth)
                {
                    depths[conn.To] = proposedDepth;
                    changed = true;
                }
            }
        }

        //Макс
        int maxDepth = 0;
        foreach (var pair in depths)
        {
            var neuron = neurons[pair.Key];
            if (neuron.Type != NodeType.Output)
                maxDepth = Mathf.Max(maxDepth, pair.Value);
        }

        int outputDepth = maxDepth + 1;

        // output нейронам самую правую глубину
        foreach (var n in neurons.Values)
        {
            if (n.Type == NodeType.Output)
                depths[n.Id] = outputDepth;
        }

        // hidden нейроны в диапазоне
        foreach (var n in neurons.Values)
        {
            if (n.Type == NodeType.Hidden)
            {
                if (!depths.ContainsKey(n.Id))
                {
                    // Изолированный hidden — ставим между input и output
                    depths[n.Id] = 1;
                }
                else
                {
                    // Ограничить глубину
                    depths[n.Id] = Mathf.Clamp(depths[n.Id], 1, outputDepth - 1);
                }
            }
        }

        return depths;
    }

  /*  private Vector2 GetNeuronPosition(Neuron neuron)
    {
        float x = 0;
        float y = 0;

        switch (neuron.Type)
        {
            case NodeType.Input:
                x = -300;
                y = neuron.Id * (-50) + 200;
                break;
            case NodeType.Hidden:
                return Vector2.zero;
                break;
            case NodeType.Output:
                x = 300;
                y = (neuron.Id - 8) * (-50) + 200;
                break;
        }

        return new Vector2(x, y);
    } */ //старое


    /*private Vector2 GetHiddenNeuronPosition(int neuronId, Dictionary<int, RectTransform> neuronMap, NeatNN neatNN)
    {
    float minDistance = float.MaxValue;
    Vector2 bestPos = Vector2.zero;
    List <Connection> connections = neatNN.GetConnections();

    foreach (var connA in connections)
    {
        if (!connA.Enabled) continue;
        if (connA.From != neuronId && connA.To != neuronId) continue;

        foreach (var connB in connections)
        {
            if (!connB.Enabled || connA == connB) continue;
            if (connB.From != neuronId && connB.To != neuronId) continue;

            int otherA = connA.From == neuronId ? connA.To : connA.From;
            int otherB = connB.From == neuronId ? connB.To : connB.From;

            if (!neuronMap.ContainsKey(otherA) || !neuronMap.ContainsKey(otherB)) continue;

            Vector2 posA = neuronMap[otherA].anchoredPosition;
            Vector2 posB = neuronMap[otherB].anchoredPosition;

            float dist = Vector2.Distance(posA, posB);

            if (dist < minDistance)
            {
                minDistance = dist;
                bestPos = (posA + posB) / 2f;
            }
        }
    }

    // 
    if (minDistance == float.MaxValue)
    {
        bestPos = new Vector2(Random.Range(-100, 100), Random.Range(-50, 50));
    }

    // 
    return bestPos + new Vector2(0, 50);
    }*/
}
