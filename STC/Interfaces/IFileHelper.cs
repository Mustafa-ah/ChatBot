using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace STC.Interfaces
{
    public interface IFileHelper
    {
        void OpenFile(string filepath, bool isRtl);
        string GetFilePath(string name);
        Stream OpenStream(string path, int bufferSize);
        string SaveFile(byte[] buffer, string fileName);
    }
}
