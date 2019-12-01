<?php
// スキーム ＝ http://
// ホスト ＝ www.exsample.com
// ポート = 33333
// これらを合わせたものをOriginと言う
header('Access-Control-Allow-Origin:*');
header('Access-Control-Allow-Methods:GET,POST,PUT,DELETE');
// 上記の設定はこのPHPのオリジン以外からアクセスされるのを許すかどうかの設定"*"にすると全部OK

// 正規表現を用いてURLからidを抜き出す
preg_match('|' . dirname($_SERVER['SCRIPT_NAME']) . '/([\w%/]*)|', $_SERVER['REQUEST_URI'], $matches);
$paths = explode('/', $matches[1]);
$id = isset($paths[1]) ? htmlspecialchars($paths[1]) : null;

switch (strtolower($_SERVER['REQUEST_METHOD']) . ':' . $paths[0]) {
case 'get:user':
  if ($id) echo "ユーザー #{$id} 取得";
  else echo 'ユーザー 一覧';
  break;
case 'post:user':
  echo 'ユーザー 登録';
  break;
case 'put:user':
  echo "ユーザー #{$id} 更新";
  break;
case 'delete:user':
  echo "ユーザー #{$id} 削除";
  break;
}
?>