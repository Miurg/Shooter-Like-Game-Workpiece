class_name Life
extends CharacterBody3D
@onready var workDistributor = $"../.."


var healthPoint:int = 100
func takeDamage(damage):
	healthPoint-=damage

const maxSpeedDown:int = 20
const gravityForce:int = 40
func applyGravitVelocity(delta:float) -> void:
	if velocity.y > -maxSpeedDown:
		velocity.y = velocity.y-gravityForce*delta
	elif velocity.y < -maxSpeedDown:
		velocity.y=-maxSpeedDown

const stopSpeed:int = 20
var moveSpeed:float
var maxMoveSpeed:float

@onready var rays:Array
func getRay(rayName,collisionMask:int, newRayTarget:Vector3,newRotation:Vector3) -> Array:
	var generalRay
	for i in rays:
		if i.name==rayName:
			generalRay=i
			break
	if generalRay==null:
		print_debug("error, ", rayName," not found in", rays)
		return [null]
	generalRay.rotation = newRotation
	generalRay.collision_mask = collisionMask
	generalRay.target_position = newRayTarget
	if generalRay.is_colliding()==true:
		return [generalRay.get_collider(),generalRay.get_collision_point(),generalRay.get_collision_normal()]
	else: return [null]
