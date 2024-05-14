# Codeer.LowCode.Blazor

## 利用方法
- [アーキテクチャ](architecture.md)
- [デザイナ](designer.md)
- [Module](module.md)
- [Field](field.md)
- [PageFrame](page_frame.md)
- [スクリプト](script.md)
- [designer.settings](designer_settings.md)
- [app.clprj](app_clprj.md)
- [プロコード](procode.md)
- [css](css.md)

## 特徴 ...
Codeer.LowCode.Blazor は、Blazor アプリにローコード機能を追加するためのライブラリです。

## Codeer.LowCode.Blazor ライセンス情報
Codeer.LowCode.Blazorの使用ライセンスの詳細については、[こちら](https://www.nuget.org/packages/Codeer.LowCode.Blazor/0.13.1/License) をご覧ください。 
- このソフトウェアの試用版は、製品の評価目的のみで使用できます。
- 商用目的でのソフトウェアの使用には開発ライセンスの購入が必要です。製品の開発時の利用も商用利用に含まれます。
- コミュニティ利用を希望される方には、申請により無償利用ライセンスを発行する予定です。

## Getting Started
Visual Studio 拡張機能を使用してプロジェクトを作成できます。 
[Codeer.LowCode.Blazor.Templates](https://marketplace.visualstudio.com/items?itemName=Codeer.LowCodeBlazor)

### Step1
プロジェクト新規作成.
<img src="../Image/step1.png">

### Step2
BlazorアプリとWPFのデザイナアプリをビルドして起動
<img src="../Image/step2.png">

### Step3
デザイナーで新しいプロジェクトを作成します。サンプルを含むプロジェクトが作成されます。
<img src="../Image/step3.png">

### Step4
それを Web アプリにデプロイします。画面がホットリロードされ、デザイナーの設定に従って画面が表示されます。
<img src="../Image/step4.png" width="800">

### Step5
デザイナーの設定を確認し、小さな変更を加えて Web アプリに送信して、感覚をつかんでください。

## こんなプロジェクトにおすすめ
- コストと時間を節約したい
- RDBを効果的に活用したい
- 既存のデータ、システムを活用したい
- こだわりの機能がある
- リリース後にカスタマイズしたい

## ポトペタで画面作成
CanvasLayout、GridLayout、FlowLayoutの組み合わせで自由に画面を作成できます。 通常の画面だけでなくダイアログも作成可能です。 各UI部品の連動もノーコードや僅かなスクリプトで作成可能です。 サイドバー、ヘッダ、フッターなど一般に必要なものは取り揃えております。 こだわりのあるお客様はプロコードで各種カスタマイズしたものに交換できます。

## RDBと自在に連携
FormとDBのTableを関連付けてデータの入出力ができます。 複数のFormを連携させることでJoinや1Nの関係を表現できます。 FormはTableだけでなくViewにも関連付けることができるのでBI機能も簡単に実現できます。 論理削除/楽観ロック/作成更新情報 など一般的にDBの操作で必要になるものは取り揃えています。 変更履歴ももちろん残せます。

## スクリプトでより自由に
C#とほぼ同じ構文で記述できます。 僅かな実装で機能を実現できるようにAPI設計をしています。 もちろんコード補完も効くので簡単に実装できます。 カスタマイズができてプロコードで実装した機能を呼び出すこともできます。 基本的にはクライアントサイドで実行されますが、サーバーサイドでの実行もサポートしています。<br/>
- 一般的な演算処理
- 画面制御
- WebAPIの実行
- Excel編集/PDF作成

## Excelとの連携のサポート
一般的なデータ入出力はもちろん、テンプレートをExcelで作成してそれを書き換えることで帳票にも対応できます。pdfへの変換も可能です。

## 認証・認可
認証は一般的なCookie認証やAzureActiveDirectoryを使った認証をデテンプレートコードで提供いたします。 その他の認証もカスタマイズで対応可能です。 認可に関してはアプリ、画面、データそれぞれでアクセス制御が可能です。

## こだわりの機能はプロコードで実装
場合によっては特殊な画面/機能が必要になることもあります。 Codeer.LowCodeはBlazorのライブラリなので、そのような場合は.NETのコードを追加して作りこみが可能です。 さらにField単位で作っておけばそれを様々な箇所で利用することができます。
