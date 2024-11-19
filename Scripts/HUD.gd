extends Node

@onready var iconsSelected = $IconsSelected
# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	pass

func iconsSelectedApply(delta,sizeX,sizeY,positionX,positionY) -> void:
	iconsSelected.position = iconsSelected.position.lerp(Vector2(positionX,positionY),6*delta)
	iconsSelected.size = iconsSelected.size.lerp(Vector2(sizeX,sizeY),6*delta)
