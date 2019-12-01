// URL各自ポート等書き換えてください
const URL = "http://localhost:33333/chat_tool/user";

// HTMLの中身を全部上書きする用
let main;

let get_textbox;
let put_textbox;
let delete_textbox;

// Windowのロードが終わる前にHTMLの要素を抜き出そうとしてもまだ生成されていない
// 各種ボタンが押された時に関数を実行するように登録
window.onload = ()=>{
	main = document.getElementById("main");

	post_button = document.getElementById("post_button");
	post_button.onclick = CreateNewUser;

	get_textbox = document.getElementById("get_textbox");
	get_button = document.getElementById("get_button");
	get_button.onclick = GetUser;

	put_textbox = document.getElementById("put_textbox");
	put_button = document.getElementById("put_button");
	put_button.onclick = UpdateUser;

	delete_textbox = document.getElementById("delete_textbox");
	delete_button = document.getElementById("delete_button");
	delete_button.onclick = DeleteUser;
};

// ++++++++++++++++++インターフェイスの統一がされている(POST、GET、PUT、DELETE)++++++++++++++++++
// ++++++++++++++++++アドレス指定可能なURLで公開されている++++++++++++++++++
const CreateNewUser = ()=>{
	RequestAsync("POST", URL, LoadResponceText);
};

const GetUser = ()=>{
	RequestAsync("GET", URL + get_textbox.value, LoadResponceText);
};

const UpdateUser = ()=>{
	RequestAsync("PUT", URL + put_textbox.value, LoadResponceText);
};

const DeleteUser = ()=>{
	RequestAsync("DELETE", URL + delete_textbox.value, LoadResponceText);
};

// 非同期でリクエストを投げる
//                    メソッド名POST等 URL 読み込み時に呼び出す関数のポインタ
const RequestAsync = (openMethodName, url, onLoadEventHandler)=>{
	// ++++++++++++++++++ステートレス++++++++++++++++++
	let xhr  = new XMLHttpRequest();
	xhr.open(openMethodName, url, true);

	xhr.onload = onLoadEventHandler;

	xhr.send(null);
};

// HTMLをリスポンステキストで上書きする
//                        XMLHttpRequest(xhr)のProgressEvent xhrのonloadで自動的に渡される
const LoadResponceText = (progressEvent)=>{
	let xhr = progressEvent.srcElement;

	// ++++++++++++++++++RESTの処理結果がHTTPステータスコードで通知される++++++++++++++++++
	if (xhr.readyState == 4 && xhr.status == "200") {
		main.innerHTML = xhr.responseText;
	}else {
		console.error(xhr.responseText);
	}
};
