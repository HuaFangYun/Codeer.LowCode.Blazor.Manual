# PageFrame
アプリの外枠の部分です。
またそのPageFrame内で表示可能なModuleを設定します。
表示可能なModuleはTopPage, ヘッダ、サイドバー(Left, Right)に指定されたModuleその他表示可能で指定されたModuleです。

## TopPage

## Header, SideBar(Left, Right)

- SideBar

| フィールド      | 説明                                            |
|------------|-----------------------------------------------|
| Icon       | メニューのアイコンを設定                                  |
| Title      | メニューのタイトルを設定<br/>`/` で区切ることでメニューを階層にできる       |
| Layout     | TBD                                           |
| Module     | [Module](module.md) を指定                       |
| LayoutType | - List（一覧画面）<br/> - Detail（詳細画面）<br/>のいずれかを指定 |



## Other Pages