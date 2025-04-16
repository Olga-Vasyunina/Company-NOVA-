using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleDatabaseWalkthrough
{
    public static class UserSession
    {
        public static int CurrentUserId { get; set; }
        public static string UserRole { get; set; }
        public static bool IsAuthenticated { get; set; }
    }
}
