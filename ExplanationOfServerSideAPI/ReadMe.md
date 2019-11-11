# Server side API の動きを解説

以下の動作が行われるときの処理の流れを解説するreportを作成する。

1. localPCにweb pageを表示
2. web page上のbuttonがclickされたらserver side APIを呼び出し
3. server side側ではPHP programを実行し、何らかのobjectをJSON形式で返却
4. localPCではresponseとして受け取ったJSONをobjectに変換して画面に表示

client側、server side側それぞれがどういう処理を行うのかがわかるように整理して記述しましょう。

ここに個人ごとのfolderを作成し、その中にMarkdown形式で記述したreportを置いて下さい。
