using Microsoft.AspNetCore.Identity;

namespace HomeStorage.DataAccess.UserEntities
{
    public class HomeStorageUser : IdentityUser<Guid>
    {
        public override string Email
        {
            get
            {
                base.Email ??= string.Empty;
                return base.Email;
            }
            set
            {
                base.Email = value;
                base.NormalizedEmail = value.ToUpper();
            }
        }
        public override string NormalizedEmail
        {
            get
            {
                base.NormalizedEmail ??= string.Empty;
                return base.NormalizedEmail;
            }
            set
            {
                base.NormalizedEmail = value;
            }
        }
        public override string UserName
        {
            get
            {
                base.UserName ??= string.Empty;
                return base.UserName;
            }
            set
            {
                base.UserName = value;
                base.NormalizedUserName = value.ToUpper();
            }
        }
        public override string NormalizedUserName
        {
            get
            {
                base.NormalizedUserName ??= string.Empty;
                return base.NormalizedUserName;
            }
            set
            {
                base.NormalizedUserName = value;
            }
        }
    }
}
