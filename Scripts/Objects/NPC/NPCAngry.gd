extends NPC

@onready var player = $"../Player"
# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	healthPoint = 100
	moveSpeed = 1
	fieldOfView = 60
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _physics_process(delta: float) -> void:
	rotate_y(0.01)
	var kek = isPlayerVisible()
	if kek: print_debug("YES")
	pass

func isPlayerVisible() -> bool:
	var selfDirection = global_transform.basis.x.normalized()
	var toTargetDirection = position.direction_to(player.position)
	var angle = rad_to_deg(acos(selfDirection.dot(toTargetDirection)))
	return angle<=fieldOfView/2
	#direction = -rad_to_deg(atan2(direction.z,direction.x))
	#print_debug(direction)
	return false
