# DetailListList

<img src="../../images/DetailList表示.png" alt="DetailList表示" title="DetailList表示" style="border: 1px solid;">

<img src="../../images/DetailList設定.png" alt="DetailList設定" title="DetailList設定" style="border: 1px solid;" >

### GENERAL
1. FieldType
    - DetailListを設定する
2. Name
    - フィールド名の設定. 全体設定時に表示される.
3. DisplayDane
    - TBD
4. UseIndexSort
5. DeleteTogether
   - 親データの削除時に削除する
6. CanCreate
   - 親画面で作成する
7. CanUpdate
   - 親画面で更新する
8. CanDelete
   - 親画面で削除する
9. CanSelect
   - 親画面で選択する
10. DbColumn
    - テーブルのカラムの設定

### CONDITION
- ModuleName
  - Moduleを指定する.
- Conditions
  - 表示する条件を指定する.
- MatchType
  - 複数の条件がある場合に，`And` or `Or` を指定する.
- LimitCount
  - 表示する上限
- SortFieldVariable
  - ソートに使用する項目
- SortOrder
  - ソート順（`Asc` or `Desc`）
<img src="../../images/DetailList詳細.png" alt="DetailList詳細" title="DetailList詳細" style="border: 1px solid;">


## スクリプト
| プロパティ名          | 説明             |
|-----------------|----------------|
| AllowLoad       | ロードの可否         |
| Color           | Fieldの色        |
| BackgroundColor | Fieldの背景色      | 
| IsEnabled       | Fieldの有効/無効    |
| IsVisible       | Fieldの表示/非表示   |
| IsViewOnly      | Fieldの編集可/編集不可 |
| IsModified      | Fieldが変更されたどうか |
| Limit           | 表示する最大件数       |
| Page            | ページ            |
| PageCount       | ページ数           |
| RowCount        | 行のカウント         |
| Rows            | 行数             |
| SelectedIndex   | 選択されたインデックス    |

## メソッド
| メソッド名                                       | 説明                  |
|---------------------------------------------|---------------------|
| AddRow()                                    | 1行追加する              |
| AddRow(Module row)                          | 指定されたモジュールで1行追加する   |
| DeleteRow(Module row)                       | 指定されたモジュールを削除する     |
| DeleteAllRows()                             | 全て削除する              |
| Reload()                                    | リロードする              |
| SetSearchCondition(ModuleSearcher searcher) | 指定された検索条件をセットする     |
| UpdateRow(int index, Module src)            | 指定されたインデックス，引数で更新する |
