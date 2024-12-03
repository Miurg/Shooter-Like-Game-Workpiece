class_name LifeWeapon
extends Node

var _currentlyShoot = false
var _timeFromLastShoot = 0
var _spreadCurrent = 0
@onready var _currentWeapon

func setSpread(value) -> void:
	_spreadCurrent = value

func getSpread() -> float:
	return _spreadCurrent

func setCurrentlyShoot(shoot) -> void:
	_currentlyShoot = shoot

func getCurrentlyShoot() -> bool:
	return _currentlyShoot

func setWeapon(instance) -> void:
	var newWeapon = instance.instantiate()
	add_child(newWeapon)
	_currentWeapon = newWeapon
	_currentWeapon.currentOwner = get_parent().get_parent()
	_currentWeapon.currentMasterWeapon = get_parent()

func getWeapon() -> Node3D:
	return _currentWeapon

func getAwayWeapon() -> void:
	setWeapon(null)
	getWeapon().queue_free()

func shooting(delta) -> void:
	if getWeapon()!=null:
		if getCurrentlyShoot() and _timeFromLastShoot>getWeapon().rateOfFire:
			getWeapon().shootBullet(getSpread())
			_timeFromLastShoot=0
			if getSpread()<=getWeapon().spreadMax:
				setSpread(getSpread() + getWeapon().spreadSpeedUp)
		elif _timeFromLastShoot<getWeapon().rateOfFire:
			_timeFromLastShoot+=delta
		if getSpread()>getWeapon().spreadMin:
			setSpread(getSpread() - getWeapon().spreadSpeedDown*delta)
	elif getSpread()!=0:
		setSpread(0)
		
