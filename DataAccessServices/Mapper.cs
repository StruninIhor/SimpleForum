using DataAccessServices.Models;
using DataContract.Identity.Models;
using DataContract.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessServices
{
    internal static class Mapper
    {
        /// <summary>
        /// Maps one object to another if fields name are the same and they are not Generic
        /// </summary>
        /// <param name="source">Source obeject</param>
        /// <param name="target">Object to map from source</param>
        /// <returns></returns>
        public static object MapObjects(object source, object target, params string[] NotIncludedProperties)
        {
            foreach (PropertyInfo sourceProp in source.GetType().GetProperties())
            {
                PropertyInfo targetProp = target.GetType().GetProperties().Where(p => p.Name == sourceProp.Name).FirstOrDefault();

                string ReflectionPropertyName = sourceProp.Name;

                if (targetProp != null && 
                    targetProp.GetType().Name == sourceProp.GetType().Name && 
                    !NotIncludedProperties.Contains(ReflectionPropertyName))
                {
                    targetProp.SetValue(target, sourceProp.GetValue(source));
                }
            }
            return target;
        }

        public static User Map(AppUser user)
        {
            if (user == null) return null;

            var result = new User();
            MapObjects(user, result);

            result.RegistrationDate = user.Profile.RegistrationDate;
            result.UserName = user.Profile.UserName;
            result.Status = user.Profile.Status;
            result.EmailNotificationsEnabled = user.Profile.EmailNotificationsEnabled;
            result.ForumNotificationsEnabled = user.Profile.ForumNotificationsEnabled;
            result.SubscriptionEnabled = user.Profile.SubscriptionEnabled;
            result.Rating = user.Profile.Rating;

            return result;
        }

        public static CommentModel Map(Comment comment, bool includeReplies = true)
        {
            if (comment == null) return null;

            var result = new CommentModel();
            MapObjects(comment, result, NotIncludedProperties: new string[] { "Replies" });

            result.Replies = new List<CommentModel>();
            result.CreatedDateString = comment.CreatedDate.ToShortDateString();
            result.AuthorName = comment.Author.Profile.UserName;
            if (includeReplies)
            {
                foreach (var reply in comment.Replies)
                {
                    result.Replies.Add(Map(reply));
                }
            }
            return result;

        }

        public static TopicModel Map(Topic topic, bool includeComments)
        {
            if (topic == null) return null;

            var result = new TopicModel();
            MapObjects(topic, result, NotIncludedProperties: new string[] { "Comments" });

            result.Comments = new List<CommentModel>();
            result.CreatedDateString = topic.CreatedDate.ToShortDateString();

            if (includeComments)
            {
                foreach (var comment in topic.Comments.Where(c => c.Order == 0))
                {
                    result.Comments.Add(Map(comment));
                }
            }
            return result;
        }

        public static ForumModel Map(Forum forum)
        {
            if (forum == null) return null;
            var result = new ForumModel();
            MapObjects(forum, result, NotIncludedProperties: new string[] { "Topics" });

            result.CreatedDateString = result.CreatedDate.ToShortDateString();
            result.AuthorName = forum.Author.Profile.UserName;
            result.Topics = new List<TopicModel>();

            foreach (var topic in forum.Topics)
            {
                result.Topics.Add(Map(topic, false));
            }
            return result;

        }

        public static ArticleModel Map(Article article)
        {
            var result = new ArticleModel();
            //Debugger.Launch();
            MapObjects(article, result);
            result.AuthorName = article.Author.Profile.UserName;
            result.CreatedDateString = result.CreatedDate.ToShortDateString();
            return result;
        }

    }
}
