### HTMLにおけるHTTPメソッドについて

#### HTMLで送信する

基本的なHTTPメソッドである。  
`GET`と`POST`でフォームデータなどを送信する場合、  
下記の様にすることで送信することができる。

```
<form action="<URL>" method="<Method>">
    <-- inputのタグに関して適宜buttonなどに変更 -->
    <input type="submit" value="<Value>"/>
</form>
```

しかし、REST APIを作るためには、他に少なくとも`PUT`と`DELETE`のメソッドを呼ぶ必要がある。  
残念ながら、HTML規格では`GET`と`POST`のみサポートしている。  
過去[色々な議論](http://jxck.hatenablog.com/entry/why-form-dosent-support-put-delete)があったが、結果として`GET`と`POST`のみをサポートする仕様となっている。  

HTMLで`PUT`と`DELETE`で送信を行う方法を検索すると以下のようなコードを見つけることができる。

```
<form method="post" action="<URL>">
    <input type="text" name="textArea">
    <input type="hidden" name="_method" value="<Method>">
    <input type="submit" value="<Method>">
</form>
```

注意が必要なのは、この内容で送信できるのは`Ruby On Rails`を使っている場合だけだ。  
使用していなければ`POST`で送信されてしまう。  

#### JavaScriptで送信する  

前項のことから、フレームワークを使用しないとHTMLとHTTPメソッドの振り分けでREST APIを作るのは厳しいと判断できる。  

ならば、JavaScriptを利用することにしよう。  
バニラのJavaScriptで達成したいので、`Jquery`等の利用はしない。  

そういうことなので`XMLHttpRequest`オブジェクトを使って送信をする。  
これは、非同期でページ遷移を必要としないデータ送受信とページ内容の変更ができる。  

```
var xmlHttpRequest = new XMLHttpRequest();
//  第3引数は非同期送信するかを示す。既定値はtrue、同期処理は非推奨
xmlHttpRequest.open( '<Method>', '<URL>',true );
xmlHttpRequest.send( <送信データ>);
```

上記の流れで送信はすることができる。

何もなければこれで終わりであるが、
```
Access to XMLHttpRequest at '<URL>' from origin 'null' has been blocked by CORS policy: No 'Access-Control-Allow-Origin' header is present on the requested resource.
```
というエラーがブラウザの開発者ツールを開くと出てくることがある。
これは、送信リクエストヘッダーの`Request Headers`の'Access-Control-Allow-Origin'が`null`なのが`CORS`によって弾かれたことを意味する。  
* [`CORS`](https://developer.mozilla.org/ja/docs/Web/HTTP/CORS)はセキュリティの一種で、異なるドメインからのアクセスの対するものである。  

いろいろ検索してみたが、クライアント側からいじる方法についてよくわからなかったので、サーバの該当ファイルで権限を設定することにした。  

```
header('Access-Control-Allow-Origin:*');
```
PHPファイルの頭に付けるだけ。`*`はワイルドカードなので、特定のドメインを許可したいなら`*`を指定のURLに変更する。  

これで送信をすると、`GET`と`POST`は送信できたが、`PUT`と`DELETE`は以下のようなエラーを吐かれる。  
```
//DELETEの場合
Access to XMLHttpRequest at '<URL>' from origin 'null' has been blocked by CORS policy: Method DELETE is not allowed by Access-Control-Allow-Methods in preflight response.

//PUTの場合
Access to XMLHttpRequest at '<URL>' from origin 'null' has been blocked by CORS policy: Method PUT is not allowed by Access-Control-Allow-Methods in preflight response.
```

今度は、「`PUT`と`DELETE`のメソッドが許可されてないので駄目です」と言われる。  
先の変更と同じようにPHPファイルに対して設定をすることにする。

```
header('Access-Control-Allow-Methods:GET,POST,PUT,DELETE');
```
これで、`GET`,`POST`,`PUT`,`DELETE`の4種のメソッドに対する許可を出せた。  

送信してみれば、ブラウザの開発者ツール上からそれぞれのメソッドでリクエストにを投げれていることがわかる。  

#### 受信内容

ここまでで、送信をすることまでは完了した。  
今度は、受信結果を表示することにしよう。

今回のPHPのページはメソッドごとに異なる文字列を返却してくるようになっている。  
可能であれば、`form`タグの時のようにページ遷移していきたいと思うが、  
返ってきたデータのURLへのアクセスは`GET`か`POST`のどちらかにしか移動できない。  

あるのかもしれないが、`PUT`と`DELETE`でリクエストしたページに飛ぶ方法が見つからなかったため、  
送信結果をHTMLの方に結果を表示することにする。  

内容は簡単だ。  
リクエストの送受信が完了したら、返却されたデータを`element.innerHTML`に置くだけだ。
```
    let responseText = document.querySelector(".ResponseText");
    responseText.innerHTML = xmlHttpRequest.responseText;
```


