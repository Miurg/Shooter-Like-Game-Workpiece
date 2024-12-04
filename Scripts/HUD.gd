extends Node

@onready var iconsSelected = $IconsSelected
@onready var roundsPocket = $HBoxContainer/RoundsPocket
@onready var roundsCurrent = $HBoxContainer/RoundsCurrent
var positionX:int
var positionY:int 
var sizeX:int
var sizeY:int
var spreadWeapons = 0:
	set(value):
		spreadWeapons = value
		iconsSpreadApply()

func _ready() -> void:
	toNormalIcons()

func setRoundsPocket(value) -> void:
	roundsPocket.text = str(value)

func setRoundsCurrent(value) -> void:
	roundsCurrent.text = str(value)

func updateIconsSelected(sizeXCurrent:int,sizeYCurrent:int,positionXCurrent:int,positionYCurrent:int) -> void:
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

func iconsSelectedApply(delta:float) -> void:
	iconsSelected.size = iconsSelected.size.lerp(Vector2(sizeX,sizeY),10*delta)
	iconsSelected.position = iconsSelected.position.lerp(Vector2(positionX,positionY),10*delta)

func _process(delta:float) -> void:
	if spreadWeapons<=0:
		iconsSelectedApply(delta)
