extends Node
#0 - name of weapon, 1 - screne of weapon, 2 - path to node of weapon
var weaponsStatusArray:Array[Array]
@onready var playerMain = $".."
var currentlyActiveWeapon: int = 0
var currentlyHoldsWeapon:bool = false
var currentlyShoot:bool = false
func _ready() -> void:
	weaponsStatusArray.append(Array())
	weaponsStatusArray[0].append("AK")
	weaponsStatusArray[0].append(preload("res://Nodes/AK.tscn"))
	weaponsStatusArray[0].append($AK)

var timeFromLastShoot = 0
func _physics_process(delta: float) -> void:
	if currentlyShoot and timeFromLastShoot>weaponsStatusArray[currentlyActiveWeapon][2].rateOfFire and currentlyHoldsWeapon==true:
		shootBullet()
		timeFromLastShoot=0
	else:timeFromLastShoot+=delta

func pickupWeapon(InstanceWeapon) -> void:
	for i in weaponsStatusArray:
		if InstanceWeapon.get_child(0).name == i[2].name:
			InstanceWeapon.queue_free()
			if currentlyHoldsWeapon:
				dropWeapon()
				i[2].visible = true
				currentlyHoldsWeapon = true
			else:
				i[2].visible = true
				currentlyHoldsWeapon = true
		
	
func dropWeapon() -> void:
		weaponsStatusArray[currentlyActiveWeapon][2].visible=false
		var weaponInstance = weaponsStatusArray[currentlyActiveWeapon][1].instantiate()
		playerMain.toDistributorPlaceWeapon(weaponInstance)
		currentlyHoldsWeapon = false
		return

func shootBullet() -> void:
		var arrayOfCollider = playerMain.toCameraRayFromCamera(0b00000000_00000000_00000000_00000011,1000)
		if arrayOfCollider[0]!=null:
			if arrayOfCollider[0].get_collision_layer()==1:
				playerMain.toDistributorCreateHole(arrayOfCollider[0],arrayOfCollider[1],arrayOfCollider[2])
			#else: 
				
	
