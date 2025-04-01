using Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Contracts.Seeds
{
    public static class DefaultData
    {
        public static async Task SeedPaymentMethodsAsync(ApplicationDbContext context)
        {
            if (!context.PaymentMethods.Any())
            {
                var PaymentMethods = new List<PaymentMethod>
                {
                    new PaymentMethod {  Name = "كاش", RegDate = DateTime.Now },
                    new PaymentMethod {  Name = "فيزا", RegDate = DateTime.Now }
                };

                // Add the branches to the database
                context.PaymentMethods.AddRange(PaymentMethods);

                // Save changes asynchronously
                await context.SaveChangesAsync();
            }
        }

        public static async Task SeedAddressTypesAsync(ApplicationDbContext context)
        {
            if (!context.AddressTypes.Any())
            {
                var AddressTypes = new List<AddressType>
                {
                    new AddressType {  Name = "المنزل", Icon = "fa-solid fa-house" },
                    new AddressType {  Name = "العمل", Icon = "fa-solid fa-building" },
                    new AddressType {  Name = "اخرى", Icon = "fa-solid fa-location-dot" }
                };

                // Add the branches to the database
                context.AddressTypes.AddRange(AddressTypes);

                // Save changes asynchronously
                await context.SaveChangesAsync();
            }
        }
    }
}
