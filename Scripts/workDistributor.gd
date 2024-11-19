extends Node
@onready var HUD = $HUD
@onready var objectsNode = $Objects
@onready var allBulletsAndHolesNode = $Objects/AllBulletsAndHoles
@onready var holeNode = preload("res://Nodes/Hole.tscn")
# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	Input.set_mouse_mode(Input.MOUSE_MODE_CAPTURED)
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	pass


func _input(event):
	if event is InputEventKey:
		if event.keycode == KEY_ESCAPE:
			get_tree().quit()

func HUDIconsSelectedApply(delta,sizeX,sizeY,positionX,positionY) -> void:
	HUD.iconsSelectedApply(delta,sizeX,sizeY,positionX,positionY)

func placeWeapon(weaponInstance,impulse,position) -> void:
	objectsNode.add_child(weaponInstance)
	weaponInstance.position = position
	weaponInstance.apply_impulse(impulse)

func placeBullet(bulletInstance,velocity,position,rotation) -> void:
	allBulletsAndHolesNode.add_child(bulletInstance)
	bulletInstance.position = position
	bulletInstance.velocity = velocity
	bulletInstance.position = position
	bulletInstance.rotation = rotation

func createHoleFromBullet(wallInstance,newPosition,newNormal):
	var newHole = holeNode.instantiate()
	newHole.position = newPosition+newNormal/1000
	newHole.rotation = wallInstance.rotation
	allBulletsAndHolesNode.add_child(newHole)
	if newNormal.y!=1 and newNormal.y!=-1:
		newHole.look_at(newPosition+newNormal,Vector3.UP)
	else:newHole.look_at(newPosition+newNormal,Vector3.FORWARD)
