# Date

<img src="../images/Date表示.png" alt="Date表示" title="Date表示" style="border: 1px solid;">

<img src="../images/Date設定.png" alt="Date設定" title="Date設定" style="border: 1px solid;" >

1. FieldType
    - Dateを設定する
2. Name
    - フィールド名の設定. 全体設定時に表示される.
3. DisplayName
    - TBD
4. IsRequired
    - 登録時，必須にする
5. DbColumn
    - テーブルのカラムの設定

<img src="../images/Date詳細.png" alt="Date詳細" title="Date詳細" style="border: 1px solid;">

## スクリプト
| プロパティ名          | 型         | 説明             |
|-----------------|-----------|----------------|
| Value           | DateOnly? | Fieldの値        |
| BackgroundColor | string?   | Fieldの背景色      | 
| Color           | string?   | Fieldの色        |
| IsEnabled       | bool      | Fieldの有効/無効    |
| IsVisible       | bool      | Fieldの表示/非表示   |
| IsViewOnly      | bool      | Fieldの編集可/編集不可 |
| IsModified      | bool      | Fieldが変更されたどうか |
| SearchMax       | DateOnly? | 検索条件の日付の最大値    |
| SearchMin       | DateOnly? | 検索条件の日付の最小値    |
