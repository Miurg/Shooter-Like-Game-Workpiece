class_name NPC
extends CharacterBody3D

@onready var player = $"../Player"
var healthPoint:int
var moveSpeed:float
var maxMoveSpeed:float
var fieldOfView:int
const maxSpeedDown:int = 20
const gravityForce:int = 40

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	pass
	
func isPlayerVisible() -> bool:
	var selfDirection = -global_transform.basis.z.normalized()
	var toTargetDirection = position.direction_to(player.position)
	var angle = rad_to_deg(acos(selfDirection.dot(toTargetDirection)))
	return angle<=fieldOfView/2
