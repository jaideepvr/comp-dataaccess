using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Reflection;

namespace Jsits.DataAccess.Core
{
    /// <summary>
    /// Jsits.DataAccess generic exception class
    /// </summary>
    [global::System.Serializable]
    public class DataAccessException : Exception
    {

        #region Properties

        /// <summary>
        /// Name of the Assembly
        /// </summary>
        public string AssemblyName { get; private set; }

        /// <summary>
        /// Name of the class
        /// </summary>
        public string ClassName { get; private set; }

        /// <summary>
        /// Name of the method
        /// </summary>
        public string MethodName { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor 
        /// </summary>
        public DataAccessException() {
            UpdateExceptionMetaInfo();
        }

        /// <summary>
        /// constructor with message
        /// </summary>
        /// <param name="message"></param>
        public DataAccessException(string message) : base(message) {
            UpdateExceptionMetaInfo();
        }

        /// <summary>
        /// Constructor with message and exception object
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public DataAccessException(string message, Exception inner) : base(message, inner) {
            UpdateExceptionMetaInfo();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Update exception details
        /// </summary>
        private void UpdateExceptionMetaInfo() 
        {
            StackFrame stackFrame = new System.Diagnostics.StackFrame(3); //Go back 3 methods.
            MethodBase methodBase = stackFrame.GetMethod();

            AssemblyName = methodBase.DeclaringType.Assembly.GetName().Name;
            ClassName = methodBase.DeclaringType.Name;
            MethodName = methodBase.Name;
        }

        #endregion

    }
}
