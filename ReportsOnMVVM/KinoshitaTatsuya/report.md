### MVVMについて

MVVMパターンはソフトウェア・アーキテクチャパターンのひとつである。GUIを持つアプリケーションをModel,View,ViewModelに３分割して設計、実装する方法である。

#### Model,View,ViewModelの違い

- Model

    アプリケーションが扱うデータ本体の保存、更新、取得など
    を行う部分のことを指す。

- View

    viewとは、その名の通りUI部分のことを指しており、
    ユーザーが入力したり、入力した情報を出力する部分のことを指す。

- ViewModel

    Viewを描画するための情報を保持している。
    また、Viewからきた情報を適切な形に直してModelに渡したり、Modelの情報をViewに渡したりする役目を指す。
    Viewとの通信はデータバインディング機構のような仕組みを通じて行うため、ViewModelの変更は開発者から見て自動的に反映される。

#### ３つの関係性
ModelはView,ViewModelを知らない。
ViewはViewModelを知っているが、Modelは知らない。
ViewModelはModelを知っているが、Viewは知らない。

Model,View,ViewModelは一方通行の関係なのである。
