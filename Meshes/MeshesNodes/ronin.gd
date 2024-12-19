extends Node3D

enum {IDLE,WALK,RUN,ATTACKRUN,DIE}
var curAnim = IDLE

@onready var animation_tree: AnimationTree = $AnimationTree

@export var blend_speed = 15 

var run_val = 0
var walk_val = 0
var attackrun_val = 0
var die_val = 0

func _physics_process(delta):
	handle_animations(delta)
	
	curAnim = RUN

func handle_animations(delta):
	match curAnim:
		IDLE:
			run_val = lerpf(run_val,0, blend_speed*delta)
			walk_val = lerpf(walk_val,0, blend_speed*delta)
			attackrun_val = lerpf(attackrun_val,0, blend_speed*delta)
			die_val = lerpf(die_val,0, blend_speed*delta)
		RUN: 	
			run_val = lerpf(run_val,1, blend_speed*delta)
			walk_val = lerpf(walk_val,0, blend_speed*delta)
			attackrun_val = lerpf(attackrun_val,0, blend_speed*delta)
			die_val = lerpf(die_val,0, blend_speed*delta)
		WALK:
			run_val = lerpf(run_val,0, blend_speed*delta)
			walk_val = lerpf(walk_val,1, blend_speed*delta)
			attackrun_val = lerpf(attackrun_val,0, blend_speed*delta)
			die_val = lerpf(die_val,0, blend_speed*delta)	
		ATTACKRUN:
			run_val = lerpf(run_val,0, blend_speed*delta)
			walk_val = lerpf(walk_val,0, blend_speed*delta)
			attackrun_val = lerpf(attackrun_val,1, blend_speed*delta)
			die_val = lerpf(die_val,0, blend_speed*delta)
		DIE:
			run_val = lerpf(run_val,0, blend_speed*delta)
			walk_val = lerpf(walk_val,0, blend_speed*delta)
			attackrun_val = lerpf(attackrun_val,0, blend_speed*delta)
			die_val = lerpf(die_val,1, blend_speed*delta)	

func update_tree():
	animation_tree["parameters/Run/blend_amount"] = run_val
	animation_tree["parameters/Walk/blend_amount"] = walk_val
	animation_tree["parameters/AttackRun/blend_amount"] = attackrun_val
	animation_tree["parameters/Die/blend_amount"] = die_val
	
