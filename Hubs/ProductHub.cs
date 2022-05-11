using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Pizza_Hut.Models;
using Pizza_Hut.Repository;
using System;
using System.Threading.Tasks;

namespace Pizza_Hut.Hubs
{
    public class ProductHub : Hub
    {
        //private IHubContext<ProductHub> hubContext;
        //private ICommentRepository commentRepository;

        //public ProductHub(ICommentRepository _commentRepository,
        //    IHubContext<ProductHub> _hubContext)
        //{
        //    commentRepository = _commentRepository;
        //    hubContext = _hubContext;
        //}
        //[Authorize]
        //public async void Add(string comment, string UserName, int ProductID)
        //{
        //    if (comment != null)
        //    {
        //        Comment Newcomment = new Comment()
        //        {
        //            comment = comment,
        //            ProductID = ProductID,
        //            UserName = UserName
        //        };

        //        if (commentRepository.Insert(Newcomment) > 0)
        //        {
        //            await hubContext.Clients.All.SendAsync("AddComment", Newcomment);
        //        }
        //    }
        //}
    }
}
