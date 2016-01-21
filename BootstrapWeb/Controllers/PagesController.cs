using System;
using System.Web;
using Connections;
using System.Linq;
using System.Web.Mvc;
using Orange.Business;
using Orange.Core.Results;
using BootstrapWeb.Models;
using System.Collections.Generic;

namespace BootstrapWeb.Controllers
{
    public class PagesController : Controller
    {
        public ActionResult Home()
        {
            return View();
        }

        // this is really the page load for Posts
        public ActionResult Comments(int postId)
        {
            Database database = new Database("DevOrange");
            UserResult userDetails = new UserOps(database).GetByUsername("Orange");
            PostResult postDetails = new PostOps(database).Get(postId);
            CommentResultList comments = new CommentOps(database).GetTopLevel(postId); // this won't load denied (isActive = false) items by default

            // if user is NOT logged in then only show approved items
            // if user IS logged in AND does NOT have approval permissions show only approved items
            //comments = comments.Results.RemoveAll(c => c.Approval != Orange.Core.Enums.Approval.Approved);

            // if user IS logged in AND HAS approval permissions show approved/pending items
            // do nothing and just load up all comments

            BlogPost post = new BlogPost()
            {
                UserDetails = userDetails,
                PostDetails = postDetails,
                TopLevelComments = comments
            };

            return View(post);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commentId"></param>
        /// <returns></returns>
        public ActionResult GetReplies(int commentId)
        {
            Database database = new Database("DevOrange");
            CommentResultList replies = new CommentOps(database).GetChildren(commentId);

            // if user is NOT logged in then only show approved items
            // if user IS logged in AND does NOT have approval permissions show only approved items
            //comments = comments.Results.RemoveAll(c => c.Approval != Orange.Core.Enums.Approval.Approved);

            // if user IS logged in AND HAS approval permissions show approved/pending items
            // do nothing and just load up all comments


            return PartialView("_Comments", replies);
        }

        public ActionResult CkEditorTest()
        {
            return View();
        }
    }
}
