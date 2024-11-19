extends Camera3D

@onready var playerMain = $".."
@onready var rayForWeapon = $RayForWeapon
@onready var generalRay = $GeneralRay
var rotationSpeed = 0.1
var lerpWeight = 40
var cameraInput =  Vector2.ZERO
var rotationVelocity =  Vector2.ZERO

func _process(delta: float) -> void:
	rotationVelocity = rotationVelocity.lerp(cameraInput*rotationSpeed, delta*lerpWeight)
	rotate_x(-deg_to_rad(rotationVelocity.y))
	playerMain.get_child(3).rotate_x(-deg_to_rad(rotationVelocity.y))
	playerMain.get_child(3).rotation_degrees.x = clamp(rotation_degrees.x, -90,90)
	playerMain.rotate_y(-deg_to_rad(rotationVelocity.x))
	rotation_degrees.x = clamp(rotation_degrees.x, -90,90)
	cameraInput = Vector2.ZERO
	selectProcess(delta)
	
func _input(event: InputEvent) -> void:
	if event is InputEventMouseMotion:
		cameraInput = event.relative

				
var rayInstanceWeapon
var mdt = MeshDataTool.new()
func selectProcess(delta) -> void:
	rayForWeapon.force_raycast_update() 
	if rayForWeapon.is_colliding()==true: 
		rayInstanceWeapon = rayForWeapon.get_collider()
		var meshInstance = rayInstanceWeapon.get_child(2)
		var meshLocal = meshInstance.mesh.get_faces()
		for i in meshLocal.size():
			meshLocal[i] = meshLocal[i].rotated(Vector3(1,0,0),rayInstanceWeapon.rotation.x)
			meshLocal[i] = meshLocal[i].rotated(Vector3(0,1,0),rayInstanceWeapon.rotation.y)
			meshLocal[i] = meshLocal[i].rotated(Vector3(0,0,1),rayInstanceWeapon.rotation.z)
		var unproject = PackedVector2Array()
		for i in meshLocal:
			unproject.append(get_viewport().get_camera_3d().unproject_position(rayInstanceWeapon.position-i))
		var p1 : Vector2 = unproject[0]
		var p2 : Vector2 = unproject[0]
			
		for i in unproject:
			p1.x = min(p1.x, i.x)
			p1.y = min(p1.y, i.y)
			p2.x = max(p2.x, i.x)
			p2.y = max(p2.y, i.y)
		playerMain.toDistributorIconsSelected(delta,p2.x-p1.x,p2.y-p1.y,p1.x,p1.y)
	else:
		var positionX = (get_viewport().get_visible_rect().size.x / 2)-17
		var positionY = (get_viewport().get_visible_rect().size.y / 2)-17
		playerMain.toDistributorIconsSelected(delta,34,34,positionX,positionY)
		rayInstanceWeapon = null
			
func rayFromCamera(collisionMask:int, newRayLength:int) -> Array:
	generalRay.collision_mask = collisionMask
	generalRay.target_position = Vector3(0,0,-newRayLength)
	if generalRay.is_colliding()==true:
		return [generalRay.get_collider(), generalRay.get_collision_point(),generalRay.get_collision_normal()]
	else: return [null]
