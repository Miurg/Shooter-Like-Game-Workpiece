extends WeaponShooting


# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	spreadMax = 2
	spreadMin = 0
	spreadSpeedUp = 0.1
	spreadSpeedDown = 2
	rateOfFire = 0.02
	damage = 1
	holeNode = preload("res://Nodes/Objects/Hole.tscn")
	shootParticle = preload("res://Nodes/Particles/Shoot.tscn")
	pass # Replace with function body.
