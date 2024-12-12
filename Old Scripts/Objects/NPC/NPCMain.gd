class_name NPC
extends Life

@onready var player = $"../Player"
@onready var masterWeapon = $MasterWeapon
var fieldOfView:int
var maxDistanceOfView:int
var rotationSpeed:float = PI



func isPlayerVisible() -> bool:
	if position.distance_to(player.position)<maxDistanceOfView:
		var selfDirection = -global_transform.basis.z.normalized()
		var toTargetDirection = position.direction_to(player.position)
		var angle = rad_to_deg(acos(selfDirection.dot(toTargetDirection)))
		if angle<=fieldOfView/2:
			return getRayVision(0b00000000_00000000_00000000_00000001,to_local(player.global_position))[0]==null
	return false

func slowLookAt(newLook,delta):
	var vectorFromTo = Vector2(position.z,position.x)-Vector2(newLook.z,newLook.x)
	var angle = vectorFromTo.angle()
	var r = rotation.y
	var angle_delta = rotationSpeed * delta
	angle = lerp_angle(r,angle,1)
	angle = clamp(angle,r-angle_delta,r+angle_delta)
	rotation = Vector3(rotation.x,angle,rotation.z)
	
