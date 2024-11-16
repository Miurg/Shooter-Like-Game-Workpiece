extends Camera3D

var rotationSpeed = 0.1
var lerpWeight = 40
var cameraInput =  Vector2.ZERO
var rotationVelocity =  Vector2.ZERO
# Called when the node enters the scene tree for the first time.

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	rotationVelocity = rotationVelocity.lerp(cameraInput*rotationSpeed, delta*lerpWeight)
	rotate_x(-deg_to_rad(rotationVelocity.y))
	get_parent().get_child(3).rotate_x(-deg_to_rad(rotationVelocity.y))
	get_parent().get_child(3).rotation_degrees.x = clamp(rotation_degrees.x, -90,90)
	get_parent().rotate_y(-deg_to_rad(rotationVelocity.x))
	rotation_degrees.x = clamp(rotation_degrees.x, -90,90)
	cameraInput = Vector2.ZERO
	
func _input(event: InputEvent) -> void:
	if event is InputEventMouseMotion:
		cameraInput = event.relative
