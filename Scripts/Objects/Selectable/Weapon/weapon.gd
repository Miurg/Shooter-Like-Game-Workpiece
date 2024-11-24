class_name Weapon
extends Node

@onready var currentOwner = get_parent().get_parent()
@onready var currentMasterWeapon = get_parent()
var rateOfFire:float = 1
var damage:int = 1
