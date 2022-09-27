using WebGoatCore.Models;
using WebGoatCore.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Text.Encodings.Web;
using System.Collections.Generic;

namespace WebGoatCore.Controllers
{
    [Route("[controller]/[action]")]
    public class BlogController : Controller
    {
        HtmlEncoder _htmlEncoder;
        JavaScriptEncoder _javaScriptEncoder;
        UrlEncoder _urlEncoder;
        private readonly BlogEntryRepository _blogEntryRepository;
        private readonly BlogResponseRepository _blogResponseRepository;

        public BlogController(BlogEntryRepository blogEntryRepository, BlogResponseRepository blogResponseRepository, NorthwindContext context, HtmlEncoder htmlEncoder, JavaScriptEncoder javaScriptEncoder, UrlEncoder urlEncoder)
        {
            _blogEntryRepository = blogEntryRepository;
            _blogResponseRepository = blogResponseRepository;
            // Fixed Code
            _htmlEncoder = htmlEncoder;
            _javaScriptEncoder = javaScriptEncoder;
        }

        public IActionResult Index()
        {
            List<BlogEntry> blogs=_blogEntryRepository.GetTopBlogEntries();
            foreach(BlogEntry blog in blogs)
            {
                foreach(BlogResponse i in blog.Responses)
                {
                    i.Contents = _htmlEncoder.Encode(i.Contents);
                    i.Contents = _javaScriptEncoder.Encode(i.Contents);
                }
            }
            return View(blogs);
        }

        [HttpGet("{entryId}")]
        public IActionResult Reply(int entryId)
        {
            return View(_blogEntryRepository.GetBlogEntry(entryId));
        }

        [HttpPost("{entryId}")]
        public IActionResult Reply(int entryId, string contents)
        {
            var userName = User.Identity.Name ?? "Anonymous";
            //var safeContent = _htmlEncoder.Encode(contents);
            //safeContent = _urlEncoder.Encode(safeContent);
            //safeContent = _javaScriptEncoder.Encode(safeContent);
            var response = new BlogResponse()
            {
                Author = userName,
                Contents = contents,
                BlogEntryId = entryId,
                ResponseDate = DateTime.Now
            };
            _blogResponseRepository.CreateBlogResponse(response);

            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Create() => View();

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Create(string title, string contents)
        {
            var blogEntry = _blogEntryRepository.CreateBlogEntry(title, contents, User.Identity.Name!);
            return View(blogEntry);
        }

    }
}