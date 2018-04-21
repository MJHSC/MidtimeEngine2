//Midtime Engine
//	本ソースコードのライセンスについては付属の LICENSE.html をお読みください。
//	Please read LICENSE.html for detail license information.

using System;
using System.Dynamic;
using System.Reflection;


namespace MJHSC.MidtimeEngine.Languages.SmallBasic.GameAPI {

	// Midtime v2 alpha引継時のコードそのまま。
	// ここの解析が必要では？
	// https://blogs.msdn.microsoft.com/davidebb/2009/10/23/using-c-dynamic-to-call-static-members/
    class StaticMembersDynamicWrapper : DynamicObject {
        private Type _type;

        public StaticMembersDynamicWrapper(Type type) {
			_type = type;
		}

        // Handle static properties
        public override bool TryGetMember(GetMemberBinder binder, out object result) {
            PropertyInfo prop = _type.GetProperty(binder.Name, BindingFlags.FlattenHierarchy | BindingFlags.Static | BindingFlags.Public);
            if (prop == null) {
                result = null;
                return false;
            }

            result = prop.GetValue(null, null);
            return true;
        }

        // Handle static methods
        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result) {
            MethodInfo method = _type.GetMethod(binder.Name, BindingFlags.FlattenHierarchy | BindingFlags.Static | BindingFlags.Public);
            if (method == null) {
                result = null;
                return false;
            }

            result = method.Invoke(null, args);
            return true;
        }
    }
}
