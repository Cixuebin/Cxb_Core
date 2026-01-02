using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using UnityEngine;

public class MateCommunityVirtualPet
{/// <summary>
 /// 主人ID
 /// </summary>
    [DisplayName("主人ID")]
    public long? UserId { get; set; }

    /// <summary>
    /// 虚拟宠物名称
    /// </summary>
    [DisplayName("虚拟宠物名称")]
    public string PetName { get; set; }

    /// <summary>
    /// 能量值
    /// </summary>
    [DisplayName("能量值")]
    public long? EnergyNum { get; set; }

    /// <summary>
    /// 灵力值
    /// </summary>
    [DisplayName("灵力值")]
    public long? SpiritualPowerNum { get; set; }

    /// <summary>
    /// 最后喂食时间
    /// </summary>
    [JsonProperty("lastFeedTime")]
    [JsonConverter(typeof(Newtonsoft.Json.Converters.IsoDateTimeConverter))]
    public DateTime? LastFeedTime { get; set; }

    /// <summary>
    /// 最后互动时间
    /// </summary>
    [JsonProperty("lastInteractTime")]
    [JsonConverter(typeof(Newtonsoft.Json.Converters.IsoDateTimeConverter))]
    public DateTime? LastInteractTime { get; set; }

    /// <summary>
    /// 状态（1=正常, 2=饥饿, 3=疲劳, 4=休息中）
    /// </summary>
    [DisplayName("状态")]
    public long Status { get; set; } = 1L;

    /// <summary>
    /// 最后减少时间
    /// </summary>
    [JsonProperty("lastReduceTime")]
    [JsonConverter(typeof(Newtonsoft.Json.Converters.IsoDateTimeConverter))]
    public DateTime? LastReduceTime { get; set; }

    // --------------------

    /// <summary>
    /// 主人名称（非数据库字段）
    /// </summary>
    [JsonIgnore]
    public string UserName { get; set; }
}
/*
更新宠物信息  post
unity/api/mate_community/virtual_pet/update 
id integer 虚拟宠物ID
petName string 虚拟宠物名称

新增虚拟宠物 post
http://localhost:8004/unity/api/mate_community/virtual_pet/add
petName string  宠物名称

查询自己的宠物信息 Get
http://localhost:8004/unity/api/mate_community/virtual_pet/info/8

查询自己的宠物列表 Get
http://localhost:8004/unity/api/mate_community/virtual_pet/myPetList
*/