using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebFramework.Api;
using Entities;
using DAL.Dapper.Infrastructure;

namespace Api.Controllers.v1
{
    [ApiVersion("1")]
    public class PostsController : BaseController
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper _mapper;

        public PostsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {

                //var result = await unitOfWork.Post.GetAsync(a =>  a.Category);
                //unitOfWork.Post.LoadReference(result, a => a.Category);








                var result1 = await unitOfWork.Post.AddAsync(new Post { Title = "test new 20" });
                var result2 = await unitOfWork.Post.AddAsync(new Post { Title = "test new 21" });
                unitOfWork.Complete();


                return Ok();


            }
            catch (System.Exception ex)
            {

                throw;
            }

        }

    }
}
