extends Node
#0 - name of weapon, 1 - screne of weapon, 2 - path to node of weapon
var weaponsStatusArray:Array[Array]
@onready var playerMain = $".."
@onready var bullet = preload("res://Nodes/bullet.tscn")

func _ready() -> void:
	weaponsStatusArray.append(Array())
	weaponsStatusArray[0].append("AK")
	weaponsStatusArray[0].append(preload("res://Nodes/AK.tscn"))
	weaponsStatusArray[0].append($AK)


func pickupWeapon(InstanceWeapon):
	for i in weaponsStatusArray:
		if InstanceWeapon.get_child(0).name == i[2].name:
			InstanceWeapon.queue_free()
			if i[2].visible == true:
				dropWeapon()
				i[2].visible = true
			else:i[2].visible = true
		
	
func dropWeapon():
	for i in weaponsStatusArray:
		if i[2].visible==true:
			i[2].visible=false
			var weaponInstance = i[1].instantiate()
			playerMain.toDistributorCreateWeapon(weaponInstance)
			return

func shootOneBullet():
	var newBullet = bullet.instantiate()
	
