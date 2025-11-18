using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;
 
public class CreatureMover : MonoBehaviour //TODO: following, growing, hunting behaviour (search_food is target choosing stage), remake WANDERing
{ //also predators get input about health and state of the prey


   // float worldBorder = 20f; //TODO: move to animal controller
    public float maxSpeed = 3f; // The maxium speed can reach
    public float accelerationTime = 4f;

    public float speed; //with modifier applied
    public float visionRadius; //with modifier applied
 
    private Rigidbody2D rigidBody;
    private float timeBetweenChangeDirection;
    
    private Vector2 movement;

    private float lookAroundTime = 0.25f;

    private Collider2D target; //remain target as coordinates

    public float targetEvaluation = 0;

    private Vector2 movementTarget = Vector2.zero;

    //for finding food/water/enemies/mates
    public LayerMask foodMask;

    public LayerMask waterMask;

    public LayerMask animalMask; //this given species, myght be animalMask, might be carnivoreAnimal


    //For behavior - given by parent?
    public States state = States.WANDER;

    EnvironmentController environmentController;

    EyeScript eye;

    bool OnCenterMoving = false;

    bool[] wanderingDirection;

 
    // Start is called before the first frame update
    void Awake(){
        wanderingDirection = new bool[8];
        SetFirstRandomMoveDirection();
        eye = gameObject.GetComponent<EyeScript>();
        environmentController = GameObject.Find("EnvironmentController").GetComponent<EnvironmentController>();
    }

    void SetFirstRandomMoveDirection(){
        for(int i = 0; i < wanderingDirection.Length; i++){
            wanderingDirection[i] = false;
        }
        int index = Random.Range(0, 8);
//        Debug.Log(index);
        wanderingDirection[index] = true;
    }

    int NextIndex(int index, int change){
        if(index + change >= 0){
            return (index + change) % wanderingDirection.Length;
        } else {
            return (wanderingDirection.Length + index + change) % wanderingDirection.Length;
        }
    }

    void ChangeRandomMoveDirection(){
        int index = 0;
        for(int i = 0; i < wanderingDirection.Length; i++){
            if(wanderingDirection[i]){
                index = i;
                break;
            }
        }
        float newMovement = Random.value;

        //Debug.Log("CHANGE DIRECTION " + newMovement);

        int clockwise = 1;
        if(Random.value < 0.5f){
            clockwise = -1;
        }

        if(newMovement >= 0.63f){
            return;
        } else
        if(newMovement >= 0.3f){
            wanderingDirection[NextIndex(index, clockwise)] = true;
            wanderingDirection[index] = false;
        } else
        if(newMovement >= 0.15f){
            wanderingDirection[NextIndex(index, clockwise * 2)] = true;
            wanderingDirection[index] = false;
        } else
        if(newMovement >= 0.05f){
            wanderingDirection[NextIndex(index, clockwise * 3)] = true;
            wanderingDirection[index] = false;
        } else {
            wanderingDirection[NextIndex(index, clockwise * 4)] = true;
            wanderingDirection[index] = false;
        }
    }

    Vector2 getVectorForRandomMove(){
        int index = 0;
        for(int i = 0; i < wanderingDirection.Length; i++){
            if(wanderingDirection[i]){
                index = i;
                break;
            }
        }

//        Debug.Log(index);

        switch (index){
            case 0:
                return new Vector2(transform.position.x, transform.position.y + visionRadius);
            case 1:
                return new Vector2(transform.position.x + visionRadius, transform.position.y + visionRadius);
            case 2:
                return new Vector2(transform.position.x + visionRadius, transform.position.y);
            case 3:
                return new Vector2(transform.position.x + visionRadius, transform.position.y - visionRadius);
            case 4:
                return new Vector2(transform.position.x, transform.position.y - visionRadius);
            case 5:
                return new Vector2(transform.position.x - visionRadius, transform.position.y - visionRadius);
            case 6:
                return new Vector2(transform.position.x - visionRadius, transform.position.y);
            case 7:
                return new Vector2(transform.position.x - visionRadius, transform.position.y + visionRadius);
            default:
                return new Vector2(transform.position.x, transform.position.y);
        }

    }


    void NewRandomMovementTarget(){
        ChangeRandomMoveDirection();
        movementTarget = getVectorForRandomMove();
    }

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();

    }

    public void SetVisionAndSpeed(float visionWithMod, float speedWithMod){
        visionRadius = visionWithMod;
        speed = speedWithMod;
    }

    public void SetSpeed(float speedWithMod)
    {
        speed = speedWithMod;
    }

    public void setState(States state){
        if(state != this.state && !OnCenterMoving){

            if(!(this.state == States.SEARCH_FOOD && state == States.HUNT)){
               // Debug.Log("current state: " + this.state + "Target to null, now " + state);
                this.target = null;
                this.movementTarget = transform.position; 
            }

            if(state == States.REST || this.state == States.REST){
                eye.OpenCloseEye();
            }


            this.state = state;
          
            if(state == States.GO_CENTER){
                this.movementTarget = Vector2.zero;
                OnCenterMoving = true;
                Invoke("StopMovingCenter", accelerationTime*3);

            }
        }
  
        //Debug.Log(this.name + "-----------NEW STATE:" + this.state + "-----------");
        //Debug.Log("Movement target: " + this.movementTarget.x + " " + this.movementTarget.y);
    }


    public void stopMoving(){
        this.target = null;
        this.movementTarget = Vector2.zero;
        //Debug.Log("Movement stop");
    }

    public void moveAway(){
        this.target = null;
        this.movementTarget = new Vector2(0.1f, 0.1f);
        //Debug.Log("Moving away");
    }

    void StopMovingCenter(){
   //     Debug.Log("Stopped moving center--------------------------!");
        OnCenterMoving = false;
        setState(States.WANDER);
    }

    public Collider2D getTarget(){
        return target;
    }


    public LayerMask getFoodMask(){
        return this.foodMask;
    }

    public LayerMask getWaterMask(){
        return this.waterMask;
    }

    public LayerMask getAnimalMask(){
        return this.animalMask;
    }

    public States getState(){
        return state;
    }



    //for finding food/water/enemies/mates
    public void LookAround(){
        countSignals++;

        if(state == States.SEARCH_FOOD){
            FindVisibleFood();
        }
        if(state == States.SEARCH_WATER){
            FindVisibleWatersource();
        }
        if(state == States.SEARCH_MATE){
            FindMate();
            if(target != null){
                CheckMate();
            }
        }
        if(state == States.FLEE){
            FindFleeingDirection();
        }
        if(state == States.HUNT){
            AttackPriorityPrey();
        }

        if(Vector2.Distance(transform.position, Vector2.zero) > environmentController.worldBorder){
            setState(States.GO_CENTER);
        }
    }

    Collider2D FindNearest(Collider2D[] targetsInvisionRadius){
        Collider2D res = null;
        float minDistance = environmentController.worldBorder * 2;
        for(int i=0;i<targetsInvisionRadius.Length;i++){
            if(Vector2.Distance(targetsInvisionRadius[i].transform.position, this.transform.position) < minDistance && 
            targetsInvisionRadius[i].gameObject.name !=  this.name){
                minDistance = Vector2.Distance(targetsInvisionRadius[i].transform.position, this.transform.position);
                res = targetsInvisionRadius[i];
            }

        }
//        Debug.Log("++++++++++++++++++++++++");
//        Debug.Log(res == null);
        return res;
    }

    float FindDistanceToNearest(Collider2D[] targetsInvisionRadius){
        
        float minDistance = environmentController.worldBorder * 2;
        for(int i=0;i<targetsInvisionRadius.Length;i++){
            if(Vector2.Distance(targetsInvisionRadius[i].transform.position, this.transform.position) < minDistance && 
            targetsInvisionRadius[i].gameObject.name !=  this.name){
                minDistance = Vector2.Distance(targetsInvisionRadius[i].transform.position, this.transform.position);
            }

        }
        return minDistance;
    }


    public float FindIfHuntingTargetFoundNN(){
        if(state == States.SEARCH_FOOD && target != null){
            return 1;
        } else {
            return 0;
        }
    }


        public float FindIfHuntingTargetFoundNN2(){ //calculate "priority" based on: other prey nearby, health, hunger, distance //or transfer it to NN?
        if(state == States.SEARCH_FOOD && target != null){
            return 1;
        } else {
            return 0;
        }
    }


    void FindFleeingDirection(){
        Collider2D[] predators = Physics2D.OverlapCircleAll(transform.position, visionRadius, LayerMask.GetMask("CarnivoreAnimal"));
        target =  FindNearestHuntingPredator(predators);
        if(target != null){
            movementTarget = new Vector2(transform.position.x * 2 - target.transform.position.x, transform.position.y * 2 - target.transform.position.y);
        } else {
            target = FindNearest(predators);
            if(target != null){
                movementTarget = new Vector2(transform.position.x * 2 - target.transform.position.x, transform.position.y * 2 - target.transform.position.y);
            }
        }
    }


    Collider2D FindNearestHuntingPredator(Collider2D[] predatorsInSight){ 
        Collider2D res = null;
        float minDistance = visionRadius;

        for(int i=0;i<predatorsInSight.Length;i++){
            if(Vector2.Distance(predatorsInSight[i].transform.position, this.transform.position) < minDistance && 
            predatorsInSight[i].gameObject.GetComponent<CreatureMover>().getState() == States.HUNT &&
            predatorsInSight[i].gameObject.name !=  this.name){
                minDistance = Vector2.Distance(predatorsInSight[i].transform.position, this.transform.position);
                res = predatorsInSight[i];
            }

        }
        return res;
    }


   /* public float FindDistanceToFoodNN(){
        Collider2D[] targetsInvisionRadius = Physics2D.OverlapCircleAll(transform.position, visionRadius, foodMask);
            if(targetsInvisionRadius.Length > 0){
                return FindDistanceToNearest(targetsInvisionRadius);
            } else {
                return environmentController.worldBorder * 2;
            }
    }*/

    public float FindPriorityDistanceFoodNN(){ //works for both carnivores and herbivores
        Collider2D[] targetsInvisionRadius = Physics2D.OverlapCircleAll(transform.position, visionRadius, foodMask);
            if(targetsInvisionRadius.Length > 0){
                return 1.0f - FindDistanceToNearest(targetsInvisionRadius) / visionRadius;
            } else {
                return 0;
            }
    }

    public float FindPriorityDistanceWaterNN(){
        Collider2D[] targetsInvisionRadius = Physics2D.OverlapCircleAll(transform.position, visionRadius, waterMask);
            if(targetsInvisionRadius.Length > 0){
                return 1.0f - FindDistanceToNearest(targetsInvisionRadius) / visionRadius;
            } else {
                return 0;
            }    
    }


    public float FindPriorityDistancePredatorNN(){//for prey animals
        Collider2D[] targetsInvisionRadius = Physics2D.OverlapCircleAll(transform.position, visionRadius, LayerMask.GetMask("CarnivoreAnimal"));
            if(targetsInvisionRadius.Length > 0){
                return 1.0f - FindDistanceToNearest(targetsInvisionRadius) / visionRadius; //fD / vs = 1 if at the edge of vision
            } else {
                return 0; //no predators or too far
            }  
    }


    //public float FindDistanceToNearestPredator(){ //for prey animals

    //}


    public float FindIfHuntingPredatorsInSightNN(){ //for prey animals
        Collider2D[] targetsInvisionRadius = Physics2D.OverlapCircleAll(transform.position, visionRadius, LayerMask.GetMask("CarnivoreAnimal"));
        for(int i = 0; i < targetsInvisionRadius.Length; i++){
            if(targetsInvisionRadius[i].gameObject.GetComponent<CreatureMover>().getState() == States.HUNT){
                return 1.0f;
            }
        }
        return -1.0f;
    }
    //for carnivores. - first seek food. If there is prey around, then nn signals state HUNT, then choose priority prey after the hunt.

    //for herbivores - check for closest carnivore every whatever sec, move away from target during FLEE

    public void FindPriorityPrey(){ //for carnivores
        Collider2D[] targetsInvisionRadius = Physics2D.OverlapCircleAll(transform.position, visionRadius, foodMask);
        target = FindNearest(targetsInvisionRadius);
        if(target != null){
            movementTarget = target.transform.position;
        } else {
            setState(States.SEARCH_FOOD);
        }
    }

    public void AttackPriorityPrey(){
        if(target != null && Vector2.Distance(target.transform.position, this.transform.position) <= visionRadius){
            movementTarget = target.transform.position;
//            Debug.Log(targetEvaluation + " evalPrey, name " + target.gameObject.name);
        } else {
//            Debug.Log("Sight of prey lost");
            //setState(States.SEARCH_FOOD);
            targetEvaluation = 0;
            //Debug.Log("Sight of prey lost");
        }
    }

    public float FindPopulationDensityNN(){
        Collider2D[] targetsPopulationDenRadius = Physics2D.OverlapCircleAll(transform.position, 5f, animalMask);
//        Debug.Log(targetsInvisionRadius.Length / 15f);
        return targetsPopulationDenRadius.Length / 5f;
    }


    public float FindPriorityDistanceClosestAnimalNN(){
        Collider2D[] targetsInvisionRadius = Physics2D.OverlapCircleAll(transform.position, visionRadius, animalMask);
            if(targetsInvisionRadius.Length > 0){
                return 1.0f - FindDistanceToNearest(targetsInvisionRadius) / visionRadius;
            } else {
                return 0;
            }  
    }


    public float FindPopulationDensityNearMovementTarget(){ //TODO
        return 0;
    }

    public float FindPopulationDensityNearClosestAnimalNN(){ //to check at mate or prey?
        Collider2D[] targetsPopulationDenRadius = null;
//        Debug.Log("=================");
//        Debug.Log(null == Physics2D.OverlapCircleAll(transform.position, visionRadius, animalMask | LayerMask.GetMask("CarnivoreAnimal")));
        Collider2D nearestAnimal = FindNearest(Physics2D.OverlapCircleAll(transform.position, visionRadius, animalMask | LayerMask.GetMask("CarnivoreAnimal"))); //add carnivores to the list
        if(nearestAnimal != null){
            targetsPopulationDenRadius = Physics2D.OverlapCircleAll(nearestAnimal.transform.position, 5f, animalMask);
            return targetsPopulationDenRadius.Length / 5f;
        } else {
            return 0;
        }  
    }



    public float GetPreyEvaluationNN(){
        return targetEvaluation;
    }

    public float GetPreyHealth(){
         if(target != null && IsInLayerMask(target.gameObject.layer, getFoodMask())){
            return target.gameObject.GetComponent<Animal>().GetPercentOfLostHealth();
        } else {
            return 0;
        }
    }

    public float GetPreyDistance(){
        if(target != null && IsInLayerMask(target.gameObject.layer, getFoodMask())){
            Vector2.Distance(target.transform.position, this.transform.position);
            return 1.0f - Vector2.Distance(target.transform.position, this.transform.position) / visionRadius;
        } else {
            return 0;
        }
    }

    public float EvaluateSizeDifferenceNN()
    {
        if(target != null && IsInLayerMask(target.gameObject.layer, getFoodMask())){
            
            float x = target.transform.localScale.x / transform.localScale.x;

//            Debug.Log(Mathf.Exp( - Mathf.Pow(x - 1.0f, 2.0f)));

            return Mathf.Exp( - Mathf.Pow(x - 1.0f, 2.0f));
        } else {
            return 0;
        }
    }

    public void SetPreyEvaluation(float val){
        if(target != null && IsInLayerMask(target.gameObject.layer, getFoodMask())){
            targetEvaluation = val;
        } else {
            targetEvaluation = 0;
        }
    }

    public static bool IsInLayerMask(GameObject obj, LayerMask mask) => (mask.value & (1 << obj.layer)) != 0;
    public static bool IsInLayerMask(int layer, LayerMask mask) => (mask.value & (1 << layer)) != 0;


    void FindVisibleFood(){ //FindVisibleHerbFood
        Collider2D[] targetsInvisionRadius;
        //if(LayerMask.GetMask("Herbfoood") == foodMask){
        if(gameObject.GetComponent<Animal>().isHerbivore){
            if(target == null){
                targetsInvisionRadius = Physics2D.OverlapCircleAll(transform.position, visionRadius, foodMask);
                if(targetsInvisionRadius.Length > 0){
                    target = FindNearest(targetsInvisionRadius);
                    movementTarget = target.transform.position;
                } else if (movementTarget.y == transform.position.y  || (movementTarget.x == transform.position.x)){ 
                    //change to timer instead of position/ because ponds

                NewRandomMovementTarget();
                }
            } else {
                movementTarget = target.transform.position;
            }
        } else {
            //Debug.Log("Searching for prey");
            if(target == null){
                targetsInvisionRadius = Physics2D.OverlapCircleAll(transform.position, visionRadius, foodMask);
                if(targetsInvisionRadius.Length > 0){
                    target = FindNearest(targetsInvisionRadius);
                    movementTarget = target.transform.position;
                } else if (movementTarget.y == transform.position.y  || (movementTarget.x == transform.position.x)){ 
                    //change to timer instead of position/ because ponds

                NewRandomMovementTarget();
                }
            //Debug.Log("Target null");
            } else {
                NewRandomMovementTarget();
//                Debug.Log("Target not null " + target.gameObject.name + " " + targetEvaluation + ", " + LayerMask.LayerToName(target.gameObject.layer)); //why targets fruits
            //    Debug.Log(gameObject.GetComponent<Animal>().isHerbivore + " is herbivore?");
            }
        }


    }



    void FindVisibleWatersource(){
         Collider2D[] targetsInvisionRadius;
       // if(target == null){
       // if(movementTarget == Vector2.zero){
        if(target == null){
            targetsInvisionRadius = Physics2D.OverlapCircleAll(transform.position, visionRadius, waterMask);
            if(targetsInvisionRadius.Length > 0){
                target = FindNearest(targetsInvisionRadius);
                movementTarget = target.transform.position;
            } else if (movementTarget.y == transform.position.y  || (movementTarget.x == transform.position.x)){ //change to timer instead of position/ because ponds
               
               //Debug.Log("Problem?");
               
               NewRandomMovementTarget();
               
            }
        } else {
           
            movementTarget = target.transform.position;
        }
       // else { 

       // }
    }

    void FindMate(){ //find all with layer mask animal, then sort by species, then by state search_mate. Also request to other?
    //TODO: criteria for female preference?
        Collider2D[] targetsInvisionRadius;
        bool searchFemale = !this.gameObject.GetComponent<Animal>().female; //todo: get species and sex into other class?


        if(target == null){
            targetsInvisionRadius = Physics2D.OverlapCircleAll(transform.position, visionRadius, animalMask); //TODO done: DETECTS ITSELF, SO CREATURE STOPS IN ITS OWN COORDINATES
            if(targetsInvisionRadius.Length > 1){ //check min radius if present in verlapCircleAll //TODO: add species filter

                //Debug.Log("=====================================");

                for(int i=0;i<targetsInvisionRadius.Length;i++){
                    //Debug.Log(this.name + " Creature found: " + targetsInvisionRadius[i].gameObject.name);

                    //currObject = targetsInvisionRadius[i].gameObject;
                    if(targetsInvisionRadius[i].gameObject.name !=  this.name &&
                    targetsInvisionRadius[i].gameObject.GetComponent<Animal>().female == searchFemale &&
                    targetsInvisionRadius[i].gameObject.GetComponent<CreatureMover>().getState() == States.SEARCH_MATE &&
                    targetsInvisionRadius[i].gameObject.GetComponent<CreatureMover>().interactTargetMate(this.GetComponent<Collider2D>())){
                        //if(target.gameObject.GetComponent<CreatureMover>().interactTargetMate(this.GetComponent<Collider2D>())){
                            target = targetsInvisionRadius[i];
                            movementTarget = target.transform.position;
                          //  Debug.Log("Target is " + target.gameObject.name + " position: " + target.transform.position);
                            targetsInvisionRadius = null;
                            break;
                        //}
                        //target.gameObject.GetComponent<CreatureMover>().interactTargetMate(this.GetComponent<Collider2D>()); //TODO: "propose" through interactTargetMate
                        //break;
                    }
                }
//bringing it here because if same sex they stuck
                if (movementTarget == Vector2.zero || (movementTarget.x == transform.position.x)){ //change to timer instead of position/ because ponds
                         
                    NewRandomMovementTarget();

                }


            }  else if (movementTarget == Vector2.zero || (movementTarget.x == transform.position.x)){ //change to timer instead of position/ because ponds
               
               NewRandomMovementTarget();
               
            }
        } else {
            movementTarget = target.transform.position;
        }


    }

    void CheckMate(){
       // Debug.Log(this.name + " Checked mate " + target.gameObject.name + ", position: " + target.transform.position);
        if(target.gameObject.GetComponent<CreatureMover>().getState() != States.SEARCH_MATE){
            target = null;
            movementTarget = Vector2.zero;
        } else {
            movementTarget = target.transform.position;
            if(target.GetComponent<Animal>().isHerbivore != gameObject.GetComponent<Animal>().isHerbivore)
            Debug.Log("=========PROBLEM====================");
        }
    }


    public bool interactTargetMate(Collider2D mate){  //this is reaction as recepient of invitation
   // Debug.Log(this.name + " Interacting with "+ mate.gameObject.name);
        if(state == States.SEARCH_MATE && target == null){
            target = mate;
            movementTarget = target.transform.position;
            return true;
        } else {
            return false;
        }
    }



     void OnCollisionStay2D(Collision2D col)
    {
        NewRandomMovementTarget();
        
        //movementTarget = new Vector2(transform.position.x + RandomMove(), transform.position.y + RandomMove());
        timeBetweenChangeDirection = accelerationTime;
    }


 
    int countSignals = 0;
    private float timer = 0f;
    private float bigTimer = 0f;
    int errors = 0;
    void Update() //TODO sometimes freezes when searching - timeBetween mut be applied to all conditioins.
    {
        // Will set a new direction for the alien to walk in every frame
        timeBetweenChangeDirection -= Time.deltaTime;


        timer += Time.deltaTime;
        //bigTimer += Time.deltaTime;
        if(timer >= 1.0f){
            timer = 0;
            if(countSignals <= 1){
//                Debug.Log("-----------one or ZERO UPDATES ON MOVER " + gameObject.name);
                errors++;
            } //else
           /* if(countSignals < 2){
                Debug.Log("TOO FEW UPDATES" + gameObject.name + " " + countSignals);
                errors++;
            }*/
            countSignals = 0;
        }

    if(state == States.WANDER){
        if(timeBetweenChangeDirection <= 0 || movementTarget.x == transform.position.x) //TODO: add better check
        {
            //movementTarget = new Vector2(transform.position.x + RandomMove(), transform.position.y + RandomMove());
            NewRandomMovementTarget();
            
            timeBetweenChangeDirection = accelerationTime;
        } //else {
           // transform.position = Vector2.MoveTowards(transform.position, movement, speed * Time.deltaTime);
      //  }
    } 

    if(state != States.REST){
        transform.position = Vector2.MoveTowards(transform.position, movementTarget, speed * Time.deltaTime);
    }

    }
 

    float RandomMove(){
        if(Random.value <0.33f){
             return visionRadius * 1.5f;
        } else if (Random.value >= 0.66f) {
            return -visionRadius * 1.5f;
        } else {
            return 0;
        }
            
    }

    public CreatureMoverData ToData()
    {
        CreatureMoverData data = new CreatureMoverData();

        data.OnCenterMoving = this.OnCenterMoving;

        int index = 0;

        for(int i = 0; i < wanderingDirection.Length; i++){
            if(wanderingDirection[i]){
                index = i;
                break;
            }
        }

        data.randomMoveDirectionIndex = index;

        data.state = this.state;

        data.timeBetweenChangeDirection = this.timeBetweenChangeDirection;

        data.x_movement = this.movementTarget.x;

        data.y_movement = this.movementTarget.y;

        return data;
  
    }


    public void FromData(CreatureMoverData data)
    {
        wanderingDirection = new bool[8];
        for(int i = 0; i < wanderingDirection.Length; i++){
            wanderingDirection[i] = false;
        }

        this.wanderingDirection[data.randomMoveDirectionIndex] = true;

        this.state = data.state;

        this.OnCenterMoving = false;

        if(this.state == States.GO_CENTER){
            setState(States.WANDER);
        }

        this.timeBetweenChangeDirection = data.timeBetweenChangeDirection;

        this.movementTarget = new Vector2(data.x_movement, data.y_movement);

        this.target = null;
  
    }
 

}
