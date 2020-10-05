using System;
using System.Collections.Generic;
using System.Linq;
using Gvs.DataAccess.Core.Extensions;
using Gvs.DataAccess.Core.Interfaces;
using Gvs.DataAccess.Core.QueryBuilder;

namespace Gvs.DataAccess.Core
{
    public static class DataAccess
    {

        /// <summary>
        /// Creates a new DataAccess instance from the provider's assembly and namespace
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static IDataAccess createConnection(string connectionString)
        {
            IDataAccess connection = null;

            try
            {
                var connectionStringParts = connectionString.SplitDataAccessConnectionString();
                var classes = getAllDataAccessClasses(connectionStringParts["provider"], typeof(DataAccessBase));

                if (classes.Count != 1)
                {
                    throw new DataAccessException("Too many DataAccess derived classes found in provider's assembly");
                }

                var instance = Activator.CreateInstance(classes[0], new object[] { connectionStringParts["provider connection string"] });

                ((DataAccessBase)instance).ProviderAssembly = connectionStringParts["provider"];
                connection = instance as IDataAccess;
            }
            catch (Exception ex)
            {
                connection = null;
            }

            return connection;
        }

        internal static Parameter createParameter(this DataAccessBase dbAccess, QueryParameter queryParameter)
        {
            Parameter parameter = null;

            try
            {
                if (string.IsNullOrEmpty(dbAccess.ProviderAssembly))
                {
                    throw new DataAccessException("Provider assembly not defined");
                }
                var classes = getAllDataAccessClasses(dbAccess.ProviderAssembly, typeof(Parameter));

                if (classes.Count != 1)
                {
                    throw new DataAccessException("Too many Parameter derived classes found in provider's assembly");
                }

                parameter = Activator.CreateInstance(classes[0], new object[] { queryParameter }) as Parameter;
            }
            catch (Exception)
            {
                parameter = null;
            }

            return parameter;
        }

        internal static IQueryBuilder<T> createQueryBuilder<T>(this DataAccessBase dbAccess) where T: class, new() {
            IQueryBuilder<T> queryBuilder = null;

            try {
                if (string.IsNullOrEmpty(dbAccess.ProviderAssembly)) {
                    throw new DataAccessException("Provider assembly not defined");
                }
                var classes = getAllDataAccessClasses(dbAccess.ProviderAssembly, typeof(IQueryBuilder<T>));

                if (classes.Count != 1) {
                    throw new DataAccessException("Too many Parameter derived classes found in provider's assembly");
                }

                queryBuilder = Activator.CreateInstance(classes[0]) as IQueryBuilder<T>;
            } catch (Exception) {
                queryBuilder = null;
            }

            return queryBuilder;
        }

        /// <summary>
        /// Loads the provider's assembly and finds the classes that derive from the given base class type
        /// </summary>
        /// <param name="rootNameSpace"></param>
        /// <param name="baseClassType"></param>
        /// <returns></returns>
        private static List<Type> getAllDataAccessClasses(string rootNameSpace, Type baseClassType)
        {
            var assembly = AppDomain.CurrentDomain.Load(rootNameSpace);
            var types = assembly.GetTypes()
                .Where(c => baseClassType.IsAssignableFrom(c) && !c.IsInterface && !c.IsAbstract)
                .ToList();

            return types;
        }

    }
}
