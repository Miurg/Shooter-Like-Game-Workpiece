extends Node
#0 - name of weapon, 1 - scene of weapon, 2 - path to node of weapon
var weaponsStatusArray:Array[Array]
@onready var playerMain = $".."
var currentlyActiveWeapon:int = 0
var currentlyHoldsWeapon:bool = false
var currentlyShoot:bool = false
func _ready() -> void:
	weaponsStatusArray.append(Array())
	weaponsStatusArray[0].append("AK")
	weaponsStatusArray[0].append(preload("res://Nodes/Objects/AK.tscn"))
	weaponsStatusArray[0].append($AK)
	for i in weaponsStatusArray:
		i[2].get_child(1).disabled = true

var spreadCurrent:float = 0:
	set(value):
		spreadCurrent = value
		playerMain.toDistributorHUDUpdateSpread(value)
var timeFromLastShoot = 0
func _physics_process(delta: float) -> void:
	if currentlyShoot and timeFromLastShoot>weaponsStatusArray[currentlyActiveWeapon][2].rateOfFire and currentlyHoldsWeapon==true:
		weaponsStatusArray[currentlyActiveWeapon][2].shootBullet(spreadCurrent)
		timeFromLastShoot=0
		if spreadCurrent<=weaponsStatusArray[currentlyActiveWeapon][2].spreadMax:
			spreadCurrent+=weaponsStatusArray[currentlyActiveWeapon][2].spreadSpeedUp
	elif currentlyShoot and timeFromLastShoot<weaponsStatusArray[currentlyActiveWeapon][2].rateOfFire:
		timeFromLastShoot+=delta
	else:
		if spreadCurrent>weaponsStatusArray[currentlyActiveWeapon][2].spreadMin:
			spreadCurrent-=weaponsStatusArray[currentlyActiveWeapon][2].spreadSpeedDown*delta

func pickupWeapon(InstanceWeapon) -> void:
	for i in weaponsStatusArray:
		if InstanceWeapon.get_child(0).name == i[0]:
			InstanceWeapon.queue_free()
			if currentlyHoldsWeapon:
				dropWeapon()
				i[2].visible = true
				currentlyHoldsWeapon = true
			else:
				i[2].visible = true
				currentlyHoldsWeapon = true
			spreadCurrent = i[2].spreadMin
		
	
func dropWeapon() -> void:
	if currentlyHoldsWeapon==true:
		weaponsStatusArray[currentlyActiveWeapon][2].visible=false
		var weaponInstance = weaponsStatusArray[currentlyActiveWeapon][1].instantiate()
		playerMain.toDistributorPlaceWeapon(weaponInstance)
		currentlyHoldsWeapon = false
