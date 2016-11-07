﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Nop.Plugin.Api.DTOs.Customers
{
    public class BaseCustomerDto
    {
        private List<int> _roleIds;

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the customer unique identifier.
        /// </summary>
        /// <value>
        /// The customer unique identifier.
        /// </value>
        [JsonProperty("customer_guid")]
        public Guid CustomerGuid { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }
        /// <summary>
        /// Gets or sets the email
        /// </summary>
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [JsonProperty("last_name")]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the admin comment
        /// </summary>
        [JsonProperty("admin_comment")]
        public string AdminComment { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the customer is tax exempt
        /// </summary>
        [JsonProperty("is_tax_exempt")]
        public bool? IsTaxExempt { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this customer has some products in the shopping cart
        /// <remarks>The same as if we run this.ShoppingCartItems.Count > 0
        /// We use this property for performance optimization:
        /// if this property is set to false, then we do not need to load "ShoppingCartItems" navigation property for each page load
        /// It's used only in a couple of places in the presenation layer
        /// </remarks>
        /// </summary>
        [JsonProperty("has_shopping_cart_items")]
        public bool? HasShoppingCartItems { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the customer is active
        /// </summary>
        [JsonProperty("active")]
        public bool? Active { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the customer has been deleted
        /// </summary>
        [JsonProperty("deleted")]
        public bool? Deleted { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the customer account is system
        /// </summary>
        [JsonProperty("is_system_account")]
        public bool? IsSystemAccount { get; set; }

        /// <summary>
        /// Gets or sets the customer system name
        /// </summary>
        [JsonProperty("system_name")]
        public string SystemName { get; set; }

        /// <summary>
        /// Gets or sets the last IP address
        /// </summary>
        [JsonProperty("last_ip_address")]
        public string LastIpAddress { get; set; }

        /// <summary>
        /// Gets or sets the date and time of entity creation
        /// </summary>
        [JsonProperty("created_on_utc")]
        public DateTime? CreatedOnUtc { get; set; }

        /// <summary>
        /// Gets or sets the date and time of last login
        /// </summary>
        [JsonProperty("last_login_date_utc")]
        public DateTime? LastLoginDateUtc { get; set; }

        /// <summary>
        /// Gets or sets the date and time of last activity
        /// </summary>
        [JsonProperty("last_activity_date_utc")]
        public DateTime? LastActivityDateUtc { get; set; }

        [JsonProperty("role_ids")]
        public List<int> RoleIds
        {
            get
            {
                if (_roleIds == null)
                {
                    _roleIds = new List<int>();
                }

                return _roleIds;
            }
            set { _roleIds = value; } 
        } 
    }
}