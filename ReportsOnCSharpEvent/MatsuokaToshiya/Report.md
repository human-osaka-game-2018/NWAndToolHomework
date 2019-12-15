# C#のイベントについて

## イベント

イベントは主にGUIでのボタンのクリックやメニュー選択などのユーザの操作を通知するために使用される。
イベントの発行者が発火タイミングを判断し、受け手が対応したアクションを実行する。

## Delegate

特定のパラメータリスト及び戻り値の型を使用して、メソッドへの参照を表す。  
まず、`Delegate`を宣言する。

```CS
public delegate string SampleEventHandler(int x,int y);
```

`Delegate`に一致するメソッドはすべて追加と削除ができる。  
そのときには、`+=`で追加、`-=`で削除ができる。

```CS
public event SampleEventHandler SampleEvent;
SampleEvent += (int x,int y) =>{ return Func() };
```

宣言されたイベントの変数は初期化されていないと`null`なので、  
初期化せずに呼出をすると例外が発生する。
そのため、呼び出し前にnullチェックをするか、空の関数で初期化するとよいらしい。  

## Action< T >

`Delegate`を継承していて、使用する手順が簡素化されている。
実行される関数の引数を指定できる。  
戻り値は`Void`  

```CS
Action<var> SampleEvent;
SampleEvent += s =>{ Console.WriteLine(s); };
SampleEvent("str");
```

## Func< in T,out T_Result >

`Delegate`を継承していて、引数と戻り値の型を指定できる。

```CS
Func<string,string> SampleEvent;
SampleEvent += s =>{ return s};
Console.WriteLine(SampleEvent("str"));
```
