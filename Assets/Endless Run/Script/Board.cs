using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : Obstacle
{

	public override void OnTriggerEnter(Collider other)
	{
		if (!PlayController.instance.isRoll) // 如果不在滚动状态就生命值减一
			base.OnTriggerEnter(other);
	}
}
