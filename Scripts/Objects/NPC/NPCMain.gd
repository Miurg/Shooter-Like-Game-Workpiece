class_name NPC
extends CharacterBody3D

@onready var player = $"../Player"
@onready var ray = $RayCast3D
var healthPoint:int
var moveSpeed:float
var maxMoveSpeed:float
var fieldOfView:int
var maxDistanceOfView:int
const maxSpeedDown:int = 20
const gravityForce:int = 40
var playerVisible:bool = false

func isPlayerVisible() -> bool:
	if position.distance_to(player.position)<maxDistanceOfView:
		var selfDirection = -global_transform.basis.z.normalized()
		var toTargetDirection = position.direction_to(player.position)
		var angle = rad_to_deg(acos(selfDirection.dot(toTargetDirection)))
		if angle<=fieldOfView/2:
			ray.target_position = player.position-position
			ray.rotation = -rotation
			return !ray.is_colliding()
	return false

func takeDamage(damage) -> void:
	healthPoint-=damage
