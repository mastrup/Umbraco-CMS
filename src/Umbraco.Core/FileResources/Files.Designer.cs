﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Umbraco.Core.FileResources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Files {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Files() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Umbraco.Core.FileResources.Files", typeof(Files).Assembly);
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
        ///   Looks up a localized string similar to &lt;?xml version=&quot;1.0&quot;?&gt;
        ///
        ///&lt;!-- Blocks public downloading of anything in this folder and sub folders --&gt;
        ///
        ///&lt;configuration&gt; 
        ///  &lt;system.web&gt;
        ///    &lt;httpHandlers&gt;
        ///      &lt;add path=&quot;*&quot; verb=&quot;*&quot; type=&quot;System.Web.HttpNotFoundHandler&quot;/&gt;
        ///    &lt;/httpHandlers&gt;
        ///  &lt;/system.web&gt;
        ///  &lt;system.webServer&gt;
        ///    &lt;validation validateIntegratedModeConfiguration=&quot;false&quot; /&gt;
        ///    &lt;handlers&gt;
        ///      &lt;remove name=&quot;BlockViewHandler&quot;/&gt;
        ///      &lt;add name=&quot;BlockViewHandler&quot; path=&quot;*&quot; verb=&quot;*&quot; preCondition=&quot;integratedMode&quot; type=&quot;System.Web.H [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string BlockingWebConfig {
            get {
                return ResourceManager.GetString("BlockingWebConfig", resourceCulture);
            }
        }
    }
}