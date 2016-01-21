//using System;
//using System.Web;
//using Web.Models;
//using System.Linq;
//using System.Web.Mvc;
//using Orange.Business;
//using RC = Connections;
//using Orange.Core.Enums;
//using Orange.Core.Results;
//using Orange.Core.Entities;
//using System.Collections.Generic;

//namespace Web.Controllers
//{
//    public class PostsController : Controller
//    {
//        // TODO: add an attribute here (for example, if the post isn't public and you aren't logged in we need to redirect)
//        public ActionResult BlogPost(int locatorId)
//        {
//            //RC.Database orange = new RC.Database(ConfigurationManager.ConnectionStrings["DevOrange"].ConnectionString);
//            RC.Database orange = new RC.Database("DevOrange");
//            return View("BlogPost", LoadBlogPostBody(locatorId));
//        }

//        public ActionResult SavePost(string title, string effectiveDate, bool publiclyVisible, string body)
//        {
//            RC.Database orange = new RC.Database("DevOrange");
//            DateTime effective = DateTime.Parse(effectiveDate);
//            PostAdd post = new PostAdd
//            {
//                UserId = Convert.ToInt32(Session["UserId"]),
//                Subject = title,
//                EffectiveDate = effective,
//                IsPubliclyVisible = publiclyVisible,
//                Body = body
//            };
//            PostResult result = new PostOps(orange).Add(post);
//            return PartialView("~/Views/Partials/_Notification.cshtml", result);
//        }

//        // UPDATE POST

//        // REMOVE POST

//        public ActionResult SubmitComment(int postId, string username, string comment)
//        {
//            RC.Database orange = new RC.Database("DevOrange");

//            int userId = 0;
//            UserResult userDetails = new UserResult();
//            if(!string.IsNullOrEmpty(Convert.ToString(Session["Username"]))) // can't rely on someone typing in their real username
//            {
//                userDetails = new UserOps(orange).GetByUsername(username);
//                userId = userDetails.Result.Id;
//                username = userDetails.Result.Name;
//            }

//            CommentAdd save = new CommentAdd
//            {
//                UserId = userId,
//                PostId = postId,
//                ProvidedName = username,
//                Body = comment
//            };
//            CommentResult result = new CommentOps(orange).Add(save);
//            return PartialView("~/Views/Partials/_Notification.cshtml", result);
//        }

//        public ActionResult GetApprovedComments(int postId)
//        {
//            RC.Database orange = new RC.Database("DevOrange");
//            CommentResultList comments = new CommentOps(orange).GetAll(postId);
//            return PartialView("_ApprovedComments", LoadBlogPostBody(postId));
//        }

//        public ActionResult GetPendingComments(int postId)
//        {
//            RC.Database orange = new RC.Database("DevOrange");
//            CommentResultList comments = new CommentOps(orange).GetAll(postId);
//            return PartialView("_PendingComments", LoadBlogPostBody(postId));
//        }

//        public ActionResult ApproveAll(List<int> comments)
//        {
//            RC.Database orange = new RC.Database("DevOrange");
//            UserResult userDetails = new UserOps(orange).Get(Convert.ToInt32(Session["UserId"])); // TODO: this needs to be set!
//            CommentOps commentOps = new CommentOps(orange);
//            BoolResult result = new BoolResult();
//            foreach(int commentId in comments)
//            {
//                CommentApproval approval = new CommentApproval
//                {
//                    Id = commentId,
//                    UserId = 1,
//                    Approval = Approval.Approved
//                };
//                result = commentOps.CommentApproval(approval);
//                if (result.Severity != Severity.Success) break;
//            }
//            return PartialView("~/Views/Partials/_Notification.cshtml", result);
//        }

//        private BlogPost LoadBlogPostBody(int postId)
//        {
//            RC.Database orange = new RC.Database("DevOrange");
//            bool viewPendingComments = false;
//            if (Convert.ToBoolean(Session["Authenticated"]))
//            {
//                // TODO: is this the best way to handle this? Show approving comments be a permission level thing?
//                UserResult userDetails = new UserOps(orange).Get(1);
//                viewPendingComments = (userDetails.Result.RoleId <= 2);
//            }

//            PostResult postDetails = new PostOps(orange).Get(postId); // TODO: check for success?
//            CommentResultList postComments = new CommentOps(orange).GetAll(postId);

//            BlogPost model = new BlogPost
//            {
//                Post = postDetails.Result,
//                PendingComments = postComments.Results.Where(c => c.Approval == Approval.Pending).ToList(),
//                ApprovedComments = postComments.Results.Where(c => c.Approval == Approval.Approved).ToList(),
//                ViewPendingComments = viewPendingComments
//            };
//            return model;
//        }
//    }
//}
