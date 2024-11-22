extends NPC

@onready var navAgent = $NavigationAgent3D
var playerVisible:bool = false
var targetToMove 
var timeUntilUnsee:float
var tempTimeUntilUnsee:float
# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	moveSpeed = get_meta("moveSpeed")
	fieldOfView = get_meta("fieldOfView")
	healthPoint = get_meta("healthPoint")
	maxMoveSpeed = get_meta("maxMoveSpeed") 
	timeUntilUnsee = get_meta("timeUntilUnsee")
	maxDistanceOfView = get_meta("maxDistanceOfView")
	tempTimeUntilUnsee = timeUntilUnsee
	pass # Replace with function body.

func _physics_process(delta: float) -> void:
	if isPlayerVisible():
		tempTimeUntilUnsee=timeUntilUnsee
		playerVisible=true
	elif tempTimeUntilUnsee>0:
		tempTimeUntilUnsee-=delta
	else: playerVisible=false

func _process(delta: float) -> void:
	if playerVisible: 
		targetToMove = player.position
		navAgent.target_position = targetToMove
		velocity = velocity.lerp((navAgent.get_next_path_position()-global_position).normalized()*maxMoveSpeed,delta*moveSpeed)
		look_at(Vector3(navAgent.get_next_path_position().x,position.y,navAgent.get_next_path_position().z))
	else:
		velocity = velocity.lerp(Vector3(0,velocity.y,0), delta*10)
	
	if velocity.y > -maxSpeedDown:
		velocity.y = velocity.y-gravityForce*delta
	elif velocity.y < -maxSpeedDown:
		velocity.y=-maxSpeedDown
	move_and_slide()
