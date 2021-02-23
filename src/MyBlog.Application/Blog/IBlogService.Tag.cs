﻿using MyBlog.Application.Contracts.Blog;
using MyBolg.ToolKits.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyBlog.Application.Blog
{
    public partial interface IBlogService
    {
        /// <summary>
        /// 查询标签列表
        /// </summary>
        /// <returns></returns>
        Task<ServiceResult<IEnumerable<QueryTagDto>>> QueryTagsAsync();
    }
}