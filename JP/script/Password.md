# Password

## Clear() `method`
パスワード，確認用パスワードの入力をクリアする

### 引数
なし

### 戻り値
なし 

## CheckPassword() `method`
パスワードと確認用パスワードが一致するか

### 引数
`fileName` string
- ファイル名

`value` string
- base64エンコードされた文字列

### 戻り値
boolean
- 一致する場合 `true`
- 一致しない場合 `false`
