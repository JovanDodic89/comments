using Comments.API.ModelBinders;
using Comments.Application.Comments.Commands.Add;
using Comments.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Comments.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CommentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CommentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<Comment>> AddComment([ModelBinder(typeof(BodyCommandModelBinder))] AddCommentCommand addCommentCommand)
            => Ok(await _mediator.Send(addCommentCommand));
    }
}