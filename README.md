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
