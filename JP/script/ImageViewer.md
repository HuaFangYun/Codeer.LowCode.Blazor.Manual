# ImageViewer

## Base64Data `property`
base64エンコードされた文字列

## ResourcePath `property`
リソースのパス

## ImageExtension `property`
イメージファイルの拡張子

## SetBase64Data(string fileName, string value) `method`
ファイル名，ファイルを設定する.

### 引数
`fileName` string
- ファイル名

`value` string
- base64エンコードされた文字列

### 戻り値
なし

## SetMemoryStream(string fileName, MemoryStream memoryStream) `method`
ファイル名，ファイルを設定する.

`fileName` string
- ファイル名

`memoryStream` MemoryStream
- ファイルのメモリーストリーム
 
### 戻り値
なし