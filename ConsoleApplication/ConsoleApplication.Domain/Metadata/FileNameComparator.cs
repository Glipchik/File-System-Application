using System.Collections;

namespace ConsoleApplication.Domain.MetaData
{
    [Serializable]
    internal class FileNameComparator : IComparer<FileData>
    {
        [NonSerialized]
        private CaseInsensitiveComparer? _caseInsensitiveComparer;

        public int Compare(FileData? x, FileData? y)
        {
            if (_caseInsensitiveComparer is null) _caseInsensitiveComparer = new CaseInsensitiveComparer();

            if (x is null && y is null)
            {
                return 0;
            }

            if (x is null)
            {
                return -1;
            }

            if (y is null)
            {
                return 1;
            }

            return _caseInsensitiveComparer.Compare(x.Name, y.Name);
        }
    }
}