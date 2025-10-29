# 开发日志
## 2025-10-21（Slaice）
- 基础移动功能，加入Rigidbody，挂载PlayerMovement脚本
- 基础UI，挂载UIManager脚本
- 相机跟随功能，使用Cinemachine

## 2025-10-23 (Slaice)
- 添加了Ore和OreDrop的prefab，并添加了矿石生成脚本OreGenerate、矿石挖掘脚本Ore以及矿石被拾取脚本OrePickedup
- 解决了Virtual Camera跟随玩家运动过程中的bug，修改了相关参数

## 2025-10-24（Slaice）
- 添加了子弹bullet、敌人enemy预制体以及激光raycast预制体（未实现）
- 添加了玩家移动射击功能，并且由鼠标控制攻击方向，挂载了PlayerShoot脚本以及Bullet属性脚本，实现了基础的受击逻辑提示
- 解决了玩家射线检测时摄像机与鼠标冲突的bug

## 2025-10-25（Slaice）
- 添加了敌人enemy的最基本逻辑（EnemyAI脚本），自动随机生成在地图四周（EnemyGenerate脚本）
- 实现enemy自动侦测玩家位置，自动朝玩家移动，解决了enemy与player在update时的冲突问题：将enemy的位移方式从rb改为transform.position
- 解决了enemy与ore碰撞的问题：直接取消layer的物理碰撞
- 解决了enemy会在玩家周围聚成一坨的问题：添加avoidRadius

## 2025-10-26（Slaice）
- 完善了enemy死亡销毁及掉落矿石机制，修改了EnemyAI
- 完善了ore摧毁后矿石掉落机制，并解决掉落数量以及拾取范围bug
- 解决了生成enemy因为朝向玩家移动导致的悬空bug
- 添加了随时间生成波数enemy的功能

## 2025-10-27（Slaice）
- 添加了enemy近战攻击player的功能，写在EnemyAI中
- 修复了enemy因为is Kinematic会与玩家重合的问题
- 添加了player阵亡时的UI界面，可以选择重新开始游戏

## 2025-10-28（Slaice）
- 去assert store寻找合适资源

## 2025-10-29（Slaice）
- 将找到的模型、贴图import到项目中
- 给ore和oredrop添加模型，调整色彩和大小
- 给enemy1添加模型，并引入动画系统，完成animator controller的连接，并添加Parameters
- 修改脚本（EnemyAI和EnemyGenerate）使其触发Parameters，调整各个动画状态的响应顺序，实现完整动画逻辑
- 修复DealDamage函数在Events无法被找到的问题：子物体没有挂载EnemyAI，添加了中间脚本EventForwarder
- 解决了enemy伤害出伤时间、伤害范围判定、动画切换迟钝等问题
