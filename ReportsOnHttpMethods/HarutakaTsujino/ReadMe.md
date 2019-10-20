# GET POSTの違いpros/cons
### GET
URLにparameterがつく
- **pros** debugがしやすい
- **cons** 検索engineのcrawlerが見に来るので大事なdataをつけれない

cashされることが多い
- **pros** 読み込みが早くなり、data通信量を減らせる
- **cons** 有効期間が長いと、ずっと古い表示のまま

**pros** bookmarkできる  
**cons** binaryで送れない  
**cons** data量に制限2048文字  
### POST
parameterはbodyにつく
- **pros** crawlerが見に来ても安心
- **cons** debugしにくい

cashされないことが多い
- **pros** serverのfileを書き換えた場合、すぐに同じ状態を読み込める
- **cons** 同じ内容を毎回読み込む場合無駄が多い

**pros** binaryで送れる  
**pros** data量制限なし  
**cons** bookmarkできない  

### 使用時の留意点
**GET**はその自身の特徴からdataを取得する際に用いられることを前提としている。
crawlerはurlを参照するのでdatabaseに変更を加えたり、
passwordでlog inするなどを**GET**で行ってはならない。
**POST**は**GET**と反対にdataを送る目的に用いられる。
基本的に**POST**の方が安全である。

#### bookmark
bookmarkはurlを保存する機能である。
それによりpost等parameterがurlにつかないものは
parameterがない不完全なurlしか保存できない

#### crawler
internet上に無数にあるweb pageを巡回しserverに保存することにより
検索を行いやすくするための機能。
**GET**はこれが見に来る可能性があるので実行されたり
保存されたら困ることは行わないようにする。
