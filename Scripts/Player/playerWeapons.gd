extends Node
#0 - name of weapon, 1 - scene of weapon, 2 - path to node of weapon
var weaponsArray:Array
@onready var playerMain = $".."
var currentlyActiveWeapon:int = 0
var currentlyHoldsWeapon:bool = false
var currentlyShoot:bool = false:
	set(value):
		if value==true and timeFromLastShoot>weaponsArray[currentlyActiveWeapon].rateOfFire and currentlyHoldsWeapon==true:
			weaponsArray[currentlyActiveWeapon].shootBullet(spreadCurrent)
			timeFromLastShoot = 0
			if spreadCurrent<=weaponsArray[currentlyActiveWeapon].spreadMax:
				spreadCurrent+=weaponsArray[currentlyActiveWeapon].spreadSpeedUp
		currentlyShoot = value
		

func _ready() -> void:
	weaponsArray.append($AK)
	for i in weaponsArray:
		i.get_child(1).disabled = true

var spreadCurrent:float = 0:
	set(value):
		spreadCurrent = value
		playerMain.toDistributorHUDUpdateSpread(value)
var timeFromLastShoot = 0
func _physics_process(delta: float) -> void:
	if currentlyShoot and timeFromLastShoot>weaponsArray[currentlyActiveWeapon].rateOfFire and currentlyHoldsWeapon==true:
		weaponsArray[currentlyActiveWeapon].shootBullet(spreadCurrent)
		timeFromLastShoot=0
		if spreadCurrent<=weaponsArray[currentlyActiveWeapon].spreadMax:
			spreadCurrent+=weaponsArray[currentlyActiveWeapon].spreadSpeedUp
	elif timeFromLastShoot<weaponsArray[currentlyActiveWeapon].rateOfFire:
		timeFromLastShoot+=delta
	
	if spreadCurrent>=weaponsArray[currentlyActiveWeapon].spreadMin:
		spreadCurrent-=weaponsArray[currentlyActiveWeapon].spreadSpeedDown*delta


func pickupWeapon(InstanceWeapon) -> void:
	for i in weaponsArray:
		if InstanceWeapon.get_child(0).name == i.nameOfWeapon:
			InstanceWeapon.queue_free()
			if currentlyHoldsWeapon:
				dropWeapon()
				i.visible = true
				currentlyHoldsWeapon = true
			else:
				i.visible = true
				currentlyHoldsWeapon = true
			spreadCurrent = i.spreadMin
		
	
func dropWeapon() -> void:
	if currentlyHoldsWeapon==true:
		weaponsArray[currentlyActiveWeapon].visible=false
		var weaponInstance = weaponsArray[currentlyActiveWeapon].resourceOfWeapon.instantiate()
		playerMain.toDistributorPlaceWeapon(weaponInstance)
		currentlyHoldsWeapon = false
