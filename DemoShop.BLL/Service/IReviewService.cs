using DemoShop.DAL.DTO.Request;
using System;
using System.Collections.Generic;
using System.Text;

namespace DemoShop.BLL.Service
{
    public interface IReviewService
    {
        Task<bool> AddReview(string userId, AddReviewRequest request);
    }
}
