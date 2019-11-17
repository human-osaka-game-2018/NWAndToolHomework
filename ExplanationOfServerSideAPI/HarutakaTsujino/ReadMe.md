# ServerSideのPHP programからJSONを返却し、localPC上のJavaScriptで処理してブラウザに表示するprogramの手順

## 大まかな手順
 1. ブラウザ上からWebAPIをたたいてJSON形式でデータを取得す
 1. 読み取ったJSON形式のデータをJavaScript(以下JS)でパースし必要なデータを抜き出す
 1. 抜き出したデータをHTMLで表示

## 詳細説明
今回はJSでパースをするがあるためHTML上でボタンを作り、Ebr>
JSでそのボタンのクリックイベントに、仮想マシンのPHPにアクセスする関数を登録します、<br>
アクセスにはXMLHttpRequestを用います。<br>
PHPでは連想配列を自前で用意しそれをjson_encodeでJSONに変換その後echoで出力します。<br>
JSでJSONを受け取った後、JSON.parseで連想配列に変換し必要なデータをHTMLに出力したら完了です。
