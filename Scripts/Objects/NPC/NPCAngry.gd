extends NPC

@onready var navAgent = $NavigationAgent3D
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

var nextPath
func _physics_process(delta: float) -> void:
	if isPlayerVisible():
		tempTimeUntilUnsee=timeUntilUnsee
		playerVisible=true
		navAgent.target_position = player.position
		nextPath = navAgent.get_next_path_position()
	elif tempTimeUntilUnsee>0:
		tempTimeUntilUnsee-=delta
		if !navAgent.is_navigation_finished():
			nextPath = navAgent.get_next_path_position()
	else: playerVisible=false

func _process(delta: float) -> void:
	if playerVisible: 
		velocity = velocity.lerp((nextPath-global_position).normalized()*maxMoveSpeed,delta*moveSpeed)
		var newLook = Vector3(nextPath.x,position.y,nextPath.z)
		if newLook!=position:
			look_at(newLook)
	else:
		velocity = velocity.lerp(Vector3(0,velocity.y,0), delta*10)
	
	if velocity.y > -maxSpeedDown:
		velocity.y = velocity.y-gravityForce*delta
	elif velocity.y < -maxSpeedDown:
		velocity.y=-maxSpeedDown
	move_and_slide()
	
	if healthPoint<0:
		self.queue_free()
