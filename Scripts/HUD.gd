extends Node

@onready var iconsSelected = $IconsSelected



func iconsSelectedApply(delta,sizeX,sizeY,positionX,positionY) -> void:
	iconsSelected.position = iconsSelected.position.lerp(Vector2(positionX,positionY),10*delta)
	iconsSelected.size = iconsSelected.size.lerp(Vector2(sizeX,sizeY),10*delta)
