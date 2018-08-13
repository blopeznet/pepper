using System;

namespace Xam.Marvelous.Model.Base
{
    public class Thumbnail
    {
        public string Path { get; set; }
        public string Extension { get; set; }

        private String _image;
        [JsonIgnore]
        public String Image
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