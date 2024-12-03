extends LifeWeapon
@onready var playerMain = $".."
var currentlyActiveWeapon:int = 0
		
func _ready() -> void:
	#setWeapon(preload("res://Nodes/Objects/Weapons/AK.tscn"))
	pass
		
#override
func setCurrentlyShoot(value) -> void:
	if value==true and getWeapon()!=null:
		if _timeFromLastShoot>getWeapon().rateOfFire:
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
	var newWeapon = instance
	newWeapon.rotation = Vector3.ZERO
	newWeapon.position = Vector3(0.331,0,-0.419)
	newWeapon.get_parent().remove_child(newWeapon)
	add_child(newWeapon)
	_currentWeapon = newWeapon
	setSpread(_currentWeapon.spreadMin)
	_currentWeapon.currentOwner = get_parent()
	_currentWeapon.currentMasterWeapon = self

func getAwayWeapon() -> void:
	if getWeapon()!=null:
		var weaponInstance = getWeapon().resourceOfWeapon.instantiate()
		playerMain.toDistributorPlaceWeapon(weaponInstance)
		getWeapon().queue_free()
		_currentWeapon = null
