# Config Library in Source Framework for Unity3D

# Project Aim

Abstract the ORM operation about from excel row data to project logic data.

For example, imagagin that you have a excel data like the following table:

|ID|Name|Team|TeamIndex|Skills|Costs|
|--|--|--|--|--|--|
|1001|Kakashi|1|0|10001,10002,10003|1,100;2:200|
|1002|Naruto|1|1|10001,10002|1,80|
|1003|Sasuke|1|2|10001,10002|1,80|
|1004|Sakura|1|3|10001|1,70|

If you want to parse the row data into the following struct in `code list 1.1`. One way to achieve this target is write some code generate tools to generate the parse code and struct from the excel. But Actually I donot like this way because it will break my programming procedure from writing code to making a excel. I want to keep my mind focus on programming as possible. So my habit is write the struct first, write the code which work with this struct and mock some data to test it finallly.

```csharp
// code list 1.1
class ConfigCharacter
{
    public int ID;
    public int Team;
    public int TeamID;
    public string Name;
    public int[] SkillIDs;
    public Dictionary<int, int> Costs;
}
```

So after I finish the code, How can I mapping my code to the excel data. This is what this library does! With this library, you can add the mapping to the existing struct. Suc as :

```csharp
class ConfigCharacter
{
    [ExcelColumn(ExcelColumn.Mode.Index)]
    [ExceColumnIndex(0)]
    public int ID;
    
    [ExcelColumn(ExcelColumn.Mode.Index)]
    [ExceColumnIndex(1)]
    public int Team;
    
    [ExcelColumn(ExcelColumn.Mode.Index)]
    [ExceColumnIndex(2)]
    public int TeamID;
    
    [ExcelColumn(ExcelColumn.Mode.Index)]
    [ExceColumnIndex(3)]
    public string Name;
    
    [ExcelColumn(ExcelColumn.Mode.Index)]
    [ExceColumnIndex(4)]
    public int[] SkillIDs;
    
    [ExcelColumn(ExcelColumn.Mode.Index)]
    [ExceColumnIndex(5)]
    public Dictionary<int, int> Costs;
}
```

And then use to create struct instance from excel data.

```csharp
    ExcelUtils.Parse<ConfigCharacter>(excel, (c)=>Debug.Log(c));
```

# How to use

## Add dependencies

You can add package from git url through the Package Manager.

All the following package should be added.

|Package|Description|
|--|--|
|[https://github.com/kakashiio/Unity-Reflection.git#1.0.0](https://github.com/kakashiio/Unity-Reflection.git#1.0.0)|Reflection Library|
|[https://github.com/kakashiio/Unity-Config.git#1.0.0](https://github.com/kakashiio/Unity-Config.git#1.0.0)|Config Library|

## Import Samples From Package Manager and run `BasicDemo.unity` scene

The basic usage of this library is in the sample script `BasicDemo.cs`

# Features

| Feature | Status | Description |
|--|--|--|
|ORM| `DONE` | Mapping raw excel data to config data |
|ExcelManager| `DONE` | Manager the config data. |

# Tutorial

## Simple usage 1 - without ExcelManager

```csharp
class ConfigCharacter
{
    [ExcelColumn(ExcelColumn.Mode.Index)]
    [ExceColumnIndex(0)]
    public int ID;
    
    [ExcelColumn(ExcelColumn.Mode.Index)]
    [ExceColumnIndex(1)]
    public int Team;
    
    [ExcelColumn(ExcelColumn.Mode.Index)]
    [ExceColumnIndex(2)]
    public int TeamID;
    
    [ExcelColumn(ExcelColumn.Mode.Index)]
    [ExceColumnIndex(3)]
    public string Name;
    
    [ExcelColumn(ExcelColumn.Mode.Index)]
    [ExceColumnIndex(4)]
    public int[] SkillIDs;
    
    [ExcelColumn(ExcelColumn.Mode.Index)]
    [ExceColumnIndex(5)]
    public Dictionary<int, int> Costs;
}
```

```csharp
    // You can implement the `IExcel` occording your excel parser library.
    IExcel excel = new StaticExcel(new List<IExcelRow>()
        {
            // ID Team TeamIndex Name SkillID
            new StaticExcelRow(new object[] { 1001, 1, 1, "Kakashi", "1001; 1002; 1003; 1004", "1,100;2,200" }),
            new StaticExcelRow(new object[] { 1002, 1, 2, "Naruto", "1001; 1002", "1,99" }),
            new StaticExcelRow(new object[] { 1003, 1, 3, "Sasuke", "1001; 1002; 1003", "1,80" }),
            new StaticExcelRow(new object[] { 1004, 1, 4, "Sakura", "1001", "1,60" }),
            new StaticExcelRow(new object[] { 1005, 2, 1, "Kai", "1001; 1002; 1003", "1,50;2,100" }),
            new StaticExcelRow(new object[] { 1006, 2, 2, "Lee", "1001; 1002", "1,80" }),
            new StaticExcelRow(new object[] { 1007, 2, 3, "Neji", "1001; 1002; 1003", "1,70" }),
            new StaticExcelRow(new object[] { 1008, 2, 4, "Hinata", "1001", "1,65" })
        });

    ExcelUtils.Parse<ConfigCharacter>(excel, (c)=>Debug.Log(c));    //output all data.
```

## Simple usage 2 - with ExcelManager

```csharp
class ConfigCharacter
{
    [ExcelColumn(ExcelColumn.Mode.Index)]
    [ExceColumnIndex(0)]
    [ExcelDictKey(ID = 0)]
    public int ID;
    
    [ExcelColumn(ExcelColumn.Mode.Index)]
    [ExceColumnIndex(1)]
    [ExcelDictKey(ID = 1, KeyType = typeof(TeamID))]
    [ExcelListKey]
    public int Team;
    
    [ExcelColumn(ExcelColumn.Mode.Index)]
    [ExceColumnIndex(2)]
    [ExcelDictKey(ID = 1, KeyPropNameAlias = "TeamIndex")]
    public int TeamID;
    
    [ExcelColumn(ExcelColumn.Mode.Index)]
    [ExceColumnIndex(3)]
    public string Name;
    
    [ExcelColumn(ExcelColumn.Mode.Index)]
    [ExceColumnIndex(4)]
    public int[] SkillIDs;
    
    [ExcelColumn(ExcelColumn.Mode.Index)]
    [ExceColumnIndex(5)]
    public Dictionary<int, int> Costs;
}
```

```csharp
class TeamID
{
    public int Team;
    public int TeamIndex;

    public TeamID(int team, int teamIndex)
    {
        Team = team;
        TeamIndex = teamIndex;
    }

    public TeamID()
    {
    }

    public override int GetHashCode()
    {
        return Team ^ TeamIndex;
    }

    public override bool Equals(object obj)
    {
        if (obj == null)
        {
            return false;
        }
        
        if (base.Equals(obj))
        {
            return true;
        }
        
        var teamID = obj as TeamID;
        if (teamID == null)
        {
            return false;
        }

        return teamID.Team == Team && teamID.TeamIndex == TeamIndex;
    }
}
```


```csharp
    // You can implement the `IExcel` occording your excel parser library.
    IExcel excel = new StaticExcel(new List<IExcelRow>()
        {
            // ID Team TeamIndex Name SkillID
            new StaticExcelRow(new object[] { 1001, 1, 1, "Kakashi", "1001; 1002; 1003; 1004", "1,100;2,200" }),
            new StaticExcelRow(new object[] { 1002, 1, 2, "Naruto", "1001; 1002", "1,99" }),
            new StaticExcelRow(new object[] { 1003, 1, 3, "Sasuke", "1001; 1002; 1003", "1,80" }),
            new StaticExcelRow(new object[] { 1004, 1, 4, "Sakura", "1001", "1,60" }),
            new StaticExcelRow(new object[] { 1005, 2, 1, "Kai", "1001; 1002; 1003", "1,50;2,100" }),
            new StaticExcelRow(new object[] { 1006, 2, 2, "Lee", "1001; 1002", "1,80" }),
            new StaticExcelRow(new object[] { 1007, 2, 3, "Neji", "1001; 1002; 1003", "1,70" }),
            new StaticExcelRow(new object[] { 1008, 2, 4, "Hinata", "1001", "1,65" })
        });

    ExcelManager excelManager = new ExcelManager();
    excelManager.Load<ConfigCharacter>(new StaticExcelStream(excel));
    
    Debug.LogError(excelManager.Get<ConfigCharacter>(1001));                //output which ID == 1001
    Debug.LogError(excelManager.Get<ConfigCharacter>(1008));                //output which ID == 1008
    Debug.LogError(excelManager.Get<ConfigCharacter>(new TeamID(1,1)));     //output which Team == 1 && TeamID == 1
    Debug.LogError(excelManager.Get<ConfigCharacter>(new TeamID(1,2)));     //output which Team == 1 && TeamID == 2
```