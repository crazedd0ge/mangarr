public class NaturalStringComparer : IComparer<string>
{
    [System.Runtime.InteropServices.DllImport("shlwapi.dll", CharSet = System.Runtime.InteropServices.CharSet.Unicode)]
    public static extern int StrCmpLogicalW(string psz1, string psz2);

    public int Compare(string? a, string? b)
    {
        if (a == null || b == null) return 0;
        return StrCmpLogicalW(a, b);
    }
}
