using System;

using Microsoft.AspNetCore.Http;
using System.Text.Json;


namespace DatingApp.API.Helpers
{
    public static class Extensions
    {
        public static void AddApplicationError(this HttpResponse response, string message)
        {
            response.Headers.Add("Application-Error", message);
            response.Headers.Add("Access-Control-Expose-Headers", "Application-Error");
            response.Headers.Add("Access-Control-Allow-Origin", "*");
        }

        public static void AddPagination(this HttpResponse response, 
            int currentPage, int itemPerPage, int totalItems, int totalPages)
        {
            var paginationHeader = new PaginationHeader(currentPage, itemPerPage, totalItems, totalPages);

            var camelCaseFormatter = new JsonSerializerOptions();
            camelCaseFormatter.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;

            response.Headers.Add("Pagination", JsonSerializer.Serialize<PaginationHeader>(paginationHeader, camelCaseFormatter));
            response.Headers.Add("Access-Control-Expose-Headers", "Pagination");
        }

        public static int CalculetAge(this DateTime theDateTime)
        {
            var age = DateTime.Today.Year - theDateTime.Year;
            if(theDateTime.AddYears(age) > DateTime.Today)
                age--;
            return age;
        }
    }
}