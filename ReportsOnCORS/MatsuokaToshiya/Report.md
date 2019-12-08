### CORSについて

#### CORS

CORS(**C**ross-**O**rigin **R**esource **S**haring)  
異なるオリジンとのリソースへのアクセスに関する仕組み。  

* **オリジン**  
スキーム、ホスト、ポートの組み合わせ、どこかが違えばそれは違うオリジンと認識される。  
スキーム ＝ http://  
ホスト ＝ www.google.co.jp  
ポート ＝ 80  (省略されることが多い)  

#### なぜ作られたか？

サーバ側のセキュリティ強化のため。  
CORSが作られる前のHTTPリクエストの仕組みでは、  
サーバ内でのみ動作してほしい公開していないAPIに、  
URLさえ知っていれば、外部から叩くことができた。  
それができてしまうと、ユーザーの情報などを不正規の手段で更新削除ができてしまう。  

それを、URLをただ叩くことではアクセスをできないようにした仕組みがCORSである。  

#### 動き

CORSの制限を動かしているのは、サーバではなくクライアントの**ブラウザ**である。  
異なるオリジン(クロスオリジン)である時の通信で制限の対象となるリソースとそうでないものがある。  

CORSポリシーの制限を受けるWebAPI
```
XMLHttpRequest   
Canvas  
Web Storage  
X-Frame-Options  
```
制限を受けないもの
```html
<script src="..."></script> によって読み込まれるJavaScript
<link rel="stylesheet" href="..."> によって読み込まれるCSS
<img>, <audio>, <video> のメディアファイル
@font-face で指定されたフォント
<frame>, <iframe>
<object>, <embad>, <applet>によるプラグイン
```

なので、`form`タグで通信される場合にはCORSポリシーの制限外なのでこれを気にする必要はない。  
こういったリクエストを**単純リクエスト**と呼ばれている。  

* **単純リクエスト**  
    * 許可されているメソッドのうちの一つであること。  
      * GET  
      * HEAD  
      * POST  
    * ユーザーエージェントによって自動的に設定されたヘッダー (たとえば Connection、 User-Agent、 
または Fetch 仕様書で「禁止ヘッダー名」として定義されているヘッダー) を除いて、手動で設定できるヘッダーは、
 Fetch 仕様書で「CORS セーフリストリクエストヘッダー」として定義されている以下のヘッダーだけです。  
      * Accept
      * Accept-Language  
      * Content-Language  
      * Content-Type (但し、下記の要件を満たすもの)  
      * DPR  
      * Downlink  
      * Save-Data  
      * Viewport-Width  
      * Width  
    * Content-Type ヘッダーでは以下の値のみが許可されています。  
      * application/x-www-form-urlencoded  
      * multipart/form-data  
      * text/plain  
    * リクエストに使用されるどの`XMLHttpRequestUpload`にもイベントリスナーが登録されていないこと。これらは正しく`XMLHttpRequest.upload`を使用してアクセスされます。  
    * リクエストに`ReadableStream`オブジェクトが使用されていないこと。  

この単純リクエストが許可されている理由は、それまでの`GET`や`POST`でのカスタムヘッダを使用しないリクエストに対して対応が必要ではないからである。  

#### 対処

端的に言えば、サーバ側の外に公開するPHPファイルなどに異なるオリジンからのアクセスを許可するようにする。  
```php:index.php
header("Access-Control-Allow-Origin: *");
header('Access-Control-Allow-Methods:GET,POST,PUT,DELETE,OPTIONS');
```

---

#### Preflight
pre…あらかじめ  
flight…飛ぶこと  
CORSの話題では「予め飛ばす通信」のことを指す。  

本命のリクエスト(`PUT`や`DELETE`)を送る前に、送ることで送信しても安全か確かめるために行う。  
これをせずに本命のリクエストを送ってしまうと、  
「サーバはリクエストを受け付けるが、ブラウザはそのレスポンスを受け付けない。」  
という状態に陥ってしまう。  

それでは、CORSでリソースは弾くことができるが、API自体は発火するので困ってしまう。  

そうならないために、`XMLHttpRequest`などで送信するような時に、先に`OPTIONS`メソッドでHTTPリクエストを送って、  
それで判断した結果OKなら本命のリクエストを送る。  
 * この仕組は自動で働くので自作する必要はない。  

失敗したときにはブラウザの開発者ツールのコンソールに以下のようなメッセージが出現する。
```
オリジンが許可されていない時
Access to XMLHttpRequest at '<URL>' from origin 'null' has been blocked by CORS policy:
No 'Access-Control-Allow-Origin' header is present on the requested resource.
```
```
メソッドが許可されていない時
Access to XMLHttpRequest at '<URL>' from origin 'null' has been blocked by CORS policy:
Method <メソッド名> is not allowed by Access-Control-Allow-Methods in preflight response.
```
開発者ツールのNetworkで見れば、`OPTIONS`のリクエストだけ送られて、本命が送られていないことがわかる。  

