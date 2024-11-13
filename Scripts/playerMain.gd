extends CharacterBody3D

var playerVelocity:Vector3 = Vector3.ZERO
var playerSpeed:int = 10
var jumpForce:int = 20
var numberOfAvailableJump = 2

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	get_node("fatguy").get_node("AnimationPlayer").play("steps")
	pass # Replace with function body.
	



var maxSpeedDown:int = 20
var timeInAirForJump:float = 0.2
var firstJumpOnFloorHappend = false
var gravityForce:int = 40
var lerpWeight:int = 15
var inAirTime:float = 0
var jumpButtonClicks:int = 0
# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
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
		playerVelocity = direction.normalized().rotated(Vector3(0,1,0),rotation.y) * playerSpeed
		playerVelocity.y = velocity.y
		velocity = velocity.lerp(playerVelocity, delta*lerpWeight)
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
	
