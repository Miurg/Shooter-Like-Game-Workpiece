extends Camera3D

@onready var playerMain = $".."
@onready var generalRay = $GeneralRay
@onready var weaponRay = $WeaponRay
var rays:Array
var rotationSpeed = 0.1
var lerpWeight = 40
var cameraInput =  Vector2.ZERO
var rotationVelocity =  Vector2.ZERO

func _ready() -> void:
	rays.append($WeaponRay)
	rays.append($GeneralRay)

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
	generalRay.force_raycast_update() 
	if generalRay.is_colliding()==true: 
		rayInstanceWeapon = generalRay.get_collider()
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
			
func rayFromCamera(rayName:String,collisionMask:int, newRayTarget:Vector3,newRotation:Vector3) -> Array:
	var generalRayNew
	for i in rays:
		if i.name==rayName:
			generalRayNew=i
			print_debug(i.name==rayName, i.name,rayName)
			break
	if generalRayNew==null:
		print_debug("error, ", rayName," not found in", rays)
		return [null]
	generalRayNew.rotation = newRotation
	generalRayNew.collision_mask = collisionMask
	generalRayNew.target_position = newRayTarget
	if generalRayNew.is_colliding()==true:
		return [generalRayNew.get_collider(), generalRayNew.get_collision_point(),generalRayNew.get_collision_normal()]
	else: return [null]
