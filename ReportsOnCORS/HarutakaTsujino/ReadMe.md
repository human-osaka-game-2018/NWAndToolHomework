# CORSについて
## SOP概要
**同一生成元ポリシー** (**Same-Origin Policy** 通称**SOP**)<br>
異なる**オリジン** (スキーム + ホスト + ポート)<br>
のリソースにアクセスできないように制限するものです。<br>

## CORS概要
**オリジン間リソース共有** (**Cross-Origin Resource Sharing** 通称**CORS**)<br>
異なる**オリジン**間でのリソースへのアクセスを**ブラウザ**で制御するための仕組みです。<br>

## CORS導入の背景
クロスサイトリクエストフォージェリ (CSRF) などのセキュリティ攻撃を防止するために、<br>
ブラウザは**SOP**という仕組みを実装しました。<br>
しかし一部分は異なる**オリジン**間でアクセスする必要があるため**CORS**等が導入されました。<br>

## CORSの仕組み
**CORS**は**ブラウザ**でリソースのアクセスの制限を行います。<br>
* Access-Control-Allow-Origin (アクセスを許可する**オリジン**)<br>
* Access-Control-Allow-Credentials (Cookie等を有効にするか)<br>
* Access-Control-Expose-Headers (どのヘッダーを公開するか)<br>
* Access-Control-Max-Age (キャッシュする秒 後述する**Preflight**で用いる)<br>
* Access-Control-Allow-Methods (許可する通信メソッド)<br>
* Access-Control-Allow-Headers (許可するリクエストヘッダー)<br>

これらのレスポンスのヘッダの値をブラウザで判断し通信の可否を決定します。<br>
基本的にサーバー側では通信の可否を決定せずリクエストされたリソースを返します。<br>
このようにリクエストを通信の可否を判断する前に実行するので、<br>
データを書き換えるようなリクエストは後述する**Preflight**で制御する必要があります。<br>

## CORS対応方法
サーバー側で返すレスポンスのヘッダに最低でも<br>
* Access-Control-Allow-Origin<br>
* Access-Control-Allow-Methods<br>

これらの設定を表記する必要があります。<br>

PHPでの例
```
<?php
	header('Access-Control-Allow-Origin:*');
	header('Access-Control-Allow-Methods:GET,POST,PUT,DELETE');
?>
```

「*」は全てに対しての許可を表します。<br>

## Preflight概要
**PUT**及び**DELETE**での通信を行う際、<br>
事前に通信を行い**CORS**での通信が許可されていなかった場合<br>
**ブラウザ**で**PUT**及び**DELETE**でのリクエストを遮断します。<br>

## Preflight導入の背景
**CORSの仕組**で前述した通り**CORS**だけでは許可されていない<br>
**オリジン**からでも
データを書き換える事ができてします。<br>
そこで導入されたのが**Preflight**です。<br>

## Preflight仕組み
**Preflight概要**でほとんど説明していますが、<br>
以下を満たさない場合<br>
- 以下のメソッドいずれかで行われていること。<br>
  - **GET**<br>
  - **HEAD**<br>
  - **POST**<br>
- ブラウザ(ユーザーエージェント)によって自動的に設定されたヘッダであること。<br>
- リクエストに使用されるどの XMLHttpRequestUpload にもイベントリスナーが登録されていないこと。<br>
- リクエストに ReadableStream オブジェクトが使用されていないこと。<br>

ブラウザが事前通信 (**OPTION**通信) を自動で行いリクエストの可否を制御します。<br>
事前通信の成功可否はキャッシュで保存しています。<br>

## Preflight対応方法
**OPTION**でアクセス (**Preflight**) された時に<br> 
**CORS対応方法**に加え Access-Control-Max-Age を設定しレスポンスを返す必要があります。<br>

PHPでの例
```
<?php
header("Access-Control-Allow-Origin: *");
header("Access-Control-Allow-Methods: *");
header('Access-Control-Max-Age: 86400');

if ($_SERVER['REQUEST_METHOD'] == 'OPTIONS') {
	exit(0);
}
?>
```
