﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ComercialColindres.Datos.Recursos {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class MensajesValidacion {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal MensajesValidacion() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("ComercialColindres.Datos.Recursos.MensajesValidacion", typeof(MensajesValidacion).Assembly);
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
        ///   Looks up a localized string similar to Debe de ingresar una fecha valida para.
        /// </summary>
        internal static string Campo_FechaValida {
            get {
                return ResourceManager.GetString("Campo_FechaValida", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} es requerido.
        /// </summary>
        internal static string Campo_Requerido {
            get {
                return ResourceManager.GetString("Campo_Requerido", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Debe de ingresar un valor para.
        /// </summary>
        internal static string Campo_Vacio {
            get {
                return ResourceManager.GetString("Campo_Vacio", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} el valor debe ser mayor a 0.
        /// </summary>
        internal static string Invalid_Value {
            get {
                return ResourceManager.GetString("Invalid_Value", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No se puede eliminar.
        /// </summary>
        internal static string Validacion_NoSePuedeEliminar {
            get {
                return ResourceManager.GetString("Validacion_NoSePuedeEliminar", resourceCulture);
            }
        }
    }
}
