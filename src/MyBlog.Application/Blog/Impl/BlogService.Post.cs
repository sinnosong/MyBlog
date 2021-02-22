﻿using MyBlog.Application.Contracts.Blog;
using MyBlog.Domain.Shared;
using MyBolg.ToolKits.Base;
using MyBolg.ToolKits.Extensions;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlog.Application.Blog.Impl
{
    public partial class BlogService
    {
        /// <summary>
        /// 分页查询文章列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ServiceResult<PagedList<QueryPostDto>>> QueryPostAsync(PaginInput input)
        {
            return await _blogCacheService.QueryPostAsync(input, async () =>
             {
                 var result = new ServiceResult<PagedList<QueryPostDto>>();

                 var count = await _postRepository.GetCountAsync();

                 var list = _postRepository.OrderByDescending(x => x.CreationTime)
                                           .PageByIndex(input.Page, input.Limit)
                                           .Select(x => new PostBrieDto
                                           {
                                               Title = x.Title,
                                               Url = x.Url,
                                               Year = x.CreationTime.Year,
                                               CreateTime = x.CreationTime.TryToDateTime()
                                           }).GroupBy(x => x.Year)
                                           .Select(x => new QueryPostDto
                                           {
                                               Year = x.Key,
                                               Posts = x.ToList()
                                           }).ToList();
                 result.IsSuccess(new PagedList<QueryPostDto>(count.TryToInt(), list));
                 return result;
             });
        }

        /// <summary>
        /// 根据URl获取文章详情
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<ServiceResult<PostDetailDto>> GetPostDetailAsync(string url)
        {
            return await _blogCacheService.GetPostDetailAsync(url, async () =>
             {
                 var result = new ServiceResult<PostDetailDto>();
                 var post = await _postRepository.FindAsync(x => x.Url.Equals(url));
                 if (post == null)
                 {
                     result.IsFailed(ResponseText.WHAT_NOT_EXIST.Format("URL", url));
                     return result;
                 }

                 var category = await _categoryRepository.GetAsync(post.CategoryId);

                 var tags = from post_tags in await _postTagsRepository.GetListAsync()
                            join tag in await _tagRepository.GetListAsync()
                            on post_tags.TagId equals tag.Id
                            where post_tags.PostId.Equals(post.Id)
                            select new TagDto
                            {
                                TagName = tag.TagName,
                                DisplayName = tag.DisplayName
                            };
                 var previous = _postRepository.Where(x => x.CreationTime > post.CreationTime).Take(1).FirstOrDefault();
                 var next = _postRepository.Where(x => x.CreationTime < post.CreationTime).OrderByDescending(x => x.CreationTime).Take(1).FirstOrDefault();

                 var postDetail = new PostDetailDto
                 {
                     Title = post.Title,
                     Author = post.Author,
                     Url = post.Url,
                     Html = post.Html,
                     Markdown = post.Markdown,
                     CreationTime = post.CreationTime.TryToDateTime(),
                     Category = new CategoryDto
                     {
                         CategoryName = category.CategoryName,
                         DisplayName = category.DisplayName
                     },
                     Tags = tags,
                     Previous = previous == null ? null : new PostForPagedDto
                     {
                         Title = previous.Title,
                         Url = previous.Url
                     },
                     Next = next == null ? null : new PostForPagedDto
                     {
                         Title = next.Title,
                         Url = next.Url
                     }
                 };

                 result.IsSuccess(postDetail);
                 return result;
             });
        }
    }
}