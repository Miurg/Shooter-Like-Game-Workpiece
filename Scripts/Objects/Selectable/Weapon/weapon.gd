class_name Weapon
extends Node

@onready var resourceOfWeapon
@onready var currentOwner = get_parent().get_parent()
@onready var currentMasterWeapon = get_parent()
@onready var soundOfWeaponForNPC
@onready var soundOfWeapon 
var rateOfFire:float = 1
var maxDistanceForNPC:float = 15
var damage:int = 1
var nameOfWeapon
