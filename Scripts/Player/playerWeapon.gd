extends Node

var weaponsResources:Array
var weaponsName:Array
@onready var playerMain = $".."
@onready var weapons = get_children()

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	weaponsResources.append(preload("res://Nodes/AK.tscn"))
	weaponsName.append("AK")
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	pass

func pickupWeapon(InstanceWeapon):
	for i in weapons:
		if InstanceWeapon.name == i.name:
			InstanceWeapon.queue_free()
			i.visible = true
		
	
func dropWeapon():
	for i in weapons:
		if i.visible==true:
			for j in weaponsResources.size():
				if i.name==weaponsName[j]:
					i.visible=false
					var weaponInstance = weaponsResources[j].instantiate()
					weaponInstance.name = weaponsName[j]
					playerMain.toDistributorCreateWeapon(weaponInstance)
					return
