extends NPC

@onready var meshNode = $copskeleton
@onready var navAgent = $NavigationAgent3D
var targetToMove 
var timeUntilUnsee:float
var tempTimeUntilUnsee:float
var nextPath:Vector3
@onready var currentlyNeedLookTo:Vector3 = transform * Vector3(0,0,-1)
# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	await get_tree().physics_frame
	await get_tree().physics_frame
	visionRay = $VisionRay
	moveSpeed = get_meta("moveSpeed")
	maxMoveSpeed = get_meta("maxMoveSpeed")
	fieldOfView = get_meta("fieldOfView")
	healthPoint = get_meta("healthPoint")
	timeUntilUnsee = get_meta("timeUntilUnsee")
	maxDistanceOfView = get_meta("maxDistanceOfView")
	tempTimeUntilUnsee = 0
	space_state = get_world_3d().direct_space_state


var playerVisible:bool = false
var playerVisibleFromTime:bool = false
func _physics_process(delta: float) -> void:
	if isPlayerVisible():
		playerVisible = true
		tempTimeUntilUnsee=timeUntilUnsee
	else:playerVisible=false
	
	if tempTimeUntilUnsee>0:
		playerVisibleFromTime=true
		navAgent.target_position = player.position
		tempTimeUntilUnsee-=delta
	else: playerVisibleFromTime=false
	
	if !navAgent.is_navigation_finished():
			nextPath = navAgent.get_next_path_position()

func _process(delta: float) -> void:
	slowLookAt(currentlyNeedLookTo,delta)
	movementProcess(delta)
	if !is_on_floor():
		applyGravitVelocity(delta)
	move_and_slide()
	

func movementProcess(delta):
	if masterWeapon.getWeapon()!=null and position.distance_to(player.position)<masterWeapon.getWeapon().maxDistanceForNPC and playerVisible:
		masterWeapon.setCurrentlyShoot(true)
		velocity = velocity.lerp(Vector3(0,velocity.y,0), delta*stopSpeed)
		look_at(player.position)
		meshNode.get_child(1).play("Run")
	elif !navAgent.is_navigation_finished() or playerVisible:
		masterWeapon.setCurrentlyShoot(false)
		velocity = velocity.lerp((nextPath-global_position).normalized()*maxMoveSpeed,delta*moveSpeed)
		var newLook = Vector3(nextPath.x,position.y,nextPath.z)
		if newLook!=position:
			currentlyNeedLookTo = newLook
		meshNode.get_child(1).play("Run")
	else:
		velocity = velocity.lerp(Vector3(0,velocity.y,0), delta*stopSpeed)
		meshNode.get_child(1).stop()


func toDistributorCreateHole(wallCollider,positionOfHole,normalOfHole,holeNode) -> void:
	workDistributor.createHoleFromBullet(wallCollider,positionOfHole,normalOfHole,holeNode)
	
func getRayForWeapon(collisionMask:int, newRayTarget:Vector3) -> Array:
	var rayStart = visionRay.global_position
	var rayEnd = rayStart+newRayTarget.rotated(Vector3(1,0,0),rotation.x).rotated(Vector3(0,1,0),rotation.y)
	var generalRayNew = space_state.intersect_ray(PhysicsRayQueryParameters3D.create(rayStart,rayEnd, collisionMask))
	if generalRayNew.has("collider"):
		return [generalRayNew.collider, generalRayNew.position,generalRayNew.normal]
	else: return [null]
	
func takeDamage(damage:int,fromWho):
	healthPoint-=damage
	seekForDamageApplayer(fromWho)
	if healthPoint<0:
		die()
	
func seekForDamageApplayer(fromWho):
	if fromWho.name=="Player":
		currentlyNeedLookTo = Vector3(fromWho.position.x,position.y,fromWho.position.z)
	
