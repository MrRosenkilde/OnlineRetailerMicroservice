using Microsoft.EntityFrameworkCore;

namespace OrderApi.Data
{
    public interface IDbInitializer
    {
        void Initialize(OrderApiContext context);
        void Initialize(CustomerAPIContext context);
    }
}
