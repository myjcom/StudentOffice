using Newtonsoft.Json;
using System.IO;

namespace StudentOffice.Settings
{

    public class Config
    {
        public string DbFileName { get; set; }

        private string contractDocFileName;
        private string referenceDocFileName;

        public string ContractDocFileName
        {
            get => contractDocFileName;
            set
            {
                if (value != contractDocFileName)
                {
                    contractDocFileName = value;
                }
            }
        }
        public string ReferenceDocFileName
        {
            get => referenceDocFileName; 
            set
            {
                if (value != referenceDocFileName)
                {
                    referenceDocFileName = value;
                }
            }
        }
    }
}
