# Boolean

<img src="../images/Boolean表示.png" alt="Boolean表示" title="Boolean表示" style="border: 1px solid;">

<img src="../images/Boolean設定.png" alt="Boolean設定" title="Boolean設定" style="border: 1px solid;" >

1. FieldType
- Booleanを設定する
2. Name
    - フィールド名
3. UIType
   - CheckBox
   - Switch
   - Toggle
4. Text
   - 表示テキスト
5. OnDataChanged
   - 変更時のスクリプト
6. DbColumn
   - OnClick 時の動作を設定する

<img src="../images/Boolean詳細.png" alt="Boolean詳細" title="Boolean詳細" style="border: 1px solid;">

## スクリプト
| プロパティ名          | 型       | 説明               |
|-----------------|---------|------------------|
| Value           | bool?   | Fieldの値          |
| SearchValue     | string? | 検索条件のフィールドのvalue |
| BackgroundColor | string? | Fieldの背景色        | 
| Color           | string? | Fieldの色          |
| IsEnabled       | bool    | Fieldの有効/無効      |
| IsVisible       | bool    | Fieldの表示/非表示     |
| IsViewOnly      | bool    | Fieldの編集可/編集不可   |
| IsModified      | bool    | Fieldが変更されたどうか   |
