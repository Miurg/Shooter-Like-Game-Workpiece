extends Camera3D

@onready var playerMain = $".."
@onready var rayForWeapon = $RayForWeapon
@onready var generalRay = $GeneralRay
var rotationSpeed = 0.2
var lerpWeight = 40
var cameraInput =  Vector2.ZERO
var rotationVelocity =  Vector2.ZERO

func _process(delta: float) -> void:
	rotationVelocity = cameraInput*rotationSpeed
	rotate_x(-deg_to_rad(rotationVelocity.y))
	playerMain.rotate_y(-deg_to_rad(rotationVelocity.x))
	rotation_degrees.x = clamp(rotation_degrees.x, -90,90)
	playerMain.get_child(3).rotation.x = rotation.x
	cameraInput = Vector2.ZERO

func _physics_process(delta: float) -> void:
	selectProcess(delta)

func _input(event: InputEvent) -> void:
	if event is InputEventMouseMotion:
		cameraInput = event.relative

				
var rayInstanceWeapon
func selectProcess(delta) -> void:
	rayForWeapon.force_raycast_update() 
	if rayForWeapon.is_colliding()==true: 
		rayInstanceWeapon = rayForWeapon.get_collider()
		var meshInstance = rayInstanceWeapon.get_child(2)
		var meshFaces = meshInstance.mesh.get_faces()
		var unproject = PackedVector2Array()
		for i in meshFaces:
			unproject.append(get_viewport().get_camera_3d().unproject_position(rayInstanceWeapon.global_transform * i))
		var p1 : Vector2 = unproject[0]
		var p2 : Vector2 = unproject[0]
		for i in unproject:
			p1.x = min(p1.x, i.x)
			p1.y = min(p1.y, i.y)
			p2.x = max(p2.x, i.x)
			p2.y = max(p2.y, i.y)
		playerMain.toDistributorHUDUpdateIconsSelected(p2.x-p1.x,p2.y-p1.y,p1.x,p1.y)
	else:
		playerMain.toDistributorHUDToNormalIcons()
		rayInstanceWeapon = null
			
func rayFromCamera(collisionMask:int, newRayTarget:Vector3) -> Array:
	generalRay.collision_mask = collisionMask
	generalRay.target_position = newRayTarget
	if generalRay.is_colliding()==true:
		return [generalRay.get_collider(), generalRay.get_collision_point(),generalRay.get_collision_normal()]
	else: return [null]
