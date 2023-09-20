using Comments.API.ModelBinders;
using Comments.Application.Comments.Commands.Add;
using Comments.Application.Comments.Commands.Delete;
using Comments.Application.Comments.Commands.Update;
using Comments.Application.Comments.Queries.Get;
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

        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> DeleteComment([ModelBinder(typeof(BodyCommandModelBinder))] DeleteCommentCommand deleteCommentsCommand)
        {
            await _mediator.Send(deleteCommentsCommand);
            return NoContent();
        }

        [HttpPut("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> UpdateComment([ModelBinder(typeof(BodyCommandModelBinder))] UpdateCommentCommand updateComment)
        => Ok(await _mediator.Send(updateComment));

        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<Comment>> GetComment([FromRoute] GetCommentQuery getCommentQuery)
            => Ok(await _mediator.Send(getCommentQuery));
    }
}