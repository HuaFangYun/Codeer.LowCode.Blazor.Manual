# Field

FieldはModuleを構成する部品です。わかりやすいものはTextFieldなどのUIを持つ部品です。これもWinFormsなどでFormクラスがTextContolをプロパティとして持つことをイメージしてもらうとわかりやすいと思います。UIに表示せずにデータの入出力だけに使うことも可能です。
- 大部分のFieldはデータを持ちます。
- ModuleをDBとマッピングしたときにカラムを割り当てて入出力することができます。
- 大部分のFieldはUIを持ちレイアウトに配置することができます。
- Webのフロントで表示する場合はレイアウトに配置されてるかもしくはDataOnlyFieldsに配置しているFieldのみサーバーからデータを取得します。
- メソッドプロパティを持ちスクリプトから操作することもできます。

  ## Fields
- [AnchorTag](../fields/AnchorTag.md)
- [Boolean](../fields/Boolean.md)
- [Button](../fields/Button.md)
- [Date](../fields/Date.md)
- [DateTime](../fields/DateTime.md)
- [DetailList](../fields/DetailList.md)
- [File](../fields/File.md)
- [Id](../fields/Id.md)
- [ImageViewer](../fields/ImageViewer.md)
- [Label](../fields/Label.md)
- [Link](../fields/Link.md)
- [List](../fields/List.md)
- [ListNumber](../fields/ListNumber.md)
- [MarkupString](../fields/MarkupString.md)
- [Module](../fields/Module.md)
- [ModuleSelect](../fields/ModuleSelect.md)
- [Number](../fields/Number.md)
- [OptimisticLocking](../fields/OptimisticLocking.md)
- [Password](../fields/Password.md)
- [Search](../fields/Search.md)
- [Select](../fields/Select.md)
- [SubmitButton](../fields/SubmitButton.md)
- [Text](../fields/Text.md)
- [TileList](../fields/TileList.md)
- [Time](../fields/Time.md)
