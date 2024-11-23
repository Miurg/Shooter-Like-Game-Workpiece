class_name WeaponShooting
extends Weapon


func shootBullet() -> void:
		var arrayOfCollider = currentOwner.getRay(0b00000000_00000000_00000000_00000011,1000)
		if arrayOfCollider[0]!=null:
			if arrayOfCollider[0].get_collision_layer()==1:
				currentOwner.toDistributorCreateHole(arrayOfCollider[0],arrayOfCollider[1],arrayOfCollider[2])
			else: 
				arrayOfCollider[0].healthPoint-=damage
				print_debug(arrayOfCollider[0].healthPoint)
