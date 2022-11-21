using WebGoatCore.Models;
using WebGoatCore.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace WebGoatCore.Controllers
{
    [Route("[controller]/[action]")]
    public class BlogController : Controller
    {
        private readonly BlogEntryRepository _blogEntryRepository;
        private readonly BlogResponseRepository _blogResponseRepository;

        public BlogController(BlogEntryRepository blogEntryRepository, BlogResponseRepository blogResponseRepository, NorthwindContext context)
        {
            _blogEntryRepository = blogEntryRepository;
            _blogResponseRepository = blogResponseRepository;
        }

        public IActionResult Index()
        {
            return View(_blogEntryRepository.GetTopBlogEntries());
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
            CalculateSpace(contents);

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

        public unsafe void CalculateSpace(string? contents)
        {
            if(contents==null)
            {

                ModelState.AddModelError(string.Empty, "Please enter input");
                
                
            }
            else
            {
            string msg = contents;
            const int INPUT_LEN = 256;
            char[] fixedChar = new char[INPUT_LEN];

            for (int i = 0; i < fixedChar.Length; i++)
                fixedChar[i] = '\0';

                fixed (char* revLine = fixedChar)
                {
                    int lineLen = contents.Length;
                    if (lineLen > INPUT_LEN)
                    {
                        ModelState.AddModelError(string.Empty,"Large Inputs");
                    }
                    else
                    {



                        for (int i = 0; i < lineLen; i++)
                            *(revLine + i) = contents[lineLen - i - 1];

                        char* revCur = revLine;

                        string blogContents = string.Empty;
                        while (*revCur != '\0')
                            blogContents += (char)*revCur++;
                    }
                }
            }
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