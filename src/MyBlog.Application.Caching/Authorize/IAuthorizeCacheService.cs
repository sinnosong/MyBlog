﻿using MyBolg.ToolKits.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Application.Caching.Authorize
{
    public interface IAuthorizeCacheService
    {
        /// <summary>
        /// 获取登录地址(github)
        /// </summary>
        /// <param name="factory"></param>
        /// <returns></returns>
        Task<ServiceResult<string>> GetLoginAddressAsync(Func<Task<ServiceResult<string>>> factory);

        /// <summary>
        /// 获取AccessToken
        /// </summary>
        /// <param name="code"></param>
        /// <param name="factory"></param>
        /// <returns></returns>
        Task<ServiceResult<string>> GetAccessTokenAsync(string code, Func<Task<ServiceResult<string>>> factory);

        /// <summary>
        /// 登录成功，生成Token
        /// </summary>
        /// <param name="access_token"></param>
        /// <param name="factory"></param>
        /// <returns></returns>
        Task<ServiceResult<string>> GenerateTokenAsync(string access_token, Func<Task<ServiceResult<string>>> factory);
    }
}