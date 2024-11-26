extends NPC

@onready var navAgent = $NavigationAgent3D
var targetToMove 
var timeUntilUnsee:float
var tempTimeUntilUnsee:float
# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	rays.append($WeaponRay)
	rays.append($GeneralRay)
	moveSpeed = get_meta("moveSpeed")
	maxMoveSpeed = get_meta("maxMoveSpeed")
	fieldOfView = get_meta("fieldOfView")
	healthPoint = get_meta("healthPoint")
	timeUntilUnsee = get_meta("timeUntilUnsee")
	maxDistanceOfView = get_meta("maxDistanceOfView")
	tempTimeUntilUnsee = timeUntilUnsee


var nextPath
var currentlyShoot = false
var timeFromLastShoot = 0
var spreadCurrent = 0
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
	if currentWeapon!=null:
		if currentlyShoot and timeFromLastShoot>currentWeapon.rateOfFire:
			currentWeapon.shootBullet(spreadCurrent)
			timeFromLastShoot=0
			if spreadCurrent<=currentWeapon.spreadMax:
				spreadCurrent+=currentWeapon.spreadSpeedUp
		elif currentlyShoot and timeFromLastShoot<currentWeapon.rateOfFire:
			timeFromLastShoot+=delta
		else:
			if spreadCurrent>currentWeapon.spreadMin:
				spreadCurrent-=currentWeapon.spreadSpeedDown*delta
	

func _process(delta: float) -> void:
	if playerVisible: 
		if currentWeapon!=null and position.distance_to(player.position)<currentWeapon.maxDistanceForNPC:
			currentlyShoot=true
			velocity = velocity.lerp(Vector3(0,velocity.y,0), delta*stopSpeed)
			look_at(player.position)
		else:
			velocity = velocity.lerp((nextPath-global_position).normalized()*maxMoveSpeed,delta*moveSpeed)
			var newLook = Vector3(nextPath.x,position.y,nextPath.z)
			if newLook!=position:
				look_at(newLook)
	else:
		velocity = velocity.lerp(Vector3(0,velocity.y,0), delta*stopSpeed)
	if !is_on_floor():
		applyGravitVelocity(delta)
	move_and_slide()
	
	if healthPoint<0:
		self.queue_free()

func toDistributorCreateHole(wallCollider,positionOfHole,normalOfHole,holeNode) -> void:
	workDistributor.createHoleFromBullet(wallCollider,positionOfHole,normalOfHole,holeNode)
