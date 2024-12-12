extends LifeWeapon
@onready var playerMain = $".."

func _ready() -> void:
	#setWeapon(preload("res://Nodes/Objects/Weapons/AK.tscn"))
	pass

#override
func setCurrentRounds(value) -> void:
	playerMain.toDistributorHUDSetRoundsCurrent(value)
	_numberOfRoundsCurrent = value

#override
func setPocketRounds(value) -> void:
	playerMain.toDistributorHUDSetRoundsPocket(value)
	_numberOfRoundsInPocket = value

#override
func setCurrentlyShoot(value) -> void:
	if value==true and getWeapon()!=null:
		if _timeFromLastShoot>getWeapon().rateOfFire and getCurrentRounds()>0:
			getWeapon().shootBullet(getSpread())
			_timeFromLastShoot=0
			if getSpread()<=getWeapon().spreadMax:
				setSpread(getSpread() + getWeapon().spreadSpeedUp)
	_currentlyShoot = value

#override
func setSpread(value) -> void:
	_spreadCurrent = value
	playerMain.toDistributorHUDUpdateSpread(value)

func _physics_process(delta: float) -> void:
	shooting(delta)

func setWeapon(instance) -> void:
	if _currentWeapon!=null:
		getAwayWeapon()
	var newWeapon = instance.resourceOfWeapon.instantiate()
	add_child(newWeapon)
	instance.queue_free()
	newWeapon.rotation = Vector3.ZERO
	newWeapon.position = Vector3(0.331,0,-0.419)
	_currentWeapon = newWeapon
	setSpread(_currentWeapon.spreadMin)

func getAwayWeapon() -> void:
	if getWeapon()!=null:
		var weaponInstance = getWeapon().resourceOfWeapon.instantiate()
		playerMain.toDistributorPlaceWeapon(weaponInstance)
		getWeapon().queue_free()
		_currentWeapon = null
