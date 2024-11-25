extends Node

@onready var iconsSelected = $IconsSelected
var positionX
var positionY 
var sizeX 
var sizeY
var spreadWeapons = 0:
	set(value):
		spreadWeapons = value
		iconsSpreadApply()

func _ready() -> void:
	toNormalIcons()

func updateIconsSelected(sizeXCurrent,sizeYCurrent,positionXCurrent,positionYCurrent) -> void:
	sizeX = sizeXCurrent
	sizeY = sizeYCurrent
	positionX = positionXCurrent
	positionY = positionYCurrent
	
func toNormalIcons() -> void:
	sizeX = 34
	sizeY = 34
	positionX = (get_viewport().get_visible_rect().size.x / 2)-sizeX/2
	positionY = (get_viewport().get_visible_rect().size.y / 2)-sizeY/2

func iconsSpreadApply() -> void:
	iconsSelected.size = Vector2(sizeX*(spreadWeapons+1),sizeY*(spreadWeapons+1))
	iconsSelected.position = Vector2((positionX+sizeX/2)-(iconsSelected.size.x/2),(positionY+sizeY/2)-(iconsSelected.size.y/2))
	print_debug("EWQ")

func iconsSelectedApply(delta) -> void:
	iconsSelected.size = iconsSelected.size.lerp(Vector2(sizeX,sizeY),10*delta)
	iconsSelected.position = iconsSelected.position.lerp(Vector2(positionX,positionY),10*delta)

func _process(delta: float) -> void:
	if spreadWeapons<=0:
		iconsSelectedApply(delta)
