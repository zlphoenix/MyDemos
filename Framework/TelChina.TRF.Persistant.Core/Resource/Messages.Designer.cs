﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.237
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TelChina.TRF.Persistant.Core.Resource {
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
    internal class Messages {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Messages() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("TelChina.TRF.Persistant.Core.Resource.Messages", typeof(Messages).Assembly);
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
        ///   Looks up a localized string similar to 实体没有被跟踪或者状态不正常,操作无法执行.
        /// </summary>
        internal static string exception_ChangeTrackerIsNullOrStateIsNOK {
            get {
                return ResourceManager.GetString("exception_ChangeTrackerIsNullOrStateIsNOK", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 表达式路径不合法.
        /// </summary>
        internal static string exception_ExpressionPathNotValid {
            get {
                return ResourceManager.GetString("exception_ExpressionPathNotValid", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 过滤条件不能为空.
        /// </summary>
        internal static string exception_FilterCannotBeNull {
            get {
                return ResourceManager.GetString("exception_FilterCannotBeNull", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Include路径不能为空.
        /// </summary>
        internal static string exception_IncludePathCannotBeNullOrEmpty {
            get {
                return ResourceManager.GetString("exception_IncludePathCannotBeNullOrEmpty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to pageCount必须大于0.
        /// </summary>
        internal static string exception_InvalidPageCount {
            get {
                return ResourceManager.GetString("exception_InvalidPageCount", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to PageIndex必须大于0.
        /// </summary>
        internal static string exception_InvalidPageIndex {
            get {
                return ResourceManager.GetString("exception_InvalidPageIndex", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 参数不能为空.
        /// </summary>
        internal static string exception_ItemArgumentIsNull {
            get {
                return ResourceManager.GetString("exception_ItemArgumentIsNull", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 数据上下文不能为空.
        /// </summary>
        internal static string exception_ObjectContextIsNull {
            get {
                return ResourceManager.GetString("exception_ObjectContextIsNull", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to OrderBy表达式不能为空.
        /// </summary>
        internal static string exception_OrderByExpressionCannotBeNull {
            get {
                return ResourceManager.GetString("exception_OrderByExpressionCannotBeNull", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 条件表达式不能为空.
        /// </summary>
        internal static string exception_SpecificationIsNull {
            get {
                return ResourceManager.GetString("exception_SpecificationIsNull", resourceCulture);
            }
        }
    }
}
