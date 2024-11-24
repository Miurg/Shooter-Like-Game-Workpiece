class_name WeaponShooting
extends Weapon
@onready var holeNode
var spreadMax
var spreadMin
var spreadSpeedUp
var spreadSpeedDown

func shootBullet(spread) -> void:
		var posOrNeg = [-1,1]
		var spreadVector = Vector3(0,0,-10)
		if spread>0:
			var dotsForSpread = Vector2(randf_range(0,spread)*posOrNeg[randi() % posOrNeg.size()],randf_range(0,spread)*posOrNeg[randi() % posOrNeg.size()])
			while pow(dotsForSpread.x,2)+pow(dotsForSpread.y,2)>spread:
				dotsForSpread = Vector2(randf_range(0,spread)*posOrNeg[randi() % posOrNeg.size()],randf_range(0,spread)*posOrNeg[randi() % posOrNeg.size()])
			spreadVector = Vector3(dotsForSpread.x,dotsForSpread.y,-10).normalized()
		var arrayOfCollider = currentOwner.getRay(0b00000000_00000000_00000000_00000011,spreadVector*100)
		if arrayOfCollider[0]!=null:
			if arrayOfCollider[0].get_collision_layer()==1:
				currentOwner.toDistributorCreateHole(arrayOfCollider[0],arrayOfCollider[1],arrayOfCollider[2],holeNode)
			else: 
				arrayOfCollider[0].takeDamage(damage)
