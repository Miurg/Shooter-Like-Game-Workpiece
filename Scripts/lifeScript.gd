class_name Life
extends CharacterBody3D
@onready var workDistributor = $"../.."
var healthPoint:int = 10
func takeDamage(damage:int,fromWho):
	healthPoint-=damage
	#if healthPoint<0:
		#die()

func die():
	self.queue_free()

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

@onready var visionRay
var space_state
func getRayVision(collisionMask:int, newRayTarget:Vector3) -> Array:
	visionRay.collision_mask = collisionMask
	visionRay.target_position = newRayTarget
	if visionRay.is_colliding()==true:
		return [visionRay.get_collider(),visionRay.get_collision_point(),visionRay.get_collision_normal()]
	else: return [null]
