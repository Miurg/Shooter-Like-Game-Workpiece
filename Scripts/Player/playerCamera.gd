extends Camera3D

@onready var playerMain = $".."
var rotationSpeed = 0.1
var lerpWeight = 40
var cameraInput =  Vector2.ZERO
var rotationVelocity =  Vector2.ZERO
# Called when the node enters the scene tree for the first time.

var spaceState
func _ready() -> void:
	spaceState = get_tree().get_root().get_world_3d().direct_space_state
# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	rotationVelocity = rotationVelocity.lerp(cameraInput*rotationSpeed, delta*lerpWeight)
	rotate_x(-deg_to_rad(rotationVelocity.y))
	get_parent().get_child(3).rotate_x(-deg_to_rad(rotationVelocity.y))
	get_parent().get_child(3).rotation_degrees.x = clamp(rotation_degrees.x, -90,90)
	get_parent().rotate_y(-deg_to_rad(rotationVelocity.x))
	rotation_degrees.x = clamp(rotation_degrees.x, -90,90)
	cameraInput = Vector2.ZERO
	selectProcess(delta)
	
func _input(event: InputEvent) -> void:
	if event is InputEventMouseMotion:
		cameraInput = event.relative

				

var rayLength = 2
var rayInstanceWeapon
var mdt = MeshDataTool.new()
func selectProcess(delta):
	var rayStart = project_ray_origin(get_viewport().get_visible_rect().size / 2)
	var rayEnd = rayStart + project_ray_normal(get_viewport().get_visible_rect().size / 2) * rayLength
	if spaceState != null:
		var query = PhysicsRayQueryParameters3D.create(rayStart, rayEnd, 0b00000000_00000000_00000000_00001000)
		if spaceState.intersect_ray(query).has("collider_id"): 
			rayInstanceWeapon = spaceState.intersect_ray(query).collider
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
			playerMain.toDistributorIconsSelected(delta,34,34,973,523)
			rayInstanceWeapon = null
