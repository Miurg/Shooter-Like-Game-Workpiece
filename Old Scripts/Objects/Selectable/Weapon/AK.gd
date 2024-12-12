extends WeaponShooting


# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	spreadMax = 5
	spreadMin = 0
	spreadSpeedUp = 0.5
	spreadSpeedDown = 3
	rateOfFire = 0.1
	damage = 1
	nameOfWeapon = "AK"
	holeNode = preload("res://Nodes/Objects/Hole.tscn")
	shootParticle = preload("res://Nodes/Particles/ShootParticles.tscn")
	resourceOfWeapon = preload("res://Nodes/Objects/Weapons/AK.tscn")
	soundOfWeaponForNPC = preload("res://Nodes/Sounds/AKShootSoundNPC.tscn")
	soundOfWeapon = preload("res://Nodes/Sounds/AKShootSound.tscn")
	pass # Replace with function body.
