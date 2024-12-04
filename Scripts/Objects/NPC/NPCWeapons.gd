extends LifeWeapon

func _ready() -> void:
	setWeapon(preload("res://Nodes/Objects/Weapons/AK.tscn")) 

func _physics_process(delta: float) -> void:
	shooting(delta)
	if getCurrentRounds()==0:
		reload()

func setWeapon(instance) -> void:
	var newWeapon = instance.instantiate()
	newWeapon.rotation = Vector3.ZERO
	newWeapon.position = Vector3(0.5,1.5,-0.4)
	add_child(newWeapon)
	_currentWeapon = newWeapon
