# 概略

ようこそ、**Codeer.LowCode.Blazor**はBlazorアプリに実行エンジン型のロー（ノー）コード機能を組み込むためのライブラリです。
そのため、以下をシームレスに組み合わせて非常に効率の高い開発手法を利用することができます。

- **ノーコード**（Designerでの設定）
- **ローコード**（スクリプト）
- **プロコード**（.NETアプリとしての実装）

## 画面の構成(PageFrame, Module, Field)
BlazorアプリにNugetで実行エンジン型のローコード機能を組込むためのライブラリです。
ローコード実行エンジンで実現する機能とBlazorアプリとして通常にC#で作成する機能をシームレスにつなぎます。
ローコード実行エンジンは強力なのでそれだけでアプリの大部分を作成することも可能です。
基本的には[PageFrame](page_frame.md)と[Module](module.md)で画面を作成していきます。
Moduleには[Field](field.md)を配置します。

<img src="../Image/pageframe_and_module.png">

PageFrameではヘッダ、サイドバー、画面に表示可能なModuleを指定します。
画面はModuleでレイアウトしていきます。

### [PageFrame](page_frame.md)
アプリの外枠の部分です。
またそのPageFrame内で表示可能なModuleを設定します。
表示可能なModuleはHome, ヘッダ、サイドバー(Left, Right)に指定されたModuleその他表示可能で指定されたModuleです。

### [Module](module.md)
ModuleはC#のclassに近い概念です。
- [Field](field.md)を配置することによりデータとして振る舞うことができます。
- DBとマッピングすることによりORマッパーのEntityとして振る舞うこともできます。
- またUIのレイアウトも持ち画面表示（一覧画面、詳細画面、ダイアログ、他の画面の一部）をすることもできます。
- スクリプトでメソッドを定義することもできます。

### [Field](field.md)
FieldはModuleを構成する部品です。わかりやすいものはTextFieldなどのUIを持つ部品です。これもWinFormsなどでFormクラスがTextContolをプロパティとして持つことをイメージしてもらうとわかりやすいと思います。UIに表示せずにデータの入出力だけに使うことも可能です。
- 大部分のFieldはデータを持ちます。
- ModuleをDBとマッピングしたときにカラムを割り当てて入出力することができます。
- 大部分のFieldはUIを持ちレイアウトに配置することができます。
- メソッドプロパティを持ちスクリプトから操作することもできます。

## [ProCode](procode.md)との連携も Codeer.LowCode.Blaozr では重要な機能です。
やはり込み入ったところはプロコードの実装が最適なケースも多々あります。またプロコードを使うことにより既存のC#のエコシステムの機能と連携することができます。
- プロコードで実装した画面の表示
- CodeBehind
- Custom Field
- スクリプトからプロコード機能呼び出し

