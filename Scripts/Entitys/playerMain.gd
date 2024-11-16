extends CharacterBody3D

var playerVelocity:Vector3 = Vector3.ZERO
var playerSpeed:float = 6
var playerMaxSpeed:float = 10
var jumpForce:int = 10
var numberOfAvailableJump = 1

var spaceState
func _ready() -> void:
	spaceState = get_tree().get_root().get_world_3d().direct_space_state
	get_node("fatguy").get_node("AnimationPlayer").play("steps")


func _process(delta: float) -> void:
	movement(delta)
	selectProcess(delta)



const maxSpeedDown:int = 20
const timeInAirForJump:float = 0.2
const gravityForce:int = 40
const lerpWeight:int = 20
var firstJumpOnFloorHappend = false
var inAirTime:float = 0
var jumpButtonClicks:int = 0
func movement(delta):
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
			jumpButtonClicks+=1
			firstJumpOnFloorHappend = true
		playerVelocity = direction.normalized().rotated(Vector3(0,1,0),rotation.y) * playerMaxSpeed
		playerVelocity.y = velocity.y
		var maxVelocity = Vector2(playerMaxSpeed,playerMaxSpeed).normalized() * playerMaxSpeed
		if abs(velocity.x)<maxVelocity.x and abs(velocity.z)<maxVelocity.y and playerVelocity!=Vector3(0,velocity.y,0):
			velocity = velocity.lerp(playerVelocity, delta*playerSpeed)
		elif is_on_floor():
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
		
		

@onready var mainCamera = $PlayerCameraMain
@onready var iconsSelected = $"../../CanvasLayer/IconsSelected"
var rayLength = 10
var mdt = MeshDataTool.new()
func selectProcess(delta):
	var rayStart = mainCamera.project_ray_origin(get_viewport().get_visible_rect().size / 2)
	var rayEnd = rayStart + mainCamera.project_ray_normal(get_viewport().get_visible_rect().size / 2) * rayLength
	if spaceState != null:
		var query = PhysicsRayQueryParameters3D.create(rayStart, rayEnd, 0b00000000_00000000_00000000_00001000)
		if spaceState.intersect_ray(query).has("collider_id"): 
			var rayInstance = spaceState.intersect_ray(query).collider
			var meshInstance = rayInstance.get_child(2)
			var meshLocal = meshInstance.mesh.get_faces()
			var unproject = PackedVector2Array()
			for i in meshLocal:
				unproject.append(get_viewport().get_camera_3d().unproject_position(rayInstance.position-i))
			var p1 : Vector2 = unproject[0]
			var p2 : Vector2 = unproject[0]

			for i in unproject:
				p1.x = min(p1.x, i.x)
				p1.y = min(p1.y, i.y)
				p2.x = max(p2.x, i.x)
				p2.y = max(p2.y, i.y)
			iconsSelected.position = Vector2(p1.x,p1.y)
			iconsSelected.size = Vector2(p2.x-p1.x,p2.y-p1.y)
		else:
			iconsSelected.position = Vector2(973,523)
			iconsSelected.size = Vector2(34,34)
#func _input(event: InputEvent) -> void:
	#if event is InputEventKey:
		#if event.keycode == "g":
			#
			
			
