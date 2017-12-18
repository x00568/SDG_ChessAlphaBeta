#### GameManager类

方法名 | 返回值 |参数|功能描述
---|---|---|---
SetDifficult| int |难度的枚举DifficultyEnum|主要是设置难度
SetupBoard| void |null|初始化棋盘格
SwapPieces|void|Move|棋子的的移动
ReturnChess|void|null|悔棋
AIMove|void |null |调用AI的棋子移动
WhoPlay|bool|null|判断谁走棋，返回true的话白子走
WhiteWin|bool|null|返回true白棋赢
BlackWin|bool|null|返回true黑棋赢
WhiteAttack|bool|null|返回true白色King受到威胁
BlackAttack|bool|null|返回true黑色King受到威胁

#### 几个比较补充的类说明

```
1. Weights 权重类，此类作用在于计算每个棋子对于其他棋子的权重值
2. Piece 棋子类， 使用时，将此类挂载到棋子上，并指定以下数据即可
    棋子类型型enum pieceType { KING, QUEEN, BISHOP, ROOK, KNIGHT, PAWN, UNKNOWN = -1 }
    棋子颜色 enum playerColor { BLACK, WHITE, UNKNOWN = -1 }
    棋子位置 Vector2 position
3. Board 棋盘类，生成棋盘格，划分坐标
4. Tile 棋盘格类，根据坐标棋盘格的坐标查找对应格子上的棋子
5. Move 移动类，存储选择当前棋子后，存储选择棋子以及可走区域和吃子的信息
```

