// To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
//    using GameAuthoringAPI;
//    var studentGameConfig = StudentGameConfig.FromJson(jsonString);

namespace GameAuthoringAPI
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class StudentGameConfig
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("student")]
        public Student Student { get; set; }

        [JsonProperty("game")]
        public Game Game { get; set; }

        [JsonProperty("dgbl_config")]
        public DgblConfig DgblConfig { get; set; }

        [JsonProperty("game_features_config")]
        public GameFeaturesConfig GameFeaturesConfig { get; set; }

    }

    public partial class StudentGameConfigsByUsernameGame
    {
        [JsonProperty("student_game_configs")]
        public List<StudentGameConfig> StudentGameConfigs { get; set; }
    }

    public partial class DgblConfig
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("ilos_config")]
        public List<IloConfig> IlosConfig { get; set; }
    }

    public partial class IloConfig
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("is_active")]
        public bool IsActive { get; set; }

        [JsonProperty("ilo_parameters")]
        public List<ParameterConfig> IloParameters { get; set; }
        /* public List<Parameter> IloParameters { get; set; } */

        [JsonProperty("activities_config")]
        public List<ActivityConfig> ActivitiesConfig { get; set; }

        [JsonProperty("ilos_config")]
        public List<IloConfig> IlosConfig { get; set; }
    }

    public partial class ActivityConfig
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("is_active")]
        public bool IsActive { get; set; }

        [JsonProperty("missions_config", NullValueHandling = NullValueHandling.Ignore)]
        public List<MissionConfig> MissionsConfig { get; set; }
    }

    public partial class MissionConfig
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("is_active")]
        public bool IsActive { get; set; }

        [JsonProperty("mission_elements_config", NullValueHandling = NullValueHandling.Ignore)]
        public List<MissionElementConfig> MissionElementsConfig { get; set; }
    }

    public partial class MissionElementConfig
    {
        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("parameters")]
        public List<ParameterConfig> Parameters { get; set; }
        /* public List<Parameter> Parameters { get; set; } */
    }
    
    public partial class ParameterConfig
    {
        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("type")]
        public TypeEnum Type { get; set; }

        [JsonProperty("is_active")]
        public bool IsActive { get; set; }

        [JsonProperty("game_mission_element_config", NullValueHandling = NullValueHandling.Ignore)]
        public long? GameMissionElementConfig { get; set; }

        [JsonProperty("game_ilo_config", NullValueHandling = NullValueHandling.Ignore)]
        public long? GameIloConfig { get; set; }
        
    }

/*
    public partial class IloElementConfig
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("is_active")]
        public bool IsActive { get; set; }

        [JsonProperty("parameters")]
        public List<Parameter> Parameters { get; set; }
    }

    public partial class ParameterType
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("elements", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, string> Elements { get; set; }
    }

    public partial class Parameter
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
        //public Value Value { get; set; }

        [JsonProperty("type")]
        public ParameterType Type { get; set; }

        [JsonProperty("is_active")]
        public bool IsActive { get; set; }
    }
*/
    public partial class StudentProfile
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        /*
        [JsonProperty("elements", NullValueHandling = NullValueHandling.Ignore)]
        public Elements Elements { get; set; }

        [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
        public Uri Url { get; set; }
        */
    }

    /*
    public partial class Element
    {
        [JsonProperty("Do menor")]
        public Uri DoMenor { get; set; }

        [JsonProperty("Fa mayor")]
        public Uri FaMayor { get; set; }

        [JsonProperty("La menor")]
        public Uri LaMenor { get; set; }

        [JsonProperty("Re mayor")]
        public Uri ReMayor { get; set; }

        [JsonProperty("Re menor")]
        public Uri ReMenor { get; set; }

        [JsonProperty("Re menor A")]
        public Uri ReMenorA { get; set; }

        [JsonProperty("Re menor B")]
        public Uri ReMenorB { get; set; }

        [JsonProperty("Do bemol mayor")]
        public Uri DoBemolMayor { get; set; }

        [JsonProperty("La bemol mayor")]
        public Uri LaBemolMayor { get; set; }

        [JsonProperty("Mi bemol mayor")]
        public Uri MiBemolMayor { get; set; }

        [JsonProperty("Sol bemol mayor")]
        public Uri SolBemolMayor { get; set; }
    }
    */
    public partial class Game
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("game_description")]
        public GameDescription GameDescription { get; set; }

        [JsonProperty("dgbl_features")]
        public DgblFeatures DgblFeatures { get; set; }

        [JsonProperty("game_features")]
        public GameFeatures GameFeatures { get; set; }
    }

    public partial class GameCatalog
    {
        [JsonProperty("games")]
        public List<Game> Games { get; set; }
    }

    public partial class LearningArea
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }
    }

    public partial class DgblFeatures
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("education_level")]
        public EducationLevel EducationLevel { get; set; }

        [JsonProperty("learning_areas")]
        public List<LearningArea> LearningAreas { get; set; }

        [JsonProperty("ilos")]
        public List<Ilo> Ilos { get; set; }

        [JsonProperty("student_profiles")]
        public List<StudentProfile> StudentProfiles { get; set; }
    }

    public partial class EducationLevel
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
    /*
        public partial class IloCategory
        {
            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("description")]
            public string Description { get; set; }

            [JsonProperty("selectable")]
            public bool Selectable { get; set; }

            [JsonProperty("ilos")]
            public List<Ilo> Ilos { get; set; }

            [JsonProperty("subcategories")]
            public List<IloCategory> Subcategories { get; set; }
        }
    */
    public partial class Ilo
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("topics")]
        public List<Topic> Topics { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }

        [JsonProperty("activities")]
        public List<Activity> Activities { get; set; }

        [JsonProperty("selectable")]
        public bool Selectable { get; set; }

        [JsonProperty("selected")]
        public bool Selected { get; set; }

        [JsonProperty("ilos")]
        public List<Ilo> Ilos { get; set; }

        [JsonProperty("ilo_parameters")]
        public List<Parameter> IloParameters { get; set; }

        [JsonProperty("ilo_aids")]
        public List<IloAid> IloAids { get; set; }
    }

    public partial class LearningMechanic
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }


    public partial class Activity
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }

        [JsonProperty("learning_mechanics")]
        public List<LearningMechanic> LearningMechanics { get; set; }

        [JsonProperty("missions")]
        public List<Mission> Missions { get; set; }

        [JsonProperty("selectable")]
        public bool Selectable { get; set; }
    }

    /*
    public partial class MissionParameter
    {
        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("parameter_type")]
        public ParameterType Parameter_type { get; set; }

        [JsonProperty("default_value")]
        public string DefaultValue { get; set; }
        //public Value DefaultValue { get; set; }
    }
    */

    /*
    public partial class MissionAid
    {
        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("selectable")]
        public bool Selectable { get; set; }
    }

    public partial class MissionReward
    {
        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("type")]
        public ParameterType Type { get; set; }

        [JsonProperty("default_value")]
        public string DefaultValue { get; set; }
        //public Value DefaultValue { get; set; }
    }
    
    public partial class Mission
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("statement")]
        public string Statement { get; set; }

        [JsonProperty("mission_parameters")]
        public List<MissionParameter> MissionParameters { get; set; }

        [JsonProperty("mission_aids")]
        public List<MissionAid> MissionAids { get; set; }

        [JsonProperty("mission_reward")]
        public MissionReward MissionReward { get; set; }

        [JsonProperty("selectable")]
        public bool Selectable { get; set; }
    }
    */
    public partial class Mission
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }

        [JsonProperty("mission_parameters")]
        public List<Parameter> MissionParameters { get; set; }

        [JsonProperty("mission_aids")]
        public List<object> MissionAids { get; set; }

        [JsonProperty("mission_reward")]
        public object MissionReward { get; set; }

        [JsonProperty("selectable")]
        public bool Selectable { get; set; }
    }

    /*
    public partial class IloParameter
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("parameter_type")]
        public ParameterType ParameterType { get; set; }

        [JsonProperty("default_value")]
        public string DefaultValue { get; set; }
        //public Value DefaultValue { get; set; }
    }
    */
    public partial class Parameter
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("parameter_type")]
        public TypeEnum ParameterType { get; set; }

        [JsonProperty("default_value")]
        public string DefaultValue { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }

        [JsonProperty("elements")]
        public List<string> Elements { get; set; }

        [JsonProperty("icons")]
        public List<Uri> Icons { get; set; }

        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }

        [JsonProperty("min_value", NullValueHandling = NullValueHandling.Ignore)]
        public string MinValue { get; set; }

        [JsonProperty("max_value", NullValueHandling = NullValueHandling.Ignore)]
        public string MaxValue { get; set; }
    }

    public partial class IloAid
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("parameter_type")]
        public TypeEnum ParameterType { get; set; }

        [JsonProperty("selectable")]
        public bool Selectable { get; set; }

        [JsonProperty("ilo_aids")]
        public List<IloAid> IloAids { get; set; }
    }

    public partial class Topic
    {
        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }
    
    public partial class GameURL
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
        public Uri Url { get; set; }
    }
    /*
    public partial class GameVideo
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
        public Uri Url { get; set; }
    }

    public partial class GameDescription
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("short_description")]
        public string ShortDescription { get; set; }

        [JsonProperty("long_description")]
        public string LongDescription { get; set; }

        [JsonProperty("registered_by")]
        public string RegisteredBy { get; set; }

        [JsonProperty("properties")]
        public Properties Properties { get; set; }

        [JsonProperty("urls")]
        public List<GameURL> Urls { get; set; }

        [JsonProperty("images")]
        public List<Image> Images { get; set; }

        [JsonProperty("videos")]
        public List<GameVideo> Videos { get; set; }
    }
    */
    public partial class GameDescription
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("short_description")]
        public string ShortDescription { get; set; }

        [JsonProperty("long_description")]
        public string LongDescription { get; set; }

        [JsonProperty("registered_by")]
        public string RegisteredBy { get; set; }

        [JsonProperty("properties")]
        public object Properties { get; set; }

        [JsonProperty("urls")]
        public List<GameURL> Urls { get; set; }

        [JsonProperty("images")]
        public List<Image> Images { get; set; }

        [JsonProperty("videos")]
        public List<object> Videos { get; set; }
    }

    public partial class Image
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
        public Uri Url { get; set; }

        [JsonProperty("image", NullValueHandling = NullValueHandling.Ignore)]
        public Uri ImageImage { get; set; }
    }
    /*
    public partial class Genre
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }
    }

    public partial class GameType
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }

    }

    public partial class Properties
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("genre")]
        public Genre Genre { get; set; }

        [JsonProperty("game_type")]
        public GameType GameType { get; set; }

        [JsonProperty("software_requirements")]
        public string SoftwareRequirements { get; set; }
    }
*/
    public partial class GameMechanic
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }

    public partial class Character
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
        public Uri Url { get; set; }
    }

    public partial class GameScenario
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
        public Uri Url { get; set; }
    }
    
    public partial class GameFeaturesGameObject
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

    }

    public partial class PlayerType
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }

    public partial class GameFeatures
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("game_missions")]
        public List<GameMission> GameMissions { get; set; }

        [JsonProperty("game_mechanics")]
        public List<GameMechanic> GameMechanics { get; set; }

        [JsonProperty("characters")]
        public List<Character> Characters { get; set; }

        [JsonProperty("game_scenarios")]
        public List<GameScenario> GameScenarios { get; set; }

        [JsonProperty("game_objects")]
        public List<GameFeaturesGameObject> GameObjects { get; set; }

        [JsonProperty("player_types")]
        public List<PlayerType> PlayerTypes { get; set; }
    }

    public partial class GameMission
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("selectable")]
        public bool Selectable { get; set; }

        [JsonProperty("icon")]
        public Uri Icon { get; set; }

        [JsonProperty("ilos")]
        public List<Ilo> Ilos { get; set; }

        [JsonProperty("game_mission_parameters")]
        public List<Parameter> GameMissionParameters { get; set; }
    }

    public partial class GameFeaturesConfig
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("game_features_mission_configs")]
        public List<GameFeaturesMissionConfig> GameFeaturesMissionConfigs { get; set; }
    }

    /*
    public partial class GameFeaturesMissionParameterConfig
    {
        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("type")]
        public TypeEnum Type { get; set; }

        [JsonProperty("is_active")]
        public bool IsActive { get; set; }

    */
        /*
        [JsonProperty("game_mission_element_config", NullValueHandling = NullValueHandling.Ignore)]
        public long? GameMissionElementConfig { get; set; }

        [JsonProperty("game_ilo_config", NullValueHandling = NullValueHandling.Ignore)]
        public long? GameIloConfig { get; set; }
        */
    /* } */

    public partial class GameFeaturesMissionConfig
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("is_active")]
        public bool IsActive { get; set; }

        [JsonProperty("game_mission_parameters")]
        public List<ParameterConfig> GameMissionParameters { get; set; }
        /* public List<GameFeaturesMissionParameterConfig> GameMissionParameters { get; set; } */

        [JsonProperty("ilos_config")]
        public List<IloConfig> IlosConfig { get; set; }
    }

    /*
    public partial class Student
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [JsonProperty("last_name")]
        public string LastName { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("institution")]
        public string Institution { get; set; }

        [JsonProperty("level")]
        public long Level { get; set; }

        [JsonProperty("experience")]
        public long Experience { get; set; }

        [JsonProperty("experience_to_level_up")]
        public long ExperienceToLevelUp { get; set; }

        [JsonProperty("inventory")]
        public object Inventory { get; set; }

    }
    */

    public partial class Student
    {
        [JsonProperty("user")]
        public User User { get; set; }

        [JsonProperty("institution")]
        public string Institution { get; set; }

        [JsonProperty("level")]
        public long Level { get; set; }

        [JsonProperty("experience")]
        public long Experience { get; set; }

        [JsonProperty("experience_to_level_up")]
        public long ExperienceToLevelUp { get; set; }

        [JsonProperty("inventory")]
        public object Inventory { get; set; }
    }

    public partial class User
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [JsonProperty("last_name")]
        public string LastName { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }
    }


    public partial class Inventory
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("gold")]
        public long Gold { get; set; }

        [JsonProperty("diamonds")]
        public long Diamonds { get; set; }
    }

    public enum TypeEnum { Integer, Text, Url, Boolean };
    
    /*
    public partial struct Value
    {
        public long? Integer;
        public Uri PurpleUri;

        public static implicit operator Value(long Integer) => new Value { Integer = Integer };
        public static implicit operator Value(Uri PurpleUri) => new Value { PurpleUri = PurpleUri };
    }
    */

    public partial class Login
    {
        [JsonProperty("token")]
        public string Token { get; set; }
    }

    public partial class Login
    {
        public static Login FromJson(string json) => JsonConvert.DeserializeObject<Login>(json, GameAuthoringAPI.Converter.Settings);
    }



    public partial class Student
    {
        //public static List<Student> FromJson(string json) => JsonConvert.DeserializeObject<List<Student>>(json, Converter.Settings);
        public static Student FromJson(string json) => JsonConvert.DeserializeObject<Student>(json, Converter.Settings);
    }

    public partial class GameCatalog
    {
        public static List<Game> FromJson(string json) => JsonConvert.DeserializeObject<List<Game>>(json, Converter.Settings);
    }

    public partial class StudentGameConfigsByUsernameGame
    {
        public static List<StudentGameConfig> FromJson(string json) => JsonConvert.DeserializeObject<List<StudentGameConfig>>(json, GameAuthoringAPI.Converter.Settings);
    }

    public partial class StudentGameConfig
    {
        public static StudentGameConfig FromJson(string json) => JsonConvert.DeserializeObject<StudentGameConfig>(json, GameAuthoringAPI.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this StudentGameConfig self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                //ValueConverter.Singleton,
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    internal class ValueConverter : JsonConverter
    {
        //public override bool CanConvert(Type t) => t == typeof(Value) || t == typeof(Value?);
        public override bool CanConvert(Type t) => t == typeof(string); //|| t == typeof(String?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            switch (reader.TokenType)
            {
                case JsonToken.String:
                case JsonToken.Date:
                    var stringValue = serializer.Deserialize<string>(reader);
                    //long l;
                    if (long.TryParse(stringValue, out long l))
                    {
                        //return new Value { Integer = l };
                        return l;
                    }
                    try
                    {
                        //var uri = new Uri(stringValue);
                        return stringValue;
                        //return new Value { PurpleUri = uri };
                    }
                    catch (UriFormatException) { }
                    break;
            }
            throw new Exception("Cannot unmarshal type Value");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            //var value = (Value)untypedValue;
            var value = (string)untypedValue;
            //long number;
            if (long.TryParse(value, out long number))// value.Integer != null)
            {
                serializer.Serialize(writer, number.ToString());
                //serializer.Serialize(writer, value.Integer.Value.ToString());
                return;
            }
            /*
            if (value.PurpleUri != null)
            {
                serializer.Serialize(writer, value.PurpleUri.ToString());
                return;
            }*/
            if (value != null)
            {
                serializer.Serialize(writer, value);
                return;
            }
            throw new Exception("Cannot marshal type Value");
        }

        public static readonly ValueConverter Singleton = new ValueConverter();
    }
}
