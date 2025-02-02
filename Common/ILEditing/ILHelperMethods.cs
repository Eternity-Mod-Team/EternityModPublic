using MonoMod.Cil;

namespace EternityMod.Common.ILEditing
{
    public partial class ILChanges
    {
        public static void DumpToLog(ILContext il) => EternityMod.Instance.Logger.Debug(il.ToString());

        public static void LogFailure(string name, string reason) => EternityMod.Instance.Logger.Warn($"IL edit \"{name}\" failed! {reason}");
    }
}
