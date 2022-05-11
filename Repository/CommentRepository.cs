using Pizza_Hut.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pizza_Hut.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly Context context;

        public CommentRepository(Context context)
        {
            this.context = context;
        }
        public int Delete(int id)
        {
            try
            {
                Comment comment = GetById(id);
                context.Comments.Remove(comment);
                return context.SaveChanges();
            }
            catch(Exception ex)
            {
                return 0;
            }
        }

        public List<Comment> GetAll()
        {
            return context.Comments.ToList();
        }

        public Comment GetById(int id)
        {
            try
            {
                Comment comment = context.Comments.FirstOrDefault(c => c.Id == id);
                return comment;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public int Insert(Comment entity)
        {
            try
            {
                context.Comments.Add(entity);
                return context.SaveChanges();
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public int Update(int id, Comment entity)
        {
            try
            {
                Comment commentOld = GetById(id);
                commentOld.comment = entity.comment;
                commentOld.UserName = entity.UserName;
                commentOld.ProductID = entity.ProductID;
                return context.SaveChanges();
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }
}
