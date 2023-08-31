using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ComercialColindres.ReglasNegocio.Utilities
{
    public static class ListComparer
    {
        // Generic function to determine deleted elements from one list compared to another
        public static List<T> GetMissingElements<T>(List<T> currentItems, List<T> newItems)
        {
            var identityProperty = GetIdentityProperty(typeof(T));
            var newItemsId = newItems.Select(item => identityProperty.GetValue(item)).ToList();
            return currentItems.Where(item => !newItemsId.Contains(identityProperty.GetValue(item))).ToList();
        }

        // Generic function to determine deleted elements from one list compared to another by identitySelector
        public static List<T> GetMissingElementsByIdentity<T, TKey>(List<T> listA, List<T> listB, Func<T, TKey> identitySelector)
        {
            var listBIds = listB.Select(identitySelector).ToList();
            return listA.Where(item => !listBIds.Contains(identitySelector(item))).ToList();
        }

        // Helper method to get the PropertyInfo of the identity property
        private static PropertyInfo GetIdentityProperty(Type entityType)
        {
            var identityProperty = entityType.GetProperties()
                .FirstOrDefault(prop => prop.GetCustomAttribute(typeof(System.ComponentModel.DataAnnotations.KeyAttribute)) != null);

            if (identityProperty == null)
            {
                throw new InvalidOperationException("Entity must have a property marked with [Key] attribute.");
            }

            return identityProperty;
        }

        // Helper method to get the identity value of an entity
        public static object GetIdentityValue<T>(T entity)
        {
            var identityProperty = GetIdentityProperty(typeof(T));
            return identityProperty.GetValue(entity);
        }
    }
}