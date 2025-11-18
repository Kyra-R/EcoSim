using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NN : MonoBehaviour
{
    int [] networkShape = {8,5};
    public Layer [] layers;

    // Awake is called when the script instance is being loaded.
    // Start is called before the first frame update.
    // Awake gets called before Start which is why we use Awake here
    public void Awake()
    {
        Random.InitState((int)System.DateTime.Now.Ticks);
        /*layers = new Layer[networkShape.Length - 1];

        for(int i = 0; i < layers.Length; i++)
        {
            layers[i] = new Layer(networkShape[i], networkShape[i+1]);
        }*/

        //This ensures that the random numbers we generate aren't the same pattern each time. 
    }

    public void Start(){
        layers = new Layer[networkShape.Length - 1];

        for(int i = 0; i < layers.Length; i++)
        {
            layers[i] = new Layer(networkShape[i], networkShape[i+1]);
        }

    }


    public void CreateOffspringNetwork(NN a, NN b, float mutationChance, float mutationAmount){
        if(Random.value < 0.5f){
            this.layers = a.copyLayers();
        } else {
            this.layers = b.copyLayers();
        }
        this.MutateNetwork(mutationChance, mutationAmount);
    }

    //This function is used to feed forward the inputs through the network, and return the output, which is the decision of the network, in this case, the direction to move in.
    public float[] Brain(float [] inputs)
    {
        for(int i = 0; i < layers.Length; i++)
        {
            if(i == 0)
            {
                layers[i].Forward(inputs);
                //layers[i].Activation();
            } 
            else if(i == layers.Length - 1)
            {
                layers[i].Forward(layers[i - 1].nodeArray);
            }
            else
            {
                layers[i].Forward(layers[i - 1].nodeArray);
                //layers[i].Activation();
            }    
        }

        return(layers[layers.Length - 1].nodeArray);
    }

    //This function is used to copy the weights and biases from one neural network to another.
    public Layer[] copyLayers()
    {
        Layer[] tmpLayers = new Layer[networkShape.Length - 1];
        for(int i = 0; i < layers.Length; i++)
        {
            tmpLayers[i] = new Layer(networkShape[i], networkShape[i+1]);
            System.Array.Copy (layers[i].weightsArray, tmpLayers[i].weightsArray, layers[i].weightsArray.GetLength(0)*layers[i].weightsArray.GetLength(1));
            System.Array.Copy (layers[i].biasesArray, tmpLayers[i].biasesArray, layers[i].biasesArray.GetLength(0));
        }
        return(tmpLayers);
    }

    public class Layer
    {
        //attributes, variables and properties of the class Layer
        public float[,] weightsArray;
        public float[] biasesArray;
        public float [] nodeArray;

        public int n_inputs;
        public int n_neurons;

        //constructor, this is called when we create a new layer, it sets the number of inputs and nodes, and creates the arrays.
        public Layer(int n_inputs, int n_neurons)
        {
            this.n_inputs = n_inputs;
            this.n_neurons = n_neurons;

            weightsArray = new float [n_neurons, n_inputs]; //give random values
            for(int i = 0; i < n_neurons; i++){
                 for(int j = 0; j < n_inputs; j++){
                    //weightsArray[i, j] = Random.Range(-1.0f, 1.0f);
                    weightsArray[i, j] = 0;
                }
            }
            biasesArray = new float[n_neurons];
            for(int i = 0; i < n_neurons; i++){
                //biasesArray[i] = Random.value;
                biasesArray[i] = 0;
            }

            //if() check if first gen
            MiddleLayer();
        }

        public void MiddleLayer(){ //leave just one for now?
            //specific weights and biases
            
            //hunger
            weightsArray[1, 1] = 1f;
            weightsArray[1, 2] = 0.05f;
            weightsArray[1, 5] = 0.1f; 
            biasesArray[1] = 0f;

            //thirst
            weightsArray[2, 2] = 1f;
            weightsArray[2, 5] = 0.05f;
            weightsArray[2, 6] = 0.1f;
            biasesArray[2] = 15f;

            //mate search
            weightsArray[3, 3] = -0.5f;
            weightsArray[3, 4] = 100f;
            weightsArray[3, 7] = -20f;
            biasesArray[3] = 0;

            //rest
            weightsArray[4, 3] = 1f;
            biasesArray[4] = 5f;

            //wander
            weightsArray[0, 4] = -15f;
            weightsArray[0, 7] = 90f;
            biasesArray[0] = 25f;

        }

        //forward pass, takes in an array of inputs and returns an array of outputs, which is then used as the input for the next layer, and so on, until we get to the output layer, which is returned as the output of the network.
        public void Forward(float [] inputsArray)
        {
            nodeArray = new float [n_neurons];

            for(int i = 0;i < n_neurons ; i++)
            {
                //sum of weights times inputs
                for(int j = 0; j < n_inputs; j++)
                {
                    nodeArray[i] += weightsArray[i,j] * inputsArray[j];
                }

                //add the bias
                nodeArray[i] += biasesArray[i];
//                Debug.Log(nodeArray[i]);
            }
        }

        //This function is the activation function for the neural network uncomment the one you want to use.
        public void Activation()
        {
            // //leaky relu function
            // for(int i = 0; i < nodeArray.Length; i++)
            // {
            //     if(nodeArray[i] < 0)
            //     {
            //         nodeArray[i] = nodeArray[i]/10;
            //     }
            // }


            //sigmoid function
            // for(int i = 0; i < nodeArray.Length; i++)
            // {
             //   nodeArray[i] = 1/(1 + Mathf.Exp(-nodeArray[i]));
            //}

            //tanh function
            for(int i = 0; i < nodeArray.Length; i++)
            {
                nodeArray[i] = (float)System.Math.Tanh(nodeArray[i]);
                Debug.Log(nodeArray[i] + "--------ACTIVATION");
            }

            // //relu function
            // for(int i = 0; i < nodeArray.Length; i++)
            // {
            //     if(nodeArray[i] < 0)
            //     {
            //         nodeArray[i] = 0;
            //     }
            // }
        }

        //This is used to randomly modify the weights and biases for the Evolution Sim and Genetic Algorithm.
        public void MutateLayer(float mutationChance, float mutationAmount)
        {
            for(int i = 0; i < n_neurons; i++)
            {
                for(int j = 0; j < n_inputs; j++)
                {
                    if(Random.value < mutationChance)
                    {
                        weightsArray[i,j] += Random.Range(-1.0f, 1.0f)*mutationAmount; //check so no lower than 0
                    }
                }

                if(Random.value < mutationChance)
                {
                    biasesArray[i] += Random.Range(-1.0f, 1.0f)*mutationAmount;
                }
            }
        }
    }


    //Call the randomness function for each layer in the network.
    public void MutateNetwork(float mutationChance, float mutationAmount)
    {
        Debug.Log("NetowrkMutated " + this.gameObject.name);
        for(int i = 0; i < layers.Length; i++)
        {
            layers[i].MutateLayer(mutationChance, mutationAmount);
        }
    }
}