extends Node

var currentlyShoot = false
var timeFromLastShoot = 0
var spreadCurrent = 0
@onready var currentWeapon = $AK

func _physics_process(delta: float) -> void:
	if currentWeapon!=null:
		if currentlyShoot and timeFromLastShoot>currentWeapon.rateOfFire:
			currentWeapon.shootBullet(spreadCurrent)
			timeFromLastShoot=0
			if spreadCurrent<=currentWeapon.spreadMax:
				spreadCurrent+=currentWeapon.spreadSpeedUp
		elif currentlyShoot and timeFromLastShoot<currentWeapon.rateOfFire:
			timeFromLastShoot+=delta
		else:
			if spreadCurrent>currentWeapon.spreadMin:
				spreadCurrent-=currentWeapon.spreadSpeedDown*delta
				
func setNewWeapon(instance):
	add_child(instance)
	currentWeapon = instance

func getAwayWeapon(instance):
	currentWeapon = null
	instance.queue_free()
