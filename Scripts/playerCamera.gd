extends Camera3D

var rotationSpeed = 1
var rotationX = 0
# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _physics_process(delta: float) -> void:
	rotate_x(rotationX*delta)
	if rotation.x>2: rotation.x = 2
	elif rotation.x<-2: rotation.x = -2
	rotationX = 0
	pass

func _input(event: InputEvent) -> void:
	if event is InputEventMouseMotion:
		rotationX = -(event.relative.y * rotationSpeed)
		get_parent().playerRotationY = -(event.relative.x * rotationSpeed)
