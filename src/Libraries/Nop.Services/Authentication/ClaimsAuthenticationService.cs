using System;
using System.Web;
using System.Web.Security;
using Nop.Core.Domain.Customers;
using Nop.Services.Customers;
using System.Security.Claims;
using Nop.Core;
using System.Linq;
using Nop.Services.Common;

namespace Nop.Services.Authentication
{
    /// <summary>
    /// Authentication service
    /// </summary>
    public partial class ClaimsAuthenticationService : IAuthenticationService
    {
        #region Fields

        private readonly HttpContextBase _httpContext;
        private readonly ICustomerService _customerService;
        private readonly IGenericAttributeService _genericAttributeService;

        private Customer _cachedCustomer;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="httpContext">HTTP context</param>
        /// <param name="customerService">Customer service</param>
        /// <param name="customerSettings">Customer settings</param>
        public ClaimsAuthenticationService(HttpContextBase httpContext,
            ICustomerService customerService,
            IGenericAttributeService genericAttributeService)
        {
            this._httpContext = httpContext;
            this._customerService = customerService;
            this._genericAttributeService = genericAttributeService;
        }

        #endregion

        #region Utilities

        private static string FindFirstValue(ClaimsIdentity identity, string claimType)
        {
            Claim claim = identity.FindFirst(claimType);

            if (claim == null)
                return null;

            return claim.Value;
        }

        private static Guid FindFirstGuid(ClaimsIdentity identity, string claimType)
        {
            string value = FindFirstValue(identity, claimType);
            if (value == null)
                return Guid.Empty;

            Guid result;

            if (!Guid.TryParse(value, out result))
                return Guid.Empty;

            return result;
        }

        /// <summary>
        /// Get authenticated customer
        /// </summary>
        /// <param name="ticket">Ticket</param>
        /// <returns>Customer</returns>
        protected virtual Customer GetAuthenticatedCustomerFromIdentity(ClaimsIdentity identity)
        {
            if (identity == null)
                throw new ArgumentNullException("identity");

            Guid userGuid = FindFirstGuid(identity, ClaimTypes.NameIdentifier);
            if (userGuid == Guid.Empty)
                return null;

            return _customerService.GetCustomerByGuid(userGuid);
        }

        protected virtual void UpdateCustomerFromIdentity(ClaimsIdentity identity, Customer customer)
        {
            string email = FindFirstValue(identity, ClaimTypes.Email);
            if (email != null && !email.Trim().Equals(customer.Email, StringComparison.InvariantCulture))
            {
                customer.Email = email.Trim();
                _customerService.UpdateCustomer(customer);
            }

            string username = FindFirstValue(identity, ClaimTypes.Name);
            if (username != null && !username.Trim().Equals(customer.Username, StringComparison.InvariantCulture))
            {
                customer.Username = username.Trim();
                _customerService.UpdateCustomer(customer);
            }

            string firstName = FindFirstValue(identity, ClaimTypes.GivenName);
            if (firstName != null && !firstName.Trim().Equals(customer.GetAttribute<string>(SystemCustomerAttributeNames.FirstName)))
                _genericAttributeService.SaveAttribute(customer, SystemCustomerAttributeNames.FirstName, firstName.Trim());
            
            string lastName = FindFirstValue(identity, ClaimTypes.Surname);
            if (lastName != null && !lastName.Trim().Equals(customer.GetAttribute<string>(SystemCustomerAttributeNames.LastName)))
                _genericAttributeService.SaveAttribute(customer, SystemCustomerAttributeNames.LastName, lastName.Trim());
        }

        #endregion

        #region Methods

        /// <summary>
        /// Sign in
        /// </summary>
        /// <param name="customer">Customer</param>
        /// <param name="createPersistentCookie">A value indicating whether to create a persistent cookie</param>
        public virtual void SignIn(Customer customer, bool createPersistentCookie)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sign out
        /// </summary>
        public virtual void SignOut()
        {
            _cachedCustomer = null;
        }

        /// <summary>
        /// Get authenticated customer
        /// </summary>
        /// <returns>Customer</returns>
        public virtual Customer GetAuthenticatedCustomer()
        {
            if (_cachedCustomer != null)
                return _cachedCustomer;

            if (_httpContext == null ||
                _httpContext.Request == null ||
                !_httpContext.Request.IsAuthenticated ||
                !(_httpContext.User.Identity is ClaimsIdentity))
            {
                return null;
            }

            var identity = (ClaimsIdentity)_httpContext.User.Identity;
            var customer = GetAuthenticatedCustomerFromIdentity(identity);

            if (customer == null)
            {
                customer = new Customer();

                customer.CustomerGuid = FindFirstGuid(identity, ClaimTypes.NameIdentifier);

                //add to 'Registered' role
                var registeredRole = _customerService.GetCustomerRoleBySystemName(SystemCustomerRoleNames.Registered);
                if (registeredRole == null)
                    throw new NopException("'Registered' role could not be loaded");

                customer.CustomerRoles.Add(registeredRole);

                //remove from 'Guests' role
                var guestRole = customer.CustomerRoles.FirstOrDefault(cr => cr.SystemName == SystemCustomerRoleNames.Guests);
                if (guestRole != null)
                    customer.CustomerRoles.Remove(guestRole);

                customer.Active = true;
                customer.CreatedOnUtc = DateTime.UtcNow;
                customer.LastActivityDateUtc = DateTime.UtcNow;

                _customerService.InsertCustomer(customer);
            }

            UpdateCustomerFromIdentity(identity, customer);

            if (customer != null && customer.Active && !customer.Deleted && customer.IsRegistered())
                _cachedCustomer = customer;

            return _cachedCustomer;
        }

        #endregion
    }
}