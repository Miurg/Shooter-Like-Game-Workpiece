extends Node
@onready var HUD = $HUD
@onready var objectsNode = $Objects
@onready var allBulletsAndHolesNode = $Objects/AllBulletsAndHoles

func _ready() -> void:
	Input.set_mouse_mode(Input.MOUSE_MODE_CAPTURED)
	#Engine.max_fps = 17


func _input(event):
	if event is InputEventKey:
		if event.keycode == KEY_ESCAPE:
			get_tree().quit()
func HUDUpdateSpread(spread):
	HUD.spreadWeapons = spread
				

func HUDToNormalIcons():
	HUD.toNormalIcons()

func HUDUpdateIconsSelected(sizeX,sizeY,positionX,positionY) -> void:
	HUD.updateIconsSelected(sizeX,sizeY,positionX,positionY)

func HUDSetRoundsPocket(roundsPocket) -> void:
	HUD.setRoundsPocket(roundsPocket)

func HUDSetRoundsCurrent(roundsCurrent) -> void:
	HUD.setRoundsCurrent(roundsCurrent)

func placeWeapon(fromWho,weaponInstance,impulse,position) -> void:
	objectsNode.add_child(weaponInstance)
	weaponInstance.position = position
	weaponInstance.apply_impulse(impulse)
	if weaponInstance.position!=Vector3(fromWho.position.x,position.y,fromWho.position.z):
		weaponInstance.look_at(Vector3(fromWho.position.x,position.y,fromWho.position.z))
	weaponInstance.rotate_y(deg_to_rad(-90))

func placeBullet(bulletInstance,velocity,position,rotation) -> void:
	allBulletsAndHolesNode.add_child(bulletInstance)
	bulletInstance.position = position
	bulletInstance.velocity = velocity
	bulletInstance.rotation = rotation

func createHoleFromBullet(wallInstance,newPosition,newNormal,holeNode) -> void:
	var newHole = holeNode.instantiate() 
	newHole.position = newPosition+newNormal/1000
	allBulletsAndHolesNode.add_child(newHole)
	if newNormal.y!=1 and newNormal.y!=-1:
		newHole.look_at(newPosition+newNormal,Vector3.UP)
	else:newHole.look_at(newPosition+newNormal,Vector3.FORWARD)
	newHole.rotation.z = deg_to_rad(randf_range(0,360))
