﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Gvs.DataAccess.Core.ResourceFiles {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class ExceptionMessages {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ExceptionMessages() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Gvs.DataAccess.Core.ResourceFiles.ExceptionMessages", typeof(ExceptionMessages).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The functionality to handle arrays of blobs or clobs has not been implemented..
        /// </summary>
        internal static string ArraysOfBlobsOrClobs {
            get {
                return ResourceManager.GetString("ArraysOfBlobsOrClobs", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A data access error occurred when beginning a new connection..
        /// </summary>
        internal static string BeginTransaction {
            get {
                return ResourceManager.GetString("BeginTransaction", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A data access error occurred when commiting a transaction..
        /// </summary>
        internal static string CommitTransaction {
            get {
                return ResourceManager.GetString("CommitTransaction", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A data access error occurred when accessing the embedded connection object..
        /// </summary>
        internal static string DBConnection {
            get {
                return ResourceManager.GetString("DBConnection", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A data access error occurred when executing a stored procedure..
        /// </summary>
        internal static string ExecuteProc {
            get {
                return ResourceManager.GetString("ExecuteProc", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A data access error occurred when executing a SQL statement..
        /// </summary>
        internal static string ExecuteSQL {
            get {
                return ResourceManager.GetString("ExecuteSQL", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A data access error occurred when returning a dataset..
        /// </summary>
        internal static string GetDataSet {
            get {
                return ResourceManager.GetString("GetDataSet", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A data access error occurred when returning a data reader..
        /// </summary>
        internal static string GetReader {
            get {
                return ResourceManager.GetString("GetReader", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A data access error occurred when checking to see if application is currently in a transaction..
        /// </summary>
        internal static string InTransaction {
            get {
                return ResourceManager.GetString("InTransaction", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Use Oracle Parameter..
        /// </summary>
        internal static string OracleParameterError {
            get {
                return ResourceManager.GetString("OracleParameterError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A data access error occurred when rolling back a transaction..
        /// </summary>
        internal static string RollBackTransaction {
            get {
                return ResourceManager.GetString("RollBackTransaction", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Use SqlDbType..
        /// </summary>
        internal static string SQLParameterError {
            get {
                return ResourceManager.GetString("SQLParameterError", resourceCulture);
            }
        }
    }
}
