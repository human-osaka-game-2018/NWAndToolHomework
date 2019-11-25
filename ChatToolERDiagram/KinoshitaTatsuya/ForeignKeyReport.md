#### ChatToolの外部キー制約について

- 更新について
    
    message_id、parent_message_id、user_id、channel_idはCASCADEで他はRESTRICTでいいと思う。
    主に使う外部キーなので依存先での変更が簡単になると思うから。

- 削除について

    すべてRESTRICTにしたほうがいいと思う。
    どのデータが消えたのかが分かりずらく、ほしいデータまで消えてしまう可能性があるから。
