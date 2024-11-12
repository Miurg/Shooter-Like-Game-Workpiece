extends CharacterBody3D

var playerSpeed = 500
var playerVelocity = Vector3(0,0,0)
var playerRotationY = 0


# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	get_node("fatguy").get_node("AnimationPlayer").play("steps")
	Input.set_mouse_mode(Input.MOUSE_MODE_CAPTURED)
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _physics_process(delta: float) -> void:
	var direction = Vector3()
	if Input:
		if Input.is_action_pressed('ui_left'):
			direction.x = -1
		if Input.is_action_pressed('ui_right'):
			direction.x = 1
		if Input.is_action_pressed('ui_down'):
			direction.z = 1
		if Input.is_action_pressed('ui_up'):
			direction.z = -1
		direction*=playerSpeed * delta
		direction = direction.rotated(Vector3(0,1,0),rotation.y)
		playerVelocity.x = direction.x
		playerVelocity.z = direction.z
		velocity = playerVelocity
		move_and_slide()
	rotate_y(playerRotationY*delta)
	playerRotationY = 0
	pass
