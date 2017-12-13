### 接口
#### MoveInterface接口

```
各种类型棋子的移动
```



### 逻辑实现类
#### Weights类

```
主要是获取棋子和棋盘的权重值
```

方法 | 返回类型 | 参数类型 |功能描述
---|---|---|---
GetBoardWeight   |int  | Piece.pieceType, Vector2 , Piece.playerColor|获取棋盘对于每个棋子的权重值
GetPieceWeight   | int |Piece.pieceType|棋子的权重


#### Piece类

```
棋子类主要的功能是定义棋子的类型和棋子的颜色，以及棋子的点击事件
```

```
enum pieceType 棋子类型
enum playerColor 棋子颜色
```

方法 | 返回类型 | 参数类型 |功能描述
---|---|---|---
OnMouseOver |void | null| 点击棋子
HasMoved    |bool | null| 棋子是否移动
MovePiece   |void |Vector3|得到棋子的移动位置


#### Board类

```
棋盘类主要分割棋盘的坐标
```


方法 | 返回类型 | 参数类型 |功能描述
---|---|---|---
SetupBoard|void|null|给棋盘划分坐标
GetTileFromBoard|Tile|Vector2|获取棋盘上的棋子

#### Tile类

```
主要是根据坐标获取棋子
```


```
构造函数：Tile，参数(int,int)，根据传入的坐标返回具体的棋子
```


方法 | 返回类型 | 参数类型 |功能描述
---|---|---|---
CurrentPiece|Piece|null|获得当前棋子
SwapFakePieces|void|Piece|交换棋子

#### Move类

```
主要是定义关于移动棋子的一些变量
```


```
Tile firstPosition   第一个棋子的位置
Tile secondPosition  将要将棋子移动到的位置
Piece pieceMoved     移动的棋子
Piece pieceKilled    被吃掉的棋子
int score            棋子的分数
```

#### MoveFactory类


```
处理的棋子的移动以及King是否被威胁
```
```
构造函数：MoveFactory，参数（Board），初始化棋子
```

方法 | 返回类型 | 参数类型 |功能描述
---|---|---|---
_GenerateMove| void |int,Vector2|计算可移动区域
_CheckAndStoreMove|void|Vector2|存储移动轨迹
_IsEnemy|bool|Tile|判断两个棋子的颜色是否一致
_ContainsPiece|bool|Tile|判断棋盘的位置上是否有棋子
_IsOnBoard|bool|Vector2|判断位置是否超出棋盘位置
CheckKing|bool|null|检查King是否受到威胁


#### GameManager类

```
游戏管理，包括棋子的具体移动以及悔棋
```
方法 | 返回类型 | 参数类型 |功能描述
---|---|---|---
_DoAIMove|void|Move|处理AImove
SwapPieces|void|Move|移动棋子
ReturnChess|void|null|悔棋


#### AlphaBeta类

```
AI算法类
```
方法 | 返回类型 | 参数类型 |功能描述
---|---|---|---
GetMove|Move|null|选择最合适移动棋子
Alpha|int|int,int,int,bool|计算Alpha值
_UndoFakeMove|void|null|暂不移动
_DoFakeMove|void|Tile,Tile|AI棋子移动
_Evaluate|int|null|计算棋子权重
_GetMoves|List<Move>|Piece.playerColor|棋子可移动的位置
_GetBoardState|void|null|得到棋盘的状态

