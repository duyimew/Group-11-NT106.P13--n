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
        public static event Action UpdateGroupname;
        public static event Action UpdateRole;
        private static bool _avatarUrl;
        private static bool _avatargroupUrl;
        private static bool _createnew;
        public static bool _newgroupname;
        public static bool _newUserRole;

        public static bool AvatarUrl
        {
            get => _avatarUrl;
            set
            {
                _avatarUrl = value;
                if (_avatarUrl)
                {
                    AvatarUpdated?.Invoke(); 
                }
            }
        }
        public static (bool url,bool status) AvatarGroupUrl
        {
            get => ( _avatargroupUrl, _createnew );
            set
            {
                _avatargroupUrl = value.url;
                _createnew = value.status;
                if (_avatargroupUrl)
                {
                    if (_createnew == false)
                    {
                        AvatarGroupUpdated?.Invoke();
                    }
                    else AvatarGroupCreated?.Invoke();
                }
            }
        }
        public static bool RenameGroupname
        {
            get => _newgroupname;
            set
            {
                _newgroupname = value;
                if (_newgroupname)
                {
                    UpdateGroupname?.Invoke();
                }
            }
        }
        public static bool UpdateUserRole
        {
            get => _newUserRole;
            set
            {
                _newUserRole = value;
                if (_newUserRole)
                {
                    UpdateRole?.Invoke();
                }
            }
        }
    }

}
