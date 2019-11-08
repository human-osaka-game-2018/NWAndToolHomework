## Transactionについて

### Transaction  

単語の意味としては、売買取引や複数形で議事録を表す。  
ITの分野においては、<font color="red">**分割することのできない複数の処理を一つにまとめたもの**</font>と定義される。
一つにまとめられた処理は、「途中で失敗した」、「一部処理だけ完遂した」といった結果を残さず。
その処理全体で<font color="red">**成功**か**失敗**</font>の2択のみ結果を表す。  
成功ならばそれで良いのだが、失敗したならば、処理の実行前の状態に戻さなくてはならない。  

銀行で口座振込を行う場合、  
* 口座Aには30,000円、口座Bには20,000円の残高がある。  
口座Aから口座Bに5,000円を送るとき、

       1. 口座Aの残高から5,000円を引く。
       2. 上記処理が完了したならば、口座Bの残高を5,000円増やす。

上記の処理をTransactionとして扱い、  
仮に、2の処理が失敗したならば、1の処理で減らした金額を元に戻さなくてはならない。  

　分離できない複数の処理というのは上記のように、  
　処理の一連の流れのうちどこかが失敗したときに成功できた処理をRollbackする必要が出てくる最小の単位である。  

---

### ACID特性
Transaction処理に求められる特性のこと。

* **原子性 (Atomicity)**  
Transactionに含まれる個々の手順が「<font color="red">**すべて実行される**</font>」か「<font color="red">**一つも実行されない**</font>」のどちらかの状態になるという性質である。

* **一貫性 (Consistency)**  
Transactionの実行前と後でデータに矛盾がなく整合性が保たれる性質である。

* **分離性 (Isolation)**  
Transaction実行中の処理過程が外部から隠蔽され、他の処理などに影響を与えない性質である。

* **永続性 (Durability)**  
Transactionが完了すると、その処理結果は永続的となります。たとえシステム障害が発生してもデータが失われることがない性質である。  

---

### Transaction分離Level

データベース管理システム上での一括処理が複数同時に行われた場合に、どれほどの一貫性、正確性で実行するかを4段階で定義したものである。隔離レベル、独立性レベルとも呼ばれる。  
Transactionを定義づけるACID特性のうち,分離性に関する概念である。

#### 同時実行されたTransaction間における現象

* ##### Dirty Read  
異なるTransactionの未Commitデータを読み取ってしまう現象

* ##### Fuzzy Read / Non-Repeatable Read  
下の図の<font color="red">TransactionA</font>の実行中に<font color="blue">TransactionB</font>で**データの更新**を<font color="blue">Commit</font>したときに、  
実行中の<font color="red">TransactionA</font>で<font color="blue">TransactionB</font>の<font color="blue">Commit</font>済みデータを読み取ってしまう現象  

* ##### Phantom Read  
下の図の<font color="red">TransactionA</font>の実行中に<font color="blue">TransactionB</font>で**データの追加・削除**を<font color="blue">Commit</font>したときに、  
実行中の<font color="red">TransactionA</font>で<font color="blue">TransactionB</font>の<font color="blue">Commit</font>済みデータを読み取ってしまう現象  


![Transaction Isolation](photo/transaction_isolation.png)

上記の３つの問題をどれだけ許容するかの程度がTransaction分離Levelである。

---

### 分離Levelの段階

ANSI/ISO SQL標準で分離レベルは、下記の4種類で定義されている。

* ##### READ UNCOMMITTED (確定していないデータまで読み取る)  
    他の処理によって行われている、書きかけのデータまで読み取る。  

* ##### READ COMMITTED ( 確定した最新データを常に読み取る)  
    他のTransaction更新については、常にコミット済みのデータのみを読み取る。  
  [MVCC](#注釈)はREAD COMMITTEDを実現する実装の一つである。  

* ##### REPEATABLE READ (読み取り対象のデータを常に読み取る)  
    ひとつのTransaction実行中の間、読み取り対象のデータが途中で他のTransaction変更される心配はない。  
同じTransaction中では同じデータは何度読み取りしても毎回同じ値を読むことができる。  

* ##### SERIALIZABLE (直列化可能)  
    複数の並行に動作するTransaction結果が、いかなる場合でも、それらのTransaction時間的重なりなく逐次実行した場合と同じ結果となる。  
このような性質を直列化可能性と呼ぶ.SERIALIZABLEは最も強い分離Levelであり、最も安全にデータを操作できるが、相対的に性能は低い。ただし同じ結果とされる逐次実行の順はTransaction処理のLevelでは保証されない。  
  
|分離Level|Dirty Read|Fuzzy Read|Phantom Read|
|:--|:-:|:-:|:-:|
|READ UNCOMMITTED|○|○|○|
|READ COMMITTED|x|○|○|
|[REPEATABLE READ](#注釈)|x|x|○|
|SERIALIZABLE|x|x|x|
  

---

### 対処

##### Dirty Read  

* 起こりうる問題  
購入完了できなかったアイテムを購入できたこととして扱ってしまう場合がある。  
→実際には入手できなかったアイテムを所持していると勘違いしたまま処理が進んでしまう可能性がある。

* 解決法  
分離レベルをReadCommited以上に上げる。  
入手を確定するまでデータの更新をしない。  
##### Fuzzy Read / Non-Repeatable Read  

* 起こりうる問題  
値の割引処理をする時に他の確定済みの値でみた時に数値が変わってしまう。
→10%の割合ダメージをHP100の敵に与えようという時に、処理中にHPが90になってしまったらダメージ量が変動する。

* 解決法  
分離レベルをrepeatableRead以上に上げる。  
データの値を使用する直前でデータを参照するようにする。  
データの変動を許さないならばデータをロックする。  
##### Phantom Read  

* 起こりうる問題  
10連ガチャを引くとき、演出中にアイテムインベントリに追加される仕様で、他の人のインベントリの内容を見ることができる場合。
引いたプレイヤーは演出中に中身を知ることはできないが、他のプレイヤーからはその時点で中身を知ることができる。

* 解決法  
分離レベルをSerializable(InnoDBはREPEATABLE READ)以上に上げる。  
テーブルの変動を許さないならばテーブルをロックする。  
---
##### 注釈  
1.　MultiVersion Concurrency Control(Transactionが行われている最中に他のUserによる読み取りがあった場合、書き込みの直前の状態を処理結果として返す。書き込み中も読み取りができ、読み取り中でも書き込みができる)  
2. InnoDBはREPEATABLE READでもMVCCによってPhantom Readが発生しない。

