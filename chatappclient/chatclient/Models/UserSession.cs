using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QLUSER.Models
{
    public static class UserSession
    {
        private static SynchronizationContext _uiContext;

        public static event Action ActionUpdatedpname = delegate { };
        public static event Action ActionUpdateusername = delegate { };
        public static event Action ActionAvatarUpdated = delegate { };
        public static event Action ActionUpdateGroup = delegate { };
        public static event Action ActionUpdateGroupPrivate = delegate { };
        public static event Action ActionAvatarGroupUpdated = delegate { };
        public static event Action ActionDeleteuser = delegate { };
        public static event Action ActionUpdateFriend = delegate { };
        public static event Action ActionUpdateFriendRequest = delegate { };
        public static event Action ActionUpdategdpname = delegate { };
        public static event Action ActionUpdateGroupMember = delegate { };
        public static event Action ActionUpdateChannel = delegate { };
        public static event Action ActionUpdateChannelname = delegate { };
        public static event Action ActionUpdateDanhMuc = delegate { };
        public static event Action ActionUpdatedanhmucname = delegate { };
        public static event Action ActionUpdateGroupMemberPrivate = delegate { };
        public static event Action ActionUpdateGroupname = delegate { };
        public static event Action ActionUpdateMessage = delegate { };
        public static event Action ActionUpdateMessageText = delegate { };
        public static event Action ActionUpdateRole = delegate { };
        public static bool _upuserid;
        public static bool _upgroupid;
        public static bool _updanhmucid;
        public static bool _upchannelid;
        public static bool _upmessageid;
        public static bool _deleteuser;
        public static bool _updatefriendrequest;
        public static bool _updatefriend;
        public static bool _updategroup;
        public static bool _updatestatus;
        public static bool _updategroupmember;
        public static bool _updatedanhmuc;
        public static bool _updatechannel;
        public static bool _updatemessage;
        public static void InitializeSynchronizationContext()
        {
            _uiContext = SynchronizationContext.Current;
        }
        public static void RunOnUIThread(Action action)
        {
            if (_uiContext != null)
            {
                _uiContext.Post(_ => action(), null);
            }
            else
            {
                // Nếu không tìm thấy SynchronizationContext thì gọi trực tiếp
                action();
            }
        }
        public static bool Renamedpname
        {
            get => _upuserid;
            set
            {
                    _upuserid = value;
                    ActionUpdatedpname?.Invoke();
            }
        }
        public static bool AvatarUrl
        {
            get => _upuserid;
            set
            {
                _upuserid = value;
                ActionAvatarUpdated?.Invoke();

            }
        }
        public static bool Renameusername
        {
            get => _upuserid;
            set
            {
                    _upuserid = value;
                    ActionUpdateusername?.Invoke();
                
            }
        }
        public static bool DeleteUser
        {
            get => _deleteuser;
            set
            {
                _deleteuser = value;
                ActionDeleteuser?.Invoke();
            }
        }


        public static bool UpdateFriendRequest
        {
            get => _updatefriendrequest;
            set
            {
                _updatefriendrequest= value;
                ActionUpdateFriendRequest?.Invoke();
            }
        }


        public static bool UpdateFriend
        {
            get => _updatefriend;
            set
            {
                _updatefriend= value;
                ActionUpdateFriend?.Invoke();
            }
        }


        public static bool AvatarGroupUrl
        { 
            get => _upgroupid;
            set
            {
                _upgroupid = value;
                ActionAvatarGroupUpdated?.Invoke();
            }
        }

        public static (bool update, bool status) UpdateGroup
        {
            get => ( _updategroup,_updatestatus);
            set
            {
                    _updategroup = value.update;
                    _updatestatus = value.status;

                    if(_updatestatus) ActionUpdateGroup?.Invoke();
                    else ActionUpdateGroupPrivate?.Invoke();
                    //0 la update group private
                    //1 la update group public
            }
        }

        public static bool RenameGroupname
        {
            get => _upgroupid;
            set
            {
                    _upgroupid = value;
                    ActionUpdateGroupname?.Invoke();
                
            }
        }


        public static (bool update, bool status) UpdateGroupMember
        {
            get => (_updategroupmember,_updatestatus);
            set
            {
                _updategroupmember = value.update;
                _updatestatus=value.status;
                if (_updatestatus)
                {
                    ActionUpdateGroup?.Invoke();
                    ActionUpdateGroupMember?.Invoke();
                }
                else
                {
                    ActionUpdateGroupPrivate?.Invoke();
                    ActionUpdateGroupMemberPrivate?.Invoke(); 
                }
                //0 la update groupmember private
                //1 la update groupmember public
            }
        }
        public static (bool userid, bool groupid) Renamegdpname
        {
            get => (_upuserid, _upgroupid);
            set
            {
                    _upuserid = value.userid;
                    _upgroupid = value.groupid;
                    ActionUpdategdpname?.Invoke();
                
            }
        }
        public static (bool userid, bool groupid) UpdateUserRole
        {
            get => (_upuserid, _upgroupid);
            set
            {
                _upgroupid = value.groupid;
                    _upuserid = value.userid;
                    ActionUpdateRole();
                
            }
        }



        public static bool UpdateDanhMuc
        {
            get => _updatedanhmuc;
            set
            {
                _updatedanhmuc = value;
                ActionUpdateDanhMuc?.Invoke();
            }
        }

        public static bool Renamedanhmucname
        {
            get => _updanhmucid;
            set
            {
                _updanhmucid = value;
                ActionUpdatedanhmucname?.Invoke();

            }
        }
        public static bool UpdateChannel
        {
            get => _updatechannel;
            set
            {
                _updatechannel = value;
                ActionUpdateChannel?.Invoke();
            }
        }
        public static bool Renamechannelname
        {
            get => _upchannelid;
            set
            {
                _upchannelid = value;
                ActionUpdateChannelname?.Invoke();
            }
        }
        public static bool UpdateMessage
        {
            get => _updatemessage;
            set
            {
                _updatemessage = value;
                ActionUpdateMessage?.Invoke();
            }
        }
        public static bool RenameMessageText
        {
            get => _upmessageid;
            set
            {
                _upmessageid = value;
                ActionUpdateMessageText?.Invoke();
            }
        }
    }

}
