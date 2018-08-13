using System;

namespace Xam.Marvelous.Model.Base
{
    public class Image
    {
        public string Path { get; set; }
        public string Extension { get; set; }

        [JsonIgnore]
        public String DisplayPath
        {
            get
            {

                if (this != null)
                {
                    String ret = this.Path + "." + this.Extension;
                    return ret;
                }
                else
                    return "";
            }
        }
    }
}
