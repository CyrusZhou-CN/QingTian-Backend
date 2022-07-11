﻿using System.ComponentModel.DataAnnotations;

namespace QingTian.Core.Services
{
    public class UpdatePositionParam : AddPositionParam
    {
        /// <summary>
        /// 职位Id
        /// </summary>
        [Required(ErrorMessage = "职位Id不能为空")]
        public long Id { get; set; }
    }
}