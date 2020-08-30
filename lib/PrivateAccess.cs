using System.Reflection;
using System.Runtime.CompilerServices;

namespace Com.NarvinSingh.Test
{
    public static class PrivateAccess
    {
        const BindingFlags DEFAULT_BINDINGS = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;
        const BindingFlags GET_BINDINGS = DEFAULT_BINDINGS | BindingFlags.GetField | BindingFlags.GetProperty;
        const BindingFlags SET_BINDINGS = DEFAULT_BINDINGS | BindingFlags.SetField | BindingFlags.SetProperty;
        const BindingFlags INVOKE_BINDINGS = DEFAULT_BINDINGS | BindingFlags.InvokeMethod;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static object Get(string name, object target)
        {
            return target.GetType().InvokeMember(name, GET_BINDINGS, null, target, null);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Set(string name, object target, object value)
        {
            target.GetType().InvokeMember(name, SET_BINDINGS, null, target, new object[] { value });
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static object Call(string name, object target, object[] args = null)
        {
            return target.GetType().InvokeMember(name, INVOKE_BINDINGS, null, target, args);
        }
    }
}
