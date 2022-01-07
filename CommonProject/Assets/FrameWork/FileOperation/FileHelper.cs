using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrameWork.File
{
    public class FileHelper
    {
        public string MD5(string path)
        { 
            System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(path);
            byte[] hash = md5.ComputeHash(inputBytes);
            return "";
        }
    }
}
