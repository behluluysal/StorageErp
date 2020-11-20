using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using k1835web.Models;

namespace k1835web.Security
{
    public class CustomPrincipal : IPrincipal
    {

        private Kullanici Account;

        public CustomPrincipal(Kullanici account)
        {
            this.Account = account;
            this.Identity = new GenericIdentity(account.Kullaniciadi);
        }
        public IIdentity Identity { get;set;}

        public bool IsInRole(string role)
        {
            var roles = role.Split(new char[] { ',' });
            return roles.Any(r => this.Account.Roletr.Contains(r));
        }
    }
}