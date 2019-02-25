using DataAccessServices.Models;
using DataContract.Identity.Models;
using DataContract.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessServices
{
    internal static class Mapper
    {
        //TODO: Implement reflection here
        private static void MapObjects(object source, object destination, bool selectPrimitives = true)
        {
            Type sourceType = source.GetType();
            Type destinationType = destination.GetType();

            var sourceProperties = sourceType.GetProperties();
            var destionationProperties = destinationType.GetProperties();

            var commonProperties = from sp in sourceProperties
                                   join dp in destionationProperties on new { sp.Name, sp.PropertyType } equals
                                       new { dp.Name, dp.PropertyType }
                                   select new { sp, dp };

            foreach (var match in commonProperties)
            {
                if (selectPrimitives)
                    if (match.sp.GetType().IsPrimitive)
                    {
                        match.dp.SetValue(destination, match.sp.GetValue(source, null), null);
                    }
                    else { }
                else
                {
                    match.dp.SetValue(destination, match.sp.GetValue(source, null), null);
                }
            }
        }

        public static User Map(AppUser user)
        {
            if (user == null) return null;

            return new User
            {
                Id = user.Id,
                Email = user.Email,
                IsBlocked = user.IsBlocked,
                RegistrationDate = user.Profile.RegistrationDate,
                UserName = user.Profile.UserName,
                Status = user.Profile.Status,
                EmailNotificationsEnabled = user.Profile.EmailNotificationsEnabled,
                ForumNotificationsEnabled = user.Profile.ForumNotificationsEnabled,
                SubscriptionEnabled = user.Profile.SubscriptionEnabled,
                EmailConfirmed = user.EmailConfirmed
            };
        }

        public static CommentModel Map(Comment comment, bool includeReplies = true)
        {
            if (comment == null) return null;
            var result = new CommentModel
            {
                Id = comment.Id,
                AuthorId = comment.AuthorId,
                CreatedDate = comment.CreatedDate,
                CreatedDateString = comment.CreatedDate.ToShortDateString(),
                Order = comment.Order,
                ReplyToCommentId = comment.ReplyToCommentId,
                Text = comment.Text,
                TopicId = comment.TopicId,
                Replies = new List<CommentModel>()
            };

            if (includeReplies)
            {
                foreach (var reply in comment.Replies)
                {
                    result.Replies.Add(Map(reply));
                }
            }
            return result;

        }

        public static TopicModel Map(Topic topic)
        {
            if (topic == null) return null;
            var result = new TopicModel
            {
                Id = topic.Id,
                AuthorId = topic.AuthorId,
                Comments = new List<CommentModel>(),
                CreatedDate = topic.CreatedDate,
                CreatedDateString = topic.CreatedDate.ToShortDateString(),
                ForumId = topic.ForumId,
                Name = topic.Name,
                Text = topic.Text
            };

            foreach (var comment in topic.Comments)
            {
                result.Comments.Add(Map(comment));
            }
            return result;
        }

        public static ForumModel Map(Forum forum)
        {
            if (forum == null) return null;
            var result = new ForumModel
            {
                Id = forum.Id,
                AuthorId = forum.AuthorId,
                CreatedDate = forum.CreatedDate,
                CreatedDateString = forum.CreatedDate.ToShortDateString(),
                Name = forum.Name,
                Topics = new List<TopicModel>()
            };

            foreach (var topic in forum.Topics)
            {
                result.Topics.Add(new TopicModel
                {
                    Id = topic.Id,
                    Name = topic.Name,
                    Text = topic.Text,
                    ForumId = topic.ForumId,
                    AuthorId = topic.AuthorId,
                    CreatedDate = topic.CreatedDate,
                    CreatedDateString = topic.CreatedDate.ToShortDateString()
                });
            }
            return result;

        }

        public static ArticleModel Map(Article article)
        {
            return new ArticleModel
            {
                Id = article.Id,
                AuthorId = article.AuthorId,
                CreatedDate = article.CreatedDate,
                CreatedDateString = article.CreatedDate.ToShortDateString(),
                Name = article.Name,
                Text = article.Text
            };
        }

    }
}
