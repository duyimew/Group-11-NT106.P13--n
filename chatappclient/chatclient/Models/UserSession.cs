using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLUSER.Models
{
    public static class UserSession
    {
        public static event Action AvatarUpdated;
        private static string _avatarUrl;

        public static string AvatarUrl
        {
            get => _avatarUrl;
            set
            {
                if (_avatarUrl != value)
                {
                    _avatarUrl = value;
                    AvatarUpdated?.Invoke(); 
                }
            }
        }
    }

}
