using Discount.Grpc.Data;
using Discount.Grpc.Models;
using Grpc.Core;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Services
{
    public class DiscountService(DiscountContext _context, ILogger<DiscountService> _logger) : DiscountProtoService.DiscountProtoServiceBase
    {
        public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            var coupon = await _context.Coupons.FirstOrDefaultAsync(x => x.ProductName == request.ProductName);
            if (coupon == null)
                coupon = new Coupon { ProductName = "No Discount", Amount = 0, Description = "No Discount Desc" };

            _logger.LogInformation("Discount is retrieved for ProductName : {productName}, Amount : {amount}", coupon.ProductName, coupon.Amount);

            var couponModel = coupon.Adapt<CouponModel>();
            return couponModel;
        }

        public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            var coupon = request.Coupon.Adapt<Coupon>();

            if (coupon is null)
                throw new RpcException(new Status(statusCode: StatusCode.InvalidArgument, "Invalid request object."));

            _context.Coupons.Add(coupon);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Discount is successfully created. ProductName : {ProductName}", coupon.ProductName);

            var result = coupon.Adapt<CouponModel>();
            return result;
        }

        public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            var coupon = request.Coupon.Adapt<Coupon>();

            if (coupon is null)
                throw new RpcException(new Status(statusCode: StatusCode.InvalidArgument, "Invalid request object"));

            _context.Coupons.Update(coupon);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Discount is successfully updated. ProductName : {ProductName}", coupon.ProductName);

            var result = coupon.Adapt<CouponModel>();
            return result;
        }

        public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            var coupon = await _context.Coupons.FirstOrDefaultAsync(x => x.ProductName == request.ProductName);

            if (coupon is null)
                throw new RpcException(new Status(statusCode: StatusCode.InvalidArgument, "Invalid request object"));

            _context.Coupons.Remove(coupon);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Discount is successfully deleted. ProductName : {ProductName}", coupon.ProductName);

            return new DeleteDiscountResponse { Success = true };
        }
    }
}