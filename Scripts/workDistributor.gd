extends Node
@onready var HUD = $HUD
@onready var Entitys = $Entitys
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

func HUDIconsSelectedApply(delta,sizeX,sizeY,positionX,positionY):
	HUD.iconsSelectedApply(delta,sizeX,sizeY,positionX,positionY)

func createWeapon(weaponInstance,impulse):
	Entitys.add_child(weaponInstance)
	weaponInstance.apply_impulse(impulse)
