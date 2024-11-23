class_name Weapon
extends Node

@onready var currentOwner = get_parent().get_parent()
@onready var currentMasterWeapon = get_parent()
var rateOfFire:float = 1
var damage:int = 1

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	pass
	
