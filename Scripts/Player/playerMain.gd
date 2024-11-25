extends CharacterBody3D

@onready var workDistributor = $"../.."
@onready var playerWeapons = $Weapons
@onready var playerCamera = $PlayerCameraMain


var healthPoint:int = 100
var playerVelocity:Vector3 = Vector3.ZERO
var playerSpeed:float = 6
var playerMaxSpeed:float = 10
var jumpForce:int = 10
var numberOfAvailableJump = 1
func _ready() -> void:
	
	get_node("fatguy").get_node("AnimationPlayer").play("steps")


func _process(delta: float) -> void:
	movementProcess(delta)
	

func _input(event: InputEvent) -> void:
	if event is InputEventKey:
		if OS.get_keycode_string(event.keycode) == "E" and playerCamera.rayInstanceWeapon!=null and event.pressed:
			playerWeapons.pickupWeapon(playerCamera.rayInstanceWeapon)
		if OS.get_keycode_string(event.keycode) == "G" and event.pressed:
			playerWeapons.dropWeapon()
	if event is InputEventMouseButton:
		if event.button_index == MOUSE_BUTTON_LEFT and event.pressed:
			playerWeapons.currentlyShoot = true
		elif event.button_index == MOUSE_BUTTON_LEFT and !event.pressed:
			playerWeapons.currentlyShoot = false


const maxSpeedDown:int = 20
const timeInAirForJump:float = 0.2
const gravityForce:int = 40
const lerpWeight:int = 20
var firstJumpOnFloorHappend = false
var inAirTime:float = 0
var jumpButtonClicks:int = 0
func movementProcess(delta) -> void:
	var direction = Vector3()
	if Input:
		if Input.is_action_pressed('left'):
			direction.x = -1
		if Input.is_action_pressed('right'):
			direction.x = 1
		if Input.is_action_pressed('backward'):
			direction.z = 1
		if Input.is_action_pressed('forward'):
			direction.z = -1
		if Input.is_action_just_pressed('jump'):
			if inAirTime<timeInAirForJump or jumpButtonClicks<numberOfAvailableJump:
				velocity.y = jumpForce
				inAirTime = timeInAirForJump
			jumpButtonClicks+=1
			firstJumpOnFloorHappend = true
		playerVelocity = direction.normalized().rotated(Vector3(0,1,0),rotation.y) * playerMaxSpeed
		playerVelocity.y = velocity.y
		var maxVelocity = Vector2(playerMaxSpeed,playerMaxSpeed).normalized() * playerMaxSpeed
		if abs(velocity.x)<maxVelocity.x and abs(velocity.z)<maxVelocity.y and playerVelocity!=Vector3(0,velocity.y,0):
			velocity = velocity.lerp(playerVelocity, delta*playerSpeed)
		else:
			velocity = velocity.lerp(Vector3(0,velocity.y,0), delta*lerpWeight)
	if !is_on_floor():
		inAirTime+=delta
		if firstJumpOnFloorHappend == false and inAirTime>timeInAirForJump and jumpButtonClicks==0:jumpButtonClicks+=1
		if velocity.y > -maxSpeedDown:
			velocity.y = velocity.y-gravityForce*delta
		elif velocity.y < -maxSpeedDown:
			velocity.y=-maxSpeedDown
	move_and_slide()
	if is_on_floor():
		firstJumpOnFloorHappend = false
		jumpButtonClicks = 0
		inAirTime = 0
		
		
func toDistributorHUDToNormalIcons() -> void:
	workDistributor.HUDToNormalIcons()
	
func toDistributorHUDUpdateIconsSelected(sizeX,sizeY,positionX,positionY) -> void:
	workDistributor.HUDUpdateIconsSelected(sizeX,sizeY,positionX,positionY)
	
func toDistributorPlaceWeapon(weaponInstance) -> void:
	var weaponsPosition = Vector3(position.x,position.y+playerCamera.position.y,position.z)+Vector3(0,0,-1).rotated(Vector3(1,0,0),playerCamera.rotation.x).rotated(Vector3(0,1,0),rotation.y)
	var impulse = Vector3(0,0,-10).rotated(Vector3(1,0,0),playerCamera.rotation.x).rotated(Vector3(0,1,0),rotation.y)
	workDistributor.placeWeapon(self,weaponInstance,impulse,weaponsPosition)

func toDistributorCreateHole(wallCollider,positionOfHole,normalOfHole,holeNode) -> void:
	workDistributor.createHoleFromBullet(wallCollider,positionOfHole,normalOfHole,holeNode)
	
func toDistributorHUDUpdateSpread(spread) -> void:
	workDistributor.HUDUpdateSpread(spread)
	
func getRay(collisionMask:int, newRayTarget:Vector3) -> Array:
	return playerCamera.rayFromCamera(collisionMask,newRayTarget)
	
