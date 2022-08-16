using System.Collections.Generic;
using System.Reflection;
using System.Text;
using IO.Unity3D.Source.Config;
using IO.Unity3D.Source.Reflection;
using UnityEngine;
using Object = System.Object;

public class BasicDemo : MonoBehaviour
{
    void Start()
    {
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
        
        //ExcelUtils.Parse<ConfigCharacter>(excel, (c)=>Debug.Log(c));
        ExcelManager excelManager = new ExcelManager();
        excelManager.Load<ConfigCharacter>(new StaticExcelStream(excel));
        
        excelManager.IterateAll<ConfigCharacter>((c)=>Debug.Log(c));
        
        Debug.LogError(excelManager.Get<ConfigCharacter>(1001));
        Debug.LogError(excelManager.Get<ConfigCharacter>(1008));
        Debug.LogError(excelManager.Get<ConfigCharacter>(new TeamID(1,1 )));
        Debug.LogError(excelManager.Get<ConfigCharacter>(new TeamID(1,2 )));
        Debug.LogError(excelManager.Get<ConfigCharacter>(new TeamID(2,1 )));
        Debug.LogError(excelManager.Get<ConfigCharacter>(new TeamID(2,2 )));
        
        excelManager.IterateList<ConfigCharacter>(1, (t) => Debug.Log("TEAM#1 -- " + t));
    }

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

    public enum Resource
    {
        Gold = 1,
        Diamond = 2
    }
    
    public class ConfigCosts
    {
        public List<ConfigCost> List = new List<ConfigCost>();


        public void Add(Resource resource, int value)
        {
            List.Add(new ConfigCost(resource, value));
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("List=[");
            for (int i = 0; i < List.Count; i++)
            {
                stringBuilder.Append(List[i].Resource).Append("=").Append(List[i].Value);

                if (i < List.Count - 1)
                {
                    stringBuilder.Append(", ");
                }
            }
            stringBuilder.Append("]");
            return stringBuilder.ToString();
        }
    }
    
    public class ConfigCost
    {
        public Resource Resource;
        public int Value;

        public ConfigCost(Resource resource, int value)
        {
            Resource = resource;
            Value = value;
        }
    }
    
    public class ConfigCostsParser : IExcelFieldParser
    {
        public object ParseValue(IExcelRow row, int index, IPropertyOrField propertiesAndField)
        {
            var str = row.GetString(index);
            var itemStrings = str.Split(';');
            ConfigCosts configCosts = new ConfigCosts();
            for (int i = 0; i < itemStrings.Length; i++)
            {
                var itemString = itemStrings[i];
                var kvStrings = itemString.Split(',');
                var resource = (Resource)int.Parse(kvStrings[0]);
                var value = int.Parse(kvStrings[1]);
                configCosts.Add(resource, value);
            }
            return configCosts;
        }
    }
    
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
        
        [ExcelColumn(ExcelColumn.Mode.Index)]
        [ExceColumnIndex(5)]
        public Dictionary<Resource, int> Costs2;
        
        [ExcelColumn(ExcelColumn.Mode.Index, ParserType = typeof(ConfigCostsParser))]
        [ExceColumnIndex(5)]
        public ConfigCosts Costs3;

        public override string ToString()
        {
            StringBuilder extraString = new StringBuilder();
            extraString.Append("Skills=[");
            for (int i = 0; i < SkillIDs.Length; i++)
            {
                extraString.Append(SkillIDs[i]);
                if (i < SkillIDs.Length - 1)
                {
                    extraString.Append(',');
                }
            }
            extraString.Append("], costs=[");

            int costIndex = 0;
            foreach (var cost in Costs)
            {
                extraString.Append(cost.Key).Append("->").Append(cost.Value);
                if (costIndex < Costs.Count - 1)
                {
                    extraString.Append(',');
                }

                costIndex++;
            }
            extraString.Append("]");
            
            costIndex = 0;
            extraString.Append(", cost2=[");
            foreach (var cost in Costs2)
            {
                extraString.Append(cost.Key).Append("->").Append(cost.Value);
                if (costIndex < Costs2.Count - 1)
                {
                    extraString.Append(',');
                }

                costIndex++;
            }
            extraString.Append("]");
            return $"ID={ID} Name={Name}, Team={Team}, TeamID={TeamID} {extraString} Costs3={Costs3}";
        }
    }
}
