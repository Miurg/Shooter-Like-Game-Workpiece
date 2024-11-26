class_name NPC
extends Life

@onready var player = $"../Player"
@onready var masterWeapon = $MasterWeapon
@onready var currentWeapon = $MasterWeapon/AK
var fieldOfView:int
var maxDistanceOfView:int
var playerVisible:bool = false

func setNewWeapon(instance):
	masterWeapon.add_child(instance)
	currentWeapon = instance

func getAwayWeapon(instance):
	currentWeapon = null
	instance.queue_free()

func isPlayerVisible() -> bool:
	if position.distance_to(player.position)<maxDistanceOfView:
		var selfDirection = -global_transform.basis.z.normalized()
		var toTargetDirection = position.direction_to(player.position)
		var angle = rad_to_deg(acos(selfDirection.dot(toTargetDirection)))
		if angle<=fieldOfView/2:
			return getRay("GeneralRay",0b00000000_00000000_00000000_00000001,player.position-position,-rotation)[0]==null
	return false
