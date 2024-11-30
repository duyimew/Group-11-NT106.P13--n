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
        public static event Action AvatarGroupUpdated;
        public static event Action AvatarGroupCreated;
        private static string _avatarUrl;
        private static string _avatargroupUrl;
        private static bool _createnew;

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
        public static (string url,bool status) AvatarGroupUrl
        {
            get => ( _avatargroupUrl, _createnew );
            set
            {
                if (_avatargroupUrl != value.url)
                {
                    _avatargroupUrl = value.url;
                    _createnew = value.status;
                    if (_createnew == false)
                    {
                        AvatarGroupUpdated?.Invoke();
                    }
                    else AvatarGroupCreated?.Invoke();
                }
            }
        }
    }

}
