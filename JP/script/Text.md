# TextField 

## SearchValue `property`

一覧の検索条件のinputフィールドのvalue

例
```csharp
void SearchLayoutDesign_OnInitializeSearch()
{
    Recipename.SearchValue = "tomato";
}
```

<img src="./images/text/SearchValue.png" alt="SearchValue表示" title="SearchValue表示" style="border: 1px solid;">


## Comparison `property`

```csharp
Recipename.Comparison = MatchComparison.Like;
```

Textで使用できる条件区分
- Equal
- Like


