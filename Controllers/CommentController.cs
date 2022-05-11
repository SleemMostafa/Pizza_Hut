using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Pizza_Hut.Hubs;
using Pizza_Hut.Models;
using Pizza_Hut.Repository;
using Pizza_Hut.ViewModel;
using System;
using System.Threading.Tasks;

namespace Pizza_Hut.Controllers
{
    public class CommentController : Controller
    {
        private readonly ICommentRepository commentRepository;
        private readonly IHubContext<ProductHub> hubContext;
        private readonly INotyfService notyf;
        public CommentController(ICommentRepository _commentRepository,
            IHubContext<ProductHub> hubContext,
            INotyfService _notyf
            )
        {
            commentRepository = _commentRepository;
            this.hubContext = hubContext;
            notyf = _notyf;
        }
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Add(string comment, string UserName, int ProductID)
        {
            if (comment != null)
            {
                try
                {
                    Comment Newcomment = new Comment()
                    {
                        comment = comment,
                        ProductID = ProductID,
                        UserName = UserName
                    };

                    if (commentRepository.Insert(Newcomment) > 0)
                    {
                        notyf.Success("Added Successfully", 5);
                        await hubContext.Clients.All.SendAsync("AddComment", Newcomment);
                    }
                    else
                    {
                        notyf.Error("Error !!", 5);
                    }
                }
                catch (Exception ex)
                {
                    return View("Error", new ErrorViewModel() { RequestId = ex.Message });
                }
            }
            return Ok();
        }
    }
}
