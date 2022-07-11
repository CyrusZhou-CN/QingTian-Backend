using System;
using SqlSugar;
using System.ComponentModel;
using QingTian.Core.Entity;
namespace QingTian.Application.Entity
{
    /// <summary>
    /// 
    /// </summary>
    [SugarTable("test")]
    [Description("Test")]
    public class Test : DbEntityBase
    {
    }	
}