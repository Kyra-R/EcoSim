using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

    public class Neuron
    {
        public int Id;
        public NodeType Type;
        public float Value = 0.0f;

        public float Bias = 0.0f;

        public int Depth;

        public Neuron(int id, NodeType type)
        {
            Id = id;
            Type = type;
        }

        public Neuron(int id, NodeType type, float bias)
        {
            Id = id;
            Type = type;
            Bias = bias;
        }
    }

    public enum NodeType
    {
        Input,
        Hidden,
        Output
    }


    public class Connection
    {
        public int From;              
        public int To;               
        public float Weight;          
        public bool Enabled;          
        public int Innovation;       

        public Connection(int from, int to, float weight, bool enabled, int innovation)
        {
            From = from;
            To = to;
            Weight = weight;
            Enabled = enabled;
            Innovation = innovation;
        }

        public Connection Clone()
        {
            return new Connection(From, To, Weight, Enabled, Innovation);
        }
    }

public class NeatNN : MonoBehaviour
{

    private Dictionary<int, Neuron> Neurons = new Dictionary<int, Neuron>();
    private List<Connection> Connections = new List<Connection>();

    private List<Neuron> sortedNeurons;

    private Dictionary<int, List<Connection>> outgoingConnections;


    public void SortNeuronsByDepth() {
        sortedNeurons = Neurons.Values.OrderBy(n => n.Depth).ToList();
    }

    private void BuildConnectionMap()
    {
        outgoingConnections = new Dictionary<int, List<Connection>>();

        foreach (var conn in Connections)
        {
            if (!conn.Enabled) continue;

            if (!outgoingConnections.ContainsKey(conn.From))
                outgoingConnections[conn.From] = new List<Connection>();

            outgoingConnections[conn.From].Add(conn);
        }
    }

    private int[] inputIds;// = new int[8];
    private int[] outputIds;// = new int[5];

    //private static int nextNodeId = 0;

    private static System.Random rand = new System.Random();


    public InnovationTracker globalInnovationTracker;


    public List<Connection> GetConnections(){
        return Connections;
    }

    public Dictionary<int, Neuron> GetNeurons(){
        return Neurons;
    }

    
    public void Awake()
    {
        Random.InitState((int)System.DateTime.Now.Ticks);

        Neurons = new Dictionary<int, Neuron>();

        Connections = new List<Connection>();

        inputIds = new int[8];

        outputIds = new int[6];
        
        if(globalInnovationTracker == null)
        globalInnovationTracker = GameObject.Find("InnovationTracker").GetComponent<InnovationTracker>();

    }

    public void Start(){

    }

    public NeatNN(int inputCount, int outputCount)
    {
        for (int i = 0; i < inputCount; i++)
        {
            Neurons.Add(globalInnovationTracker.GetNextNodeId(), new Neuron(globalInnovationTracker.GetNextNodeId(), NodeType.Input));
            //globalInnovationTracker.IncreaseNextNodeId();
        }

        for (int i = 0; i < outputCount; i++)
        {
            Neurons.Add(globalInnovationTracker.GetNextNodeId(), new Neuron(globalInnovationTracker.GetNextNodeId(), NodeType.Output));
            //globalInnovationTracker.IncreaseNextNodeId();
        }

    }

    public NeatNN(){
        Neurons = new Dictionary<int, Neuron>();

        Connections = new List<Connection>();

        inputIds = new int[8];

        outputIds = new int[6];

//        globalInnovationTracker = innovationObject.GetComponent<InnovationTracker>(); //todo: remove later
    }


    void CheckStructure(){
        Debug.Log("CHECK THE STRUCTURE--------------------------------");
        foreach (var neuron in Neurons.Values)
        {
            Debug.Log(neuron.Id + "=id bias=" + neuron.Bias);
        }
        Debug.Log("================");
        for (int i = 0; i < Connections.Count; i++)
        {
            Debug.Log(Connections[i].From + "->" + Connections[i].To + " weight:" + Connections[i].Weight);
        }
        Debug.Log("================");
        Debug.Log(globalInnovationTracker.GetNextNodeIdWithoutIncrease() + " = nextNodeId " );
        Debug.Log("================");
        for (int i = 0; i < outputIds.Length; i++)
        {
            Debug.Log(i + ": output id " + outputIds[i]);
        }
        Debug.Log("CHECK END--------------------------------");
    }

    public void InitializeBaseStructure(InnovationTracker tracker)
    {
        //this.Connections.Clear();
        if(tracker == null)
        tracker = GameObject.Find("InnovationTracker").GetComponent<InnovationTracker>();

        int inputIndex = 0;
        int outputIndex = 0;

        for (int i = 0; i < 8; i++)
        {
            Debug.Log("INPUT");
            var n = new Neuron(tracker.GetNextNodeId(), NodeType.Input);
            Neurons.Add(n.Id, n);
            inputIds[i] = n.Id;
        }



        float[] outputBiases = { 0.25f, 0.0f, 0.15f, 0.0f, 0.0f, 0.0f }; // WANDER ... REST +FLEE/HUNT
        for (int i = 0; i < 6; i++)
        {
            var n = new Neuron(tracker.GetNextNodeId(), NodeType.Output);
           
            n.Bias = outputBiases[i];
            Neurons[n.Id] = n;
            outputIds[i] = n.Id;
         
        }
        //Debug.Log("----------------");

        // Connections  
        Connections.Add(new Connection(inputIds[2], outputIds[2], 1.0f, true,  tracker.GetInnovation(inputIds[2], outputIds[2])));    // thirst → search water
        
        Connections.Add(new Connection(inputIds[2], outputIds[1], 0.05f, true,  tracker.GetInnovation(inputIds[2], outputIds[1])));    // thirst → search food (a little)
        
        Connections.Add(new Connection(inputIds[6], outputIds[2], 0.1f, true,  tracker.GetInnovation(inputIds[6], outputIds[2])));   // waterDistance → search water
        
        Connections.Add(new Connection(inputIds[3], outputIds[4], 1.1f, true,  tracker.GetInnovation(inputIds[3], outputIds[4])));    // fatigue → rest
        Connections.Add(new Connection(inputIds[1], outputIds[4], -0.5f, true,  tracker.GetInnovation(inputIds[1], outputIds[4])));   // hunger → rest
        Connections.Add(new Connection(inputIds[2], outputIds[4], -0.5f, true,  tracker.GetInnovation(inputIds[2], outputIds[4])));   // thirst → rest
        
        Connections.Add(new Connection(inputIds[3], outputIds[3], -0.4f, true,  tracker.GetInnovation(inputIds[3], outputIds[3])));   // fatigue → search mate
        Connections.Add(new Connection(inputIds[1], outputIds[3], -0.6f, true,  tracker.GetInnovation(inputIds[1], outputIds[3])));   // hunger → search mate
        Connections.Add(new Connection(inputIds[2], outputIds[3], -0.5f, true,  tracker.GetInnovation(inputIds[2], outputIds[3])));   // thirst → search mate
        Connections.Add(new Connection(inputIds[4], outputIds[3], 1.1f, true,  tracker.GetInnovation(inputIds[4], outputIds[3])));    // reproductionReadiness → search mate
        //Connections.Add(new Connection(inputIds[7], outputIds[3], -0.2f, true,  tracker.GetInnovation(inputIds[7], outputIds[3])));   // populationDensity → search mate
        
        Connections.Add(new Connection(inputIds[7], outputIds[0], 0.2f, true,  tracker.GetInnovation(inputIds[7], outputIds[0])));    // populationDensity → wander
        Connections.Add(new Connection(inputIds[0], outputIds[0], - 0.1f, true,  tracker.GetInnovation(inputIds[0], outputIds[0])));    // health -> wander
        //CheckStructure();
    }


    public void ModifyBaseStructureHerbivore(InnovationTracker tracker)
    {
        //int inputIndex = 10;
        System.Array.Resize<int>(ref inputIds, inputIds.Length + 2);
        for (int i = 8; i < 10; i++)
        {
            var n = new Neuron(tracker.GetNextNodeId(), NodeType.Input);
            Neurons.Add(n.Id, n);
            inputIds[i] = n.Id;
         //   Debug.Log(i + " i, id: " + n.Id);
        }
        Connections.Add(new Connection(inputIds[1], outputIds[1], 1.0f, true, tracker.GetInnovation(inputIds[1], outputIds[1])));    // hunger → search food
        Connections.Add(new Connection(inputIds[5], outputIds[1], - 0.1f, true, tracker.GetInnovation(inputIds[5], outputIds[1])));   // foodDistance → search food

        Connections.Add(new Connection(inputIds[7], outputIds[3], -0.2f, true,  tracker.GetInnovation(inputIds[7], outputIds[3]))); 
        
        Connections.Add(new Connection(inputIds[8], outputIds[5], 1.5f, true,  tracker.GetInnovation(inputIds[8], outputIds[5])));
        Connections.Add(new Connection(inputIds[9], outputIds[5], 4.5f, true,  tracker.GetInnovation(inputIds[9], outputIds[5])));

        Neurons[outputIds[5]].Bias = 0.1f;

        Debug.Log("MOD HERBIVORE " + inputIds.Length);
        //Connections.Add(new Connection(inputIds[0], outputIds[0], - 0.1f, true,  tracker.GetInnovation(inputIds[0], outputIds[0])));
    }
    public void ModifyBaseStructureCarnivore(InnovationTracker tracker)
    {
        System.Array.Resize<int>(ref inputIds, inputIds.Length + 4);
        System.Array.Resize<int>(ref outputIds, outputIds.Length + 1);

        for (int i = 8; i < 11; i++) //TODO: add size eval from 11 to 12
        {
            var n = new Neuron(tracker.GetNextNodeId(), NodeType.Input);
            Neurons.Add(n.Id, n);
            inputIds[i] = n.Id;
        }
        for (int i = 6; i < 7; i++)
        {
            var n = new Neuron(tracker.GetNextNodeId(), NodeType.Output);
            n.Bias = 0.0f;
            Neurons[n.Id] = n;
            outputIds[i] = n.Id;
            //Debug.Log(n.Id);
        }
        Connections.Add(new Connection(inputIds[1], outputIds[1], 0.85f, true, tracker.GetInnovation(inputIds[1], outputIds[1])));    // hunger → search food
        Connections.Add(new Connection(inputIds[5], outputIds[1], - 0.12f, true, tracker.GetInnovation(inputIds[5], outputIds[1])));   // foodDistance → search food

        Connections.Add(new Connection(inputIds[7], outputIds[3], -0.4f, true,  tracker.GetInnovation(inputIds[7], outputIds[3])));   // populationDensity → search mate

        Connections.Add(new Connection(inputIds[8], outputIds[5], 1.2f, true,  tracker.GetInnovation(inputIds[8], outputIds[5]))); //preychosen
        Connections.Add(new Connection(inputIds[8], outputIds[1], - 0.5f, true,  tracker.GetInnovation(inputIds[8], outputIds[1]))); //if prey chosen, stop searching
        //Connections.Add(new Connection(inputIds[1], outputIds[5], 0.3f, true, tracker.GetInnovation(inputIds[1], outputIds[5])));

        Connections.Add(new Connection(inputIds[1], outputIds[6], 0.3f, true, tracker.GetInnovation(inputIds[1], outputIds[6])));
        Connections.Add(new Connection(inputIds[9], outputIds[6], 0.5f, true,  tracker.GetInnovation(inputIds[9], outputIds[6]))); //preyeval
        Connections.Add(new Connection(inputIds[10], outputIds[6], 0.2f, true, tracker.GetInnovation(inputIds[10], outputIds[6])));
        Connections.Add(new Connection(inputIds[11], outputIds[6], 0.5f, true, tracker.GetInnovation(inputIds[11], outputIds[6])));

        Debug.Log("MOD CARNIVORE " + inputIds.Length);
    }

    public void CreateOffspringNetwork(NeatNN parent1, NeatNN parent2, float neuronMutationRate, float connectionMutationRate, float biasAndWeightsMutationRate){
        NeatNN child;

//        Debug.Log("xxxxxxx|CROSSOVER|xxxxxxx");
        float parent1GenesDominant = Random.value;
        if(parent1GenesDominant < 0.5){
            child = Crossover(parent1, parent2, true);
        } else {
            child = Crossover(parent1, parent2, false);
        }

        for (int i = 0; i < parent1.inputIds.Length; i++) //always the same in both parents
        {
            child.inputIds[i] = parent1.inputIds[i];
        }
        for (int i = 0; i < parent1.outputIds.Length; i++)
        {
            child.outputIds[i] = parent1.outputIds[i]; //TODO: some bug here (with carnivores?) outputs
        }
        

        // Мутация: добавить нейрон
        if (Random.value <= neuronMutationRate)
        {
  //          Debug.Log("NEURON ADDED/REMOVED");
            if(Random.value < 0.05){
                child.Mutate_RemoveNeuron();
            } else {
                child.Mutate_AddNeuron(globalInnovationTracker);
            }
        }

        // Мутация: добавить соединение
        if (Random.value <= connectionMutationRate)
        {
   //         Debug.Log("CONNECTION ADDED/DISABLED/ENABLED");
            if(Random.value < 0.1){
                child.Mutate_DisableConnection();
            } else if(Random.value < 0.2){
                child.Mutate_ReenableConnection();
            }
            else {
                child.Mutate_AddConnection(globalInnovationTracker);
            }
        }

        if (Random.value <= biasAndWeightsMutationRate)
        {
  //          Debug.Log("WEIGHTS AND BIASES CHANGED");
            if(Random.value < 0.1){
                child.Mutate_AllWeights();
            } else {
                child.Mutate_Weight();
            }

            if(Random.value < 0.1){
                child.Mutate_AllBiases();
            } else {
                child.Mutate_Bias();
            }
        }


        Clone(child);      
    }

    public void Clone(NeatNN original) //become the original
    {
        Connections.Clear();
        Neurons.Clear();


//        Debug.Log("-------CLONING THE NETWORK----------");
        inputIds = new int[original.inputIds.Length];
        outputIds = new int[original.outputIds.Length];
        
        foreach (var pair in original.Neurons)
        {
            this.Neurons[pair.Key] = new Neuron(pair.Value.Id, pair.Value.Type);
            this.Neurons[pair.Key].Bias = pair.Value.Bias;
        }

        foreach (var conn in original.Connections)
        {
            this.Connections.Add(new Connection(conn.From, conn.To, conn.Weight, conn.Enabled, conn.Innovation));
        }

        for(int i = 0; i < inputIds.Length; i++){
//            Debug.Log(i);
            inputIds[i] = original.inputIds[i];
        }

        //Debug.Log("START OUTPUTS");
        for(int i = 0; i < outputIds.Length; i++){
            outputIds[i] = original.outputIds[i];
            //Debug.Log(i + ": output id copied=" + outputIds[i] + ",  output id original=" + original.outputIds[i]);
        }

        ComputeNeuronDepths();
        SortNeuronsByDepth();
        BuildConnectionMap();
    }


    public static NeatNN Crossover(NeatNN parent1, NeatNN parent2, bool isParent1Fitter) 
    {
        GameObject basicAnimal = new GameObject("BaseObject");
        basicAnimal.AddComponent<NeatNN>();
        NeatNN child = basicAnimal.GetComponent<NeatNN>(); 

        System.Array.Resize<int>(ref child.inputIds, parent1.inputIds.Length);
        System.Array.Resize<int>(ref child.outputIds, parent1.outputIds.Length);

        Dictionary<int, Connection> conn1 = new Dictionary<int, Connection>();
        Dictionary<int, Connection> conn2 = new Dictionary<int, Connection>();

        // Индексируем связи по innovation number
        for (int i = 0; i < parent1.Connections.Count; i++)
            conn1[parent1.Connections[i].Innovation] = parent1.Connections[i];
        for (int i = 0; i < parent2.Connections.Count; i++)
            conn2[parent2.Connections[i].Innovation] = parent2.Connections[i];

        // Собираем все innovation'ы
        HashSet<int> allInnovations = new HashSet<int>();
        foreach (var key in conn1.Keys) allInnovations.Add(key);
        foreach (var key in conn2.Keys) allInnovations.Add(key);


        foreach (int innov in allInnovations)
        {
            bool has1 = conn1.ContainsKey(innov);
            bool has2 = conn2.ContainsKey(innov);

            Connection selected;

            if (has1 && has2)
            {
                // Совпадающие: выбрать случайно
                selected = Random.value < 0.5f ? conn1[innov] : conn2[innov];
            }
            else
            {
                // Несовпадающие: брать от лучшего
                if ((has1 && isParent1Fitter) || (has2 && !isParent1Fitter))
                    selected = has1 ? conn1[innov] : conn2[innov];
                else
                    continue; // не наследуем от менее приспособленного
            }

            child.Connections.Add(selected.Clone());
        }


        for(int i = 0; i < parent1.inputIds.Length; i++){ //TODO: переделать без конкретных цифр
            if(isParent1Fitter){
                child.Neurons[parent1.inputIds[i]] = CloneNeuron(parent1.Neurons[parent1.inputIds[i]]);
            } else {
                child.Neurons[parent2.inputIds[i]] = CloneNeuron(parent2.Neurons[parent2.inputIds[i]]);
            }             
        }

        for(int i = 0; i < parent1.outputIds.Length; i++){ //TODO: переделать без конкретных цифр
            if(isParent1Fitter){
                child.Neurons[parent1.outputIds[i]] = CloneNeuron(parent1.Neurons[parent1.outputIds[i]]);
            } else {
                child.Neurons[parent2.outputIds[i]] = CloneNeuron(parent2.Neurons[parent2.outputIds[i]]);
            }            
        }

        //
        for (int i = 0; i < child.Connections.Count; i++)
        {
            int from = child.Connections[i].From;
            int to = child.Connections[i].To;


            if (!child.Neurons.ContainsKey(from))
            {
                Neuron neuron;
                if (parent1.Neurons.ContainsKey(from))
                    neuron = CloneNeuron(parent1.Neurons[from]);
                else
                    neuron = CloneNeuron(parent2.Neurons[from]);

                child.Neurons[from] = neuron;
            }

            if (!child.Neurons.ContainsKey(to))
            {
                Neuron neuron;
                if (parent1.Neurons.ContainsKey(to))
                    neuron = CloneNeuron(parent1.Neurons[to]);
                else
                    neuron = CloneNeuron(parent2.Neurons[to]);

                child.Neurons[to] = neuron;
            }
        }
        
        Destroy(basicAnimal, 1f);
        //Debug.Log(child == null);
        return child;
    }

    private static Neuron CloneNeuron(Neuron original)
    {
        Neuron n = new Neuron(original.Id, original.Type);
        n.Bias = original.Bias;
        return n;
    }

    void Mutate_AllWeights()
    {
        foreach (var conn in Connections)
        {
            conn.Weight += RandomWeight();
        }
    }

    void Mutate_Weight()
    {
        int randConn = Random.Range(0, Connections.Count);
        Connections[randConn].Weight += RandomWeight();
    }


    void Mutate_AllBiases()
    {
        foreach (var neuron in Neurons.Values)
        {
            neuron.Bias += RandomWeight(); 
        }
    }

    void Mutate_Bias()
    {
        int randBias = Random.Range(0, Neurons.Count);
        List<Neuron> neuronList = new List<Neuron>(Neurons.Values);
        
        int idOfRandNeuron = neuronList[randBias].Id;
        Neurons[idOfRandNeuron].Bias += RandomWeight();
    }

    public void AddConnection(Connection connection)
    {
        // 
        foreach (var conn in Connections)
        {
            if (conn.From == connection.From && conn.To == connection.To)
            {
                Debug.LogWarning($"Connection with id {connection.Innovation} already exists in NN.");
                return;
            }
        }

        
        Connections.Add(connection);
    }


    public void Mutate_AddConnection(InnovationTracker tracker, int maxAttempts = 10)
    {
        for (int i = 0; i < maxAttempts; i++)
        {
            int[] neuronIds = new int[Neurons.Count];
            int index = 0;
            foreach (int id in Neurons.Keys)
            {
                neuronIds[index++] = id;
            }

            int fromId = neuronIds[UnityEngine.Random.Range(0, neuronIds.Length)];
            int toId = neuronIds[UnityEngine.Random.Range(0, neuronIds.Length)];

            Neuron fromNeuron = Neurons[fromId];
            Neuron toNeuron = Neurons[toId];

            // Проверка корректности: нельзя соединять выход в вход или создавать петли
            if (fromNeuron.Type == NodeType.Output || toNeuron.Type == NodeType.Input || fromId == toId)
                continue;

            if (HasConnection(fromId, toId))
                continue;

            if (CreatesCycle(fromId, toId))
            {   //Debug.Log("CYCLE!!!!!!!!!!");
                continue;
            }

            float weight = Random.Range(-1f, 1f);

            Connections.Add(new Connection(fromId, toId, weight, true,  tracker.GetInnovation(fromId, toId)));
            break;
        }
    }

    /*Dictionary<int, List<int>> BuildAdjacencyList()
    {
        Dictionary<int, List<int>> graph = new Dictionary<int, List<int>>();

        foreach (var conn in Connections)
        {
            if (!conn.Enabled) continue;

            if (!graph.ContainsKey(conn.From))
                graph[conn.From] = new List<int>();

            graph[conn.From].Add(conn.To);
        }

        return graph;
    }*/


    public bool CreatesCycle(int from, int to)
    {
        // Из включённых соединений
        Dictionary<int, List<int>> graph = new Dictionary<int, List<int>>();

        foreach (var conn in Connections)
        {
            if (!conn.Enabled) continue;

            if (!graph.ContainsKey(conn.From))
                graph[conn.From] = new List<int>();

            graph[conn.From].Add(conn.To);
        }

        // Предположим, что существует связь
        if (!graph.ContainsKey(from))
            graph[from] = new List<int>();
        graph[from].Add(to);

        // Существует ли путь
        HashSet<int> visited = new HashSet<int>();
        return HasPath(to, from, graph, visited);
    }

    private bool HasPath(int current, int target, Dictionary<int, List<int>> graph, HashSet<int> visited)
    {
        if (current == target) return true;
        if (visited.Contains(current)) return false;

        visited.Add(current);

        if (!graph.ContainsKey(current)) return false;

        foreach (int neighbor in graph[current])
        {
            if (HasPath(neighbor, target, graph, visited))
                return true;
        }

        return false;
    }

    public bool HasConnection(int fromId, int toId)
    {
        for (int i = 0; i < Connections.Count; i++)
        {
            Connection conn = Connections[i];
            if (conn.From == fromId && conn.To == toId)
            {
                return true;
            }
        }

        return false;
    }

    public void Mutate_ReenableConnection() //TODO: through id of connection
    {
        foreach (var conn in Connections)
        {
            if (!conn.Enabled && Random.value < 0.01f)
            {
                conn.Enabled = true;
            }
        }
    }


    public void Mutate_DisableConnection() //except basic input output?
    {
        foreach (var conn in Connections)
        {
            if (conn.Enabled && Random.value < 0.01f)
            {
                conn.Enabled = false;
            }
        }
    }


    public void Mutate_RemoveNeuron()
    {
        //
        List<int> hiddenNeuronIds = new List<int>();
        foreach (var neuron in Neurons)
        {
            if (neuron.Value.Type == NodeType.Hidden)
            {
                hiddenNeuronIds.Add(neuron.Key);
            }
        }

       
        if (hiddenNeuronIds.Count == 0)
            return;

    
        int index = Random.Range(0, hiddenNeuronIds.Count);
        int neuronIdToRemove = hiddenNeuronIds[index];

        
        for (int i = Connections.Count - 1; i >= 0; i--)
        {
            Connection conn = Connections[i];
            if (conn.From == neuronIdToRemove || conn.To == neuronIdToRemove)
            {
                Connections.RemoveAt(i);
            }
        }

        
        Neurons.Remove(neuronIdToRemove);
    }


    public int AddNeuron(Neuron neuron)
    {
        if (!Neurons.ContainsKey(neuron.Id))
        {
            Neurons.Add(neuron.Id, neuron);
            return neuron.Id;
        }
        else
        {
            Debug.LogWarning($"Neuron with id {neuron.Id} already exists in NN.");
            return -1;
        }
    }


    public void AddNeuron(Connection connectionToSplit, InnovationTracker innovationTracker)
    {
       
        connectionToSplit.Enabled = false;


        int newNeuronId = innovationTracker.GetNextNodeId();
        Neuron newNeuron = new Neuron(newNeuronId, NodeType.Hidden);
        newNeuron.Bias = Random.Range(-0.1f, 0.1f); //mutation for bias

        Neurons.Add(newNeuronId, newNeuron);

        int innovation1 = innovationTracker.GetInnovation(connectionToSplit.From, newNeuronId);
        int innovation2 = innovationTracker.GetInnovation(newNeuronId, connectionToSplit.To);

        Connection conn1 = new Connection(connectionToSplit.From, newNeuronId, 1.0f, true, innovation1);
        Connection conn2 = new Connection(newNeuronId, connectionToSplit.To, connectionToSplit.Weight, true, innovation2);

        //Debug.Log("New neurons: " + newNeuronId + ", connected from " + connectionToSplit.From + ", to " + connectionToSplit.To);
        //Debug.Log("------------------------");

        Connections.Add(conn1);
        Connections.Add(conn2);
    }

    public void Mutate_AddNeuron(InnovationTracker tracker)
    {
        Connection target = GetRandomEnabledConnection();
        if (target == null)
            return;

        AddNeuron(target, tracker);
    }

    public Connection GetRandomEnabledConnection()
    {
        List<Connection> enabledConnections = new List<Connection>();
            
        for (int i = 0; i < Connections.Count; i++)
        {
            if (Connections[i].Enabled)
            {
                enabledConnections.Add(Connections[i]);
            }
        }

        if (enabledConnections.Count == 0)
            return null;

        int index = Random.Range(0, enabledConnections.Count);
        return enabledConnections[index];
    }


    public void ComputeNeuronDepths()
    {
        Dictionary<int, List<int>> graph = new(); //
        Dictionary<int, int> indegree = new();

        foreach (var neuron in Neurons.Keys)
        {
            graph[neuron] = new List<int>();
            indegree[neuron] = 0;
        }

        foreach (var conn in Connections)
        {
            if (!conn.Enabled) continue;
            graph[conn.From].Add(conn.To);
            indegree[conn.To]++;
        }

        Queue<int> queue = new Queue<int>();

        foreach (var kvp in indegree)
            if (kvp.Value == 0)
                queue.Enqueue(kvp.Key);

        Dictionary<int, int> depth = new();

        while (queue.Count > 0)
        {
            int current = queue.Dequeue();
            int currentDepth = depth.ContainsKey(current) ? depth[current] : 0;

            foreach (var neighbor in graph[current])
            {
                depth[neighbor] = Mathf.Max(depth.GetValueOrDefault(neighbor, 0), currentDepth + 1);
                indegree[neighbor]--;

                if (indegree[neighbor] == 0)
                    queue.Enqueue(neighbor);
            }
        }

        foreach (var neuron in Neurons)
            neuron.Value.Depth = depth.GetValueOrDefault(neuron.Key, 0);
    }
    

    /*public float[] FeedForward(float[] inputs)
    {
        Connection conn;
        Neuron fromNeuron;
        Neuron toNeuron;

        int generation = this.GetComponent<Animal>().generation;

        foreach (var neuron in Neurons.Values)
        {
            neuron.Value = neuron.Bias; //= 0;
        }
       // Debug.Log(" ");
       // Debug.Log("CHECK INPUTS");

        for (int i = 0; i < inputIds.Length && i < inputs.Length; i++)
        {
            Neurons[inputIds[i]].Value = inputs[i];
            //Debug.Log(inputs[i]);
        }

       // Debug.Log("INPUT CHECK END");
       // Debug.Log(" ");

        for (int i = 0; i < Connections.Count; i++)
        {
            conn = Connections[i];
            if (!conn.Enabled) continue;

            fromNeuron =  Neurons[conn.From];
            toNeuron = Neurons[conn.To];
            toNeuron.Value += fromNeuron.Value * conn.Weight;

        }

        

       float[] outputs = new float[outputIds.Length]; 

        for (int i = 0; i < outputIds.Length; i++)
        {
            outputs[i] = Sigmoid(Neurons[outputIds[i]].Value);
        }

        return outputs;
    }*/


    public float[] FeedForward(float[] inputs)
    {
        
        foreach (var neuron in Neurons.Values)
        {
            neuron.Value = neuron.Bias; 
        }

       
        for (int i = 0; i < inputIds.Length && i < inputs.Length; i++)
        {
            Neurons[inputIds[i]].Value = inputs[i];
        }

        
        foreach (var neuron in sortedNeurons)
        {
            if (!outgoingConnections.TryGetValue(neuron.Id, out var conns)) continue;

            foreach (var conn in conns)
            {
                if (!conn.Enabled) continue;

                Neurons[conn.To].Value += neuron.Value * conn.Weight;
            }
        }

        
        float[] outputs = new float[outputIds.Length];

        for (int i = 0; i < outputIds.Length; i++)
        {
            outputs[i] = Sigmoid(Neurons[outputIds[i]].Value);
        }

        return outputs;
    }


    void CheckFeedForward(){
        Debug.Log("CHECK FF--------------------------------");
        foreach (var neuron in Neurons.Values)
        {
            Debug.Log(neuron.Id + " " + neuron.Value);
        }

        Debug.Log("FF CHECK END--------------------------------");
    }

    float Sigmoid(float x) => (1.0f / (1.0f + Mathf.Exp(-x)));
    float RandomWeight() => (Random.Range(-1.0f, 1.0f) * 0.5f);


    public NeatNNData ToData()
    {
        var data = new NeatNNData();

        data.inputIds = new int[inputIds.Length];
        data.outputIds = new int[outputIds.Length];

        for(int i = 0; i < inputIds.Length; i++){
            data.inputIds[i] = inputIds[i];
        }

        
        for(int i = 0; i < outputIds.Length; i++){
            data.outputIds[i] = outputIds[i];
        }

        foreach (var neuron in Neurons.Values)
        {
            data.neurons.Add(new NeuronData
            {
                id = neuron.Id,
                type = neuron.Type,
                bias = neuron.Bias
            });
        }

        foreach (var conn in Connections)
        {
            data.connections.Add(new ConnectionData
            {
                from = conn.From,
                to = conn.To,
                weight = conn.Weight,
                enabled = conn.Enabled,
                innovation = conn.Innovation
            });
        }

        return data;
    }

    public static NeatNN FromDataStatic(NeatNNData data)
    {
        NeatNN NEATnn = new NeatNN();

        NEATnn.inputIds = new int[data.inputIds.Length];
        NEATnn.outputIds = new int[data.outputIds.Length];

        for(int i = 0; i < data.inputIds.Length; i++){
            NEATnn.inputIds[i] = data.inputIds[i];
        }

        
        for(int i = 0; i < data.outputIds.Length; i++){
            NEATnn.outputIds[i] = data.outputIds[i];
        }

        foreach (var n in data.neurons)
        {
            NEATnn.AddNeuron(new Neuron(n.id, n.type, n.bias));
        }

        foreach (var c in data.connections)
        {
            NEATnn.AddConnection(new Connection(c.from, c.to, c.weight, c.enabled, c.innovation));
        }

        return NEATnn;
    }

    public void FromData(NeatNNData data)
    {

        this.inputIds = new int[data.inputIds.Length];
        this.outputIds = new int[data.outputIds.Length];

        for(int i = 0; i < data.inputIds.Length; i++){
            this.inputIds[i] = data.inputIds[i];
        }

        
        for(int i = 0; i < data.outputIds.Length; i++){
            this.outputIds[i] = data.outputIds[i];
        }

        foreach (var n in data.neurons)
        {
            this.AddNeuron(new Neuron(n.id, n.type, n.bias));
        }

        foreach (var c in data.connections)
        {
            this.AddConnection(new Connection(c.from, c.to, c.weight, c.enabled, c.innovation));
        }

        ComputeNeuronDepths();
        SortNeuronsByDepth();
        BuildConnectionMap();
    }


}
